using System;
using System.Collections;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BusinessObjects
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	/// 

	public interface BusinessObjectsInterface
	{
		string ObjectType {get;set;}
		string FormName {get;set;}
	}

	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://securitylab.cis.upenn.edu/AuthorizedRouting")]
	public abstract class BusinessObject
	{
		string _objectType;
		string _formName;

		public virtual string ObjectType 
		{
			get 
			{
				return _objectType; }
			set 
			{
				_objectType = value;
			}
		}

		public virtual string FormName 
		{
			get 
			{
				return _formName; }
			set 
			{
				_formName = value;
			}
		}
		
	}
}