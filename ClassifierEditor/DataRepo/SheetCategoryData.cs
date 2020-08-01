#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using ClassifierEditor.Tree;
using SettingsManager;

#endregion

// username: jeffs
// created:  5/12/2020 10:06:41 PM

namespace ClassifierEditor.DataRepo
{
	// // this is the actual & un-configured datastore 
	// public class ClassfDataFile :
	// 	BaseSettings<StorageMgrPath,
	// 	StorageMgrInfo<SheetCategoryData>, SheetCategoryData>, INotifyPropertyChanged
	// {
	//
	// #region public properties
	//
	// 	public bool Initialized => Path.HasPathAndFile;
	//
	// #endregion
	//
	// #region public methods
	//
	// 	public void Configure(string rootPath, string filename)
	// 	{
	// 		Path.SubFolders = null;
	// 		Path.RootFolderPath = rootPath;
	// 		Path.FileName = filename;
	//
	// 		Path.ConfigurePathAndFile();
	//
	// 		OnPropertyChange("Initialized");
	// 	}
	//
	// #endregion
	//
	// #region event processing
	//
	// 	public event PropertyChangedEventHandler PropertyChanged;
	//
	// 	private void OnPropertyChange([CallerMemberName] string memberName = "")
	// 	{
	// 		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
	// 	}
	//
	// #endregion
	//
	// #region system overrides
	//
	// 	public override string ToString()
	// 	{
	// 		return "this is ClassfDataFile";
	// 	}
	//
	// #endregion
	// }


	// this is the actual data set saved to the data file
	[DataContract(Name = "SheetCategoryData", Namespace = "", IsReference = true)]
	public class SheetCategoryData : INotifyPropertyChanged
	{
	#region public properties

		[DataMember(Order = 1)]
		public string Description { get; private set; } = "This is a full list of sheet organization categories";

		[DataMember(Order = 2)]
		public bool UsePhaseBldg { get; set; }
		
		[DataMember(Order = 3)]
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
}