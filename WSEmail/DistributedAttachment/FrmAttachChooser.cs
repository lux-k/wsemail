/*
 * 
 * switching to wse2 is bad enough without having debug other people's code
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using WSEmailProxy;
using XACLPolicy;





namespace DistributedAttachment

{
	/// <summary>
	/// Summary description for FrmAttachChooser.
	/// </summary>
	public class FrmAttachChooser : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbFiles;
		private System.Windows.Forms.GroupBox gbReceiver;
		private System.Windows.Forms.Button btAdd;
		private System.Windows.Forms.Button btRemove;
		private System.Windows.Forms.Button btOK;
		private System.Windows.Forms.Button btCancel;
		private System.Windows.Forms.OpenFileDialog ofdialog;
		private System.Windows.Forms.ListBox listFiles;


		//private bool debug = true;
		private bool policyRefreshed = false;		// Indicate whether all recipients have been refreshed
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckedListBox policyList;

		//private Recipient[] recipients = null;
		private GUIPolicy guiPolicy = null;
		public Policy returnPolicy = null;
		private FileAttachments fileattachments = null;
		public FileAttachments returnFAs = null;
		private Recipients recipients;

		public FrmAttachChooser(Recipients vrecipients)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//htAttachments = new Hashtable();
			guiPolicy = new GUIPolicy(vrecipients.recs);
			recipients = vrecipients;


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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmAttachChooser));
			this.gbReceiver = new System.Windows.Forms.GroupBox();
			this.policyList = new System.Windows.Forms.CheckedListBox();
			this.btAdd = new System.Windows.Forms.Button();
			this.btRemove = new System.Windows.Forms.Button();
			this.btOK = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.ofdialog = new System.Windows.Forms.OpenFileDialog();
			this.gbFiles = new System.Windows.Forms.GroupBox();
			this.listFiles = new System.Windows.Forms.ListBox();
			this.gbReceiver.SuspendLayout();
			this.gbFiles.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbReceiver
			// 
			this.gbReceiver.Controls.Add(this.policyList);
			this.gbReceiver.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
			this.gbReceiver.Location = new System.Drawing.Point(296, 8);
			this.gbReceiver.Name = "gbReceiver";
			this.gbReceiver.Size = new System.Drawing.Size(280, 405);
			this.gbReceiver.TabIndex = 1;
			this.gbReceiver.TabStop = false;
			this.gbReceiver.Text = "Recipients";
			// 
			// policyList
			// 
			this.policyList.CheckOnClick = true;
			this.policyList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.policyList.Location = new System.Drawing.Point(4, 20);
			this.policyList.MultiColumn = true;
			this.policyList.Name = "policyList";
			this.policyList.Size = new System.Drawing.Size(272, 378);
			this.policyList.TabIndex = 0;
			this.policyList.SelectedIndexChanged += new System.EventHandler(this.policyList_SelectedIndexChanged);
			this.policyList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.policyList_ItemCheck);
			// 
			// btAdd
			// 
			this.btAdd.Location = new System.Drawing.Point(4, 416);
			this.btAdd.Name = "btAdd";
			this.btAdd.Size = new System.Drawing.Size(104, 32);
			this.btAdd.TabIndex = 2;
			this.btAdd.Text = "Add";
			this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
			// 
			// btRemove
			// 
			this.btRemove.Location = new System.Drawing.Point(116, 416);
			this.btRemove.Name = "btRemove";
			this.btRemove.Size = new System.Drawing.Size(104, 32);
			this.btRemove.TabIndex = 3;
			this.btRemove.Text = "Remove";
			this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
			// 
			// btOK
			// 
			this.btOK.Location = new System.Drawing.Point(360, 416);
			this.btOK.Name = "btOK";
			this.btOK.Size = new System.Drawing.Size(104, 32);
			this.btOK.TabIndex = 4;
			this.btOK.Text = "OK";
			this.btOK.Click += new System.EventHandler(this.btOK_Click);
			// 
			// btCancel
			// 
			this.btCancel.Location = new System.Drawing.Point(472, 416);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(104, 32);
			this.btCancel.TabIndex = 5;
			this.btCancel.Text = "Cancel";
			this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
			// 
			// gbFiles
			// 
			this.gbFiles.Controls.Add(this.listFiles);
			this.gbFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
			this.gbFiles.Location = new System.Drawing.Point(8, 8);
			this.gbFiles.Name = "gbFiles";
			this.gbFiles.Size = new System.Drawing.Size(280, 405);
			this.gbFiles.TabIndex = 6;
			this.gbFiles.TabStop = false;
			this.gbFiles.Text = "Files";
			// 
			// listFiles
			// 
			this.listFiles.ItemHeight = 15;
			this.listFiles.Items.AddRange(new object[] {
									   "DefaultFile"});
			this.listFiles.Location = new System.Drawing.Point(4, 20);
			this.listFiles.Name = "listFiles";
			this.listFiles.Size = new System.Drawing.Size(272, 379);
			this.listFiles.TabIndex = 0;
			this.listFiles.SelectedIndexChanged += new System.EventHandler(this.listFiles_SelectedIndexChanged);
			// 
			// FrmAttachChooser
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 454);
			this.Controls.Add(this.gbFiles);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btOK);
			this.Controls.Add(this.btRemove);
			this.Controls.Add(this.btAdd);
			this.Controls.Add(this.gbReceiver);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmAttachChooser";
			this.Text = "Choose Attachments";
			this.Load += new System.EventHandler(this.FrmAttachChooser_Load);
			this.gbReceiver.ResumeLayout(false);
			this.gbFiles.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btAdd_Click(object sender, System.EventArgs e)
		{
			// Open the File Chooser Dialog
			if (this.ofdialog.ShowDialog(this) == DialogResult.OK) {

				//MessageBox.Show(ofdialog.FileName);
				FileAttachment fac = new FileAttachment(ofdialog.FileName);
				//FileAttachment fac = new FileAttachment();
				
				if (fileattachments == null)
					fileattachments = new FileAttachments();

				fileattachments.addFileAttachment(fac);
				guiPolicy.objects = fileattachments.fas;
				guiPolicy.addObject();

				this.refreshFilesList();
			}
			
		}

		private void btCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void FrmAttachChooser_Load(object sender, System.EventArgs e)
		{
			if (this.returnPolicy != null)
			{
				guiPolicy = new GUIPolicy();
				
			}
			
			this.CancelButton = btCancel;
			this.refreshFilesList();
			this.refreshPolicyList();

			
		}
		/// <summary>
		/// Refresh the File Lists.
		/// </summary>
		private void refreshFilesList(){
			this.listFiles.Items.Clear();
			if (fileattachments != null)
				if(fileattachments.numFa > 0)
					for(int i=0;i<fileattachments.numFa;i++)
						listFiles.Items.Add(fileattachments.fas[i].FileName);
		}// refreshFilesList
		/// <summary>
		/// Remove the selected attachments; update File Attachments list and Policy acl matrix.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btRemove_Click(object sender, System.EventArgs e)
		{
			if (listFiles.SelectedIndex != -1)
			{
				//htAttachments.Remove(listFiles.Items[listFiles.SelectedIndex]);
				FileAttachment faTemp = fileattachments.getFa((string)listFiles.SelectedItem);
				int indexf = fileattachments.getIndexFa(faTemp);
				fileattachments.removeFileAttachment(indexf);
				guiPolicy.objects = fileattachments.fas;
				guiPolicy.removeObject(indexf);
				listFiles.Items.RemoveAt(this.listFiles.SelectedIndex);
				this.refreshFilesList();
				this.refreshPolicyList();
			}
		}
		/// <summary>
		/// Refresh the PolicyList. This method is invoked when the form is loaded.
		/// </summary>
		private void refreshPolicyList()
		{
			this.policyRefreshed = false;
			this.policyList.Items.Clear();
			//policyList.Items.Add("All");

			for(int i=0;i<this.recipients.recs.Length;i++)
				policyList.Items.Add(recipients.recs[i].EmailAddress,false);
			this.policyRefreshed = true;
		}
		/// <summary>
		/// Refresh the policy list according to the selected FileAttachment
		/// </summary>
		/// <param name="filename">The selected file's name</param>
		private void refreshPolicyList(string filename)
		{

			this.policyRefreshed = false;
			FileAttachment faTemp = fileattachments.getFa(filename);
			policyList.Items.Clear();
			bool canRead;
			int indexo = fileattachments.getIndexFa(faTemp);	// The index of the selected file
			for(int i=0;i<recipients.numRec;i++)
			{
				canRead = (guiPolicy.getPermission(i,indexo) == GUIPolicy.READ);
				policyList.Items.Add(recipients.recs[i].EmailAddress,canRead);
			}
			this.policyRefreshed = true;
		}
		/// <summary>
		/// If the selected index of the policy list is changed, something is checked or not checked. Then refresh the ACL matrix in Policy
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void policyList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			this.refreshACL();
		}

		/// <summary>
		/// If the items in policylist is checked or not, it will refresh ACL matrix.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void policyList_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{	
			if(policyRefreshed)
				this.refreshACL();
		}
		/// <summary>
		/// If there is something changed at policy list, this method is invoked.
		/// </summary>
		private void refreshACL()
		{
			int indexs,indexo;
			FileAttachment faTemp;
			Recipient recTemp;
			if (listFiles.SelectedIndex != -1)
				for (int i = 0;i<policyList.Items.Count;i++)
				{
					faTemp = fileattachments.getFa((string)listFiles.SelectedItem);
					indexo = fileattachments.getIndexFa(faTemp);

					recTemp = recipients.getRec((string)policyList.Items[i]);
					indexs = recipients.getIndexRec(recTemp);
					if (policyList.GetItemChecked(i))
						guiPolicy.setACL(indexs,indexo,GUIPolicy.READ);
					else
						guiPolicy.setACL(indexs,indexo,GUIPolicy.DENIED);
				}
		}
		private void listFiles_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ((string)listFiles.SelectedItem != null)
				refreshPolicyList((string)listFiles.SelectedItem);
		}
		
		/// <summary>
		/// The form is hidden
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btOK_Click(object sender, System.EventArgs e)
		{
			// Only if the user clicks "OK", the returned ojbect will be updateds.

			if (listFiles.SelectedItems.Count == 0)
			{
				MessageBoxButtons buttons = MessageBoxButtons.OK;
				string caption = "No Policy set up";
				string msg = "If you choose attachment, you must set up policy for them!\n";
				MessageBox.Show(this,msg,caption,buttons,MessageBoxIcon.Exclamation);
				return;

			}

				 this.returnFAs = this.fileattachments;
			this.returnPolicy = this.createPolicy(guiPolicy);
			this.DialogResult = DialogResult.OK;
			this.Hide();
		}
		/// <summary>
		/// Set up the policy object by the guipolicies
		/// </summary>
		/// <param name="guipolicy">The policy got from GUI</param>
		/// <returns>The policy which will be sent to Server</returns>
		private Policy createPolicy(GUIPolicy guipolicy)
		{
			returnPolicy = new Policy();

			Action xaction;
			Subject xsubject;
			Acl acl;
			Rule xrule;
			XObject xobject;
			Xacl xacl;


			// Currently, we only support two recipients and only one can get read permission
			 
			for(int i=0;i<recipients.numRec;i++)
				for(int j=0;j<fileattachments.numFa;j++)
				{
					if(guiPolicy.getPermission(i,j) == GUIPolicy.READ)
						xaction = new Action(Action.OperationType.read,Action.PermissionType.grant);
					else
						xaction = new Action(Action.OperationType.read,Action.PermissionType.deny);
						
					xsubject = new Subject();
					xsubject.uid = recipients.recs[i].EmailAddress;

					acl = new Acl();
					acl.actionRead = xaction;
					acl.subject = xsubject;

					xrule = new Rule();
					xrule.acl = acl;

					xobject = new XObject();
					xobject.href = "Fileattachments/Fileattachment";	// This is fixed

					xacl = new Xacl();
					xacl.xobject = xobject;
					xacl.rule = xrule;

					if (returnPolicy.xacl == null) 
						returnPolicy.xacl = xacl;
					else
						if (xaction.permission.Equals(Action.PermissionType.grant.ToString()))
							returnPolicy.xacl = xacl;
					
						
				}

			return returnPolicy;
		}

		
	}
}
*/