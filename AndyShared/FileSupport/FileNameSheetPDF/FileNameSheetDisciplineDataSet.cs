#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using JetBrains.Annotations;
using UtilityLibrary;
using SettingsManager;
// using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

#endregion

// user name: jeffs
// created:   8/3/2024 9:08:41 PM

namespace AndyShared.FileSupport.FileNameSheetPDF
{
#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	// [CollectionDataContract(Namespace = "", IsReference = true, Name = "ObservableDictionary", ItemName = "KeyValuePair", KeyName = "string", ValueName = "DisciplineListData")]
	public class DisciplinesDataSet : INotifyPropertyChanged, IDataFile
	{
		[IgnoreDataMember]
		private ICollectionView disciplineView;

	#region data set fields
		
		[IgnoreDataMember]
		public string DataFileDescription { get; set; } = "Discipline DataSet";

		[IgnoreDataMember]
		public string DataFileNotes { get; set; } = "Sample Notes. Contains all disciplines and associated data";

		[IgnoreDataMember]
		public string DataFileVersion { get; set; } = "0.1";

		[DataMember]
		// public Dictionary<string, DisciplineListData> DisciplineList { get; set; } = new Dictionary<string, DisciplineListData>();
		public ObservableDictionary<string, DisciplineListData> DisciplineList { get; set; } = new ObservableDictionary<string, DisciplineListData>();

		[DataMember]
		public List<string> ComboDisciplines { get; set; } = new List<string>();

		[IgnoreDataMember]
		public ICollectionView DisciplineView
		{
			get => disciplineView;
			set
			{
				if (Equals(value, disciplineView)) return;
				disciplineView = value;
				OnPropertyChanged();
			}
		}

	#endregion

	#region data set functions

		// collection view
		public void UpdateDisciplineView()
		{
			disciplineView = (ICollectionView) CollectionViewSource.GetDefaultView(DisciplineList.Values);

			SortDescription sd = new SortDescription("OrderCode", ListSortDirection.Ascending);

			disciplineView.SortDescriptions.Add(sd);

			OnPropertyChanged(nameof(DisciplineView));

		}

		// status
		public bool IsNewKey(string key)
		{
			return !HasKey(key);
		}

		public bool HasKey(string key)
		{
			if (key.IsVoid()) return false;

			bool result = DisciplineList.ContainsKey(DisciplineListData.FormatKey(key.Trim()));
			return result;
		}


		// helpers
		public string FormatKey(string key)
		{
			return DisciplineListData.FormatKey(key.Trim());
		}

		// provide both keys unformatted
		// curr key does not exist == false
		// curr key matches new key == null
		public bool? UpdateKey(string currentKey, string newKey)
		{
			string kc = DisciplineListData.FormatKey(currentKey);

			// if key does not exist, return false;
			if (!DisciplineList.ContainsKey(kc)) return false;
			
			string kn = DisciplineListData.FormatKey(newKey);
			// if keys match, return null
			if (kc.Equals(kn)) return null;

			// get the current data
			DisciplineListData dd = Find(currentKey);

			// update the data's key information
			dd.OrderCode = kn;
			dd.DisciplineData.OrderCode = newKey.Trim();

			// remove the current data
			DisciplineList.Remove(kc);
			// add back with new key
			DisciplineList.Add(kn,dd);

			return true;
		}

		public DisciplineListData Find(string key)
		{
			string k = DisciplineListData.FormatKey(key);

			if (!DisciplineList.ContainsKey(k)) return null;

			return DisciplineList[k];
		}


		// operations
		public void Reset()
		{
			DisciplineList = new ObservableDictionary<string, DisciplineListData>();
			ComboDisciplines = new List<string>();
		}

		public void Add(DisciplineListData dld)
		{
			if (DisciplineList.ContainsKey(dld.OrderCode)) return;

			DisciplineList.Add(dld.OrderCode, dld);
		}

		public DisciplineListData Add(string key, string disciplineCode,
			string title, string displayName, string description, bool locked)
		{
			DisciplineData dd = new DisciplineData(key, disciplineCode, title, displayName, description, locked);
			DisciplineListData dld = new DisciplineListData(dd);
			DisciplineList.Add(dld.OrderCode, dld);

			return dld;
		}

		public bool Remove(string key)
		{
			if (key==null || !DisciplineList.ContainsKey(key)) return false;

			return DisciplineList.Remove(key);
		}

	#endregion

	#region management functions

		/// <summary>
		/// makes a shallow copy of the data set (only copies the header info)
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static DisciplinesDataSet Copy(DisciplinesDataSet orig)
		{
			DisciplinesDataSet copy = new DisciplinesDataSet();

			copy.DataFileVersion = orig.DataFileVersion;
			copy.DataFileDescription = orig.DataFileDescription;
			copy.DataFileNotes = orig.DataFileNotes;

			return copy;
		}

	#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	/// <summary>
	/// a single discipline entry in the dictionary<br/>
	/// this contains a dictionary of alternate sheet number styles
	/// </summary>
	[DataContract(Name = "DisciplineListData", IsReference = true)]
	public class DisciplineListData : INotifyPropertyChanged
	{
		public DisciplineListData() {}

		public DisciplineListData(string orderCode, DisciplineData disciplineData)
		{
			OrderCode = orderCode;
			DisciplineData = disciplineData;

			disciplineData.OrderCode = orderCode;
		}

		public DisciplineListData(DisciplineData disciplineData)
		{
			OrderCode = disciplineData.OrderCode;
			DisciplineData = disciplineData;
		}

		private string orderCode;
		private bool dataIsEditing;

		/// <summary>
		/// the key used to retrieve the data from the dictionary<br/>
		/// provide just the discipline designator letter(s)<br/>
		/// this is then formatted as "X␢␢␢␢" (letter(s)+ left justified with blanks
		/// </summary>
		[DataMember(Order = 1)]
		public string OrderCode
		{
			get => orderCode;
			set
			{
				if (value.Trim().Equals(DisciplineData?.OrderCode ?? null)) return;
				orderCode = FormatKey(value);
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisciplineData));
			}
		}

		[DataMember(Order = 4)]
		public DisciplineData DisciplineData { get; set; }

		[IgnoreDataMember]
		public bool DataIsEditing
		{
			get => dataIsEditing & !DisciplineData.Locked;
			set
			{
				dataIsEditing = value;
				OnPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public int Index { get; set; }

		public static string FormatKey(string key)
		{
			return $"{key.Trim(),-5}";
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	/* data adjustments
	 * > change key to order code
	 * > add discipline code
	 */

	[DataContract(Namespace = "")]
	public class DisciplineData : INotifyPropertyChanged
	{
		private string title;
		private string disciplineCode;
		private string orderCode;
		private string displayName;
		private string description;
		private bool locked;

		private bool dataIsSelected;
		private bool dataSave;

		public DisciplineData(string orderCode,
			string disciplineCode,
			string title,
			string displayName,
			string description,
			bool locked)
		{
			OrderCode = orderCode;
			Title = title;
			DisciplineCode = disciplineCode;
			DisplayName = displayName;
			Description = description;
			Locked = locked;
		}

		[DataMember(Order = 0)]
		public string OrderCode
		{
			get => orderCode;
			set
			{
				orderCode = value;
				OnPropertyChanged();
			}
		}

		[DataMember(Order = 1)]
		public string DisciplineCode
		{
			get => disciplineCode;
			set
			{
				// if (value == disciplineCode) return;
				disciplineCode = value;
				OnPropertyChanged();
			}
		}

		[DataMember(Order = 2)]
		public string Title
		{
			get => title;
			set
			{
				title = value;
				OnPropertyChanged();
			}
		}
		
		[DataMember(Order = 3)]
		public string DisplayName
		{
			get => displayName;
			set
			{
				displayName = value;
				OnPropertyChanged();
			}
		}

		[DataMember(Order = 4)]
		public string Description
		{
			get => description;
			set
			{
				description = value;
				OnPropertyChanged();
			}
		}

		[DataMember(Order = 5)]
		public bool Locked
		{
			get => locked;
			set
			{
				locked = value;
				OnPropertyChanged();
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		[DebuggerStepThrough]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

#endregion
}