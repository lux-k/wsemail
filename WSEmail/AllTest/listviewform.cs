using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace AllTest
{
	/// <summary>
	/// Summary description for listviewform.
	/// </summary>
	public class listviewform : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public listviewform()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		static void Main() 
		{
			Application.Run(new listviewform());
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
			// 
			// listviewform
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(712, 582);
			this.Name = "listviewform";
			this.Text = "listviewform";
			this.Load += new System.EventHandler(this.listviewform_Load);

		}
		#endregion

		private void listviewform_Load(object sender, System.EventArgs e)
		{
			CreateMyListView();

		}
		private void CreateMyListView()
		{
			// Create a new ListView control.
			ListView listView1 = new ListView();
			listView1.Bounds = new Rectangle(new Point(10,10), new Size(300,200));

			// Set the view to show details.
			listView1.View = View.Details;
			// Allow the user to edit item text.
			listView1.LabelEdit = true;
			// Allow the user to rearrange columns.
			listView1.AllowColumnReorder = true;
			// Display check boxes.
			listView1.CheckBoxes = true;
			// Select the item and subitems when selection is made.
			listView1.FullRowSelect = true;
			// Display grid lines.
			listView1.GridLines = true;
			// Sort the items in the list in ascending order.
			listView1.Sorting = SortOrder.Ascending;
                
			// Create three items and three sets f subitems for each item.
			ListViewItem item1 = new ListViewItem("item1",0);
			// Place a check mark next to the item.
			item1.Checked = true;
			item1.SubItems.Add("1");
			item1.SubItems.Add("2");
			item1.SubItems.Add("3");
			ListViewItem item2 = new ListViewItem("item2",1);
			item2.SubItems.Add("4");
			item2.SubItems.Add("5");
			item2.SubItems.Add("6");
			ListViewItem item3 = new ListViewItem("item3",0);
			// Place a check mark next to the item.
			item3.Checked = true;
			item3.SubItems.Add("7");
			item3.SubItems.Add("8");
			item3.SubItems.Add("9");

			// Create columns for the items and subitems.
			listView1.Columns.Add("Item Column", -2, HorizontalAlignment.Left);
			listView1.Columns.Add("Column 2", -2, HorizontalAlignment.Left);
			listView1.Columns.Add("Column 3", -2, HorizontalAlignment.Left);
			listView1.Columns.Add("Column 4", -2, HorizontalAlignment.Center);

			//Add the items to the ListView.
			listView1.Items.AddRange(new ListViewItem[]{item1,item2,item3});

			// Add the ListView to the control collection.
			this.Controls.Add(listView1);

		}
	}
}
