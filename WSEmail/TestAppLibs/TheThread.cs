/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using WSEmailProxy;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;

namespace TestAppLibs
{
	public class TheThread 
	{

		ModelObjects model = null;
		int id = 0;
		private Random ran = new Random();

		public TheThread (ModelObjects m, int i) 
		{
			this.model = m;
			id = i;
		}

		private void Log(string s) 
		{
			model.Log(id + ": " + s);
		}

		public void ThreadCode() 
		{
			model.Log("Thread code running...");
			try 
			{
				MailServerProxy p = new MailServerProxy(model.server);
				Log("Running...");
				for (int i = 0; i < model.cycles; i++) 
				{
					SecurityToken tok = model.securitytokens.Get();
					p.SecurityToken = tok;

					ACTIONS a = model.actions.Get();
					DateTime start = DateTime.Now;
					DateTime stop = start;
					bool add=true;
					Log("Executing action: " + a.ToString());
					bool res = false;
					switch (a) 
					{
						case ACTIONS.ListHeaders : 
						{
							//	i--;
							try 
							{
								start = DateTime.Now;
								WSEmailHeader[] h = p.WSEmailFetchHeaders(DateTime.MinValue);
								stop = DateTime.Now;
								Log("Retrieved " + h.Length.ToString());
								res = true;
								lock(model.mesnums.SyncRoot) 
								{
									foreach (WSEmailHeader head in h) 
									{
										if (model.mesnums[head.MessageID] == null)
											model.mesnums.Add(head.MessageID,1);
									}
								}
							} 
							catch (Exception e) 
							{
								Log("Failed to retrieve headers" + e.Message);
							}
							break;
						}
						case ACTIONS.ListHeadersSmartly : 
						{
							//	i--;
							try 
							{
								start = DateTime.Now;
								DateTime t = DateTime.Now;
								WSEmailHeader[] h = p.WSEmailFetchHeaders(model.lastheader);
								stop = DateTime.Now;
								model.lastheader = t;
								Log("Retrieved " + h.Length.ToString());
								res = true;
								lock(model.mesnums.SyncRoot) 
								{
									foreach (WSEmailHeader head in h) 
									{
										if (model.mesnums[head.MessageID] == null)
											model.mesnums.Add(head.MessageID,1);
									}
								}
							} 
							catch (Exception e) 
							{
								Log("Failed to retrieve headers" + e.Message);
							}
							break;
						}
						case ACTIONS.DeleteMessage:
						{
							if (model.mesnums.Keys.Count > 0) 
							{
								lock(model.mesnums.SyncRoot) 
								{
									int num = ran.Next(model.mesnums.Keys.Count);
									try 
									{
										int coun = 0;
										foreach (object o in model.mesnums.Keys) 
										{
											if (coun == num) 
											{
												num = (int)o;
												break;
											}
											coun++;
										}
										model.mesnums.Remove(num);
										start = DateTime.Now;
										p.WSEmailDelete(num);
										stop = DateTime.Now;
										Log("Deleted message #" + num.ToString());
										res = true;
									} 
									catch 
									{
										Log("Unable to retrieve message " + i.ToString());
										res = false;
									}
								}

							} 
							else 
							{
								res = false;
							}

							break;
						}
							

						case ACTIONS.RetrieveMessage:
						{
							if (model.mesnums.Keys.Count > 0) 
							{
								lock(model.mesnums.SyncRoot) 
								{
									int num = ran.Next(model.mesnums.Keys.Count);
									try 
									{
										int coun = 0;
										foreach (object o in model.mesnums.Keys) 
										{
											if (coun == num) 
											{
												num = (int)o;
												break;
											}
											coun++;
										}

										start = DateTime.Now;
										p.WSEmailRetrieve(num);
										stop = DateTime.Now;
										Log("Retrieved message #" + num.ToString());
										res = true;
									} 
									catch 
									{
										Log("Unable to retrieve message " + num.ToString());
										res = false;
									}
								}

							} 
							else 
							{
								res = false;
							}

							break;

						}
						case ACTIONS.SendMessage : 
						{
							WSEmailMessage m = model.messages.Get();
							m.Timestamp = DateTime.Now;
		
							try 
							{
								start = DateTime.Now;
								WSEmailStatus status = p.WSEmailSend(m,null);
								stop = DateTime.Now;
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
						case ACTIONS.SendIM:
						{
							add = false;
							WSEmailMessage m = model.messages.Get();
							m.Timestamp = DateTime.Now;
							m.Subject = DateTime.Now.Ticks.ToString();
							m.MessageFlags = WSEmailProxy.WSEmailFlags.InstantMessaging.SendAsInstantMessage;
		
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

					Log("Finished action.");
					if (add) 
					{
						//Log("Added results to set.");
						TimeSpan time = stop.Subtract(start);
						TestResult t = new TestResult();
						t.Action = a;
						t.Successful = res;
						t.Token = tok.ToString();
						t.Latency = time.TotalMilliseconds;

						if (model.PerfRec)
							model.PerfLatency.RawValue = (long)(time.TotalMilliseconds/1000.0000);

						model.AddResult(t);
					
						if (res) 
						{
							model.AddSuccess();
						}
						else 
						{
							model.AddFailure();
						}
					}
				}

				model.Log("Thread done");
				return;
			} 
			catch (Exception ex) 
			{
				model.Log(ex.Message);
				model.Log(ex.StackTrace);
			}
		}
		
		
	}
}
