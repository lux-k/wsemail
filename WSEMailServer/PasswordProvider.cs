/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Web;
using System.Web.Services;

using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Security.Tokens;

using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;

using MailServerInterfaces;

namespace WSEmailServer
{

	public class WSEmailPasswordProvider : UsernameTokenManager 
	{

		protected override string AuthenticateToken(UsernameToken ut)
		{
			// Ensure that the SOAP message sender passed a UsernameToken.
			if (ut == null)
				throw new ArgumentNullException();

			Global.ServerConfiguration.Log(MailServerLogType.UserAuthentication,this + " : Received password authentication request for user '"+ut.Username+"'.");
			string s = Global.ServerConfiguration.DataAccessor.ReadUserPassword(ut.Username);
			Global.ServerConfiguration.Log(MailServerLogType.ServerDebug,this + " : Returned password '"+s+"'.");
			return s;
		}
	}
	/* WSE 1.0
	/// <summary>
	/// Provides a means to get to passwords for the WSE v1.0
	/// </summary>
	public class WSEmailPasswordProvider : Microsoft.Web.Services.Security.IPasswordProvider
	{
		
		/// <summary>
		/// Given a username token, this will return the password associated with that user.
		/// </summary>
		/// <param name="ut">Username token from framework</param>
		/// <returns>Password</returns>
		public string GetPassword(Microsoft.Web.Services.Security.UsernameToken ut) 
		{
			Global.ServerConfiguration.Log(MailServerLogType.UserAuthentication,this + " : Received password authentication request for user '"+ut.Username+"'.");
			
			MailServerInterfaces.DatabaseConnection c = Global.ServerConfiguration.Database.Connection;
			Global.ServerConfiguration.Log(MailServerLogType.ServerDebug,this + " : PasswordProvider got ID " + c.Identifier.ToString());

			SqlCommand command = new SqlCommand("WSEmailRetrievePassword",c.Connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@user",ut.Username);
						
			SqlDataReader r = command.ExecuteReader(CommandBehavior.SingleResult);
			
			string s = "";
			if (r.Read())
				s=r[0].ToString();

			r.Close();

			Global.ServerConfiguration.Database.Free(c);
			Global.ServerConfiguration.Log(MailServerLogType.ServerDebug,this + " : Returned password '"+s+"'.");
			return s;
		}

	}
	*/	
}
