using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;


namespace TestApplication
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
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
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
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(200, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(152, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Test Bed WSEmail";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "E-Mail Id";
			// 
			// send
			// 
			this.send.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.send.Location = new System.Drawing.Point(464, 336);
			this.send.Name = "send";
			this.send.TabIndex = 8;
			this.send.Text = "Send";
			this.send.Click += new System.EventHandler(this.send_Click);
			// 
			// email
			// 
			this.email.Location = new System.Drawing.Point(8, 64);
			this.email.Name = "email";
			this.email.Size = new System.Drawing.Size(208, 20);
			this.email.TabIndex = 1;
			this.email.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(200, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(144, 23);
			this.label3.TabIndex = 1;
			this.label3.Text = "Weights";
			// 
			// weights
			// 
			this.weights.Location = new System.Drawing.Point(224, 64);
			this.weights.Name = "weights";
			this.weights.Size = new System.Drawing.Size(64, 20);
			this.weights.TabIndex = 2;
			this.weights.Text = "";
			// 
			// update
			// 
			this.update.Location = new System.Drawing.Point(528, 80);
			this.update.Name = "update";
			this.update.Size = new System.Drawing.Size(88, 40);
			this.update.TabIndex = 5;
			this.update.Text = "Update";
			this.update.Click += new System.EventHandler(this.update_Click);
			// 
			// quit
			// 
			this.quit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.quit.Location = new System.Drawing.Point(552, 336);
			this.quit.Name = "quit";
			this.quit.TabIndex = 9;
			this.quit.Text = "Quit";
			this.quit.Click += new System.EventHandler(this.quit_Click);
			// 
			// listBox1
			// 
			this.listBox1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox1.Location = new System.Drawing.Point(8, 192);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(144, 117);
			this.listBox1.TabIndex = 8;
			this.listBox1.TabStop = false;
			// 
			// listBox2
			// 
			this.listBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox2.Location = new System.Drawing.Point(160, 192);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(32, 117);
			this.listBox2.TabIndex = 8;
			this.listBox2.TabStop = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 168);
			this.label4.Name = "label4";
			this.label4.TabIndex = 11;
			this.label4.Text = "Updated Email id";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(144, 160);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 24);
			this.label5.TabIndex = 12;
			this.label5.Text = "Weights";
			// 
			// message
			// 
			this.message.Location = new System.Drawing.Point(8, 112);
			this.message.Name = "message";
			this.message.Size = new System.Drawing.Size(504, 20);
			this.message.TabIndex = 4;
			this.message.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 88);
			this.label6.Name = "label6";
			this.label6.TabIndex = 14;
			this.label6.Text = "Message";
			// 
			// listBox3
			// 
			this.listBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox3.Location = new System.Drawing.Point(200, 192);
			this.listBox3.Name = "listBox3";
			this.listBox3.Size = new System.Drawing.Size(104, 117);
			this.listBox3.TabIndex = 8;
			this.listBox3.TabStop = false;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(328, 160);
			this.label7.Name = "label7";
			this.label7.TabIndex = 16;
			this.label7.Text = "Message";
			// 
			// subject
			// 
			this.subject.Location = new System.Drawing.Point(312, 64);
			this.subject.Name = "subject";
			this.subject.Size = new System.Drawing.Size(200, 20);
			this.subject.TabIndex = 3;
			this.subject.Text = "";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(312, 40);
			this.label8.Name = "label8";
			this.label8.TabIndex = 18;
			this.label8.Text = "Subject";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(200, 160);
			this.label9.Name = "label9";
			this.label9.TabIndex = 19;
			this.label9.Text = "Subject";
			// 
			// listBox4
			// 
			this.listBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox4.Location = new System.Drawing.Point(320, 192);
			this.listBox4.Name = "listBox4";
			this.listBox4.Size = new System.Drawing.Size(272, 117);
			this.listBox4.TabIndex = 9;
			this.listBox4.TabStop = false;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(584, 160);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(64, 23);
			this.label10.TabIndex = 20;
			this.label10.Text = "No. of times";
			// 
			// listBox5
			// 
			this.listBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox5.Location = new System.Drawing.Point(600, 192);
			this.listBox5.Name = "listBox5";
			this.listBox5.Size = new System.Drawing.Size(40, 117);
			this.listBox5.TabIndex = 21;
			this.listBox5.TabStop = false;
			// 
			// noofemails
			// 
			this.noofemails.Location = new System.Drawing.Point(168, 336);
			this.noofemails.Name = "noofemails";
			this.noofemails.Size = new System.Drawing.Size(40, 20);
			this.noofemails.TabIndex = 6;
			this.noofemails.Text = "";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(16, 336);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(152, 24);
			this.label11.TabIndex = 23;
			this.label11.Text = "Total no. of Email to be send";
			// 
			// nooftimes
			// 
			this.nooftimes.Location = new System.Drawing.Point(288, 336);
			this.nooftimes.Name = "nooftimes";
			this.nooftimes.Size = new System.Drawing.Size(168, 24);
			this.nooftimes.TabIndex = 7;
			this.nooftimes.TabStop = true;
			this.nooftimes.Text = "Calculate number of times";
			this.nooftimes.CheckedChanged += new System.EventHandler(this.nooftimes_CheckedChanged);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(648, 374);
			this.Controls.Add(this.nooftimes);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.noofemails);
			this.Controls.Add(this.listBox5);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.listBox4);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.subject);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.listBox3);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.message);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.listBox2);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.quit);
			this.Controls.Add(this.update);
			this.Controls.Add(this.weights);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.email);
			this.Controls.Add(this.send);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "TestBed v1.0";
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

		private void quit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void send_Click(object sender, System.EventArgs e)
		{
			
			
		
		}

		private void nooftimes_CheckedChanged(object sender, System.EventArgs e)
		{   

			//MessageBox.Show((listBox1.Items.Count).ToString(),"value");
			
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
	}
}
