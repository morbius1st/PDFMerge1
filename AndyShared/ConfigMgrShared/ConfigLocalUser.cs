#region using directives

using System;
using System.Management;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using AndyShared.ConfigSupport;
using UtilityLibrary;

#endregion


// projname: $projectname$
// itemname: ConfigLocalUser
// username: jeffs
// created:  7/11/2020 4:55:34 PM

namespace AndyShared.ConfigMgrShared
{
	public class ConfigLocalUser : INotifyPropertyChanged
	{
	#region private fields

		private static readonly Lazy<ConfigLocalUser> instance =
			new Lazy<ConfigLocalUser>(() => new ConfigLocalUser());

		private ObservableCollection<ConfigFile<FileNameSimple>> users =
			new ObservableCollection<ConfigFile<FileNameSimple>>();

		private LocalUserList userList;

	#endregion

	#region ctor

		private ConfigLocalUser()
		{
			
		}

	#endregion

	#region public properties

		public static ConfigLocalUser Instance => instance.Value;

		public bool Initialized { get; set; }

		public LocalUserList LocalUserList { get; set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			if (Initialized) return;


		}

	#endregion

	#region private methods


	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ConfigLocalUser";
		}

	#endregion
	}
}