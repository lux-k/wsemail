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
using System.Data;

namespace DynamicBizObjects
{
	/// <summary>
	/// Summary description for FrmMappings.
	/// </summary>
	public class FrmMappings : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox txtDesc;
		private BusinessRequest b;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmMappings()
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

		public void ShowMappings(BusinessRequest b) 
		{
			this.b = b;
			txtDesc.Text = "This screen shows the current mappings on the form. Mappings are a way of giving a symbolic name to a signature.\r\n\r\nThe \"Role\" column gives a symbolic name to the mapping.\r\nThe \"Entity\" column is the person who can satisfy the role.\r\nThe \"Added By\" column is the person who added this role to the form.\r\nThe \"Verified\" column is a check to see whether the person who added this role digitally signed the addition.\r\nThe \"Signed\" column tests to see if the entity has signed the form.";

			dataGrid1.TableStyles.Clear();
			dataGrid1.TableStyles.Add(GetDataGridTableStyle());
			dataGrid1.DataSource = b.Mappings;
		}

		public DataGridTableStyle GetDataGridTableStyle() 
		{
			DataGridTableStyle gs = new DataGridTableStyle();
			gs.MappingName = this.b.Mappings.GetType().Name;
			
			DataGridTextBoxColumn cs = new DataGridTextBoxColumn();
			cs.MappingName = "Name";
			cs.HeaderText = "Role";
			cs.Width = 80;
			cs.ReadOnly = true;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "User";
			cs.HeaderText = "Entity";	
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 130;
			cs.ReadOnly = true;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "MappedBy";
			cs.HeaderText = "Added By";
			cs.Width = 130;
			cs.Alignment = HorizontalAlignment.Right;
			cs.ReadOnly = true;
			gs.GridColumnStyles.Add(cs);

			cs = new AddedByColumn();
			((AddedByColumn)cs).Verifier = b;
			cs.MappingName = "Signature";
			cs.HeaderText = "Verified";
			cs.Width = 50;
			cs.Alignment = HorizontalAlignment.Right;
			cs.ReadOnly = true;
			gs.GridColumnStyles.Add(cs);

			cs = new HasSignedColumn();
			((AddedByColumn)cs).Verifier = b;
			cs.MappingName = "Empty";
			cs.HeaderText = "Signed";
			cs.Width = 40;
			cs.Alignment = HorizontalAlignment.Right;
			cs.ReadOnly = true;
			gs.GridColumnStyles.Add(cs);

			return gs;
		}

		private class AddedByColumn : DataGridTextBoxColumn
		{
			protected BusinessRequest _b;
			protected ArrayList cache = new ArrayList();
			public BusinessRequest Verifier 
			{
				set 
				{
					_b = value;
				}
				get 
				{
					return _b;
				}
			}

			protected override object GetColumnValueAtRow(CurrencyManager source, int row) {
				if (cache.Count >=  row +1)
					return cache[row];

				bool res = false;
				res = Verifier.VerifySignature( Verifier.Approvals.GetSignatureByName(((MappedItem)Verifier.Mappings[row]).MappedBy)[0]);
				if (cache.Count < row)
					cache.Capacity = row;
				cache.Insert(row,res);
				//MessageBox.Show("res = " + res);
				//MessageBox.Show(((MappedItem)Verifier.Mappings[row]).Signature.ToString());
				if (((MappedItem)Verifier.Mappings[row]).Signature == null) 
				{
				//	MessageBox.Show("Sig is null.");
				//	Console.WriteLine(((MappedItem)Verifier.Mappings[row]));
				}
				return res.ToString();
			}
		}
		private class HasSignedColumn : AddedByColumn
		{
			protected override object GetColumnValueAtRow(CurrencyManager source, int row) 
			{
				if (cache.Count >=  row +1)
					return cache[row];

				bool res = false;
				
				Signature[] sigs = Verifier.Approvals.GetSignatureByName(((MappedItem)Verifier.Mappings[row]).User);
				if (sigs.Length >=1) 
				{
					Signature s =  sigs[0];
					if (s.Value != null)
						res = true;
				}
				if (cache.Count < row)
					cache.Capacity = row;
				cache.Insert(row,res);
				return res.ToString();
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.button1 = new System.Windows.Forms.Button();
			this.txtDesc = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGrid1
			// 
			this.dataGrid1.CaptionText = "Mappings:";
			this.dataGrid1.DataMember = "";
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(8, 8);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(488, 136);
			this.dataGrid1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(416, 208);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 32);
			this.button1.TabIndex = 1;
			this.button1.Text = "Ok...";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// txtDesc
			// 
			this.txtDesc.BackColor = System.Drawing.Color.White;
			this.txtDesc.Enabled = false;
			this.txtDesc.ForeColor = System.Drawing.Color.Black;
			this.txtDesc.Location = new System.Drawing.Point(8, 152);
			this.txtDesc.Multiline = true;
			this.txtDesc.Name = "txtDesc";
			this.txtDesc.ReadOnly = true;
			this.txtDesc.Size = new System.Drawing.Size(392, 136);
			this.txtDesc.TabIndex = 2;
			this.txtDesc.Text = "";
			// 
			// FrmMappings
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(504, 293);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.txtDesc,
																		  this.button1,
																		  this.dataGrid1});
			this.Name = "FrmMappings";
			this.Text = "Approval Mappings";
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}
	}
}
