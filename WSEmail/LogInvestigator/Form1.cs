/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.Configuration;
using PennLibraries;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace LogInvestigator
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private ArrayList searchEntities = new ArrayList(), searchTypes = new ArrayList();
		private CheckBox[] entityChecks = null, typeChecks = null;
		private SqlConnection conn = null;
		private System.Windows.Forms.GroupBox gbLogTypes;
		private System.Windows.Forms.GroupBox gbEntities;
		private System.Windows.Forms.TextBox txtQuery;
		private System.Windows.Forms.DataGrid logGrid;
		private System.Windows.Forms.Button btnExecute;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuClear;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.ContextMenu cnxMinMaxMenu;
		private System.Windows.Forms.MenuItem mnuSetMax;
		private System.Windows.Forms.MenuItem mnuSetMin;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		DateTime min = new DateTime(2000,1,1), max = DateTime.MaxValue;
		private System.Windows.Forms.MenuItem mnuResetDate;
		private FrmView frmView = null;

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.logGrid = new System.Windows.Forms.DataGrid();
			this.cnxMinMaxMenu = new System.Windows.Forms.ContextMenu();
			this.mnuSetMax = new System.Windows.Forms.MenuItem();
			this.mnuSetMin = new System.Windows.Forms.MenuItem();
			this.gbLogTypes = new System.Windows.Forms.GroupBox();
			this.gbEntities = new System.Windows.Forms.GroupBox();
			this.txtQuery = new System.Windows.Forms.TextBox();
			this.btnExecute = new System.Windows.Forms.Button();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuResetDate = new System.Windows.Forms.MenuItem();
			this.mnuClear = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.logGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// logGrid
			// 
			this.logGrid.CaptionVisible = false;
			this.logGrid.ContextMenu = this.cnxMinMaxMenu;
			this.logGrid.DataMember = "";
			this.logGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.logGrid.Name = "logGrid";
			this.logGrid.ReadOnly = true;
			this.logGrid.RowHeadersVisible = false;
			this.logGrid.RowHeaderWidth = 0;
			this.logGrid.Size = new System.Drawing.Size(576, 204);
			this.logGrid.TabIndex = 0;
			this.logGrid.CurrentCellChanged += new System.EventHandler(this.logGrid_CurrentCellChanged);
			// 
			// cnxMinMaxMenu
			// 
			this.cnxMinMaxMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuSetMax,
																						  this.mnuSetMin});
			// 
			// mnuSetMax
			// 
			this.mnuSetMax.Index = 0;
			this.mnuSetMax.Text = "Set Max Date";
			this.mnuSetMax.Click += new System.EventHandler(this.mnuSetMax_Click);
			// 
			// mnuSetMin
			// 
			this.mnuSetMin.Index = 1;
			this.mnuSetMin.Text = "Set Min Date";
			this.mnuSetMin.Click += new System.EventHandler(this.mnuSetMin_Click);
			// 
			// gbLogTypes
			// 
			this.gbLogTypes.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.gbLogTypes.Location = new System.Drawing.Point(0, 292);
			this.gbLogTypes.Name = "gbLogTypes";
			this.gbLogTypes.Size = new System.Drawing.Size(544, 88);
			this.gbLogTypes.TabIndex = 1;
			this.gbLogTypes.TabStop = false;
			this.gbLogTypes.Text = "Log Types";
			// 
			// gbEntities
			// 
			this.gbEntities.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.gbEntities.Location = new System.Drawing.Point(0, 212);
			this.gbEntities.Name = "gbEntities";
			this.gbEntities.Size = new System.Drawing.Size(544, 72);
			this.gbEntities.TabIndex = 2;
			this.gbEntities.TabStop = false;
			this.gbEntities.Text = "Entities";
			// 
			// txtQuery
			// 
			this.txtQuery.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.txtQuery.Location = new System.Drawing.Point(0, 388);
			this.txtQuery.Multiline = true;
			this.txtQuery.Name = "txtQuery";
			this.txtQuery.Size = new System.Drawing.Size(440, 64);
			this.txtQuery.TabIndex = 3;
			this.txtQuery.Text = "";
			// 
			// btnExecute
			// 
			this.btnExecute.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnExecute.Location = new System.Drawing.Point(456, 412);
			this.btnExecute.Name = "btnExecute";
			this.btnExecute.TabIndex = 4;
			this.btnExecute.Text = "Execute";
			this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuResetDate,
																					  this.mnuClear,
																					  this.mnuExit});
			this.menuItem1.Text = "File";
			// 
			// mnuResetDate
			// 
			this.mnuResetDate.Index = 0;
			this.mnuResetDate.Text = "Reset Date Ranges";
			this.mnuResetDate.Click += new System.EventHandler(this.mnuResetDate_Click);
			// 
			// mnuClear
			// 
			this.mnuClear.Index = 1;
			this.mnuClear.Text = "Clear Logs";
			this.mnuClear.Click += new System.EventHandler(this.mnuClear_Click);
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 2;
			this.mnuExit.Text = "Exit";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 497);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnExecute,
																		  this.txtQuery,
																		  this.gbEntities,
																		  this.gbLogTypes,
																		  this.logGrid});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "WS Email Log Viewer";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.logGrid)).EndInit();
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

		private void Form1_Load(object sender, System.EventArgs e)
		{

			object o = ConfigurationSettings.GetConfig("Database");
			if (o is DatabaseConfiguration)
				conn = new SqlConnection(((DatabaseConfiguration)o).connstr);

			refresh();


		}

		private void logGrid_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point pt = new Point(e.X, e.Y); 
			DataGrid.HitTestInfo hti = logGrid.HitTest(pt); 
			if(hti.Type == DataGrid.HitTestType.Cell) 
			{ 
				logGrid.Select(hti.Row); 
			}
		}

		private void refresh() 
		{
			logGrid.Anchor = AnchorStyles.None;
			string[] entities = getEntities();
			drawGroupBox(ref entityChecks, gbEntities,logGrid.Height + 10,entities,new EventHandler(entitiesChanged));
			string[] names = Enum.GetNames(typeof(LogType));
			Array.Sort(names);
			drawGroupBox(ref typeChecks, gbLogTypes,gbEntities.Top + gbEntities.Height + 10,names,new EventHandler(logTypesChanged));

			txtQuery.Top = gbLogTypes.Top + gbLogTypes.Height + 10;
			

			this.ClientSize = new Size(this.ClientSize.Width, this.txtQuery.Top + this.txtQuery.Height + 10);
			logGrid.Top = 0;
			gbEntities.Top = logGrid.Top + logGrid.Height + 10;
			gbLogTypes.Top = gbEntities.Top + gbEntities.Height + 10;
			btnExecute.Top = gbLogTypes.Top + gbLogTypes.Height + 20;
			txtQuery.Top = gbLogTypes.Top + gbLogTypes.Height + 10;
			logGrid.Width = gbEntities.Width;
//			viewPanel.Left = logGrid.Width + 10;
			txtQuery.Width = gbEntities.Width - 100;
			btnExecute.Left = txtQuery.Left + txtQuery.Width + 10;
			btnExecute.Top =  txtQuery.Top + ( (txtQuery.Top + txtQuery.Height) - txtQuery.Top ) / 2 - btnExecute.Height / 2;

			this.logGrid.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			regenerateQuery();
		}

		private string[] getEntities() 
		{
			conn.Open();
			SqlCommand comm = conn.CreateCommand();
			comm.CommandText = "select distinct Entity from Logs order by Entity asc";
			SqlDataReader r = comm.ExecuteReader();
			ArrayList a = new ArrayList();
			while (r.Read()) 
			{
				a.Add(r.GetString(0).Trim());
			}

			r.Close();
			conn.Close();
			return (string[])a.ToArray(typeof(string));
		}

		private void drawGroupBox(ref CheckBox[] arr, GroupBox g, int offset, string[] names, EventHandler eventhandler) 
		{
			int col = 0;
			int row = 0;
			string [] newnames  = new string[names.Length+1];
			newnames[0] = "<all>";
			Array.Copy(names,0,newnames,1,names.Length);
			names = newnames;
			ArrayList thelist = new ArrayList();
			CheckBox c = null;
			int usewidth = 170;
			foreach (string name in names) 
			{
				c = new CheckBox();
				c.Text = name;
				if (name.Equals("<all>"))
					c.Checked = true;
				else
					c.Checked = false;
				c.Left = 10 + col++ * 200;
				c.Width = usewidth;
				c.Top = 20 + row * 20;
				if (col == 3) 
				{
					col = 0;
					row++;
				}
				c.CheckedChanged += eventhandler;
				g.Controls.Add(c);
				thelist.Add(c);
			}
			g.Top = offset;
			if (row == 0)
				row = 1;
			g.Height = c.Top + c.Height + 10;
			g.Width = 10 + 2 * 200 + usewidth + 10;
			//this.Width = gbLogTypes.Width + 10;
			arr = (CheckBox[])thelist.ToArray(typeof(CheckBox));
		}

		private void regenerateQuery() 
		{
			bool where = false;
			string query = "select DateRecorded, Entity, LogType, Message, XmlMessage from Logs";
			if (searchEntities.Count > 0) 
			{
				query += " where (";
				where = true;
				bool one = false;
				foreach (string e in (string[])searchEntities.ToArray(typeof(string))) 
				{
					if (!e.Equals("<all>")) 
					{
						if (one)
							query += " OR ";
						query += "Entity = '" + e + "' ";
						if (!one)
							one = true;
					}
				}
				query += ") ";
			}
			if (searchTypes.Count > 0) 
			{
				if (!where)   
				{
					query += " where (";
					where = true;
				}
				else
					query += " and (";
				bool one = false;
				foreach (string e in (string[])searchTypes.ToArray(typeof(string))) 
				{
					if (!e.Equals("<all>")) 
					{

						if (one)
							query += " OR ";
						LogType t = (LogType)Enum.Parse(typeof(LogType),e);
						int i = (int)t.GetType().GetField(e).GetValue(t);
						query += "LogType = " + i.ToString() + " " ;
						if (!one)
							one = true;
					}
				}
				query += ") ";

			}

			{
				if (!where) 
				{
					query += " where (";
					where = true;
				} 
				else 
					query += " and (";

				query += "DateRecorded > '" + min.ToString()+ "') ";
			}

			{
				if (!where) 
				{
					query += " where (";
					where = true;
				} 
				else 
					query += " and (";

				query += "DateRecorded < '" + max.ToString()+ "') ";
			}
				txtQuery.Text = query + " order by LogID desc";
		}

		private void entitiesChanged(object sender, System.EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			if (c.Text.Equals("<all>")) 
			{
				if (c.Checked) 
				{
					clearList(ref entityChecks);
					searchEntities.Clear();
				}
				else
					c.Checked = false;
			} 
			else 
			{
				if (entityChecks[0].Checked)
					entityChecks[0].Checked = false;

				if (c.Checked)
					searchEntities.Add(c.Text);
				else
					searchEntities.Remove(c.Text);

				if (searchEntities.Count == 0)
					entityChecks[0].Checked = true;
			}
			regenerateQuery();		
		}

		private void clearList(ref CheckBox[] checks) 
		{
			foreach (CheckBox c in checks) 
			{
				if (c.Checked == true && !c.Text.Equals("<all>")) 
				{
					c.Checked = !c.Checked;
				}
			}
		}

		private void logTypesChanged(object sender, System.EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			if (c.Text.Equals("<all>")) 
			{
				if (c.Checked) 
				{
					clearList(ref typeChecks);
					searchTypes.Clear();
				}
				else
					typeChecks[0].Checked = false;
			} 
			else 
			{
				if (typeChecks[0].Checked)
					typeChecks[0].Checked = false;
				if (c.Checked)
					searchTypes.Add(c.Text);
				else
					searchTypes.Remove(c.Text);
				if (searchTypes.Count == 0)
					typeChecks[0].Checked = true;
			}
			regenerateQuery();
		}

		public DataGridTableStyle getDataGridStyle(object o) 
		{
			DataGridTableStyle gs = new DataGridTableStyle();
			gs.MappingName = "Logs";
			gs.ReadOnly = true;

			DataGridTextBoxColumn cs = new DataGridTextBoxColumn();
			cs.MappingName = "DateRecorded";
			cs.HeaderText = "Date";
			cs.Width = 120;
			cs.Format = "G";
			cs.ReadOnly = true;
//			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Entity";
			cs.HeaderText = "Entity";	
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 90;
			cs.ReadOnly = true;
//			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "LogType";
			cs.HeaderText = "Type";
			cs.Width = 30;
			cs.Alignment = HorizontalAlignment.Right;
			cs.ReadOnly = true;
//			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Message";
			cs.HeaderText = "Message";
			cs.Alignment = HorizontalAlignment.Left;
			cs.Width = 180;
			cs.Width = logGrid.Width - 130 - 100 - 10;
			cs.ReadOnly = true;
//			cs.ReadOnly = true;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "XmlMessage";
			cs.HeaderText = "XmlMessage";
			cs.Alignment = HorizontalAlignment.Left;
			cs.Width = 0;
			//cs.Width = logGrid.Width - 130 - 100 - 10;
//			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);

			/*
			 * cs = new DataGridTextBoxColumn();
			cs.MappingName = "Contents";
			cs.HeaderText = "Contents";
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 250;
			cs.TextBox.Enabled = false;
			gs.GridColumnStyles.Add(cs);
			*/

			return gs;
		}

		private void btnExecute_Click(object sender, System.EventArgs e)
		{
			DataSet ds = new DataSet(); 
			SqlDataAdapter adapter = new SqlDataAdapter(); 
			adapter.SelectCommand = new SqlCommand(txtQuery.Text, conn); 
			
			adapter.Fill(ds,"Logs");
			if (logGrid.TableStyles.Count == 0)
				logGrid.TableStyles.Add(getDataGridStyle(ds));
			logGrid.SetDataBinding(ds,"Logs");
		}



		private void logGrid_CurrentCellChanged(object sender, System.EventArgs e)
		{
			Display(new Entry(logGrid[logGrid.CurrentRowIndex,0].ToString(),
				logGrid[logGrid.CurrentRowIndex,1].ToString(),
				((LogType)((int)logGrid[logGrid.CurrentRowIndex,2])).ToString(),
				logGrid[logGrid.CurrentRowIndex,3].ToString(),
				logGrid[logGrid.CurrentRowIndex,4].ToString()));
		}

		private void Display(Entry e) 
		{
			if (frmView == null || frmView.Disposing || frmView.IsDisposed) 
			{
				frmView = new FrmView();
				frmView.Top = this.Top;
				frmView.Left = this.Left + this.Width;
			}
			frmView.LogEntry = e;
			frmView.Show();
			frmView.Focus();
		}


		private void mnuClear_Click(object sender, System.EventArgs e)
		{
			DialogResult r = MessageBox.Show("Really clear the logs?","Question...",MessageBoxButtons.YesNo,
				MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
			if (r == DialogResult.Yes) 
			{
				conn.Open();
				SqlCommand comm = conn.CreateCommand();
				comm.CommandText = "WSEmailLogClear";
				comm.CommandType = CommandType.StoredProcedure;
				comm.ExecuteNonQuery();
				conn.Close();
				logGrid.DataSource = null;
			}
		}

		private void mnuSetMax_Click(object sender, System.EventArgs e)
		{
			if (logGrid.CurrentRowIndex >= 0) 
			{
				max = DateTime.Parse(logGrid[logGrid.CurrentRowIndex,0].ToString());
				regenerateQuery();
			}
			
		}

		private void mnuSetMin_Click(object sender, System.EventArgs e)
		{
			if (logGrid.CurrentRowIndex >= 0) 
			{
				min = DateTime.Parse(logGrid[logGrid.CurrentRowIndex,0].ToString());
				regenerateQuery();
			}
		
		}

		private void mnuResetDate_Click(object sender, System.EventArgs e)
		{
			min = new DateTime(2000,1,1);
			max = DateTime.MaxValue;
			regenerateQuery();
		}
	}

	public class Entry 
	{
		public string Date;
		public string Entity;
		public string LogType;
		public string Message;
		public string XmlMessage;

		public Entry(string a, string b, string c, string d, string e) 
		{
			this.Date = a;
			this.Entity = b;
			this.LogType = c;
			this.Message = d;
			this.XmlMessage = e;
		}
	}
}
