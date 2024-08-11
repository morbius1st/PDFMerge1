#region + Using Directives

using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows.Media.Animation;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   2/6/2021 6:12:32 AM

namespace StoreAndRead2.FavHistoryAdmin
{
	public enum FilePathType
	{
		CLASSF = 0,
		SAMPLE = 1
	}

	public enum FileStorageType
	{
		SOLO = 0,
		PAIR = 1
	}



	[DataContract(Namespace = "")]
	public abstract class AFavAndHistValue // : IFavAndHist
	{
		// protected static FilePathType pathType { get; }
		protected static FilePathType pathType;

		protected ObservableCollection<FilePath<FileNameSimple>> filePaths;

		[IgnoreDataMember]
		public FilePathType PathType { get; }

		[IgnoreDataMember]
		public FilePath<FileNameSimple> DefaultValue => FilePath<FileNameSimple>.Invalid;

		[IgnoreDataMember]
		public bool IsValid
		{
			get
			{
				bool result = filePaths != null;

				for (int i = 0; i < 1 + (int) pathType; i++)
				{
					if (!result) break;

					result = filePaths[1] != null;
				}

				return result;
			}
		}
	}

	// public interface IFavAndHist
	// {
	// 	FilePathType ValueType { get; }
	// }
}