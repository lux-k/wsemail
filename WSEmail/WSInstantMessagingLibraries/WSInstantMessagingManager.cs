/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System.Windows.Forms;
using System;
using System.Collections;
using WSEmailProxy;
using System.Xml;
using WSEmailClientConfig;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;


namespace WSInstantMessagingLibraries
{
	/// <summary>
	/// Summary description for WSInstantMessagingManager.
	/// </summary>
	public class WSInstantMessagingManager
	{
		/// <summary>
		/// The hash table holds references to all the message windows created.
		/// </summary>
		private Hashtable tblForms = null;
		/// <summary>
		/// Reference to the main form. This is necessary so that we can create the message
		/// windows in the same thread as the main form. This is important because the owner thread
		/// of the main form is the gui thread.
		/// </summary>
		public Form mainForm;
		/// <summary>
		/// Holds the configuration we're using. The config holds server information
		/// and client information.
		/// </summary>
		public WSInstantMessagingConfig InstantMessagingConfig;

		/// <summary>
		/// Default constructor. Creates a new manager with an empty hashtable.
		/// </summary>
		public WSInstantMessagingManager()
		{
			// nothing much to do as initialization, just
			// create the hash table.
			tblForms = new Hashtable();
		}

		/// <summary>
		/// Processes a WSEmailMessage. This will check to see if there is a window already created
		/// that corresponds to the message to be processed. If there is it will post that message to that window
		/// otherwise it will create a new message.
		/// </summary>
		/// <param name="m">The message to be processed/displayed.</param>
		public void ProcessMessage (WSEmailMessage m) 
		{
			// we're suppose to process (display) this message.
			// first, see if we already have a window open from this sender.
			
			if (tblForms[GetRecipients(m)] != null) 
			{
				// if we do, then get that window's reference, and post the message.
				(	(IMForm)tblForms[GetRecipients(m)]).PostMessage(m);
			} 
			else 
			{
				// otherwise, we have to make a delegate for the makenew function.
				// this is because we want to take the makenew function and specify
				// where it runs. by making a delegate for the function, we are basically
				// creating a method pointer. we can then pass the pointer (delegate) to
				// the invoke method of the main form. from there, it will be executed by form's thread
				// ie, the gui thread. this is important because the makenew function creates a new window
				// if that window isn't created on the gui thread, it might become unresponsive if
				// the creator's thread is blocked or destroyed.
				MakeNewDelegate mnd = new MakeNewDelegate(MakeNew);
				mainForm.Invoke(mnd, new Object [] { m });
			}
		}

		public static string GetRecipients(WSEmailMessage m) 
		{
			ArrayList a = new ArrayList();
			a.Add(m.Sender);
			a.AddRange(m.Recipients.AllRecipients);
			a.Sort();
			return RecipientList.FormatRecipients((string[])a.ToArray(typeof(string))).ToLower();
		}

		/// <summary>
		/// Just the definition of the delegate. this delegate can be used for any 
		/// method which uses the same parameters and return data structure.
		/// </summary>
		public delegate void MakeNewDelegate(WSEmailMessage ms);

		/// <summary>
		/// This makes a new instant messaging window, records the window handle, posts the message to the window
		/// and shows the window.
		/// </summary>
		/// <param name="ms">Message to process</param>
		public void MakeNew(WSEmailMessage ms)
		{
			// this method is running on the mainform's thread (see the delegation
			// stuff above.
			//MessageBox.Show("creating new window.. recipients are: " + GetRecipients(ms).ToLower());
			IMForm f = createChatWindow(GetRecipients(ms).ToLower());
			// post the message
			f.PostMessage(ms);
			// and force the window to be shown.
			f.Show();
			f.BringToFront();
		}

		/// <summary>
		/// This is used to create a new window with the recipient set
		/// to a certain string. This is primarily used for the input box
		/// on the main window
		/// </summary>
		/// <param name="recipient">A user@hostname string representing who to send messages to.</param>
		public void MakeNew(string recipient) 
		{
			// create the new form and manually add the window
			// to the hashtable.
			IMForm f = createChatWindow(recipient);
			f.Show();
		}

		/// <summary>
		/// Creates a chat window,if needed, and returns a reference to the window. Makes sure the 
		/// reference table of windows stays updated.
		/// </summary>
		/// <param name="recipient">Who messages are to be sent to.</param>
		/// <returns>IMForm reference</returns>
		public IMForm createChatWindow(string recipient) 
		{
			// if its not in the table
			if (tblForms[recipient.ToLower()] == null) 
			{
				// creates a new form
				IMForm f = new IMForm(recipient);
				if (f.Disposing || f.IsDisposed) 
				{
					// if the window is being trashed, then delete it and recreate it 
					tblForms.Remove(recipient);
					f = createChatWindow(recipient);
				}
				f.Disposed += new System.EventHandler(this.removeWindow);
				tblForms.Add(recipient.ToLower(),f);
				f.setConfig( InstantMessagingConfig );
				return f;
			} else 
				return (IMForm)tblForms[recipient.ToLower()];
		}

		/// <summary>
		/// Updates the communications server of a new remoting URL for the message buffer
		/// </summary>
		/// <param name="s">The full URL of the published remoting object.</param>
		public void updateCommServer(string s) 
		{
			ExtensionArgument args = new ExtensionArgument("UpdateIMLocation");
			args.AddArgument("Location",s);
			ClientConfiguration.Proxy.ExecuteExtensionHandler("InstantMessaging",args.AsXmlElement());
			if (ClientConfiguration.UserID == null || ClientConfiguration.UserID.IndexOf("@") <= 0) 
			{
				MessageBox.Show("Uh oh.. no userid!!");
				foreach (SecurityToken t in ClientConfiguration.Proxy.ResponseSoapContext.Security.Tokens) 
				{
					
					if (t is X509SecurityToken)
						ClientConfiguration.UserID += "@" + PennLibraries.Utilities.GetCertCN(((X509SecurityToken)t).Certificate);
				}
			}
		}

		/// <summary>
		/// Removes a window from the window hash. This should only be used as a Dispose handler.
		/// </summary>
		/// <param name="sender">This will be the IMForm</param>
		/// <param name="e">Not needed, but required.</param>
		private void removeWindow(object sender, System.EventArgs e)
		{
			IMForm i = (IMForm)sender;
			tblForms.Remove(i.Recipient);
		}


	}
}
