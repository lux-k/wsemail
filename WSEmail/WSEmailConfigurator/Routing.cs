/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using WSERoutingTable;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WSEmailConfigurator
{
	/// <summary>
	/// Summary description for Routing.
	/// </summary>
	public class Routing : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DataGrid dgRoutes;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnDel;
		private System.Windows.Forms.Button btnDone;
		RoutingTable routes = null;

		public Button NextButton 
		{
			get 
			{
				return btnDone;
			}
		}

		public RoutingTable RoutingTable 
		{
			get 
			{
				return routes;
			}
			set 
			{
				routes = value;
			}
		}
		public Routing(RoutingTable r)
		{
			this.RoutingTable = r;
			Init();
		}

		protected void Init() 
		{
			InitializeComponent();

			dgRoutes.TableStyles.Clear();
			dgRoutes.TableStyles.Add(GetDataGridTableStyle(this.RoutingTable.Routes));
			dgRoutes.DataSource = this.RoutingTable.Routes;
		}

		public Routing()
		{
			Init();
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

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			this.RoutingTable.addRoute(new Route());
			PingDataGrid();
		}

		private void PingDataGrid() 
		{
			this.dgRoutes.DataSource = new ArrayList();
			this.dgRoutes.DataSource = this.RoutingTable.Routes;
		}

		private DataGridTableStyle GetDataGridTableStyle(object o) 
		{
			DataGridTableStyle gs = new DataGridTableStyle();
			gs.MappingName = o.GetType().Name;

			DataGridTextBoxColumn cs = new DataGridTextBoxColumn();
			cs.MappingName = "Source";
			cs.HeaderText = "Source";
			cs.Width = 120;
			cs.ReadOnly = false;
			cs.TextBox.Enabled = true;
			cs.Alignment = HorizontalAlignment.Center;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Destination";
			cs.HeaderText = "Destination";	
			cs.Alignment = HorizontalAlignment.Center;
			cs.Width = 120;
			cs.TextBox.Enabled = true;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Router";
			cs.HeaderText = "Router";
			cs.Width = 260;
			cs.Alignment = HorizontalAlignment.Center;
			cs.TextBox.Enabled = true;
			gs.GridColumnStyles.Add(cs);

			DataGridBoolColumn bc = new DataGridBoolColumn();
			bc.MappingName = "Accept";
			bc.HeaderText = "Enabled";
			bc.Alignment = HorizontalAlignment.Center;
			bc.Width = 60;
			gs.GridColumnStyles.Add(bc);

			return gs;
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dgRoutes = new System.Windows.Forms.DataGrid();
			this.btnAdd = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.btnDel = new System.Windows.Forms.Button();
			this.btnDone = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgRoutes)).BeginInit();
			this.SuspendLayout();
			// 
			// dgRoutes
			// 
			this.dgRoutes.CaptionText = "WSEmail Routes:";
			this.dgRoutes.DataMember = "";
			this.dgRoutes.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgRoutes.Location = new System.Drawing.Point(8, 8);
			this.dgRoutes.Name = "dgRoutes";
			this.dgRoutes.Size = new System.Drawing.Size(600, 176);
			this.dgRoutes.TabIndex = 0;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(480, 200);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(112, 32);
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "Add New...";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(8, 192);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(448, 192);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = @"These are the routes that WSEmail Messages will take when they are not being delivered lccally.

The use of the '*' character represents a wildcard, meaning anything will match this criteria.

Multiple routes can be defined for the same path, but the most specific will be chosen (ie. a path with a named destination instead of '*' would be given precedence).";
			// 
			// btnDel
			// 
			this.btnDel.Location = new System.Drawing.Point(480, 248);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new System.Drawing.Size(112, 32);
			this.btnDel.TabIndex = 3;
			this.btnDel.Text = "Delete Selected...";
			this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
			// 
			// btnDone
			// 
			this.btnDone.Location = new System.Drawing.Point(480, 344);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(112, 32);
			this.btnDone.TabIndex = 4;
			this.btnDone.Text = "Next -->";
			// 
			// Routing
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(616, 389);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnDone,
																		  this.btnDel,
																		  this.textBox1,
																		  this.btnAdd,
																		  this.dgRoutes});
			this.Name = "Routing";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WS Email Router Configuration...";
			((System.ComponentModel.ISupportInitialize)(this.dgRoutes)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnDel_Click(object sender, System.EventArgs e)
		{
			if (dgRoutes.CurrentRowIndex > 0) 
			{
				((ArrayList)dgRoutes.DataSource).RemoveAt(dgRoutes.CurrentRowIndex);
				this.PingDataGrid();
			}	
			else
				MessageBox.Show("Please select a row and then press this button!","Oops!");
		}
	}
}
