#region using directives

using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SettingsManager;
using UtilityLibrary;

#endregion

// projname: StoreAndRead
// itemname: StorageManager
// username: jeffs
// created:  4/11/2020 4:50:30 PM

/*
	{path/fileName} provided by owner

	get data from the file
	save data to file
	create default data source / save to file
	reset data to the default
	default must be provided
	
	holds data

	must be generic to be able to hold any type of data object

	there may be multiples of this class, but there is a
	one to one between data object and file

*/

namespace StoreAndRead.StoreMgr
{
	public class StorageManager<T> : INotifyPropertyChanged
		where T : class, new ()
	{

	#region private fields

		private class DataStore :
			BaseSettings<StorageMgrPath,
			StorageMgrInfo<T>, T> { }

	#endregion

	#region public properties

		public bool Initialized => DataStore.Admin.Path.HasPathAndFile;
		public T Data => DataStore.Data;
		public  SettingsMgr<StorageMgrPath, StorageMgrInfo<T>, T> Admin => DataStore.Admin;
		public StorageMgrInfo<T> Info => DataStore.Info;

	#endregion

	#region public methods


		public bool Read()
		{
			if (!Initialized) return false;

			DataStore.Admin.Read();

			OnPropertyChange("Data");

			return true;
		}

		public bool Write()
		{
			if (!Initialized) return false;

			DataStore.Admin.Write();

			return true;
		}

		public void Configure(string rootPath, string filename)
		{
			DataStore.Admin.Path.SubFolders = null;
			DataStore.Admin.Path.RootPath = rootPath;
			DataStore.Admin.Path.FileName = filename;

			DataStore.Admin.Path.ConfigurePathAndFile();

			OnPropertyChange("Initialized");
		}

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
			return "this is StorageManager";
		}

	#endregion
	}


}