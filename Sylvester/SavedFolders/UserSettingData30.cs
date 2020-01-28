#region + Using Directives

#endregion


// projname: Sylvester.SelectFolder
// itemname: UserSettingData30
// username: jeffs
// created:  1/18/2020 9:23:40 PM


using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Sylvester.FileSupport;

namespace Sylvester.Settings
{

	public partial class UserSettingData30
	{
		private void SavedFolderCtor()
		{
			SavedFolders = new List<Dictionary<string, SavedProject>>(2);

			SavedFolders.Add(new Dictionary<string, SavedProject>());

			SavedFolders.Add(new Dictionary<string, SavedProject>());
		}

		[DataMember]
		public List<Dictionary<string, SavedProject>> SavedFolders;

		[DataMember]
		public List<ObservableCollection<SavedProject>> SavedFolders2;
	}

	[DataContract]
	public class SavedProject
	{
		// this will be the root folder name
		public struct FolderRoot
		{
			[DataMember]
			public string Volume { get; set; }

			[DataMember]
			public string RootFolder { get; set; }
		}

		[DataMember]
		public FolderRoot Identifier { get; set; }

		[DataMember]
		public string Key { get; set; }

		[DataMember]
		public string Name { get; set; }
		
		[DataMember]
		public string Icon { get; set; }

		[DataMember]
		public int UseCount { get; set; }

		[DataMember]
		public Dictionary<string, SavedFolderPair> SavedFolderPairs { get; set; } =
			new Dictionary<string, SavedFolderPair>() ;

		public SavedProject() { }

		public SavedProject(string volume, string rootFolder, string name = "")
		{
			Identifier = new FolderRoot()
			{
				Volume = volume,
				RootFolder = rootFolder
			};
			UseCount = 0;

			Name = string.IsNullOrWhiteSpace(name) ? Identifier.RootFolder : name;

			Key = MakeSavedFolderKey(UseCount, Name);

			Icon = null;
		}

		public static string MakeSavedFolderKey(int useCount, string name)
		{
			return $"{useCount:D5} / " + name;
		}
	}

	[DataContract]
	public class SavedFolderPair
	{
		[DataMember]
		public string Key { get; set; }

		[DataMember]
		public string Icon { get; set; }

		[DataMember]
		public Route Current { get; set; }

		[DataMember]
		public Route Revision { get; set; }

		public SavedFolderPair() { }

		public SavedFolderPair(Route current,
			Route revision, string name = "")
		{
			Icon = null;
			Current = current;
			Revision = revision;

			Key = MakeCurrRevFolderPairkey(current.FolderName(-1), revision.FolderName(-1), name);
		}

		public static string MakeCurrRevFolderPairkey(string currentRootFolder, string revisionRootFolder, string name = "")
		{
			if (!string.IsNullOrWhiteSpace(name)) return name;

			return currentRootFolder + " / " + revisionRootFolder;
		}
	}
}