#region using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StoreAndRead.StoreMgr;
using StoreAndRead.TestData;
using StoreAndRead.TestData.Data1;

#endregion

// username: jeffs
// created:  4/30/2020 8:08:43 PM

namespace StoreAndRead.DataMgr
{
	public class DataManager1 : INotifyPropertyChanged
	{
	#region private fields

		private StorageManager<TestDataData1> testData1;

	#endregion

	#region ctor

		public DataManager1()
		{
			testData1 = new StorageManager<TestDataData1>();
		}

	#endregion

	#region public properties

		public bool IsConfigured { get; private set; } = false;

		public string Description => testData1.Data.Description;

		public ObservableCollection<TestItem> Root => testData1.Data.TestDataRoot;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure(string rootPath, string filename)
		{
			if (testData1 == null) return;

			testData1.Configure(rootPath, filename);

			IsConfigured = true;
		}

		public void Read()
		{
			if (!IsConfigured) return;

			testData1.Read();
			OnPropertyChange("Description");
			OnPropertyChange("Root");
		}

		public void Write()
		{
			if (!IsConfigured) return;

			testData1.Write();
		}

		public void LoadSampleData()
		{
			if (!IsConfigured) return;

			GenerateTestData1 genTd1 = new GenerateTestData1(" (TD1)");

			testData1.Data.TestDataRoot = genTd1.LoadSampleData();

			OnPropertyChange("Root");
		}

	#endregion

	#region private methods

	#endregion

	#region event processing

	#endregion

	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is DataManager1";
		}

	#endregion
	}
}