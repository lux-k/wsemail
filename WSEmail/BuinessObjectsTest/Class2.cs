using System.Collections;
using System.Windows.Forms;
using System;

namespace BusinessObjects
{
	/// <summary>
	/// Summary description for Class2.
	/// </summary>

	public interface BusinessObjectsInterface
	{
		string ObjectType {get;set;}
		string FormName {get;set;}
	}


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

	public class EmployeeInformation 
	{
		string _strName, _strTitle, _strDept, _strID;
		float _fHoursW;

		#region BoringProperties
		public string Name 
		{
			get 
			{
				return _strName;
			}
			set 
			{
				_strName = value;
			}
		}

		public string IDNum 
		{
			get 
			{
				return _strID;
			}
			set 
			{
				_strID = value;
			}
		}

		public string Title 
		{
			get 
			{
				return _strTitle;
			}
			set 
			{
				_strTitle = value;
			}
		}

		public string Department 
		{
			get 
			{
				return _strDept;
			}
			set 
			{
				_strDept = value;
			}
		}

		public float HoursPerWeek 
		{
			get 
			{
				return _fHoursW;
			}
			set 
			{
				_fHoursW = value;
			}
		}
		#endregion
	
		public EmployeeInformation() 
		{
			Name = "";
			Title = "";
			Department = "";
			IDNum = "";
			HoursPerWeek = 0;
		}

		public EmployeeInformation(string EmpName, string EmpID, string EmpTitle, string EmpDept, float EmpHours) 
		{
			Name = EmpName;
			IDNum = EmpID;
			Title = EmpTitle;
			Department = EmpDept;
			HoursPerWeek = EmpHours;
		}
	}

	public class HoursType 
	{
		string _strDay, _strPTC;
		float _fReg, _fOvr, _fPTO, _fTot;

		public delegate void TotalChangedEvent();
		public event TotalChangedEvent RegularHoursChanged;
		public event TotalChangedEvent OvertimeHoursChanged;
		public event TotalChangedEvent PaidTimeOffHoursChanged;

		public HoursType() {}

		private void SumHours() 
		{
			Total = Regular + Overtime + PaidTimeOff;
		}

		public string Day 
		{
			get 
			{
				return _strDay;
			}
			set 
			{
				_strDay = value;
			}
		}

		public float Regular 
		{
			get 
			{
				return _fReg;
			}
			set 
			{
				_fReg = value;
				SumHours();
				if (RegularHoursChanged != null)
					RegularHoursChanged.DynamicInvoke(null);
			}
		}

		public float Overtime 
		{
			get 
			{
				return _fOvr;
			}
			set 
			{
				_fOvr = value;
				SumHours();
				if (OvertimeHoursChanged != null)
					OvertimeHoursChanged.DynamicInvoke(null);
			}
		}

		public float PaidTimeOff 
		{
			get 
			{
				return _fPTO;
			}
			set 
			{
				_fPTO = value;
				SumHours();
				if (PaidTimeOffHoursChanged != null) 
					PaidTimeOffHoursChanged.DynamicInvoke(null);
			}
		}

		public string PaidTimeCode 
		{
			get 
			{
				return _strPTC;
			}
			set 
			{
				_strPTC = value;
				if (PaidTimeOffHoursChanged != null) 
					PaidTimeOffHoursChanged.DynamicInvoke(null);
			}
		}

		public float Total 
		{
			get 
			{
				return _fTot;
			}
			set 
			{
				_fTot = value;
			}
		}

		public HoursType(int i) 
		{
			switch (i) 
			{
				case 0:
					_strDay = "Sunday";
					break;
				case 1:
					_strDay = "Monday";
					break;
				case 2:
					_strDay = "Tuesday";
					break;
				case 3:
					_strDay = "Wednesday";
					break;
				case 4:
					_strDay = "Thursday";
					break;
				case 5:
					_strDay = "Friday";
					break;
				case 6:
					_strDay = "Saturday";
					break;
			}
		}
	}

	public class Timesheet : BusinessObject, IList
	{
		HoursType[] WeeklyHours = new HoursType[7];
		private float _fTotReg,_fStrOvr,_fPreOvr;
		private EmployeeInformation _eInfo;
		public delegate void TotalChangedEvent();
		public event TotalChangedEvent TotalHoursChanged;

		public EmployeeInformation EmployeeInformation
				{
					get 
					{
						return _eInfo;
					}
					set 
					{
						_eInfo = value;
					}
			}
			
		public float Regular 
		{
			get 
			{
				return _fTotReg;
			}
			set 
			{
				_fTotReg = value;
			}
		}

		public float CalculatedRegular 
		{
			get 
			{
				if (Regular + StraightTimeOvertime > EmployeeInformation.HoursPerWeek)
					return EmployeeInformation.HoursPerWeek;
				else
					return Regular;
			}
		}

		public float CalculatedOvertime 
		{
			get 
			{
				if (Regular + StraightTimeOvertime > EmployeeInformation.HoursPerWeek) 
					return StraightTimeOvertime + Regular - EmployeeInformation.HoursPerWeek;
				else
					return 0;
			}
		}

		public float StraightTimeOvertime
		{
			get 
			{
				return _fStrOvr;
			}
			set 
			{
				_fStrOvr = value;
			}
		}

		public float PremiumOvertime
		{
			get 
			{
				if (Regular + StraightTimeOvertime > 40)
					return Regular + StraightTimeOvertime -  40;
				else
					return 0;
			}
			set 
			{
				_fPreOvr = value;
			}
		}


		public Timesheet() 
		{
			HoursType h;
			EmployeeInformation = new EmployeeInformation();
			for (int i = 0; i < 7; i++) 
			{
				h = new HoursType(i);
				h.OvertimeHoursChanged += new HoursType.TotalChangedEvent(UpdateOvertime);
				h.RegularHoursChanged += new HoursType.TotalChangedEvent(UpdateRegular);
				WeeklyHours[i] =h;
			}

		}

		public void RecalculateTotals() 
		{
			if (TotalHoursChanged != null)
				TotalHoursChanged.DynamicInvoke(null);
		}

		public void UpdateOvertime() 
		{
			StraightTimeOvertime = 0;
			for (int i = 0; i < 7; i++)
				StraightTimeOvertime += WeeklyHours[i].Overtime;
			RecalculateTotals();
		}

		public void UpdateRegular() 
		{
			Regular = 0;
			for (int i = 0; i < 7; i++)
				Regular += WeeklyHours[i].Regular;
			// MessageBox.Show("Regular hours now " + Regular);
			RecalculateTotals();
		}

		public DataGridTableStyle GetDataGridTableStyle() 
		{
			DataGridTableStyle gs = new DataGridTableStyle();
			gs.MappingName = this.GetType().Name;
			
			DataGridTextBoxColumn cs = new DataGridTextBoxColumn();
			cs.MappingName = "Day";
			cs.HeaderText = "";
			cs.Width = 80;
			cs.ReadOnly = true;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Regular";
			cs.HeaderText = "Regular Hours Worked";	
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 120;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Overtime";
			cs.HeaderText = "Overtime Hours";
			cs.Width = 85;
			cs.Alignment = HorizontalAlignment.Right;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "PaidTimeOff";
			cs.HeaderText = "Paid Time Off Hours";
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 110;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "PaidTimeCode";
			cs.HeaderText = "Code";
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 33;
			cs.TextBox.CharacterCasing = CharacterCasing.Upper;
			gs.GridColumnStyles.Add(cs);

			cs = new DataGridTextBoxColumn();
			cs.MappingName = "Total";
			cs.HeaderText = "Total Hours";
			cs.Alignment = HorizontalAlignment.Right;
			cs.Width = 65;
			gs.GridColumnStyles.Add(cs);
			cs.ReadOnly = true;

			return gs;
		}


		#region IListImplementation
 
		public bool IsFixedSize 
		{
			get 
			{
				return true;
			}
		}

		public bool IsReadOnly 
		{
			get 
			{
				return false;
			}
		}

		public void CopyTo(Array array,	int index) 
		{
			if (array == null)
				throw new ArgumentNullException();

			if (index < 0)
				throw new ArgumentOutOfRangeException();

			if (index >= array.Length || index + WeeklyHours.Length >= array.Length || array.Rank > 1) 
			
				throw new ArgumentException();
			
			for (int i = 0; i < 7; i++)
				array.SetValue(WeeklyHours[i],index + i);

		}
	
		public object this[int index] 
		{
			get 
			{
				return (HoursType)WeeklyHours[index];
			}
			
			set 
			{
				WeeklyHours[index] = (HoursType)value;
			}
		}

		public void Clear() 
		{
		}

		public int Count 
		{
			get 
			{
				return 7;
			}
		}

		public bool IsSynchronized {get { return false;}}
		public object SyncRoot 
		{
			get 
			{
				return this;
			}
		}
		public IEnumerator GetEnumerator() 
		{
			return WeeklyHours.GetEnumerator();
		}

		public bool Contains(object o) 
		{
			for (int i = 0; i < 7; i++)
				if (WeeklyHours[i].Equals(o))
					return true;
			return false;
		}

		public int IndexOf(object o) 
		{
			for (int i = 0; i < 7; i++)
				if (WeeklyHours[i].Equals(o))
					return i;
			return -1;
		}

		public void Insert(int index, object value) 
		{
			throw new NotSupportedException();
		}

		public void Remove(object value) 
		{
			throw new NotSupportedException();
		}

		public void RemoveAt(int i) 
		{
			throw new NotSupportedException();
		}

		public int Add(Object o) 
		{
			//			throw new NotSupportedException();
			return -1;
		}
		#endregion
	}

	public class PurchaseOrder : BusinessObject
	{
		public int PurchaseAmount = 0;
	}




}
