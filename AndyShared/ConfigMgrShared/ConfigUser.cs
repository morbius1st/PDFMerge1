#region using directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SettingsManager;

#endregion


// projname: $projectname$
// itemname: ConfigUser
// username: jeffs
// created:  7/28/2020 7:52:37 PM

namespace AndyShared.ConfigMgrShared
{
	public class ConfigUser : INotifyPropertyChanged
	{
	#region private fields

		private static readonly Lazy<ConfigUser> instance =
			new Lazy<ConfigUser>(() => new ConfigUser());

		private bool isRead;

	#endregion

	#region ctor

		private ConfigUser() { }

	#endregion

	#region public properties

		public static ConfigUser Instance => instance.Value;

		public string SettingFolderPath => UserSettings.Path.SettingFolderPath;

		public string Description => UserSettings.Info.Description;

		public string LastClassificationFileId
		{
			get => UserSettings.Data.LastClassificationFileId;

			set
			{
				UserSettings.Data.LastClassificationFileId = value;

				Write();

				OnPropertyChange();
			}
		}


		public bool IsRead
		{
			get => isRead;
			set
			{
				isRead = value;

				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Read()
		{
			UserSettings.Admin.Read();

			IsRead = true;
		}

		public void Write()
		{
			UserSettings.Admin.Write();
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
			return "this is ConfigUser";
		}

	#endregion
	}
}