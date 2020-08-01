#region using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClassifierEditor.Tree;

#endregion

// username: jeffs
// created:  5/12/2020 10:12:49 PM

namespace ClassifierEditor.DataRepo
{

//	[DataContract(Name = "Categories", Namespace = "")]
	public class SheetCategoryDataManager : INotifyPropertyChanged
	{
	#region private fields

		private StorageManager<SheetCategoryData> storageMgr;
		private bool isModified = false;

	#endregion

	#region ctor

		public SheetCategoryDataManager()
		{
			storageMgr = new StorageManager<SheetCategoryData>();

			

		}

	#endregion

	#region public properties

		public bool IsConfigured { get; private set; } = false;

		public bool IsModified
		{
			get => isModified;
			set
			{
				isModified = value;
				OnPropertyChange();
			}
		}

//		[DataMember(Order = 1)]
		public string Description => StorageManager<SheetCategoryData>.Data.Description;

//		[DataMember(Order = 2)]
		public BaseOfTree TreeBase => StorageManager<SheetCategoryData>.Data.BaseOfTree;

		public bool UsePhaseBldg => StorageManager<SheetCategoryData>.Data.UsePhaseBldg;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure(string rootPath, string filename)
		{
			if (storageMgr == null) return;

			storageMgr.Configure(rootPath, filename);

			IsConfigured = true;
		}

		public void Read()
		{
			if (!IsConfigured) return;

			storageMgr.Read();

			if (TreeBase == null)
			{
				StorageManager<SheetCategoryData>.Data.BaseOfTree = new BaseOfTree();

				IsModified = false;
			}

			TreeBase.Initalize();

			OnPropertyChange("TreeBase");
		}

		public void Write()
		{
			if (!IsConfigured) return;

			storageMgr.Write();

			IsModified = false;
		}

//		public void LoadSampleData()
//		{
//			if (!IsConfigured) return;
//
//			SampleData sd = new SampleData();
//
//			sd.Sample(TreeBase);
//
//			storageMgr.Data.NotifyUpdate();
//		}


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