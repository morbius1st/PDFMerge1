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
using UtilityLibrary;

namespace SettingsManager
{
	/*

		data map:
		savedfolderpair (data structure only)
		-> holds a filepath for current
		-> holds a filepath for revision
		-> holds management info

		savedfolderproject
		-> holds a collection of savedfolderpairs associated with a single project
		-> holds information about the project
		-> holds management info

		SavedFolders
		-> is a list of (2) collections of savedfolderproject type
			-> [0] = favorites
			-> [1] = history

		SavedFolders.SavedFolderType is the associated enum (at SavedFolderManager)

	*/

	public partial class UserSettingDataFile
	{
		private void initializeSavedFolders()
		{
			if (SavedFolders == null)
			{ 
				SavedFolders = new List<ObservableCollection<SavedFolderProject>>(SavedFolderType.COUNT.Value());
				SavedFolders.Add(new ObservableCollection<SavedFolderProject>());
				SavedFolders.Add(new ObservableCollection<SavedFolderProject>());
			}
		}

		[DataMember]
		public List<ObservableCollection<SavedFolderProject>> SavedFolders;
	}
}