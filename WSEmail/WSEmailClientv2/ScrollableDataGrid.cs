/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace WSEmailClientv2
{
	public class ScrollableDataGrid : DataGrid 
	{
		public void ScrollToBottom() 
		{
			ScrollEventArgs a = new ScrollEventArgs(ScrollEventType.Last,this.VertScrollBar.Maximum);
			this.GridVScrolled(this,a);
		}
		public void ScrollToTop() 
		{
			ScrollEventArgs a = new ScrollEventArgs(ScrollEventType.First,this.VertScrollBar.Minimum);
			this.GridVScrolled(this,a);
		
		}
		
		public int ClientWidth 
		{
			get 
			{
				int retWidth = ClientSize.Width;

				switch (BorderStyle) 
				{
					case (BorderStyle.Fixed3D):
						retWidth -= SystemInformation.Border3DSize.Width * 2;
						break;
					case (BorderStyle.FixedSingle):
						retWidth -= SystemInformation.BorderSize.Width * 2;
						break;
				}

				if (VertScrollBar.Visible)
					retWidth = this.VertScrollBar.Width;
				if (this.RowHeadersVisible)
					retWidth -= RowHeaderWidth;
				return retWidth;
			}
		}
	}
}