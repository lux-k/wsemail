/*
using System;
//using WSEMailProxy;

namespace DistributedAttachment

{
	/// <summary>
	/// Summary description for FileAttachments.
	/// </summary>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	[Serializable]
	public class FileAttachments
	{

		public FileAttachment[] fas;
		public int numFa;

		

		public FileAttachments()
		{
			//
			// TODO: Add constructor logic here
			//
			numFa = 0;
			fas = null;
		}

		/// <summary>
		/// Add a new File Attachment
		/// </summary>
		/// <param name="fa">The added File attachment</param>
		public void addFileAttachment(FileAttachment fa)
		{
			numFa++;
			FileAttachment[] faTemp = fas;
			fas = new FileAttachment[numFa];
			if (faTemp != null)
				Array.Copy(faTemp,0,fas,0,faTemp.Length);
			fas[numFa-1] = fa;

			// some code for Policy object?

		}
		/// <summary>
		/// Get the index of the specified Attachment in FileAttachment List
		/// </summary>
		/// <param name="fa">The searched attachment</param>
		/// <returns>The index of the searched attachment</returns>
		public int getIndexFa(FileAttachment fa)
		{
			int rindex = 0;
			for (int i=0;i<this.fas.Length;i++)
				if(fas[i].FileName.ToLower().Equals(fa.FileName.ToLower()))
				{
					rindex = i;
					break;
				}
			return rindex;

		}
		/// <summary>
		/// Remove a file attachment from the FileAttachment list
		/// </summary>
		/// <param name="indexF">The index of the removed fileattachment</param>
		public void removeFileAttachment(int indexF)
		{
			FileAttachment[] fasTemp = fas;
			this.numFa--;
			fas = new FileAttachment[numFa];
			for(int i = indexF;i < fasTemp.Length-1;i++)
				fasTemp[i] = fasTemp[i+1];
			for(int i=0;i<numFa;i++)
				fas[i] = fasTemp[i];
		}
		/// <summary>
		/// Get the FileAttachment by the specified filename
		/// </summary>
		/// <param name="filename">The specified filename</param>
		/// <returns>The searched File Attachment</returns>
		public FileAttachment getFa(string filename)
		{
			FileAttachment faTemp = null;
			for(int i=0;i<this.fas.Length;i++)
				if(fas[i].FileName.ToLower().Equals(filename.ToLower()))
				{
					faTemp = fas[i];
					break;
				}
			return faTemp;
		}

		/// <summary>
		/// Return the brief description of the FileAttachments
		/// </summary>
		/// <returns>Brief Description</returns>
		public override string ToString()
		{
			String s = "[FileAttachments]\n";
			s += "Number of FileAttachments: " + numFa.ToString() + "\n";
			for(int i=0;i<numFa;i++)
				s += fas[i].ToString();

			return s;
		}

	}
}
*/