using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Http;
using System.Reflection;
using EventQueue;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace GUIDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	

	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblMailA;
		private System.Windows.Forms.Label lblMailB;
		private System.Windows.Forms.Label lblRouter;
		private System.Windows.Forms.Label lblMailARouter;
		private System.Windows.Forms.Label lblClientA;
		private System.Windows.Forms.Label lblClientB;
		private Image imgArrow = Image.FromFile("arrow.gif");
		private Image imgArrow45 = Image.FromFile("arrow45.gif");
		private System.Windows.Forms.Timer timer;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label popup;
		private int intCurrSpot = 0;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnStartStop;
		private System.Windows.Forms.TextBox txtDelay;
		private System.Windows.Forms.TrackBar trackBar;
		private ArrayList events = new ArrayList();
		private Thread bufferThread;
		private MessageBuffer theBuffer;
		private System.Windows.Forms.Button btnClear;
		private HttpChannel myChannel = null;
		private System.Windows.Forms.Label lblQueue;
		private System.Windows.Forms.Label lbMailBRouter;
		private System.Windows.Forms.Label lblMailAClientA;
		private System.Windows.Forms.Label lbMailBClientB;
		private System.Windows.Forms.Label lblMailAQueue;
		private System.Windows.Forms.Label lblMailBQueue;
		private System.Windows.Forms.Label lblQueueRouter;
		private string blarg = "";

/*		private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
		private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

		[DllImport("user32.dll")]
		private static extern bool DestroyWindow(IntPtr window);

		[DllImport("user32.dll")]
		private static extern void mouse_event(
			UInt32 dwFlags,         // motion and click options
			UInt32 dx,              // horizontal position or change
			UInt32 dy,              // vertical position or change
			UInt32 dwData,          // wheel movement
			IntPtr dwExtraInfo  // application-defined information
			);

		public static void SendClick(Point location)
		{
			Cursor.Position = location;
			mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
			mouse_event(MOUSEEVENTF_LEFTUP,   0, 0, 0, new System.IntPtr());
		}

*/		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			

			Type theType = typeof(EventItem.RecordingEntities);
			EventItem i = new EventItem();

			
			//EventItem ei = new EventItem("hello",
			/*
			events.Add(new EventItem("now",(EventItem.RecordingEntities)Enum.Parse(typeof(EventItem.RecordingEntities),"MailB",false),"Received message","BUH!"));
			events.Add(new EventItem("then",EventItem.RecordingEntities.ClientA,"Received message","BUH!"));
			events.Add(new EventItem("later",EventItem.RecordingEntities.MailA,"Received message","BUH!"));
			events.Add(new EventItem("huh",EventItem.RecordingEntities.MailB,"Received message","BUH!"));
			events.Add(new EventItem("hehe",EventItem.RecordingEntities.ClientA,"Received message","BUH!"));
			events.Add(new EventItem("nhoho",EventItem.RecordingEntities.Router,"Received message","BUH!"));
			events.Add(new EventItem("nhoho",EventItem.RecordingEntities.ClientB,"Received message","BUH!"));
			*/
			txtDelay.Text = timer.Interval.ToString();
			trackBar.Maximum = events.Count - 1;
			theBuffer = new MessageBuffer();
			bufferThread = new Thread(new ThreadStart(ProcessMessages));
			bufferThread.Start();
			myChannel = new HttpChannel(8787);
			ChannelServices.RegisterChannel(myChannel);
			RemotingServices.Marshal(theBuffer,"LogSink");
			// timer.Enabled = true;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (bufferThread != null)
					bufferThread.Abort();
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void ShowArrow(Label toShow, int degree) 
		{
			if (degree == -1) 
			{
				toShow.Image = null;
				return;
			}

			Image i = null;
			if (degree >= 360) 
			{
				int jj = degree / 360;
				degree = degree - 360 * jj;
			}

			if (degree % 90 == 0) 
			{
				i = (Image)imgArrow.Clone();
				switch (Math.Abs(degree)) 
				{
					case 0:
						i.RotateFlip(RotateFlipType.RotateNoneFlipX);
						break;
					case 90:
						i.RotateFlip(RotateFlipType.Rotate90FlipX);
						break;
					case 180:
						break;
					case 270:
						i.RotateFlip(RotateFlipType.Rotate90FlipY);
						break;
				}

			}
			else 
			{
				i = (Image)imgArrow45.Clone();
				switch (Math.Abs(degree)) 
				{
					case 45:
						i.RotateFlip(RotateFlipType.Rotate270FlipXY);
						break;
					case 135:
						break;
					case 225:
						i.RotateFlip(RotateFlipType.RotateNoneFlipY);
						break;
					case 315:
						i.RotateFlip(RotateFlipType.Rotate90FlipY);
						break;
				}

			}
			toShow.Image = i;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.lblMailA = new System.Windows.Forms.Label();
			this.lblMailB = new System.Windows.Forms.Label();
			this.lblRouter = new System.Windows.Forms.Label();
			this.lblMailARouter = new System.Windows.Forms.Label();
			this.lbMailBRouter = new System.Windows.Forms.Label();
			this.lblClientA = new System.Windows.Forms.Label();
			this.lblClientB = new System.Windows.Forms.Label();
			this.lblMailAClientA = new System.Windows.Forms.Label();
			this.lbMailBClientB = new System.Windows.Forms.Label();
			this.trackBar = new System.Windows.Forms.TrackBar();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.popup = new System.Windows.Forms.Label();
			this.txtDelay = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnStartStop = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.lblQueue = new System.Windows.Forms.Label();
			this.lblMailAQueue = new System.Windows.Forms.Label();
			this.lblMailBQueue = new System.Windows.Forms.Label();
			this.lblQueueRouter = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// lblMailA
			// 
			this.lblMailA.BackColor = System.Drawing.Color.Black;
			this.lblMailA.ForeColor = System.Drawing.Color.White;
			this.lblMailA.Location = new System.Drawing.Point(160, 208);
			this.lblMailA.Name = "lblMailA";
			this.lblMailA.Size = new System.Drawing.Size(72, 64);
			this.lblMailA.TabIndex = 0;
			this.lblMailA.Text = "MailServerA";
			this.lblMailA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMailB
			// 
			this.lblMailB.BackColor = System.Drawing.Color.Black;
			this.lblMailB.ForeColor = System.Drawing.Color.White;
			this.lblMailB.Location = new System.Drawing.Point(432, 208);
			this.lblMailB.Name = "lblMailB";
			this.lblMailB.Size = new System.Drawing.Size(72, 64);
			this.lblMailB.TabIndex = 1;
			this.lblMailB.Text = "MailServerB";
			this.lblMailB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblRouter
			// 
			this.lblRouter.BackColor = System.Drawing.Color.Black;
			this.lblRouter.ForeColor = System.Drawing.Color.White;
			this.lblRouter.Location = new System.Drawing.Point(296, 96);
			this.lblRouter.Name = "lblRouter";
			this.lblRouter.Size = new System.Drawing.Size(72, 64);
			this.lblRouter.TabIndex = 2;
			this.lblRouter.Text = "Router";
			this.lblRouter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMailARouter
			// 
			this.lblMailARouter.Location = new System.Drawing.Point(232, 152);
			this.lblMailARouter.Name = "lblMailARouter";
			this.lblMailARouter.Size = new System.Drawing.Size(64, 48);
			this.lblMailARouter.TabIndex = 3;
			// 
			// lbMailBRouter
			// 
			this.lbMailBRouter.Location = new System.Drawing.Point(368, 144);
			this.lbMailBRouter.Name = "lbMailBRouter";
			this.lbMailBRouter.Size = new System.Drawing.Size(80, 64);
			this.lbMailBRouter.TabIndex = 4;
			// 
			// lblClientA
			// 
			this.lblClientA.BackColor = System.Drawing.Color.Black;
			this.lblClientA.ForeColor = System.Drawing.Color.White;
			this.lblClientA.Location = new System.Drawing.Point(8, 208);
			this.lblClientA.Name = "lblClientA";
			this.lblClientA.Size = new System.Drawing.Size(72, 64);
			this.lblClientA.TabIndex = 5;
			this.lblClientA.Text = "ClientA";
			this.lblClientA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblClientB
			// 
			this.lblClientB.BackColor = System.Drawing.Color.Black;
			this.lblClientB.ForeColor = System.Drawing.Color.White;
			this.lblClientB.Location = new System.Drawing.Point(584, 208);
			this.lblClientB.Name = "lblClientB";
			this.lblClientB.Size = new System.Drawing.Size(72, 64);
			this.lblClientB.TabIndex = 6;
			this.lblClientB.Text = "ClientB";
			this.lblClientB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMailAClientA
			// 
			this.lblMailAClientA.Location = new System.Drawing.Point(80, 208);
			this.lblMailAClientA.Name = "lblMailAClientA";
			this.lblMailAClientA.Size = new System.Drawing.Size(80, 64);
			this.lblMailAClientA.TabIndex = 7;
			// 
			// lbMailBClientB
			// 
			this.lbMailBClientB.Location = new System.Drawing.Point(504, 208);
			this.lbMailBClientB.Name = "lbMailBClientB";
			this.lbMailBClientB.Size = new System.Drawing.Size(80, 64);
			this.lbMailBClientB.TabIndex = 8;
			// 
			// trackBar
			// 
			this.trackBar.Location = new System.Drawing.Point(8, 432);
			this.trackBar.Name = "trackBar";
			this.trackBar.Size = new System.Drawing.Size(664, 42);
			this.trackBar.TabIndex = 9;
			this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
			// 
			// timer
			// 
			this.timer.Interval = 1000;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// popup
			// 
			this.popup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.popup.Location = new System.Drawing.Point(32, 24);
			this.popup.Name = "popup";
			this.popup.Size = new System.Drawing.Size(88, 72);
			this.popup.TabIndex = 11;
			this.popup.Visible = false;
			this.popup.Click += new System.EventHandler(this.popup_Click);
			// 
			// txtDelay
			// 
			this.txtDelay.Location = new System.Drawing.Point(536, 400);
			this.txtDelay.Name = "txtDelay";
			this.txtDelay.Size = new System.Drawing.Size(120, 20);
			this.txtDelay.TabIndex = 12;
			this.txtDelay.Text = "textBox1";
			this.txtDelay.TextChanged += new System.EventHandler(this.txtDelay_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(416, 392);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 32);
			this.label1.TabIndex = 13;
			this.label1.Text = "Millisecond delay between events:";
			// 
			// btnStartStop
			// 
			this.btnStartStop.Location = new System.Drawing.Point(16, 384);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(88, 32);
			this.btnStartStop.TabIndex = 14;
			this.btnStartStop.Text = "Start...";
			this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(120, 384);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(88, 32);
			this.btnClear.TabIndex = 15;
			this.btnClear.Text = "Clear...";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// lblQueue
			// 
			this.lblQueue.BackColor = System.Drawing.Color.Black;
			this.lblQueue.ForeColor = System.Drawing.Color.White;
			this.lblQueue.Location = new System.Drawing.Point(296, 210);
			this.lblQueue.Name = "lblQueue";
			this.lblQueue.Size = new System.Drawing.Size(72, 64);
			this.lblQueue.TabIndex = 16;
			this.lblQueue.Text = "MailQueue";
			this.lblQueue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMailAQueue
			// 
			this.lblMailAQueue.Location = new System.Drawing.Point(232, 216);
			this.lblMailAQueue.Name = "lblMailAQueue";
			this.lblMailAQueue.Size = new System.Drawing.Size(64, 56);
			this.lblMailAQueue.TabIndex = 17;
			// 
			// lblMailBQueue
			// 
			this.lblMailBQueue.Location = new System.Drawing.Point(376, 216);
			this.lblMailBQueue.Name = "lblMailBQueue";
			this.lblMailBQueue.Size = new System.Drawing.Size(56, 56);
			this.lblMailBQueue.TabIndex = 18;
			// 
			// lblQueueRouter
			// 
			this.lblQueueRouter.Location = new System.Drawing.Point(312, 168);
			this.lblQueueRouter.Name = "lblQueueRouter";
			this.lblQueueRouter.Size = new System.Drawing.Size(40, 40);
			this.lblQueueRouter.TabIndex = 19;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(680, 485);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.popup,
																		  this.lblQueueRouter,
																		  this.lblQueue,
																		  this.btnClear,
																		  this.btnStartStop,
																		  this.label1,
																		  this.txtDelay,
																		  this.trackBar,
																		  this.lblClientB,
																		  this.lblClientA,
																		  this.lblMailB,
																		  this.lblMailA,
																		  this.lblRouter,
																		  this.lblMailBQueue,
																		  this.lblMailAQueue,
																		  this.lbMailBClientB,
																		  this.lblMailAClientA,
																		  this.lblMailARouter,
																		  this.lbMailBRouter});
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			lblRouter.ContextMenu.Show(lblRouter,new Point(0,0));
		
		}

		private void ProcessEvent(EventItem e) 
		{
//			if (lastMenu != null) 
//			{
//				bool h = DestroyWindow(lastMenu.Handle);
//				Console.WriteLine(h.ToString());
//			}
			popup.Hide();
			
				//lastMenu.MenuItems.Clear();
				//fsSendClick(new Point(500,500));
			Label l = null;
			string field = "lbl" + Enum.GetName(typeof(EventItem.RecordingEntities),e.RecordingEntity);
//			MessageBox.Show("Finding.. " + field); 
			System.Reflection.FieldInfo inf =this.GetType().GetField(field,
				System.Reflection.BindingFlags.NonPublic |
				System.Reflection.BindingFlags.Instance);
			object o=inf.GetValue(this);

			if (o != null)
				l = (Label)o;
			else
				return;

/*
			switch (e.RecordingEntity) 
			{
				case EventItem.RecordingEntities.Queue:
					l = lblQueue;
					break;
				case EventItem.RecordingEntities.Router :
					l = lblRouter;
					break;
				case EventItem.RecordingEntities.ClientA :
				
					l = lblClientA;
					break;
				case EventItem.RecordingEntities.ClientB :
					l = lblClientB;
					break;
				case EventItem.RecordingEntities.MailServerA :
					l = lblMailA;
					break;
				case EventItem.RecordingEntities.MailServerB :
					l = lblMailB;
					break;
			}
*/
			// assume transit
//			if (e.ShortDescription.Equals()) {

//			if (l.ContextMenu == null) 
//				l.ContextMenu = new ContextMenu();
			
			if (l == null) 
			{
				Console.WriteLine("Wussing out.. the type was: " + Enum.GetName(typeof(EventItem.RecordingEntities),e.RecordingEntity));
				return;
			}

			field = field.ToLower();
			if (e is TransitItem) 
			{
				TransitItem ti = (TransitItem)e;
				if (ti.Action == TransitItem.Actions.Erase) 
				{
					//MessageBox.Show("Erasing...");
					ShowArrow(l,-1);
					return;
				}

				int degree = 0;
				if (ti.Action == TransitItem.Actions.To)
					degree = 0;
				else
					degree = 180;

				if (ti.RecordingEntity == EventItem.RecordingEntities.MailAQueue || ti.RecordingEntity == EventItem.RecordingEntities.MailBQueue) 
					degree += 180;
				else if (ti.RecordingEntity == EventItem.RecordingEntities.QueueRouter)
					degree += 90;

				MessageBox.Show("Degree = " + degree.ToString()+", " + ti.ShortDescription);
				if (ti.Action == TransitItem.Actions.To) 
				{
					ShowArrow(l,degree);
					//MessageBox.Show("It's a TO action");
				}
				else if (ti.Action == TransitItem.Actions.From) 
				{
						ShowArrow(l,degree);
					//MessageBox.Show("It's a FROM action");
				}
			} 
			else 
			{
				// popup.Location = new Point(l.Location.X - 50, l.Location.Y - 50);
				Console.WriteLine("This.width = " + this.Width.ToString());
				Console.WriteLine("new X = " + (l.Location.X + l.Width + 100));
				if (l.Location.X + l.Width + 100 > this.Width) 
				{
					popup.Location = new Point(l.Location.X - l.Width + 20, l.Location.Y - l.Height - 20);
				
					Console.WriteLine("it's greater.");
				}
				else 
				{
					popup.Location = new Point(l.Location.X + l.Width - 20, l.Location.Y - l.Height - 20);
				}

				popup.Size = new Size(100,100);
				popup.Text = e.ShortDescription;
				popup.BackColor = Color.Wheat;
				blarg = e.Data;
				popup.Show();
			}
			
//			this.Controls.Add(popup);

//			l.ContextMenu.MenuItems.Clear();
//			l.ContextMenu.MenuItems.Add(new MenuItem(e.ShortDescription));
//			l.ContextMenu.Show(l,new Point(0,0));
//			lastMenu = l.ContextMenu;
		}

		private void timer_Tick(object sender, System.EventArgs f)
		{
			if (events.Count != 0) 
			{
				trackBar.Value = intCurrSpot;
				EventItem e = (EventItem)events[intCurrSpot++];
				Console.WriteLine("Processing event...");
				
				if (intCurrSpot == events.Count)
					intCurrSpot = 0;
				
				ProcessEvent(e);
			}
		}

		private void btnStartStop_Click(object sender, System.EventArgs e)
		{
			if (timer.Enabled == true) 
			{
				timer.Enabled = false;
				btnStartStop.Text = "Start...";
			}
			else 
			{
				timer.Enabled = true;
				btnStartStop.Text = "Stop...";
			}
		}

		private void txtDelay_TextChanged(object sender, System.EventArgs e)
		{
			int i = 0;
			try 
			{
				i = int.Parse(txtDelay.Text);
				timer.Interval = i;
			} 
			catch 
			{
				txtDelay.Text = timer.Interval.ToString();
			}

		}

		private void trackBar_Scroll(object sender, System.EventArgs e)
		{

		
		}

		private void ProcessMessages() 
		{
			while (true) 
			{
				EventItem m = null;
				// this will block (hopefully) on the message buffer's mutex
				m = theBuffer.getMessage();
				Console.WriteLine("Added new event to the buffer.");
				events.Add(m);
				trackBar.Maximum = events.Count - 1;
			}
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			timer.Enabled = false;
			events.Clear();
			intCurrSpot = 0;
			trackBar.Maximum = 0;
		}

		private void popup_Click(object sender, System.EventArgs e)
		{
			timer.Enabled = false;
			MessageBox.Show(blarg);		
			timer.Enabled = true;
		}

	}
}
