/*
using System;
using System.Collections;

namespace DistributedAttachment
{
	/// <summary>
	/// GUIPolicy is used for interface. It maintains an ACL matrix. It is different from the Policy object.
	/// The later one will be serialized to XML documents
	/// </summary>
	public class GUIPolicy
	{
		public const int DENIED = -1;
		public const int READ = 0;
		public const int WRITE = 1;
		public const int CREATE = 2;
		public const int DELETE = 3;

		private int[,] acl;	//Access Control list matrix
		public int numSubject,numObject;

		public Object[] objects;		// Objects in ACL
		public Object[] subjects;		// Subjects in ACl

		//public Recipient[] recList;
		//public FileAttachment[] faList;
		public GUIPolicy()
		{
			numObject = 0;
			numSubject = 0;
			objects = null;
			subjects = null;
		}
		/// <summary>
		/// Create the Policy with the Subjects list
		/// </summary>
		/// <param name="vsubjects"></param>
		public GUIPolicy(Object[] vsubjects)
		{
			subjects = vsubjects;
			this.numSubject = subjects.Length;
			numObject = 0;
		}
		/// <summary>
		/// After adding a new subject, the ACL matrix need be updated.
		/// Before invoke this method, the subjects list must be updated at first.
		/// </summary>
		public void addSubject()
		{
			numSubject = subjects.Length;
			if (numObject > 0)	// There is at least one object. Otherwise it is unnecessary to construct the ACL
			{
				int[,] aclTemp = acl;
				acl = new int [numSubject,numObject];

				if (aclTemp != null)		// It is not the first time that a subject is added
					for(int i=0;i<numSubject;i++)
						for(int j=0;j<numObject;j++)
							acl[i,j] = aclTemp[i,j];
				
				// Assign an action for all attachments to the new subject
				for(int j=0;j<numObject;j++)
					acl[numSubject-1,j] = GUIPolicy.DENIED;
			}
		}

		/// <summary>
		/// After adding a new object, the ACL matrix need be updated.
		/// Before execute this, the objects list must be updated at first.
		/// </summary>
		public void addObject()
		{
			numObject = objects.Length;
			if(numSubject > 0)	// There is at least one subject
			{
				int[,] aclTemp = acl;
				acl = new int[numSubject,numObject];
				
				// Duplicate the original ACL
				if(aclTemp != null)
					for(int i=0;i<numSubject;i++)
						for(int j=0;j<numObject-1;j++)
							acl[i,j] = aclTemp[i,j];
				
				// Add a new DENIED colunm in acl for newly added Object
				for(int i = 0; i<numSubject;i++)
					acl[i,numObject-1] = GUIPolicy.DENIED;
			}
		}
		/// <summary>
		/// Set the action value of an element in the ACL matrix
		/// </summary>
		/// <param name="indexS">Index of Subject</param>
		/// <param name="indexO">Index of Object</param>
		/// <param name="action">Action assigned</param>
		public void setACL(int indexS, int indexO,int action)
		{
			acl[indexS,indexO] = action;
		}
		/// <summary>
		/// Get the operation permission from Access Control Lis
		/// </summary>
		/// <param name="indexS">Index of Subject</param>
		/// <param name="indexO">Index of Object</param>
		/// <returns>The code of operation permission</returns>
		public int getPermission(int indexS,int indexO)
		{
			if (this.numObject < 1|| this.numSubject < 1)	// If Ojbects or Subjects is null
				return GUIPolicy.DENIED;
			else
				return acl[indexS,indexO];
		}
		/// <summary>
		/// Remove Subject from ACL matrix. The corresponding line is removed..
		/// Beore invoke this method, the Subject list must be updated in advance
		/// </summary>
		/// <param name="indexS">The index of the removed Subject</param>
		public void removeSubject(int indexS)
		{
			numSubject = subjects.Length;
			// Overwirte the line which is corresponding to the removed subject
			for(int i=indexS;i<numSubject;i++)
				for(int j=0;j<numObject;j++)
					acl[i,j] = acl[i+1,j];

			int[,] aclTemp = acl;
			acl = new int[numSubject,numObject];
			for(int i=0;i<numSubject;i++)
				for(int j=0;j<numObject;j++)
					acl[i,j] = aclTemp[i,j];
		}
		/// <summary>
		/// Remove Ojbect from ACL matrix. The corresponding column is removed. 
		/// Before invoke this method, the Object list must be updated at first
		/// </summary>
		/// <param name="indexO">The index of removed ojbect</param>
		public void removeObject(int indexO)
		{
			numObject = objects.Length;
			// Overwrite the column which is corresponding to the removed object
			for(int i=0;i<this.numSubject;i++)
				for(int j= indexO;j<this.numObject;j++)
					acl[i,j] = acl[i,j+1];

			// Create new acl matrix
			int[,] aclTemp = acl;
			acl = new int[numSubject,numObject];
			for(int i= 0;i<numSubject;i++)
				for(int j=0;j<numObject;j++)
					acl[i,j] = aclTemp[i,j];

		}
	}
}
*/