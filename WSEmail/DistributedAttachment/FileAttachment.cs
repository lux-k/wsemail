using System;

namespace DistributedAttachment
{
	/// <summary>
	/// Defines the information needed to send and locate file attachments.
	/// </summary>
	public class FileAttachment 
	{
		/// <summary>
		/// Holds the Url of the server where the attachment can be picked up. It's assumed to
		/// be a webservice and will be called with the ExecuteExtension() method.
		/// </summary>
		public string ServerUrl;
		/// <summary>
		/// The "key" of the file. Basically just a unique identifier.
		/// </summary>
		public string FileKey;
		/// <summary>
		/// The filename of.. the file!
		/// </summary>
		public string FileName;

		/// <summary>
		/// The SHA-1 hash of the file content.
		/// </summary>
		public string FileHash;

		/// <summary>
		/// Default constructor that does nothing.
		/// </summary>
		public FileAttachment() {}

		/// <summary>
		/// Creates a fileattachment with the given url, key and filename.
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="name"></param>
		public FileAttachment(string url, string key, string name) 
		{
			this.ServerUrl = url;
			this.FileKey = key;
			this.FileName = name;
		}

		/// <summary>
		/// Creates a fileattachment with the given filename.
		/// </summary>
		/// <param name="name"></param>
		public FileAttachment(string name) 
		{
			this.FileName = name;
		}

		/// <summary>
		/// Creates a fileattachment with the given filename and hash.
		/// </summary>
		/// <param name="name"></param>
		public FileAttachment(string name,string hash) 
		{
			this.FileName = name;
			this.FileHash = hash;
		}

	}
}

/*
using System;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Xml;
using CAPICOM;



namespace DistributedAttachment
{
	/// <summary>
	/// Defines the information needed to send and locate file attachments.
	/// </summary>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/WSEmail")]
	[Serializable]
	public class FileAttachment : System.ICloneable
	{
		
		public const string FILEATTACHMENT = "FileAttachment";
		public const string FILENAME = "FileName";
		public const string SIZE = "Size";
		public const string COMPRESSED = "Compressed";
		public const string COMPRESSED_NONE = "NONE";
		public const string COMPRESSED_ZIP = "ZIP";
		public const string COMPRESSED_RAR = "RAR";
		public const string FILE = "File";

		
		/// <summary>
		/// Holds the Url of the server where the attachment can be picked up. It's assumed to
		/// be a webservice and will be called with the ExecuteExtension() method.
		/// </summary>
		public string ServerUrl;
		/// <summary>
		/// The "key" of the file. Basically just a unique identifier.
		/// </summary>
		public string FileKey;
		/// <summary>
		/// The filename of.. the file!
		/// </summary>
		public string FileName;
		/// <summary>
		/// The size of the file
		/// </summary>
		public long FileSize;
		/// <summary>
		/// The path name of the file excluding the file name.
		/// </summary>
		private string PathName;
		/// <summary>
		/// The bytes containing the file.
		/// </summary>
		private byte[] FileBytes;
		/// <summary>
		/// The Base64 code of the file
		/// </summary>
		public string FileBase64;
		/// <summary>
		/// The SHA-1 hash of the file content.
		/// </summary>
		public string FileHash;
		/// <summary>
		/// Default constructor that does nothing.
		/// </summary>
		public FileAttachment() {}


		public object Clone() 
		{
			return this.MemberwiseClone();
		}
		
		
		/// <summary>
		/// Creates a fileattachment with the given url, key and filename.
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="name"></param>
		public FileAttachment(string url, string key, string name) 
		{
			this.ServerUrl = url;
			this.FileKey = key;
			this.FileName = name;
		}

		/// <summary>
		/// Creates a fileattachment with the given filename.
		/// </summary>
		/// <param name="name">The Filename (including path name)</param>
		public FileAttachment(string name) 
		{
			//this.PathName  = name;
			this.getFileName(name);
			//doc = vdoc;
			
			// Get File Infomation
			DirectoryInfo di = new DirectoryInfo(PathName);
			System.IO.FileInfo[] fi = di.GetFiles();
			
			foreach (FileInfo fiTemp in fi)
			{
				
				if (fiTemp.Name.Equals(this.FileName))
				{
					// Get the File Size
					this.FileSize = fiTemp.Length;

					// Get the File bytes
					System.IO.FileStream fs = new FileStream(PathName + this.FileName,FileMode.Open,FileAccess.Read);
					System.IO.BinaryReader br = new BinaryReader(fs);
					byte[] tempBytes = new byte[1024];
					int numbytes=0; // How many bytes are read in the current read cycle.
					try
					{
						do
						{
							numbytes = 0;
							for(int i=0;i<1024;i++)
							{
								tempBytes[i] = br.ReadByte();
								numbytes = i;
							}
							this.fillFileBytes(tempBytes,1024);
						}while(true);
					}
					catch(EndOfStreamException e)
					{
						this.fillFileBytes(tempBytes,numbytes);
						//MessageBox.Show(e.Message);
						e.ToString();
					}

					BytesToBase64(this.FileBytes);

				}//if
			}// foreach
		}

		/// <summary>
		/// Creates a fileattachment with the given filename and hash.
		/// </summary>
		/// <param name="name"></param>
		public FileAttachment(string name,string hash) 
		{
			this.PathName = name;
			this.FileHash = hash;
		}

		/// <summary>
		/// Get the File name and file path name seperately.
		/// </summary>
		/// <param name="fullname"></param>
		private void getFileName(string fullname){
			int indexname = fullname.LastIndexOf("\\",fullname.Length-1);
			FileName = fullname.Substring(indexname+1);
			PathName = fullname.Substring(0,indexname+1);

		}
		/// <summary>
		/// Fill or Append new  bytes to the end of the original file bytes
		/// </summary>
		/// <param name="vbytes"> The appended file bytes.</param>
		/// <param name="num">The number of bytes that is to be added.</param>
		private void fillFileBytes(byte[] vbytes,int num){
			int oLength;		// The original length of the FileBytes
			byte[] tempFBytes;
			if (FileBytes == null)
			{ 
				FileBytes = new byte[num];
				Array.Copy(vbytes,0,FileBytes,0,num);
			}else{
				oLength = FileBytes.Length;
				tempFBytes = new byte[oLength];
				Array.Copy(FileBytes,tempFBytes,oLength);
				FileBytes = new byte[FileBytes.Length + num];
				Array.Copy(tempFBytes,0,FileBytes,0,oLength);
				Array.Copy(vbytes,0,FileBytes,oLength,num);
			}
		}
		/// <summary>
		/// Convert the byte array of the file to Base64 String.Set the value of FileBase64
		/// </summary>
		/// <param name="vbytes"></param>
		/// <returns></returns>
		private void BytesToBase64(byte[] vbytes)
		{
			
			CAPICOM.UtilitiesClass u = new UtilitiesClass();
			this.FileBase64 = u.Base64Encode(u.ByteArrayToBinaryString(this.FileBytes));
		}
		/// <summary>
		/// Conver the Base64 string to Byte Array. Set the value of the Byte Arrays.
		/// </summary>
		/// <param name="strBase64"></param>
		private void Base64ToBytes(string strBase64)
		{

			CAPICOM.UtilitiesClass u = new UtilitiesClass();
			this.FileBytes = (byte[])u.BinaryStringToByteArray(u.Base64Decode(this.FileBase64));
		}

		/// <summary>
		/// A brief description of the file
		/// </summary>
		/// <returns>File description</returns>
		public override string ToString()
		{
			String s = "[FileAttachment]\n";
			s += "Filename: " + FileName + "\n";
			s += "Filesize: " + FileSize.ToString() + "\n";
			s += "FileBase64: " + FileBase64 + "\n";

			return s;
		}

	}// Class
}// namespace
*/