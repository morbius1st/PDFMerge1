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
using StoreAndRead.TestData.Data2;

#endregion

// username: jeffs
// created:  4/30/2020 6:14:39 AM

namespace StoreAndRead.DataMgr
{
	public class DataManager2 : INotifyPropertyChanged
	{
		#region private fields

		private StorageManager<TestDataData2> testData2;

		#endregion

		#region ctor

			public DataManager2()
			{
				testData2 = new StorageManager<TestDataData2>();
			}

		#endregion

		#region public properties

			public bool IsConfigured { get; private set; } = false;

			public string Description => testData2.Data.Description;

			public ObservableCollection<TestItem> Root => testData2.Data.TestDataRoot;

		#endregion

		#region private properties

		#endregion

		#region public methods

			public void Configure(string rootPath, string filename)
			{
				if (testData2 == null) return;

				testData2.Configure(rootPath, filename);

				IsConfigured = true;
			}

			public void Read()
			{
				if (!IsConfigured) return;

				testData2.Read(); 
				OnPropertyChange("Description");
				OnPropertyChange("Root");
			}

			public void Write()
			{
				if (!IsConfigured) return;

				testData2.Write();
			}

			public void LoadSampleData()
			{
				if (!IsConfigured) return;

				GenerateTestData2 genTd1 = new GenerateTestData2(" (TD2)");

				testData2.Data.TestDataRoot = genTd1.LoadSampleData();

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
			return "this is DataManager2";
		}

		#endregion
	}
}
