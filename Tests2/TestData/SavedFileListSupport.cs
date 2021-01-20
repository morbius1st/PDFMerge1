#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SettingsManager;
using Tests2.DataStore;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  1/17/2021 10:34:00 PM

namespace Tests2.TestData
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

		public SavedFile Select(string key, DataManager<SavedFileList> dms)
		{
			keyCheck(key);

			SavedFile match;

			bool result = dms.Data.SavedClassfFiles.TryGetValue(key, out match);

			if (result) return match;

			return null;
		}

		
		public static CollectionView FilterByProject(string projectNumber, DataManager<SavedFileList> dms)
		{
			projNumCheck(projectNumber);

			view = (CollectionView) CollectionViewSource.GetDefaultView(dms.Data.SavedClassfFiles);

			int len = projectNumber.Length;

			view.Filter = o =>  ((KeyValuePair<string, SavedFile>) o).Key.Substring(0, len).Equals(projectNumber);

			return view;
		}

		public static void RemoveByProject(string projectNumber, DataManager<SavedFileList> dms)
		{
			projNumCheck(projectNumber);


			KeyValuePair<string, SavedFile>[] selected = dms.Data.SavedClassfFiles.Where(kvp => kvp.Key.StartsWith(projectNumber)).ToArray();

			if (selected.Length <= 0) return;

			foreach (KeyValuePair<string, SavedFile> kvp in selected)
			{
				dms.Data.SavedClassfFiles.Remove(kvp.Key);
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