#region + Using Directives

#endregion


// projname: Sylvester.SelectFolder
// itemname: UserSettingData30
// username: jeffs
// created:  1/18/2020 9:23:40 PM


using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Sylvester.SavedFolders;

namespace Sylvester.Settings
{

	public partial class UserSettingData30
	{
		private void initialize()
		{
			if (SavedFolders == null)
			{
				SavedFolders = new List<ObservableCollection<SavedProject>>(2);
				SavedFolders.Add(new ObservableCollection<SavedProject>());
				SavedFolders.Add(new ObservableCollection<SavedProject>());
			}
		}

		[DataMember]
		public List<ObservableCollection<SavedProject>> SavedFolders;
	}
}