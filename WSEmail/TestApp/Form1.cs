/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

//using Excel;
using System.Reflection; 
using System;
using System.Diagnostics;
using System.Configuration;
using System.Drawing;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using TestAppLibs;

namespace TestApp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button send;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button update;
		private System.Windows.Forms.Button quit;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.TextBox email;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ListBox listBox3;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox message;
		private System.Windows.Forms.TextBox subject;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ListBox listBox4;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ListBox listBox5;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.RadioButton nooftimes;
		private System.Windows.Forms.TextBox noofemails;
		//private int totalWeight=0;
		//private string[] weight = new string[100];
		private System.Windows.Forms.TextBox weights;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.DataGrid dgMessages;
		public MailMessage[] Messages;
		private TcpChannel chan =null;
		private Controller _c = new Controller();
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.TextBox txtSlaves;
		private System.Windows.Forms.Label lblSlaves;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox txtThreads;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox txtCycles;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.Button btnGraph;
		private System.Windows.Forms.TextBox txtVar;
		private System.Windows.Forms.TextBox txtAvg;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Button btnGen;
		private System.Windows.Forms.TextBox txtMax;
		private System.Windows.Forms.TextBox txtMin;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.Button btnBroadcast;
//		private ModelObjects m = new ModelObjects();
		private UDPMulticastSender umcs = null;
		private int remotingPort = 9999;
		private string remotingName = "Config";
		private System.Windows.Forms.ToolTip toolTip;
		private string serverUrl = "";

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			InitializeRemotingStuff();
			this.Text = "WSEmail Testbed Server, v" + Application.ProductVersion;
			if (dgMessages.TableStyles.Count == 0)
				dgMessages.TableStyles.Add(GetDataGridTableStyle(_c.Results));
			umcs = new UDPMulticastSender();
			serverUrl = "tcp://" + System.Net.Dns.GetHostName() + ":" + remotingPort.ToString() + "/"+remotingName;
			//MessageBox.Show(serverUrl);
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void UpdateSlaveCount(object o, EventArgs e) 
		{
			this.Invoke(new EventHandler(SetSlaveToolTip));
			this.txtSlaves.Text = _c.SlaveCount.ToString();
		}

		private void SetSlaveToolTip(object o, EventArgs e) 
		{
			this.toolTip.SetToolTip(this.txtSlaves, _c.Slaves);
		}

		private void InitializeRemotingStuff() 
		{
			_c.SlaveCountChange += new EventHandler(this.UpdateSlaveCount);
			_c.ResultsReceived += new EventHandler(this.UpdateResults);

			_c.Cycles = int.Parse(ConfigurationSettings.AppSettings["Cycles"]);
			_c.Threads = int.Parse(ConfigurationSettings.AppSettings["Threads"]);

			txtCycles.Text = _c.Cycles.ToString();
			txtThreads.Text = _c.Threads.ToString();

			BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
			serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

			IDictionary props = new Hashtable();
			props["port"] = remotingPort;
			
			chan = new TcpChannel (props, null, serverProv);        
			ChannelServices.RegisterChannel ( chan );
						
			RemotingServices.Marshal(_c,remotingName);
			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			RemotingServices.Disconnect(_c);
			
//			if (m != null)
//				m.Dispose();

			if (chan != null) 
			{
				ChannelServices.UnregisterChannel( chan ) ;
				chan = null;
			}

			if( disposing )
			{
				if (components != null) 
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
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.send = new System.Windows.Forms.Button();
			this.email = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.weights = new System.Windows.Forms.TextBox();
			this.update = new System.Windows.Forms.Button();
			this.quit = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.message = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.listBox3 = new System.Windows.Forms.ListBox();
			this.label7 = new System.Windows.Forms.Label();
			this.subject = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.listBox4 = new System.Windows.Forms.ListBox();
			this.label10 = new System.Windows.Forms.Label();
			this.listBox5 = new System.Windows.Forms.ListBox();
			this.noofemails = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.nooftimes = new System.Windows.Forms.RadioButton();
			this.dgMessages = new System.Windows.Forms.DataGrid();
			this.btnStart = new System.Windows.Forms.Button();
			this.txtSlaves = new System.Windows.Forms.TextBox();
			this.lblSlaves = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.txtThreads = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.txtCycles = new System.Windows.Forms.TextBox();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.btnGraph = new System.Windows.Forms.Button();
			this.txtVar = new System.Windows.Forms.TextBox();
			this.txtAvg = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.btnGen = new System.Windows.Forms.Button();
			this.txtMax = new System.Windows.Forms.TextBox();
			this.txtMin = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.btnBroadcast = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.dgMessages)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(360, 208);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(152, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Test Bed WSEmail";
			this.label1.Visible = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(184, 240);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "E-Mail Id";
			this.label2.Visible = false;
			// 
			// send
			// 
			this.send.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.send.Location = new System.Drawing.Point(632, 536);
			this.send.Name = "send";
			this.send.Size = new System.Drawing.Size(75, 32);
			this.send.TabIndex = 8;
			this.send.Text = "Send";
			this.send.Visible = false;
			this.send.Click += new System.EventHandler(this.send_Click);
			// 
			// email
			// 
			this.email.Location = new System.Drawing.Point(176, 264);
			this.email.Name = "email";
			this.email.Size = new System.Drawing.Size(208, 20);
			this.email.TabIndex = 1;
			this.email.Text = "";
			this.email.Visible = false;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(368, 240);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(144, 23);
			this.label3.TabIndex = 1;
			this.label3.Text = "Weights";
			this.label3.Visible = false;
			// 
			// weights
			// 
			this.weights.Location = new System.Drawing.Point(392, 264);
			this.weights.Name = "weights";
			this.weights.Size = new System.Drawing.Size(64, 20);
			this.weights.TabIndex = 2;
			this.weights.Text = "";
			this.weights.Visible = false;
			// 
			// update
			// 
			this.update.Location = new System.Drawing.Point(696, 280);
			this.update.Name = "update";
			this.update.Size = new System.Drawing.Size(88, 40);
			this.update.TabIndex = 5;
			this.update.Text = "Update";
			this.update.Visible = false;
			this.update.Click += new System.EventHandler(this.update_Click);
			// 
			// quit
			// 
			this.quit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.quit.Location = new System.Drawing.Point(720, 536);
			this.quit.Name = "quit";
			this.quit.Size = new System.Drawing.Size(75, 32);
			this.quit.TabIndex = 9;
			this.quit.Text = "Quit";
			this.quit.Visible = false;
			this.quit.Click += new System.EventHandler(this.quit_Click);
			// 
			// listBox1
			// 
			this.listBox1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox1.Location = new System.Drawing.Point(176, 392);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(144, 117);
			this.listBox1.TabIndex = 8;
			this.listBox1.TabStop = false;
			this.listBox1.Visible = false;
			// 
			// listBox2
			// 
			this.listBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox2.Location = new System.Drawing.Point(328, 392);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(32, 117);
			this.listBox2.TabIndex = 8;
			this.listBox2.TabStop = false;
			this.listBox2.Visible = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(176, 368);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 32);
			this.label4.TabIndex = 11;
			this.label4.Text = "Updated Email id";
			this.label4.Visible = false;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(312, 360);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 24);
			this.label5.TabIndex = 12;
			this.label5.Text = "Weights";
			this.label5.Visible = false;
			// 
			// message
			// 
			this.message.Location = new System.Drawing.Point(176, 312);
			this.message.Name = "message";
			this.message.Size = new System.Drawing.Size(504, 20);
			this.message.TabIndex = 4;
			this.message.Text = "";
			this.message.Visible = false;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(176, 288);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 32);
			this.label6.TabIndex = 14;
			this.label6.Text = "Message";
			this.label6.Visible = false;
			// 
			// listBox3
			// 
			this.listBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox3.Location = new System.Drawing.Point(368, 392);
			this.listBox3.Name = "listBox3";
			this.listBox3.Size = new System.Drawing.Size(104, 117);
			this.listBox3.TabIndex = 8;
			this.listBox3.TabStop = false;
			this.listBox3.Visible = false;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(496, 360);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(100, 32);
			this.label7.TabIndex = 16;
			this.label7.Text = "Message";
			this.label7.Visible = false;
			// 
			// subject
			// 
			this.subject.Location = new System.Drawing.Point(480, 264);
			this.subject.Name = "subject";
			this.subject.Size = new System.Drawing.Size(200, 20);
			this.subject.TabIndex = 3;
			this.subject.Text = "";
			this.subject.Visible = false;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(480, 240);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 32);
			this.label8.TabIndex = 18;
			this.label8.Text = "Subject";
			this.label8.Visible = false;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(368, 360);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100, 32);
			this.label9.TabIndex = 19;
			this.label9.Text = "Subject";
			this.label9.Visible = false;
			// 
			// listBox4
			// 
			this.listBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox4.Location = new System.Drawing.Point(488, 392);
			this.listBox4.Name = "listBox4";
			this.listBox4.Size = new System.Drawing.Size(272, 117);
			this.listBox4.TabIndex = 9;
			this.listBox4.TabStop = false;
			this.listBox4.Visible = false;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(752, 360);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(64, 23);
			this.label10.TabIndex = 20;
			this.label10.Text = "No. of times";
			this.label10.Visible = false;
			// 
			// listBox5
			// 
			this.listBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox5.Location = new System.Drawing.Point(768, 392);
			this.listBox5.Name = "listBox5";
			this.listBox5.Size = new System.Drawing.Size(40, 117);
			this.listBox5.TabIndex = 21;
			this.listBox5.TabStop = false;
			this.listBox5.Visible = false;
			// 
			// noofemails
			// 
			this.noofemails.Location = new System.Drawing.Point(336, 536);
			this.noofemails.Name = "noofemails";
			this.noofemails.Size = new System.Drawing.Size(40, 20);
			this.noofemails.TabIndex = 6;
			this.noofemails.Text = "";
			this.noofemails.Visible = false;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(184, 536);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(152, 24);
			this.label11.TabIndex = 23;
			this.label11.Text = "Total no. of Email to be send";
			this.label11.Visible = false;
			// 
			// nooftimes
			// 
			this.nooftimes.Location = new System.Drawing.Point(456, 536);
			this.nooftimes.Name = "nooftimes";
			this.nooftimes.Size = new System.Drawing.Size(168, 24);
			this.nooftimes.TabIndex = 7;
			this.nooftimes.TabStop = true;
			this.nooftimes.Text = "Calculate number of times";
			this.nooftimes.Visible = false;
			this.nooftimes.CheckedChanged += new System.EventHandler(this.nooftimes_CheckedChanged);
			// 
			// dgMessages
			// 
			this.dgMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dgMessages.CaptionText = "Results:";
			this.dgMessages.DataMember = "";
			this.dgMessages.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgMessages.Location = new System.Drawing.Point(176, 8);
			this.dgMessages.Name = "dgMessages";
			this.dgMessages.Size = new System.Drawing.Size(648, 376);
			this.dgMessages.TabIndex = 24;
			this.dgMessages.Navigate += new System.Windows.Forms.NavigateEventHandler(this.dgMessages_Navigate);
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(24, 152);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(104, 40);
			this.btnStart.TabIndex = 25;
			this.btnStart.Text = "Start the Test";
			this.btnStart.Click += new System.EventHandler(this.button1_Click);
			// 
			// txtSlaves
			// 
			this.txtSlaves.Location = new System.Drawing.Point(120, 72);
			this.txtSlaves.Name = "txtSlaves";
			this.txtSlaves.Size = new System.Drawing.Size(40, 20);
			this.txtSlaves.TabIndex = 27;
			this.txtSlaves.Text = "0";
			// 
			// lblSlaves
			// 
			this.lblSlaves.Location = new System.Drawing.Point(8, 72);
			this.lblSlaves.Name = "lblSlaves";
			this.lblSlaves.Size = new System.Drawing.Size(104, 16);
			this.lblSlaves.TabIndex = 28;
			this.lblSlaves.Text = "Slaves Connected:";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(8, 96);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(104, 16);
			this.label12.TabIndex = 30;
			this.label12.Text = "Threads:";
			// 
			// txtThreads
			// 
			this.txtThreads.Location = new System.Drawing.Point(120, 96);
			this.txtThreads.Name = "txtThreads";
			this.txtThreads.Size = new System.Drawing.Size(40, 20);
			this.txtThreads.TabIndex = 29;
			this.txtThreads.Text = "0";
			this.toolTip.SetToolTip(this.txtThreads, "# of client to simulate");
			this.txtThreads.TextChanged += new System.EventHandler(this.txtThreads_TextChanged);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 120);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(104, 16);
			this.label13.TabIndex = 32;
			this.label13.Text = "Cycles:";
			// 
			// txtCycles
			// 
			this.txtCycles.Location = new System.Drawing.Point(120, 120);
			this.txtCycles.Name = "txtCycles";
			this.txtCycles.Size = new System.Drawing.Size(40, 20);
			this.txtCycles.TabIndex = 31;
			this.txtCycles.Text = "0";
			this.toolTip.SetToolTip(this.txtCycles, "How many actions to perform on each simulated client");
			this.txtCycles.TextChanged += new System.EventHandler(this.txtCycles_TextChanged);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem3});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem5,
																					  this.menuItem2});
			this.menuItem1.Text = "File";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 0;
			this.menuItem5.Text = "Clear";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "Exit";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem4});
			this.menuItem3.Text = "Configuration";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 0;
			this.menuItem4.Text = "Change...";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// btnGraph
			// 
			this.btnGraph.Location = new System.Drawing.Point(536, 120);
			this.btnGraph.Name = "btnGraph";
			this.btnGraph.Size = new System.Drawing.Size(104, 40);
			this.btnGraph.TabIndex = 33;
			this.btnGraph.Text = "Graph";
			this.btnGraph.Visible = false;
			this.btnGraph.Click += new System.EventHandler(this.btnGraph_Click);
			// 
			// txtVar
			// 
			this.txtVar.Location = new System.Drawing.Point(96, 256);
			this.txtVar.Name = "txtVar";
			this.txtVar.Size = new System.Drawing.Size(64, 20);
			this.txtVar.TabIndex = 36;
			this.txtVar.Text = "";
			// 
			// txtAvg
			// 
			this.txtAvg.AcceptsReturn = true;
			this.txtAvg.Location = new System.Drawing.Point(96, 232);
			this.txtAvg.Name = "txtAvg";
			this.txtAvg.Size = new System.Drawing.Size(64, 20);
			this.txtAvg.TabIndex = 34;
			this.txtAvg.Text = "";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(8, 256);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(72, 16);
			this.label15.TabIndex = 37;
			this.label15.Text = "Variance:";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(8, 232);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(72, 16);
			this.label16.TabIndex = 35;
			this.label16.Text = "Average:";
			// 
			// btnGen
			// 
			this.btnGen.Location = new System.Drawing.Point(32, 344);
			this.btnGen.Name = "btnGen";
			this.btnGen.Size = new System.Drawing.Size(104, 40);
			this.btnGen.TabIndex = 38;
			this.btnGen.Text = "Gen Stats";
			this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
			// 
			// txtMax
			// 
			this.txtMax.Location = new System.Drawing.Point(96, 304);
			this.txtMax.Name = "txtMax";
			this.txtMax.Size = new System.Drawing.Size(64, 20);
			this.txtMax.TabIndex = 41;
			this.txtMax.Text = "";
			// 
			// txtMin
			// 
			this.txtMin.AcceptsReturn = true;
			this.txtMin.Location = new System.Drawing.Point(96, 280);
			this.txtMin.Name = "txtMin";
			this.txtMin.Size = new System.Drawing.Size(64, 20);
			this.txtMin.TabIndex = 39;
			this.txtMin.Text = "";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(8, 304);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(72, 22);
			this.label14.TabIndex = 42;
			this.label14.Text = "Maximum:";
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(8, 280);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(72, 22);
			this.label17.TabIndex = 40;
			this.label17.Text = "Minimum:";
			// 
			// btnBroadcast
			// 
			this.btnBroadcast.Location = new System.Drawing.Point(32, 16);
			this.btnBroadcast.Name = "btnBroadcast";
			this.btnBroadcast.Size = new System.Drawing.Size(104, 40);
			this.btnBroadcast.TabIndex = 43;
			this.btnBroadcast.Text = "Broadcast Link Request";
			this.btnBroadcast.Click += new System.EventHandler(this.btnBroadcast_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(832, 393);
			this.Controls.Add(this.btnBroadcast);
			this.Controls.Add(this.txtMax);
			this.Controls.Add(this.txtMin);
			this.Controls.Add(this.txtVar);
			this.Controls.Add(this.txtAvg);
			this.Controls.Add(this.txtCycles);
			this.Controls.Add(this.txtThreads);
			this.Controls.Add(this.txtSlaves);
			this.Controls.Add(this.noofemails);
			this.Controls.Add(this.subject);
			this.Controls.Add(this.message);
			this.Controls.Add(this.weights);
			this.Controls.Add(this.email);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.btnGen);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.btnGraph);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.lblSlaves);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.dgMessages);
			this.Controls.Add(this.nooftimes);
			this.Controls.Add(this.label11);
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
			this.Controls.Add(this.quit);
			this.Controls.Add(this.update);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.send);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "WSEmail Testbed Server!!!";
			((System.ComponentModel.ISupportInitialize)(this.dgMessages)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			System.Windows.Forms.Application.Run(new Form1());
		}

		private void update_Click(object sender, System.EventArgs e)
		{
			{
				ArrayList a = new ArrayList();
				if (Messages != null)
					a.AddRange(Messages);

				a.Add(new MailMessage(email.Text,int.Parse(weights.Text),subject.Text,message.Text));
				Messages = (MailMessage[])a.ToArray(typeof(MailMessage));

				if (dgMessages.TableStyles.Count == 0)
					dgMessages.TableStyles.Add(GetDataGridTableStyle(Messages));
				
				dgMessages.SetDataBinding(Messages, null);
			}
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
				listBox3.Items.Add(subject.Text);
				listBox3.EndUpdate();
				subject.Text="";	
				//updating the fourth listbox
				listBox4.BeginUpdate();
				listBox4.Items.Add(message.Text);
				listBox4.EndUpdate();
				message.Text="";
			}
			catch (System.Exception caught)
			{ 
				weights.Text = caught.Message;
				weights.BackColor = Color.Red;
			}




		}

		private void UpdateResultsLocally(object o, EventArgs e) 
		{
			try 
			{
				dgMessages.SetDataBinding(_c.Results, null);
			} 
			catch (Exception ex) 
			{
				MessageBox.Show(ex.Message + ex.StackTrace);
			}
		}

		private void UpdateResults(Object o, EventArgs e) 
		{
			this.Invoke(new EventHandler(this.UpdateResultsLocally));
		}

		private void quit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		
		private void send_Click(object sender, System.EventArgs e)
		{
			
			//Form1 testAppgui = new Form1();

            
//			m.actions = (Actions)ConfigurationSettings.GetConfig("Actions");
//			m.securitytokens = (SecurityTokens)ConfigurationSettings.GetConfig("SecurityTokens");

			ArrayList a = new ArrayList();
			int total = 0;
			for (int i = 0; i < listBox1.Items.Count; i++) 
			{
				a.Add(new MailMessage((string)listBox1.Items[i],int.Parse(listBox5.Items[i].ToString()),(string)listBox3.Items[i],(string)listBox4.Items[i]));
				total += int.Parse(listBox5.Items[i].ToString());
			}
			//m.messages = (MailMessage[])a.ToArray(typeof(MailMessage));
//			m.total = total;

//			m.Run();
					
		}

		private void nooftimes_CheckedChanged(object sender, System.EventArgs e)
		{   

					
			string[] emailid = new string[100];
			string[] weight = new string[100];
			string[] message = new string[100];
			string[] subject = new string[100];
			int totalWeight=0;
			

			//passing the value to your function
			for (int i=0;i<(listBox1.Items.Count);i++)
			{
				listBox1.SetSelected(i, true);
				listBox2.SetSelected(i,true);
				listBox3.SetSelected(i,true);
				listBox4.SetSelected(i,true);
				
				
				emailid[i] = listBox1.SelectedItems[0].ToString();
				weight[i]= listBox2.SelectedItems[0].ToString();
				subject[i] = listBox3.SelectedItems[0].ToString();
				message[i] = listBox4.SelectedItems[0].ToString();
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


		private DataGridTableStyle GetDataGridTableStyle(object o) 
		{
			DataGridTableStyle gs = new DataGridTableStyle();
			gs.MappingName = o.GetType().Name;
			
			DataGridTextBoxColumn cs = new DataGridTextBoxColumn();
			cs.MappingName = "Action";
			cs.HeaderText = "Action";
			cs.Width = 110;
			cs.ReadOnly = true;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Successful";
			cs.HeaderText = "Successful?";
			cs.Width = 80;
			cs.ReadOnly = true;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);


			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Token";
			cs.HeaderText = "Token";
			cs.Width = 260;
			cs.ReadOnly = true;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Latency";
			cs.HeaderText = "Latency";	
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 70;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			return gs;
		}

		private DataGridTableStyle GetDataGridTableStyleOLD(object o) 
		{
			DataGridTableStyle gs = new DataGridTableStyle();
			gs.MappingName = this.Messages.GetType().Name;
			
			DataGridTextBoxColumn cs = new DataGridTextBoxColumn();
			cs.MappingName = "Email";
			cs.HeaderText = "Email";
			cs.Width = 120;
			cs.ReadOnly = true;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Weight";
			cs.HeaderText = "Weight";	
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 60;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Subject";
			cs.HeaderText = "Subject";
			cs.Width = 160;
			cs.Alignment = HorizontalAlignment.Right;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Body";
			cs.HeaderText = "Body";
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 180;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);


			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Number";
			cs.HeaderText = "Number";
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 250;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			return gs;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			_c.StartTest();
		}

		private void txtThreads_TextChanged(object sender, System.EventArgs e)
		{
			try 
			{
				_c.Threads = int.Parse(txtThreads.Text);
			} 
			catch {}
		}

		private void txtCycles_TextChanged(object sender, System.EventArgs e)
		{
			try 
			{
				_c.Cycles = int.Parse(txtCycles.Text);
			} 
			catch {}
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void dgMessages_Navigate(object sender, System.Windows.Forms.NavigateEventArgs ne)
		{
		
		}

		private void btnGraph_Click(object sender, System.EventArgs e)
		{
/*
			Excel.ApplicationClass xlApp=new Excel.ApplicationClass();
			Excel.Workbook wb = xlApp.Workbooks.Add(Type.Missing);
			Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets.Add(Type.Missing,Type.Missing,Type.Missing,Type.Missing);

			ws.Cells[1,1] = "Run";
			ws.Cells[2,1] = "X509 Latency";
			ws.Cells[3,1] = "Username Latency";


			for (int i = 2; i < 10; i++) 
			{
				ws.Cells[i,2] = i;
				ws.Cells[i,1] = i;
				ws.Cells[i,3] = i;

			}

			Excel.Chart chart = (Excel.Chart)wb.Charts.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value );

			//Use the ChartWizard to create a new chart from the selected data.
			Excel.Range range = ws.get_Range("B1","C10");

			chart.ChartWizard(range, Excel.XlChartType.xl3DColumn, Missing.Value,
				Excel.XlRowCol.xlColumns, Missing.Value, Missing.Value, Missing.Value, 
				"Latencies for X509 and Username Tokens", Missing.Value, Missing.Value, Missing.Value );
			chart.Visible = Excel.XlSheetVisibility.xlSheetVisible;
			Excel.SeriesCollection seriesCollection=(Excel.SeriesCollection)chart.SeriesCollection(Type.Missing);
			seriesCollection.Item(1).Name = "X509 Latency";
			seriesCollection.Item(2).Name = "Username Latency";

			xlApp.Visible = true;
*/
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			Form2 f = new Form2();
			f.ShowDialog();
			f.Dispose();
		}

		private void btnGen_Click(object sender, System.EventArgs e)
		{
			double avg = 0;
			double var = 0;
			double tot = 0;
			double min = double.MaxValue;
			double max = double.MinValue;
			int results = _c.Results.Length;

			foreach (TestResult t in _c.Results) 
			{
				if (t.Latency != 0) 
				{
					tot += t.Latency;

					if (t.Latency < min)
						min = t.Latency;

					if (t.Latency > max)
						max = t.Latency;
				} 
				else 
				{
					results--;
				}
			}

			avg = (tot / 1000) / results;

			LogD("len = " + _c.Results.Length.ToString());

			foreach (TestResult t in _c.Results) 
			{
				if (t.Latency != 0) 
				{
					var += Math.Pow(( (t.Latency / 1000) - avg),2);
					LogD("var in loop = " + var.ToString());
				}
			}
			LogD("var before divide= " + var.ToString());
			var = var / results;
			LogD("var after divide = " + var.ToString());
			min /= 1000.0;
			max /= 1000.0;
			this.txtVar.Text = var.ToString();
			this.txtAvg.Text = avg.ToString();
			this.txtMin.Text = min.ToString();
			this.txtMax.Text = max.ToString();
		}

		private void LogD(string s) 
		{
			//txtDbg.Text += s + "\r\n";
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			_c.Results = null;
			UpdateResultsLocally(null,EventArgs.Empty);

		}

		private void btnBroadcast_Click(object sender, System.EventArgs e)
		{
			umcs.Send(serverUrl);
		}
	}

}
