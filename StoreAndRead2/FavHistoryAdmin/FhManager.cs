#region using directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SettingsManager;
using StoreAndRead.Annotations;
using UtilityLibrary;

#endregion


// projname: $projectname$
// itemname: FhManager
// username: jeffs
// created:  2/10/2021 10:29:24 PM

namespace StoreAndRead2.FavHistoryAdmin
{
	public class FhManager : INotifyPropertyChanged
	{
	#region private fields

		private const string FAV_HIST_FOLDER = "Favs And History";
		private const string FAV_AND_HIST_FILE_NAME = "FavAndHistory";
		private const string SMPL_KEY_SEPARATOR = "<|>";

		private static readonly Lazy<FhManager> instance =
			new Lazy<FhManager>(() => new FhManager());

		private FilePath<FileNameSimple> dataPath;

		// private BaseDataFile<FhDataStore> fhData;
		private DataManager<FhDataStore> fhData;

		private FhAdministrator<FhClassfValue> favs;
		private FhAdministrator<FhPairValue> favPairs;

	#endregion

	#region ctor

		private FhManager()
		{
			initialize();
		}

	#endregion

	#region public properties

		public static FhManager Instance => instance.Value;

		public StorageMgrPath Path => fhData.Admin.Path;

		public FhAdministrator<FhClassfValue> Favorites => fhData.Data.Favorites;

		public FhAdministrator<FhPairValue> FavoritePairs => fhData.Data.FavoritePairs;

		public FhAdministrator<FhClassfValue> History => fhData.Data.History;

		public FhAdministrator<FhPairValue> HistoryPairs => fhData.Data.HistoryePairs;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Save() => fhData.Admin.Write();
		public void Read() => fhData.Admin.Read();

		public void Create()
		{
			fhData.Create(dataPath, "(" + Environment.UserName + ") " + FAV_HIST_FOLDER, new [] {FAV_HIST_FOLDER});
		}

	#endregion

	#region private methods

		private void initialize()
		{
			// fhData = new BaseDataFile<FhDataStore>();
			dataPath = new FilePath<FileNameSimple>(SuiteSettings.Path.SettingFolderPath);

			fhData = new DataManager<FhDataStore>(dataPath);

			string filename = "(" + Environment.UserName + ") " + FAV_HIST_FOLDER;

			fhData.Configure(dataPath, filename,  new [] {FAV_HIST_FOLDER});
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FhManager";
		}

	#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}