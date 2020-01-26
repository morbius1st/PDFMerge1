#region + Using Directives

#endregion


// projname: Sylvester.SelectFolder
// itemname: UserSettingData30
// username: jeffs
// created:  1/18/2020 9:23:40 PM


using System.Collections.Generic;
using System.Runtime.Serialization;
using Sylvester.FileSupport;

namespace Sylvester.Settings
{

	public partial class UserSettingData30
	{
		private void SavedFolderCtor()
		{
			SavedFolders = new List<Dictionary<string, SavedFolder>>(2);

			SavedFolders.Add(new Dictionary<string, SavedFolder>());

			SavedFolders.Add(new Dictionary<string, SavedFolder>());

		}

		[DataMember]
		public List<Dictionary<string, SavedFolder>> SavedFolders;
	}

	[DataContract]
	public class SavedFolder
	{
		// this will be the root folder name
		public struct FolderRoot
		{
			[DataMember]
			public string Volume;

			[DataMember]
			public string RootFolder;
		}

		[DataMember]
		public FolderRoot Identifier;

		[DataMember]
		public string Key;

		[DataMember]
		public string Name;

		[DataMember]
		public int UseCount;

		[DataMember]
		public Dictionary<string, CurrentRevisionFolderPair> FolderPairs =
			new Dictionary<string, CurrentRevisionFolderPair>();

		public SavedFolder() { }

		public SavedFolder(string volume, string rootFolder, string name = "")
		{
			Identifier = new FolderRoot()
			{
				Volume = volume,
				RootFolder = rootFolder
			};
			UseCount = 0;

			Name = string.IsNullOrWhiteSpace(name) ? Identifier.RootFolder : name;

			Key = MakeSavedFolderKey(UseCount, Name);
		}

		public static string MakeSavedFolderKey(int useCount, string name)
		{
			return $"{useCount:D5} / " + name;
		}
	}

	[DataContract]
	public class CurrentRevisionFolderPair
	{
		[DataMember]
		public string Key;

		[DataMember]
		public Route Current;

		[DataMember]
		public Route Revision;

		public CurrentRevisionFolderPair() { }

		public CurrentRevisionFolderPair(Route current,
			Route revision, string name = "")
		{
			Current = current;
			Revision = revision;

			Key = MakeCurrRevFolderPairkey(current.FolderNames[0], revision.FolderNames[0], name);
		}

		public static string MakeCurrRevFolderPairkey(string currentRootFolder, string revisionRootFolder, string name = "")
		{
			if (!string.IsNullOrWhiteSpace(name)) return name;

			return currentRootFolder + " / " + revisionRootFolder;
		}
	}
}