/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Collections;
using MailServerInterfaces;
using WSEmailProxy;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Xml;

namespace WSEmailServer
{
	/// <summary>
	/// Serializes/Deserializes to/from the database (whatever that might be)
	/// </summary>
	public class DataAccessor : IDataAccessor, IDisposable
	{

		/// <summary>
		/// Reference to the mail server interfaces.
		/// </summary>
		protected IMailServer server = null;

		/// <summary>
		/// Default constructor does nothing.
		/// </summary>
		public DataAccessor()
		{
		}

		protected bool _init = false, _shut = false;

		/// <summary>
		/// Is this plugin initialized?
		/// </summary>
		public bool IsInitialized 
		{
			get 
			{
				return _init;
			}
		}

		/// <summary>
		/// Is this plugin marked as being shutdown?
		/// </summary>
		public bool IsShutdown
		{
			get 
			{
				return _shut;
			}
		}

		/// <summary>
		/// Resumes processing of messages (currently unimplemented).
		/// </summary>
		public void Resume() {}
		/// <summary>
		/// Suspends processing of messages (currently unimplemented).
		/// </summary>
		public void Suspend() {}

		/// <summary>
		/// Shutdowns this service (currently unimplemented).
		/// </summary>
		/// <returns>True (boolean)</returns>
		public bool Shutdown() {_shut = true; return true;}

		/// <summary>
		/// Initializes the reference to the mail server configuration object.
		/// </summary>
		/// <param name="m">MailServer reference</param>
		/// <returns>True (boolean)</returns>
		public bool Initialize(IMailServer m) 
		{
			server = m;
			server.Log(MailServerLogType.ServerInfo, this + " : Data abstraction layer running...");
			_init = true;
			return true;
		}

		/// <summary>
		/// Clones this object
		/// </summary>
		/// <returns>Object</returns>
		public object Clone() 
		{
			return this.MemberwiseClone();
		}

		/// <summary>
		/// Deletes an array of integer message numbers from the user's store.
		/// </summary>
		/// <param name="messagesToDelete">Array of integer representing message numbers</param>
		/// <returns>WSEmailStatus giving number of messages deleted</returns>
		public WSEmailStatus DeleteMessage(string userID, int[] messagesToDelete) 
		{
			SqlCommand sqlCmd=new SqlCommand();
			DatabaseConnection conn = server.Database.Connection;
			sqlCmd.Connection=conn.Connection;
			// note the where recipient = x and message id = x
			// this makes it so they can only get their mail
			sqlCmd.CommandText = "WSEmailDeleteMessage";
			sqlCmd.CommandType = CommandType.StoredProcedure;
			sqlCmd.Parameters.Add("@userid",userID);

			int i = 0;
			try 
			{
				foreach (int mess in messagesToDelete) 
				{
					SqlParameter p = sqlCmd.Parameters.Add("@messid",mess);
					i += sqlCmd.ExecuteNonQuery();
					sqlCmd.Parameters.Remove(p);
				}
				server.Log(MailServerLogType.Debug, this + " : deleted " + i.ToString() + " messages for " + userID);
			} 
			catch (Exception e) 
			{
				server.Log(MailServerLogType.Error, this + " : error deleting messages. " + e.Message);
			} 
			finally 
			{
				server.Database.Free(conn);
			}
			

			if (i > 0)
				return new WSEmailStatus(200,"Sucessfully deleted " + i.ToString() + " message(s).");
			else
				return new WSEmailStatus(500,"Message deletion failed.");
		}

		/// <summary>
		/// Retrieves a user password
		/// </summary>
		/// <param name="userID">Username</param>
		/// <returns>Password</returns>
		public string ReadUserPassword(string userID) 
		{
			DatabaseConnection conn = server.Database.Connection;
			SqlCommand command = new SqlCommand("WSEmailRetrievePassword",conn.Connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@user",userID);
						
			string s = null;
			try 
			{
				SqlDataReader r = command.ExecuteReader(CommandBehavior.SingleResult);
			
				if (r.Read())
					s=r[0].ToString();

				r.Close();

			} 
			catch (Exception e) 
			{
				server.Log(MailServerLogType.Error, this + " : error listing password. " + e.Message);
			} 
			finally 
			{
				server.Database.Free(conn);
			}
			return s;
		}

		public string ReadUserName(string userID) 
		{
			DatabaseConnection conn = server.Database.Connection;
			SqlCommand c=new SqlCommand("WSEmailRetrieveUsername",conn.Connection);
			c.CommandType = CommandType.StoredProcedure;
			c.Parameters.Add("@user",userID);
			string user = null;
			try 
			{
				user = (string)c.ExecuteScalar();
			} catch (Exception e) {
				server.Log(MailServerLogType.Error, this + " : error reading username. " + e.Message);
			} 
			finally 
			{
				server.Database.Free(conn);
			}
		
			return user;
		}

		/// <summary>
		/// Fetches all the headers in a user's inbox
		/// </summary>
		/// <returns>Array of WSEmailHeader or null</returns>
		public WSEmailHeader[] ReadHeaders(string userID, DateTime t) 
		{
			if (t < new DateTime(1800,1,1))
				t = new DateTime(1800,1,1);
			// connect to the database
			//SqlConnection sqlConn = this.GetConnection;
			SqlCommand sqlCmd=new SqlCommand();
			DatabaseConnection conn = server.Database.Connection;
			sqlCmd.Connection=conn.Connection;
			sqlCmd.CommandText="WSEmailRetrieveHeaders";
			sqlCmd.CommandType = CommandType.StoredProcedure;
			sqlCmd.Parameters.Add("@user",userID);
			sqlCmd.Parameters.Add("@date",t);
			SqlDataReader rows=sqlCmd.ExecuteReader();

			ArrayList ret = new ArrayList();
			try 
			{
				while(rows.Read())
				{
					// while we can still get results
					// make a new header object and populate it
					WSEmailHeader h = new WSEmailHeader();
					h.MessageID=rows.GetInt32(0);
					h.Sender=rows.GetString(1);
					h.Subject=rows.GetString(2);
					h.Timestamp=rows.GetDateTime(3);
					h.Flags=rows.GetInt32(4);
					ret.Add(h);
					// and add it to the array
				}

				rows.Close();
			} catch (Exception e)
			{
				server.Log(MailServerLogType.Error, this + " : error listing headers. " + e.Message);
			} 
			finally 
			{
				server.Database.Free(conn);
			}

			return (WSEmailHeader[])ret.ToArray(typeof(WSEmailHeader));
		}

		public WSEmailStatus WriteMessage(string recip, WSEmailMessage theMessage, XmlElement x)
		{
			WSEmailStatus status = null;
			DatabaseConnection DB = null;
			try 
			{
				DB = server.Database.Connection;
									
				SqlConnection sqlConn= DB.Connection;
				SqlCommand command = new SqlCommand("WSEmailSaveMessage", sqlConn);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add("@recips",theMessage.Recipients.ToString());
				command.Parameters.Add("@tousr",recip);
				command.Parameters.Add("@sender",theMessage.Sender);
				command.Parameters.Add("@timestamp",theMessage.Timestamp);
				if (theMessage.Subject == null || theMessage.Subject.Length == 0)
					command.Parameters.Add("@subject","");
				else
					command.Parameters.Add("@subject",theMessage.Subject);
				if (theMessage.Body == null || theMessage.Body.Length == 0)
					command.Parameters.Add("@body","");
				else
					command.Parameters.Add("@body",theMessage.Body);
				command.Parameters.Add("@sig",x.OuterXml);
				command.Parameters.Add("@flags",theMessage.MessageFlags);
				// and its saved
				int messageID = int.Parse(command.ExecuteScalar().ToString());
				server.Log(MailServerLogType.ServerDeliveryStatus,this + " : Saved message #" + messageID.ToString() + " for " + recip + " to the database.");
				// put the attachments in the attachments table, if they exist			
				if (theMessage.XmlAttachments != null && theMessage.XmlAttachments.Length > 0) 
				{
					for (int i = 0; i < theMessage.XmlAttachments.Length; i++) 
					{
						command.CommandText = "WSEmailSaveAttachment";
						command.Parameters.Clear();
						command.Parameters.Add("@messageid",messageID);
						command.Parameters.Add("@attachnum",i);
						command.Parameters.Add("@attachment",theMessage.XmlAttachments[i].OuterXml);
						command.ExecuteNonQuery();
					}
				}
				//messageCount++;
				status = new WSEmailStatus(200,"Message saved to database by " + server.Name);
				//if (MailDelivered != null)
				//	MailDelivered(recip);
			} 
			catch (Exception e) 
			{
				server.Log(MailServerLogType.ServerError,this + " : Error saving message to " + recip + ", " + e.Message);
				status = new WSEmailStatus(500,"Internal error while saving message...");
			} 
			finally 
			{
				// free the database connection
				if (DB != null)
					server.Database.Free(DB);
			}
			return status;
		}

		/// <summary>
		/// Retrieves a WSEmail message from the user's store
		/// </summary>
		/// <param name="messageToGet">Message number to get</param>
		/// <returns>WSEmailPackage containing message id, message and signature or null</returns>
		public WSEmailPackage ReadMessage(string userID, int messageToGet) 
		{
			// connect to the remote server
			SqlCommand sqlCmd=new SqlCommand();
			DatabaseConnection conn = server.Database.Connection;
			sqlCmd.Connection=conn.Connection;
			server.Log(MailServerLogType.ServerDebug, this + " : Fetching message " + messageToGet + " for: " + userID);
			// note the where recipient = x and message id = x
			// this makes it so they can only get their mail
			sqlCmd.CommandText = "WSEmailRetrieveMessage";
			sqlCmd.CommandType = CommandType.StoredProcedure;
			sqlCmd.Parameters.Add("@id",messageToGet);
			sqlCmd.Parameters.Add("@user",userID);

			SqlDataReader rows=sqlCmd.ExecuteReader();
			// execute and try to read
			WSEmailPackage p = null;
			
			try 
			{
				if (rows.Read()) 
				{
					// build a package. the package contains:
					// the message
					// the message id
					// and the signature
					WSEmailMessage m = new WSEmailMessage();
					p = new WSEmailPackage();
					p.MessageID=rows.GetInt32(0);
					m.Sender=rows.GetString(1);
					m.Subject=rows.GetString(2);
					m.Timestamp=rows.GetDateTime(3);
					m.Recipients.AddRange(RecipientList.ParseRecipients(rows.GetString(4)));
					//m.Recipient=rows.GetString(4);
					m.Body=rows.GetString(5);
					if (m.Body == null)
						m.Body = "";

					if (m.Subject == null)
						m.Subject = "";
				{
					string thesig = rows.GetString(6);
					XmlDocument x = new XmlDocument();
					x.LoadXml(thesig);
					p.sig = (XmlElement)x.ChildNodes[0];
				}
					m.MessageFlags = rows.GetInt32(7);
					int attachmentcount = rows.GetInt32(8);
					// get any attachments that are available
					rows.Close();

					XmlDocument[] theDocs = null;
					if (attachmentcount > 0) 
					{
						sqlCmd.CommandText="WSEmailRetrieveAttachments";
						sqlCmd.Parameters.Clear();
						sqlCmd.Parameters.Add("@id",messageToGet);
						rows=sqlCmd.ExecuteReader();
						theDocs = new XmlDocument[attachmentcount];
						int i=0;
						while (rows.Read())
						{
							XmlDocument xx = new XmlDocument();
							xx.LoadXml(rows.GetString(0));
							theDocs[i++] = xx;
						}
						rows.Close();
					}

					// copy the message and attachments over				
					p.theMessage = m;
					p.theMessage.XmlAttachments = theDocs;

				} 
				else 
				{	
					rows.Close();
					throw new Exception("Unable to find message #" + messageToGet.ToString() + " for user " + userID + ".");
				}
			} catch (Exception e) {
				server.Log(MailServerLogType.ServerError,this + " : Error reading message #"+messageToGet.ToString() + " for " + userID + ", " + e.Message);
			} 
			finally 
			{
				// free the database connection
				if (conn != null)
					server.Database.Free(conn);
			}

			return p;

		}

		/// <summary>
		/// Returns the plugin type of this plugin.
		/// </summary>
		/// <returns>PluginType.Service</returns>
		public MailServerInterfaces.PluginType PluginType  
		{
			get 
			{
				return MailServerInterfaces.PluginType.Service;
			}
		}

		/// <summary>
		/// Returns the status of the object. (not implemented)
		/// </summary>
		/// <returns></returns>
		public string GetStatus() 
		{
			return "Hello!!";
		}

		/// <summary>
		/// Required so that database connections can be freed in an efficient manner.
		/// </summary>
		public void Dispose() 
		{
/*
			if (dbc != null) 
			{
				server.Database.Free(dbc);
				server.Log(MailServerLogType.ServerInfo, "Message retriever freeing database connection.");
				dbc = null;
			}
			GC.SuppressFinalize(this);
*/
		}

		/// <summary>
		/// Cleans up any left over database connections.
		/// </summary>
		~DataAccessor() 
		{
			Dispose();
		}
	}
}
