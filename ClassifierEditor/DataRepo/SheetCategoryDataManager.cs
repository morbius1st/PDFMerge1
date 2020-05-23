#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using ClassifierEditor.Tree;

#endregion

// username: jeffs
// created:  5/12/2020 10:12:49 PM

namespace ClassifierEditor.DataRepo
{
	[DataContract(Name = "Categories", Namespace = "")]
	public class SheetCategoryDataManager : INotifyPropertyChanged
	{
	#region private fields

		private StorageManager<SheetCategoryData> shtCategories;

	#endregion

	#region ctor

		public SheetCategoryDataManager()
		{
			shtCategories = new StorageManager<SheetCategoryData>();
		}

	#endregion

	#region public properties

		public bool IsConfigured { get; private set; } = false;

		private bool isModified = false;

		public bool IsModified
		{
			get => isModified;
			private set
			{
				isModified = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 1)]
		public string Description => shtCategories.Data.Description;

		[DataMember(Order = 2)]
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

			if (TreeBase == null)
			{
				shtCategories.Data.TreeBase = new TreeNode();

				IsModified = false;
			}

			OnPropertyChange("TreeBase");
		}

		public void Write()
		{
			if (!IsConfigured) return;

			shtCategories.Write();

			IsModified = false;
		}

		public void LoadSampleData()
		{
			if (!IsConfigured) return;

			SampleData sd = new SampleData();

			sd.Sample(TreeBase);

//			OnPropertyChange("TreeBase");

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