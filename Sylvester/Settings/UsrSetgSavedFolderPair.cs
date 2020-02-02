// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SavedFolderPair.cs
// Created:      -- ()

using System.Runtime.Serialization;
using Sylvester.FileSupport;

namespace Sylvester.SavedFolders {
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