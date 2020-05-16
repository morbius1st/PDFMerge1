#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using ClassifierEditor.NumberComponent;

#endregion

// username: jeffs
// created:  5/12/2020 10:12:49 PM

namespace ClassifierEditor.DataRepo
{

	[DataContract(Name = "Categories", Namespace = "")]
	public class DataManagerCategories : INotifyPropertyChanged
	{
	#region private fields

		
		private StorageManager<SheetCategories> shtCategories;

	#endregion

	#region ctor

		public DataManagerCategories()
		{
			shtCategories = new StorageManager<SheetCategories>();
		}

	#endregion

	#region public properties

		public bool IsConfigured { get; private set; } = false;

		[DataMember(Order = 1)]
		public string Description => shtCategories.Data.Description;

		[DataMember(Order = 1)]
		public TreeNode TreeBase => shtCategories.Data.TreeBase;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure(string rootPath, string filename)
		{
			if (shtCategories == null) return;

			shtCategories.Configure(rootPath, filename);

			IsConfigured = true;
		}

		public void Read()
		{
			if (!IsConfigured) return;

			shtCategories.Read();

			if (TreeBase == null) shtCategories.Data.TreeBase = new TreeNode();

			OnPropertyChange("Description");
			OnPropertyChange("TreeBase");
		}

		public void Write()
		{
			if (!IsConfigured) return;

			shtCategories.Write();
		}

		public void LoadSampleData()
		{
			if (!IsConfigured) return;

			SampleData sd = new SampleData();

			sd.Sample(TreeBase);

			OnPropertyChange("TreeBase");

			shtCategories.Data.NotifyUpdate();
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