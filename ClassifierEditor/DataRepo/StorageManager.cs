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
#pragma warning disable CS0246 // The type or namespace name 'BaseSettings<,,>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'StorageMgrPath' could not be found (are you missing a using directive or an assembly reference?)
			BaseSettings<StorageMgrPath,
#pragma warning restore CS0246 // The type or namespace name 'StorageMgrPath' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'StorageMgrInfo<>' could not be found (are you missing a using directive or an assembly reference?)
			StorageMgrInfo<T>, T> { }
#pragma warning restore CS0246 // The type or namespace name 'BaseSettings<,,>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'StorageMgrInfo<>' could not be found (are you missing a using directive or an assembly reference?)

	#endregion

	#region public properties

		[DataMember(Order = 1)]
		public static T Data => DataStore.Data;
		public bool Initialized => DataStore.Admin.Path.HasFilePath;
#pragma warning disable CS0246 // The type or namespace name 'SettingsMgr<,,>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'StorageMgrPath' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'StorageMgrInfo<>' could not be found (are you missing a using directive or an assembly reference?)
		public static SettingsMgr<StorageMgrPath, StorageMgrInfo<T>, T> Admin => DataStore.Admin;
#pragma warning restore CS0246 // The type or namespace name 'StorageMgrInfo<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'StorageMgrPath' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SettingsMgr<,,>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'StorageMgrInfo<>' could not be found (are you missing a using directive or an assembly reference?)
		public static StorageMgrInfo<T> Info => DataStore.Info;
#pragma warning restore CS0246 // The type or namespace name 'StorageMgrInfo<>' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'StorageMgrPath' could not be found (are you missing a using directive or an assembly reference?)
		public static StorageMgrPath Path => Admin.Path;
#pragma warning restore CS0246 // The type or namespace name 'StorageMgrPath' could not be found (are you missing a using directive or an assembly reference?)

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
			DataStore.Admin.Path.RootFolderPath = rootPath;
			DataStore.Admin.Path.FileName = filename;

			DataStore.Admin.Path.ConfigureFilePath();

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
