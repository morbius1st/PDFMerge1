#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
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
	public class DisciplinesDataSet : INotifyPropertyChanged
	{

	#region data set fields

		[IgnoreDataMember]
		public string DataFileDescription { get; set; } = "Discipline DataSet";

		[IgnoreDataMember]
		public string DataFileNotes { get; set; } = "Contains all disciplines and associated data";

		[IgnoreDataMember]
		public string DataFileVersion { get; set; } = "0.1";

		[DataMember]
		public ObservableDictionary<string, DisciplineListData> DisciplineList { get; set; } = new ObservableDictionary<string, DisciplineListData>();

		[DataMember]
		public List<string> ComboDisciplines { get; set; } = new List<string>();

	#endregion

	#region data set functions

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

			DisciplineListData dd = Find(currentKey);

			dd.Key = kn;
			dd.DisciplineData.Key = newKey.Trim();

			DisciplineList.ReplaceKey(currentKey, kn);

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
			if (DisciplineList.ContainsKey(dld.Key)) return;

			DisciplineList.Add(dld.Key, dld);
		}

		public DisciplineListData Add(string key, string title, string desc)
		{
			DisciplineData dd = new DisciplineData(key, title, desc);
			DisciplineListData dld = new DisciplineListData(dd);
			DisciplineList.Add(dld.Key, dld);

			return dld;
		}

		public bool Remove(string key)
		{
			if (key==null || !DisciplineList.ContainsKey(key)) return false;

			return DisciplineList.Remove(key);
		}

	#endregion



		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	/// <summary>
	/// a single discipline entry in the dictionary<br/>
	/// this contains a dictionary of alternate sheet number styles
	/// </summary>
	[DataContract(Namespace = "")]
	public class DisciplineListData : INotifyPropertyChanged
	{
		public DisciplineListData(DisciplineData disciplineData)
		{
			Key = disciplineData.Key;
			DisciplineData = disciplineData;
		}

		private string key;

		/// <summary>
		/// the key used to retrieve the data from the dictionary<br/>
		/// provide just the discipline designator letter(s)<br/>
		/// this is then formatted as "X␢␢␢␢" (letter(s)+ left justified with blanks
		/// </summary>
		[DataMember]
		public string Key
		{
			get => key;
			set
			{
				if (value.Trim().Equals(DisciplineData?.Key ?? null)) return;
				key = FormatKey(value);
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisciplineData));
			}
		}

		[DataMember]
		public DisciplineData DisciplineData { get; set; }


		public static string FormatKey(string key)
		{
			return $"{key.Trim(),-5}";
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	[DataContract(Namespace = "")]
	public class DisciplineData : INotifyPropertyChanged
	{
		private string title;
		private string key;
		private string description;

		public DisciplineData(string key, string title, string description)
		{
			Key = key;
			Title = title;
			Description = description;
		}

		[DataMember]
		public string Key
		{
			get => key;
			set
			{
				key = value;
				OnPropertyChanged();
			}
		}

		[DataMember]
		public string Title
		{
			get => title;
			set
			{
				title = value;
				OnPropertyChanged();
			}
		}

		[DataMember]
		public string Description
		{
			get => description;
			set
			{
				description = value;
				OnPropertyChanged();
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

#endregion
}