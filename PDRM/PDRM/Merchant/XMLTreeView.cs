using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

namespace Merchant
{
	/// <summary>
	/// Window to view an XML document using a TreeView window.  See http://support.microsoft.com/default.aspx?kbid=317597
	/// for more information.
	/// </summary>
	public class XMLTreeView : System.Windows.Forms.Form
	{
		/// <summary>
		/// The XML document to display in the treeview
		/// </summary>
		protected System.Xml.XmlDocument m_doc;

		/// <summary>
		/// The XML document to display in the treeview
		/// </summary>
		public System.Xml.XmlDocument Doc
		{
			get { return m_doc; }
			set
			{
				m_doc = value;

				// reload the data
				ReloadData();
			}
		}

		/// <summary>
		/// The name of the document that we are viewing
		/// </summary>
		protected string m_doc_name;

		/// <summary>
		/// The name of the document that we are viewing
		/// </summary>
		public string DocName
		{
			get { return m_doc_name; }
			set 
			{
				m_doc_name = value;

				// and put it in the textbox
				this.tbDocument.Text = m_doc_name;
			}
		}

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TreeView tvXML;
		private System.Windows.Forms.TextBox tbDocument;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public XMLTreeView()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// create defaults to prevent problems
			this.m_doc = new XmlDocument();
			this.m_doc_name = "";
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
			this.tvXML = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.tbDocument = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tvXML
			// 
			this.tvXML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tvXML.ImageIndex = -1;
			this.tvXML.Location = new System.Drawing.Point(8, 40);
			this.tvXML.Name = "tvXML";
			this.tvXML.SelectedImageIndex = -1;
			this.tvXML.Size = new System.Drawing.Size(416, 352);
			this.tvXML.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(152, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Tree view of XML source for: ";
			// 
			// tbDocument
			// 
			this.tbDocument.Location = new System.Drawing.Point(160, 8);
			this.tbDocument.Name = "tbDocument";
			this.tbDocument.ReadOnly = true;
			this.tbDocument.Size = new System.Drawing.Size(256, 20);
			this.tbDocument.TabIndex = 3;
			this.tbDocument.Text = "";
			// 
			// XMLTreeView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 397);
			this.Controls.Add(this.tbDocument);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tvXML);
			this.Name = "XMLTreeView";
			this.Text = "XML Source";
			this.Load += new System.EventHandler(this.XMLTreeView_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void XMLTreeView_Load(object sender, System.EventArgs e)
		{
			// Initialize the tree view
			ReloadData();
		}

		private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
		{
			XmlNode xNode;
			TreeNode tNode;
			XmlNodeList nodeList;
			int i;

			// Loop through the XML nodes until the leaf is reached.
			// Add the nodes to the TreeView during the looping process.
			if (inXmlNode.HasChildNodes)
			{
				nodeList = inXmlNode.ChildNodes;
				for(i = 0; i<=nodeList.Count - 1; i++)
				{
					xNode = inXmlNode.ChildNodes[i];
					inTreeNode.Nodes.Add(new TreeNode(xNode.Name));
					tNode = inTreeNode.Nodes[i];
					AddNode(xNode, tNode);
				}
			}
			else
			{
				// Here you need to pull the data from the XmlNode based on the
				// type of node, whether attribute values are required, and so forth.
				inTreeNode.Text = (inXmlNode.OuterXml).Trim();
			}
		} 
        
		/// <summary>
		/// Reload the TreeView with the data in the XML Document object
		/// just passed in
		/// </summary>
		private void ReloadData()
		{
			// take the new XML document that we have and load it up
			try 
			{
				// SECTION 1. Initialize the TreeView control.
				tvXML.Nodes.Clear();
				tvXML.Nodes.Add(new TreeNode(m_doc.DocumentElement.Name));
				TreeNode tNode = new TreeNode();
				tNode = tvXML.Nodes[0];

				// SECTION 2. Populate the TreeView with the DOM nodes.
				AddNode(m_doc.DocumentElement, tNode);
				tvXML.ExpandAll();
			}
			catch(XmlException xmlEx)
			{
				MessageBox.Show(xmlEx.Message);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
