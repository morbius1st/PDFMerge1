#region using
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SettingsManager;

#endregion

// username: jeffs
// created:  5/12/2020 10:13:22 PM

namespace ClassifierEditor.DataRepo
{
	[DataContract(Name = "DataStore", Namespace = "", IsReference = true)]
	public class StorageManager<T> : INotifyPropertyChanged
		where T : class, new ()
	{

	#region private fields

		private class DataStore :
			BaseSettings<StorageMgrPath,
			StorageMgrInfo<T>, T> { }

	#endregion

	#region public properties

		[DataMember(Order = 1)]
		public T Data => DataStore.Data;
		public bool Initialized => DataStore.Admin.Path.HasPathAndFile;
		public  SettingsMgr<StorageMgrPath, StorageMgrInfo<T>, T> Admin => DataStore.Admin;
		public StorageMgrInfo<T> Info => DataStore.Info;

	#endregion

	#region public methods


		public bool Read()
		{
			if (!Initialized) return false;

			DataStore.Admin.Read();

//			OnPropertyChange("Data");

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
