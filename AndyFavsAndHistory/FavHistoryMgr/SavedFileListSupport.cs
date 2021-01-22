#region using

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using SettingsManager;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  1/17/2021 10:34:00 PM

namespace AndyFavsAndHistory.FavHistoryMgr
{
	public class SavedFileListSupport
	{
	#region private fields

		private static CollectionView view;

	#endregion

	#region ctor

		public SavedFileListSupport() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public FileListItem Select(string key, BaseDataFile<FileListData> fld)
		{
			keyCheck(key);

			FileListItem match;

			bool result = fld.Data.FileList.TryGetValue(key, out match);

			if (result) return match;

			return null;
		}

		public static CollectionView FilterByProject(string projectNumber, BaseDataFile<FileListData> fld)

		{
			projNumCheck(projectNumber);

			view = (CollectionView) CollectionViewSource.GetDefaultView(fld.Data.FileList);

			int len = projectNumber.Length;

			view.Filter = o =>  ((KeyValuePair<string, FileListItem>) o).Key.Substring(0, len).Equals(projectNumber);

			return view;
		}


		public static void RemoveByProject(string projectNumber, BaseDataFile<FileListData> fld)

		{
			projNumCheck(projectNumber);

			KeyValuePair<string, FileListItem>[] selected =
				fld.Data.FileList.Where(kvp => kvp.Key.StartsWith(projectNumber)).ToArray();

			if (selected.Length <= 0) return;

			foreach (KeyValuePair<string, FileListItem> kvp in selected)
			{
				fld.Data.FileList.Remove(kvp.Key);
			}
		}

	#endregion

	#region private methods

		private static void keyCheck(string key)
		{
			if (key.IsVoid()) throw new InvalidEnumArgumentException("Key");
		}

		private static void projNumCheck(string projNumber)
		{
			if (projNumber.IsVoid()) throw new InvalidEnumArgumentException("ProjectNumber");
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SavedFileListSupport";
		}

	#endregion
	}
}