using System;
using WSEmailProxy;
using Microsoft.Web.Services.Security;

namespace TestApp
{
	public class TheThread 
	{
		ModelObjects model = null;
		int id = 0;

		public TheThread (ModelObjects m, int i) 
		{
			this.model = m;
			id = i;
		}

		private void Log(string s) 
		{
			Console.WriteLine(id + ": " + s);
		}

		public void ThreadCode() 
		{
			MailServerProxy p = new MailServerProxy("http://localhost/WSEmailServer/MailServer.asmx");
			Log("Running " +  model.messages[id].Number.ToString() + " times...");
			for (int i = 0; i < model.messages[id].Number; i++) 
			{
				SecurityToken tok = model.tokens.Get();
				p.SecurityToken = tok;

				ACTIONS a = model.actions.Get();

				Console.WriteLine("Executing action: " + a.ToString());
				bool res = false;
				switch (a) 
				{
					case ACTIONS.ListHeaders : 
					{
						//	i--;
						try 
						{
							WSEmailHeader[] h = p.WSEmailFetchHeaders();
							Log("Retrieved " + h.Length.ToString());
							res = true;
						} 
						catch (Exception e) 
						{
							Log("Failed to retrieve headers");
							//e.Message;
						}
						break;
					}
					case ACTIONS.SendMessage : 
					{
						WSEmailMessage m = new WSEmailMessage();
						m.Recipients.Add(model.messages[this.id].Email);
						m.Subject = model.messages[this.id].Subject;
						m.Timestamp = DateTime.Now.ToString();
						m.Body = model.messages[this.id].Body;
			
						try 
						{
							WSEmailStatus status = p.WSEmailSend(m,null);
							Log(  " (" + tok.ToString() + ") : " + status.ToString());
							res = true;
						} 
						catch (Exception ex)
						{
							Console.WriteLine(tok.ToString());
							if (tok is UsernameToken) 
							{
								Log(((UsernameToken)tok).Username + " didn't authenticate.");
								Log(ex.Message);
							} 
							else if (tok is X509SecurityToken) 
							{
								Log("X509 cert didn't authenticate.");
							}

						}
						break;
					}
				}

				if (res)
					model.AddSuccess();
				else
					model.AddFailure();

			}
				return;
			
		}
	}
}
