using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

using DistributedAttachment;
using WSEmailProxy;

namespace AttachTest
{
	/// <summary>
	/// Summary description for FrmMessageView.
	/// </summary>
	public class FrmMessageView : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtBody;
		private System.Windows.Forms.TextBox txtSubject;
		private System.Windows.Forms.TextBox txtTo;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFrom;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ColumnHeader Filename;
		private System.Windows.Forms.Button btClose;
		private System.Windows.Forms.Button btDownload;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ColumnHeader Filesize;
		private int msgID;
		private System.Windows.Forms.ListView lvFAs;
		private System.Windows.Forms.SaveFileDialog saveFD;
		private string user;


		public FrmMessageView(int vmsgId,string vuser)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			msgID = vmsgId;
			user = vuser;

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
			this.txtBody = new System.Windows.Forms.TextBox();
			this.txtSubject = new System.Windows.Forms.TextBox();
			this.txtTo = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtFrom = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.lvFAs = new System.Windows.Forms.ListView();
			this.Filename = new System.Windows.Forms.ColumnHeader();
			this.Filesize = new System.Windows.Forms.ColumnHeader();
			this.btClose = new System.Windows.Forms.Button();
			this.btDownload = new System.Windows.Forms.Button();
			this.saveFD = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// txtBody
			// 
			this.txtBody.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBody.Location = new System.Drawing.Point(120, 204);
			this.txtBody.Multiline = true;
			this.txtBody.Name = "txtBody";
			this.txtBody.ReadOnly = true;
			this.txtBody.Size = new System.Drawing.Size(248, 188);
			this.txtBody.TabIndex = 17;
			this.txtBody.Text = "";
			// 
			// txtSubject
			// 
			this.txtSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtSubject.Location = new System.Drawing.Point(120, 76);
			this.txtSubject.Name = "txtSubject";
			this.txtSubject.ReadOnly = true;
			this.txtSubject.Size = new System.Drawing.Size(248, 22);
			this.txtSubject.TabIndex = 16;
			this.txtSubject.Text = "";
			// 
			// txtTo
			// 
			this.txtTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtTo.Location = new System.Drawing.Point(120, 12);
			this.txtTo.Name = "txtTo";
			this.txtTo.ReadOnly = true;
			this.txtTo.Size = new System.Drawing.Size(248, 22);
			this.txtTo.TabIndex = 15;
			this.txtTo.Text = "";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(20, 108);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 20);
			this.label4.TabIndex = 19;
			this.label4.Text = "Attachments";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(20, 208);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 20);
			this.label3.TabIndex = 14;
			this.label3.Text = "Body:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(20, 76);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 20);
			this.label2.TabIndex = 13;
			this.label2.Text = "Subject:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(20, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 20);
			this.label1.TabIndex = 12;
			this.label1.Text = "To:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtFrom
			// 
			this.txtFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtFrom.Location = new System.Drawing.Point(120, 44);
			this.txtFrom.Name = "txtFrom";
			this.txtFrom.ReadOnly = true;
			this.txtFrom.Size = new System.Drawing.Size(248, 22);
			this.txtFrom.TabIndex = 22;
			this.txtFrom.Text = "";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(20, 44);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 20);
			this.label5.TabIndex = 21;
			this.label5.Text = "From";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lvFAs
			// 
			this.lvFAs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					this.Filename,
																					this.Filesize});
			this.lvFAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lvFAs.Location = new System.Drawing.Point(120, 108);
			this.lvFAs.Name = "lvFAs";
			this.lvFAs.Size = new System.Drawing.Size(248, 84);
			this.lvFAs.TabIndex = 23;
			this.lvFAs.View = System.Windows.Forms.View.Details;
			// 
			// Filename
			// 
			this.Filename.Text = "Filename";
			this.Filename.Width = 150;
			// 
			// Filesize
			// 
			this.Filesize.Text = "Size";
			this.Filesize.Width = 90;
			// 
			// btClose
			// 
			this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btClose.Location = new System.Drawing.Point(268, 404);
			this.btClose.Name = "btClose";
			this.btClose.Size = new System.Drawing.Size(100, 32);
			this.btClose.TabIndex = 25;
			this.btClose.Text = "Close";
			this.btClose.Click += new System.EventHandler(this.btClose_Click);
			// 
			// btDownload
			// 
			this.btDownload.Location = new System.Drawing.Point(160, 404);
			this.btDownload.Name = "btDownload";
			this.btDownload.Size = new System.Drawing.Size(100, 32);
			this.btDownload.TabIndex = 24;
			this.btDownload.Text = "Download...";
			this.btDownload.Click += new System.EventHandler(this.btDownload_Click);
			// 
			// FrmMessageView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btClose;
			this.ClientSize = new System.Drawing.Size(374, 444);
			this.Controls.Add(this.btClose);
			this.Controls.Add(this.btDownload);
			this.Controls.Add(this.lvFAs);
			this.Controls.Add(this.txtFrom);
			this.Controls.Add(this.txtBody);
			this.Controls.Add(this.txtSubject);
			this.Controls.Add(this.txtTo);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FrmMessageView";
			this.Text = "MessageView";
			this.Load += new System.EventHandler(this.FrmMessageView_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void btClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void loadMail()
		{
			DistributedAttachment.MailAttachService mas = new MailAttachService();
			DistributedAttachment.WSMessageAttach wsma = mas.WSEmailRetrieve(this.msgID,this.user);

			this.txtBody.Text  = wsma.Body;
			this.txtFrom.Text  = wsma.Sender;
			this.txtSubject.Text = wsma.Subject;
			this.Text = wsma.Subject + "-" + this.Text;
			string strTo = null;
			for(int i=0;i<wsma.Recipients.Count;i++)
				strTo += wsma.Recipients[i].ToString() + ",";
			if(strTo != null)
				strTo.Remove(strTo.Length-1,1);
			this.txtTo.Text = strTo;

			if(wsma.Fileattachments != null)
				for(int i = 0;i<wsma.Fileattachments.numFa;i++)
					this.addFa(wsma.Fileattachments.fas[i]);
		}

		private void FrmMessageView_Load(object sender, System.EventArgs e)
		{
			loadMail();
		}

		private void addFa(FileAttachment fa)
		{
			ListViewItem livs = new ListViewItem(fa.FileName);
			livs.SubItems.Add(fa.FileSize.ToString());
			this.lvFAs.Items.Add(livs);
		}

		private void btDownload_Click(object sender, System.EventArgs e)
		{
			if(lvFAs.SelectedItems.Count == 0)
			{
				MessageBoxButtons buttons = MessageBoxButtons.OK;
				string caption = "No Attachment selected";
				string msg = "You must select an Attachment from Message List!";
				MessageBox.Show(this,msg,caption,buttons,MessageBoxIcon.Exclamation);
				return;
			}
			string filename = lvFAs.SelectedItems[0].SubItems[0].Text;

			MailAttachService mas = new MailAttachService();
			string token = mas.RequestFederatedToken();

			FileAttachment fa = mas.WSEmailRetrieveAttach(token,filename,this.msgID);
			
			//MessageBox.Show(fa.FileBase64);
			saveFile(fa);
		{	
			MessageBoxButtons buttons = MessageBoxButtons.OK;
			string caption = "Save Attachment";
			string msg = "Attachment has been saved.";
			MessageBox.Show(this,msg,caption,buttons,MessageBoxIcon.Information);
		}			
		}

		private void saveFile(FileAttachment fa)
		{
			System.IO.FileStream outStream;
			saveFD.RestoreDirectory = true;
			saveFD.FileName = fa.FileName;
			if(saveFD.ShowDialog() == DialogResult.OK )
			{
				outStream = (FileStream)saveFD.OpenFile();
				byte[] filebytes = Convert.FromBase64String(fa.FileBase64);
				for(int i=0;i<filebytes.Length;i++)
					outStream.WriteByte(filebytes[i]);
				outStream.Close();
			}
		}

	}
}
