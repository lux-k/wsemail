/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;


namespace TestApp
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class Form2 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox weights;
		private System.Windows.Forms.TextBox email;
		private System.Windows.Forms.ListBox listBox5;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ListBox listBox4;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ListBox listBox3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Button update;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox noofemails;
		private System.Windows.Forms.RadioButton nooftimes;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Button quit;
		private System.Windows.Forms.RadioButton x509;
		private System.Windows.Forms.RadioButton usernametoken;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.GroupBox authentication;
		private System.Windows.Forms.RadioButton randomchoice;
		private System.Windows.Forms.TextBox sizeOfMessage;
		private System.Windows.Forms.TextBox sizeOfSubject;
		private System.Windows.Forms.Button updateXml;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form2()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.sizeOfSubject = new System.Windows.Forms.TextBox();
			this.sizeOfMessage = new System.Windows.Forms.TextBox();
			this.weights = new System.Windows.Forms.TextBox();
			this.email = new System.Windows.Forms.TextBox();
			this.listBox5 = new System.Windows.Forms.ListBox();
			this.label10 = new System.Windows.Forms.Label();
			this.listBox4 = new System.Windows.Forms.ListBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.listBox3 = new System.Windows.Forms.ListBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.update = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.noofemails = new System.Windows.Forms.TextBox();
			this.nooftimes = new System.Windows.Forms.RadioButton();
			this.label11 = new System.Windows.Forms.Label();
			this.quit = new System.Windows.Forms.Button();
			this.updateXml = new System.Windows.Forms.Button();
			this.authentication = new System.Windows.Forms.GroupBox();
			this.randomchoice = new System.Windows.Forms.RadioButton();
			this.usernametoken = new System.Windows.Forms.RadioButton();
			this.x509 = new System.Windows.Forms.RadioButton();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.authentication.SuspendLayout();
			this.SuspendLayout();
			// 
			// sizeOfSubject
			// 
			this.sizeOfSubject.Location = new System.Drawing.Point(344, 56);
			this.sizeOfSubject.Name = "sizeOfSubject";
			this.sizeOfSubject.Size = new System.Drawing.Size(72, 20);
			this.sizeOfSubject.TabIndex = 3;
			this.sizeOfSubject.Text = "";
			// 
			// sizeOfMessage
			// 
			this.sizeOfMessage.Location = new System.Drawing.Point(440, 56);
			this.sizeOfMessage.Name = "sizeOfMessage";
			this.sizeOfMessage.Size = new System.Drawing.Size(56, 20);
			this.sizeOfMessage.TabIndex = 4;
			this.sizeOfMessage.Text = "";
			// 
			// weights
			// 
			this.weights.Location = new System.Drawing.Point(248, 56);
			this.weights.Name = "weights";
			this.weights.Size = new System.Drawing.Size(64, 20);
			this.weights.TabIndex = 2;
			this.weights.Text = "";
			// 
			// email
			// 
			this.email.Location = new System.Drawing.Point(8, 56);
			this.email.Name = "email";
			this.email.Size = new System.Drawing.Size(208, 20);
			this.email.TabIndex = 1;
			this.email.Text = "";
			// 
			// listBox5
			// 
			this.listBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox5.Location = new System.Drawing.Point(368, 136);
			this.listBox5.Name = "listBox5";
			this.listBox5.Size = new System.Drawing.Size(40, 117);
			this.listBox5.TabIndex = 44;
			this.listBox5.TabStop = false;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(368, 112);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(64, 16);
			this.label10.TabIndex = 43;
			this.label10.Text = "No. of times";
			// 
			// listBox4
			// 
			this.listBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox4.Location = new System.Drawing.Point(296, 136);
			this.listBox4.Name = "listBox4";
			this.listBox4.Size = new System.Drawing.Size(56, 117);
			this.listBox4.TabIndex = 36;
			this.listBox4.TabStop = false;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(200, 112);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(80, 16);
			this.label9.TabIndex = 42;
			this.label9.Text = "Size of Subject";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(336, 32);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(80, 16);
			this.label8.TabIndex = 41;
			this.label8.Text = "Size of Subject";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(280, 112);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(100, 16);
			this.label7.TabIndex = 40;
			this.label7.Text = "Size of Message";
			// 
			// listBox3
			// 
			this.listBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox3.Location = new System.Drawing.Point(216, 136);
			this.listBox3.Name = "listBox3";
			this.listBox3.Size = new System.Drawing.Size(64, 117);
			this.listBox3.TabIndex = 35;
			this.listBox3.TabStop = false;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(432, 32);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 16);
			this.label6.TabIndex = 39;
			this.label6.Text = "Size of Message";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(152, 112);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 16);
			this.label5.TabIndex = 38;
			this.label5.Text = "Weights";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 16);
			this.label4.TabIndex = 37;
			this.label4.Text = "Updated Email id";
			// 
			// listBox2
			// 
			this.listBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox2.Location = new System.Drawing.Point(168, 136);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(32, 117);
			this.listBox2.TabIndex = 33;
			this.listBox2.TabStop = false;
			// 
			// update
			// 
			this.update.Location = new System.Drawing.Point(440, 168);
			this.update.Name = "update";
			this.update.Size = new System.Drawing.Size(88, 32);
			this.update.TabIndex = 6;
			this.update.Text = "Update";
			this.update.Click += new System.EventHandler(this.update_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(256, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 27;
			this.label3.Text = "Weights";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 26;
			this.label2.Text = "E-Mail Id";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(208, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(152, 16);
			this.label1.TabIndex = 25;
			this.label1.Text = "Test Bed WSEmail";
			// 
			// noofemails
			// 
			this.noofemails.Location = new System.Drawing.Point(160, 288);
			this.noofemails.Name = "noofemails";
			this.noofemails.Size = new System.Drawing.Size(40, 20);
			this.noofemails.TabIndex = 99;
			this.noofemails.Text = "";
			// 
			// nooftimes
			// 
			this.nooftimes.Location = new System.Drawing.Point(280, 288);
			this.nooftimes.Name = "nooftimes";
			this.nooftimes.Size = new System.Drawing.Size(168, 24);
			this.nooftimes.TabIndex = 100;
			this.nooftimes.TabStop = true;
			this.nooftimes.Text = "Calculate number of times";
			this.nooftimes.CheckedChanged += new System.EventHandler(this.nooftimes_CheckedChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 288);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(152, 24);
			this.label11.TabIndex = 49;
			this.label11.Text = "Total no. of Email to be send";
			// 
			// quit
			// 
			this.quit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.quit.Location = new System.Drawing.Point(544, 288);
			this.quit.Name = "quit";
			this.quit.Size = new System.Drawing.Size(75, 32);
			this.quit.TabIndex = 102;
			this.quit.Text = "Quit";
			this.quit.Click += new System.EventHandler(this.quit_Click);
			// 
			// updateXml
			// 
			this.updateXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.updateXml.Location = new System.Drawing.Point(456, 288);
			this.updateXml.Name = "updateXml";
			this.updateXml.Size = new System.Drawing.Size(75, 32);
			this.updateXml.TabIndex = 101;
			this.updateXml.Text = "Update XML";
			this.updateXml.Click += new System.EventHandler(this.updateXml_Click);
			// 
			// authentication
			// 
			this.authentication.Controls.Add(this.randomchoice);
			this.authentication.Controls.Add(this.usernametoken);
			this.authentication.Controls.Add(this.x509);
			this.authentication.Location = new System.Drawing.Point(536, 16);
			this.authentication.Name = "authentication";
			this.authentication.Size = new System.Drawing.Size(128, 120);
			this.authentication.TabIndex = 5;
			this.authentication.TabStop = false;
			this.authentication.Text = "Authentication";
			// 
			// randomchoice
			// 
			this.randomchoice.Location = new System.Drawing.Point(16, 88);
			this.randomchoice.Name = "randomchoice";
			this.randomchoice.TabIndex = 2;
			this.randomchoice.Text = "Random choice";
			// 
			// usernametoken
			// 
			this.usernametoken.Location = new System.Drawing.Point(16, 56);
			this.usernametoken.Name = "usernametoken";
			this.usernametoken.TabIndex = 1;
			this.usernametoken.Text = "Username Token ";
			// 
			// x509
			// 
			this.x509.Checked = true;
			this.x509.Location = new System.Drawing.Point(16, 24);
			this.x509.Name = "x509";
			this.x509.TabIndex = 0;
			this.x509.TabStop = true;
			this.x509.Text = "X 509";
			// 
			// listBox1
			// 
			this.listBox1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox1.Location = new System.Drawing.Point(8, 136);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(144, 117);
			this.listBox1.TabIndex = 34;
			this.listBox1.TabStop = false;
			// 
			// Form2
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(672, 333);
			this.Controls.Add(this.authentication);
			this.Controls.Add(this.noofemails);
			this.Controls.Add(this.nooftimes);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.quit);
			this.Controls.Add(this.updateXml);
			this.Controls.Add(this.sizeOfSubject);
			this.Controls.Add(this.sizeOfMessage);
			this.Controls.Add(this.weights);
			this.Controls.Add(this.email);
			this.Controls.Add(this.listBox5);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.listBox4);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.listBox3);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.listBox2);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.update);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "Form2";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Xml Configurator";
			this.authentication.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void update_Click(object sender, System.EventArgs e)
		{
			try
			{ 
				int.Parse(weights.Text);
				//start updating the first listbox
				listBox1.BeginUpdate();
				listBox1.Items.Add(email.Text);
				listBox1.EndUpdate();
				email.Text="";
				
				//updating the second listbox
				listBox2.BeginUpdate();
				listBox2.Items.Add(weights.Text);
				listBox2.EndUpdate();
				weights.Text="";
			
				//updating the third listbox
				listBox3.BeginUpdate();
				listBox3.Items.Add(sizeOfSubject.Text);
				listBox3.EndUpdate();
				sizeOfSubject.Text="";	
				
				//updating the fourth listbox
				listBox4.BeginUpdate();
				listBox4.Items.Add(sizeOfMessage.Text);
				listBox4.EndUpdate();
				sizeOfMessage.Text="";
				
				
				
			}

		catch (System.Exception caught)
				{ 
				weights.Text = caught.Message;
				weights.BackColor = Color.Red;
				MessageBox.Show(caught.StackTrace);
				}

		}

		private void quit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void nooftimes_CheckedChanged(object sender, System.EventArgs e)
		{
			//string[] emailid = new string[100];
			string[] weight = new string[100];
			//string[] sizeOfMessage = new string[100];
			//string[] sizeOfSubject = new string[100];
			
			int totalWeight=0;
			

			//passing the value to your function
			for (int i=0;i<(listBox1.Items.Count);i++)
			{
				//listBox1.SetSelected(i, true);
				listBox2.SetSelected(i,true);
				//listBox3.SetSelected(i,true);
				//listBox4.SetSelected(i,true);
				
				
				//emailid[i] = listBox1.SelectedItems[0].ToString();
				
				weight[i]= listBox2.SelectedItems[0].ToString();
				/*sizeOfSubject[i] = listBox3.SelectedItems[0].ToString();
				sizeOfMessage[i] = listBox4.SelectedItems[0].ToString();
				*/
				totalWeight+=int.Parse(weight[i]);
				
					
			}

			int no = 0;
			
			for (int i=0;i<(listBox1.Items.Count);i++)
			{
				//start updating listbox no. 5
				listBox5.BeginUpdate();
				no = (int)(Math.Round(((decimal.Parse(weight[i]))/(decimal.Parse(totalWeight.ToString())))*(decimal.Parse(noofemails.Text))));
				listBox5.Items.Add(no.ToString());
				listBox5.EndUpdate();
			}

		}

		private void updateXml_Click(object sender, System.EventArgs e)
		{
			
			System.Xml.XmlDocument xs = new System.Xml.XmlDocument();
			xs.Load("TestApp.exe.config");
				
			//security tokens update	
			XmlNodeList l = xs.GetElementsByTagName("SecurityTokens");
			if (l.Count > 0) 
			{
				foreach (XmlNode child in l[0].ChildNodes)
				{
						
					if (child.Attributes["name"].Value=="X509" && x509.Checked)
					{
						child.Attributes["weight"].Value = "1";
						xs.Save("TestApp.exe.config");
					} 
					else if (child.Attributes["name"].Value == "Password" && usernametoken.Checked)
					{	
						child.Attributes["weight"].Value = "1";
						xs.Save("TestApp.exe.config");
					}
					else 
					{	
						Random r = new Random();	
						child.Attributes["weight"].Value = r.Next(0,2).ToString(); 
					}
				}
			}
			
			//message update

			XmlNodeList m = xs.GetElementsByTagName("Messages");
			
			for (int i=0;i<(listBox1.Items.Count)-1;i++)
			{
				m[0].AppendChild(m[0].FirstChild.CloneNode(true));
			}	
			
			if (m.Count > 0) 
			{	
				int noOfMessages=0;
				foreach (XmlNode child in m[noOfMessages].ChildNodes)
				{	
					child.Attributes["weight"].Value = valueListBox(listBox2,noOfMessages);
					xs.Save("TestApp.exe.config");

					foreach (XmlNode grandchild in child.ChildNodes)
					{
						switch (grandchild.LocalName)
						{
							case "Recipients" :
							{	
								string [] emailList = new string[10];
								string emailsGroup = "";	
								foreach (XmlNode greatgrandchild in grandchild.ChildNodes)
								{	
									emailsGroup = (valueListBox(listBox1,noOfMessages));							
									emailList = emailsGroup.Split(',');
									greatgrandchild.InnerText = emailList[0].ToString(); 
								}

									
								for (int i=1;i<emailList.Length;i++)
								{
									XmlNode n = xs.CreateNode(XmlNodeType.Element,"Recipient",xs.NamespaceURI);
									grandchild.AppendChild(n);
									
									n.InnerText = emailList[i].ToString(); 
								}
								xs.Save("TestApp.exe.config");
								break;
							}

							case "Subject" :
							{
								grandchild.Attributes["size"].Value = valueListBox(listBox3,noOfMessages);
								xs.Save("TestApp.exe.config");
								break;
							}

							case "Body" :
							{
								grandchild.Attributes["size"].Value = valueListBox(listBox4,noOfMessages);
								xs.Save("TestApp.exe.config");
								break;
							}							
						}	
					}
				noOfMessages++;
				}
			
			}
		}

		
		private string valueListBox(ListBox l,int i)
		{
			l.SetSelected(i,true);
			return (l.SelectedItems[0].ToString());
		}

		

		
	}
}
