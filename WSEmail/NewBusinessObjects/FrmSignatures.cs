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

namespace DynamicBizObjects
{
	/// <summary>
	/// Summary description for FrmSignatures.
	/// </summary>
	public class FrmSignatures : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TreeView treeSigs;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private BusinessRequest biz;
		public FrmSignatures()
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
		/// 

		public bool	AddReceivedSignature(TreeNode node, Signature s) 
		{
			Signature[] sigs = biz.Approvals.GetDelegates(s.User);

			string val = s.User + " on ";
			if (s.Timestamp == null)
				val += "<not signed>";
			else
				val += s.Timestamp;
			val += "; Verified: ";
			bool res = true;
			res &= biz.VerifySignature(s);
			val += res.ToString();
			TreeNode node2 = node.Nodes.Add(val);
			
			if (sigs.Length > 0) 
			{
				for (int i = 0; i < sigs.Length; i++) 
				{
					res &= AddReceivedSignature(node2,sigs[i]);
				}
			}
			node2.Text += ", With delegates: " + res;

			return res;
		}
		public void DisplaySignatures(BusinessRequest biz) 
		{
			this.biz = biz;
			Signature s;
			TreeNode root = treeSigs.Nodes.Add("Approvals");
			TreeNode rec, req;

			rec = root.Nodes.Add("Received");
			req = root.Nodes.Add("Required");

			if (biz != null && biz.Approvals != null){
				if (biz.Approvals.Received != null) 
				{
					for (int i = 0; i < biz.Approvals.Received.Count; i++) 
					{

						s = (Signature)biz.Approvals.Received[i];
						if (s.AddedBy == null) 
						{
							AddReceivedSignature(rec,s);
						}
					}
				}
				if (biz.Approvals.Required != null) 
				{
					for (int i = 0; i < biz.Approvals.Required.Count; i++) 
					{
						s = (Signature)biz.Approvals.Required[i];
						if (s.AddedBy == null) 
						{
							if (s.AddedBy != null)
								req.Nodes.Add(s.User + " delegated by " + s.AddedBy);
							else
							req.Nodes.Add(s.User);
						}
					}
				}

			}
		}

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
			this.treeSigs = new System.Windows.Forms.TreeView();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// treeSigs
			// 
			this.treeSigs.ImageIndex = -1;
			this.treeSigs.Location = new System.Drawing.Point(8, 8);
			this.treeSigs.Name = "treeSigs";
			this.treeSigs.SelectedImageIndex = -1;
			this.treeSigs.Size = new System.Drawing.Size(640, 432);
			this.treeSigs.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(568, 456);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 32);
			this.button1.TabIndex = 1;
			this.button1.Text = "Ok...";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// FrmSignatures
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(656, 493);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button1,
																		  this.treeSigs});
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmSignatures";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form Signatures...";
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Dispose();
		}
	}
}
