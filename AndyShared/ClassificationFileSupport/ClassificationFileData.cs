#region using directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AndyShared.ClassificationDataSupport.TreeSupport;
using SettingsManager;

#endregion

// username: jeffs
// created:  5/12/2020 10:06:41 PM

namespace AndyShared.ClassificationFileSupport
{
	

	// this is the actual data set saved to the data file
	[DataContract(Name = "SheetCategoryData", Namespace = "", IsReference = true)]
	public class ClassificationFileData : INotifyPropertyChanged
	{

		public static string FILE_DESCRIPTION = "ClassificationFileData";

	#region public properties

		[DataMember(Order = 0)]
		public string FileDescription { get; private set; } = FILE_DESCRIPTION;

		[DataMember(Order = 1)]
		public string Description { get; set; } = "This is a full list of sheet organization categories";

		[DataMember(Order = 2)]
		public bool UsePhaseBldg { get; set; }

		[DataMember(Order = 3)]
		public string SampleFile { get; set; } = null;

		[DataMember(Order = 10)]
		public BaseOfTree BaseOfTree { get; set; } = new BaseOfTree();

	#endregion

//		#region private properties
//		#endregion
//
		#region public methods

			public void NotifyUpdate()
			{
				OnPropertyChange("BaseOfTree");
			}

		#endregion

//		#region private methods
//		#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

//		#region event handeling
//		#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SheetCategoryData";
		}

	#endregion
	}



//
// 	public class ClassfFile<T> where T : class, new()
// 	{
//
// 	#region private fields
//
// 		public class DataStore :
// 			BaseSettings<StorageMgrPath,
// 			StorageMgrInfo<T>, T> { }
//
// 	#endregion
//
// 	#region public properties
//
// 		[DataMember(Order = 1)]
// 		public T Data => DataStore.Data;
//
// 		public bool Initialized => DataStore.Admin.Path.HasPathAndFile;
// 		public SettingsMgr<StorageMgrPath, StorageMgrInfo<T>, T> Admin => DataStore.Admin;
// 		public StorageMgrInfo<T> Info => DataStore.Info;
// 		public StorageMgrPath Path => Admin.Path;
//
// 	#endregion
//
// 	#region public methods
//
// //
// // 		public bool Read()
// // 		{
// // 			if (!Initialized) return false;
// //
// // 			DataStore.Admin.Read();
// //
// // //			OnPropertyChange("Data");
// //
// // 			return true;
// // 		}
// //
// // 		public bool Write()
// // 		{
// // 			if (!Initialized) return false;
// //
// // 			DataStore.Admin.Write();
// //
// // 			return true;
// // 		}
//
//
//
// 		public void Configure(string rootPath, string filename)
// 		{
// 			DataStore.Admin.Path.SubFolders = null;
// 			DataStore.Admin.Path.RootFolderPath = rootPath;
// 			DataStore.Admin.Path.FileName = filename;
//
// 			DataStore.Admin.Path.ConfigurePathAndFile();
//
// 			DataStore.Admin.Read();
//
// 			// OnPropertyChange("Initialized");
// 		}
//
// 	#endregion
//
// 	}
}