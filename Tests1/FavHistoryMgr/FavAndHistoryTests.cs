#region using

using System;
using System.Collections.Generic;
using System.Text;
using AndyShared.FileSupport;
using SettingsManager;
using Tests1.FaveHistoryMgr;
using UtilityLibrary;
using static Tests1.FavHistoryMgr.FavAndHistMgr;
using static UtilityLibrary.MessageUtilities;

#endregion

// username: jeffs
// created:  1/31/2021 6:11:44 PM

namespace Tests1.FavHistoryMgr
{
	public class FavAndHistoryTests
	{
	#region private fields

		private const string FAV_HIST_FOLDER = "Favs And History";

		private const string FAV_AND_HIST_FILE_NAME = "FavAndHistory";

		private static string[,] favFilenamelist = new string[,]
		{
			{"(jeffs) PdfSample 1",   "Classification File 1",   "Icon fav 1" },
			{"(jeffs) PdfSample 1A",  "Classification File 1A",  "Icon 1A" },
			{"(jeffs) PdfSample 2",   "Classification File 2",   "Icon 2" },
			{"(jeffs) PdfSample 3",   "Classification File 3",   "Icon 3" },
			{"(jeffs) PdfSample 901", "Classification File 901", "Icon 901" },
		};

		private static string[,] smplFilenamelist = new string[,]
		{
			{"PdfSample A", "Sample file A",   "Icon fav A" },
			{"PdfSample A1", "Sample file A1", "Icon fav A1" },
			{"PdfSample B", "Sample file B",   "Icon fav B" },
			{"PdfSample C", "Sample file C",   "Icon fav C" },
		};

		private static string jobNum = "2099999";

	#endregion

	#region ctor

		public FavAndHistoryTests() { }

	#endregion

	#region public properties

		// public static BaseDataFile<FavAndHistoryDataStore> dataFile
		public static DataManager<FavAndHistoryDataStore> dataFile
		{
			get => FhMgr.DataFile;
			set => FhMgr.DataFile = value;
		}

		// public static ObservableDictionary<FileListKey, FilePath<FileNameSimple>>	 classifileList     =>
		// 	FhMgr.ClassifFileList	;
		//
		// public static ObservableDictionary<FileListKey, FilePath<FileNameSimple>>	 sampleFileList	    =>
		// 	FhMgr.SampleFileList		;

		public static ObservableDictionary<UserListKey, UserFavClassfListValue>	 userFavClassfList  =>
			FhMgr.UserFavClassfList;

		public static ObservableDictionary<UserListKey, UserFavPairListValue>		 userFavPairList	=>
			FhMgr.UserFavPairList;

		public static ObservableDictionary<UserListKey, UserHistClassfListValue>	 userHistClassfList =>
			FhMgr.UserHistClassfList;

		public static ObservableDictionary<UserListKey, UserHistPairListValue>		 userHistPairList   =>
			FhMgr.UserHistPairList		;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static void TestFavClassf()
		{
			int i = 2;

			Console.WriteLine("");
			Console.WriteLine("looking for| fav");
			Console.WriteLine("looking for| " + userFavClassfList[i].Value.ReferenceKeyClassf);
			Console.WriteLine("looking for| " + userFavClassfList[i].Key.DisplayName);

			UserListKey k = new UserListKey(userFavClassfList[i].Key.JobNumber,  userFavClassfList[i].Key.DisplayName);

			// find the "value"
			// get the classf file path or FilePath.Invalid

			FilePath<FileNameSimple> f;

			// bool result = FhMgr.FindClassf(k, userFavClassfList,  out f);
			bool result = FhMgr.FindClassfInFavs(k, out f);

			ListResult1(result, f);
		}

		public static void TestFavPair()
		{
			int i = 0;

			Console.WriteLine("");
			Console.WriteLine("looking for| var pair");
			Console.WriteLine("looking for| " + userFavPairList[i].Value.ReferenceKeyClassf);
			Console.WriteLine("looking for| " + userFavPairList[i].Key.DisplayName);

			UserListKey k = new UserListKey(userFavPairList[i].Key.JobNumber,  userFavPairList[i].Key.DisplayName);

			FilePath<FileNameSimple> f;

			// bool result = FhMgr.FindClassf(k, userFavPairList,  out f);
			bool result = FhMgr.FindClassfInFavPair(k, out f);

			ListResult1(result, f);


			// result = FhMgr.FindSmpl(k, userFavPairList,  out f);
			result = FhMgr.FindSmplInFavPair(k, out f);

			ListResult1(result, f);
		}

		public static void TestHistClassf()
		{
			int i = 2;

			Console.WriteLine("");
			Console.WriteLine("looking for| history fav");
			Console.WriteLine("looking for| " + userHistClassfList[i].Key.JobNumber);
			Console.WriteLine("looking for| " + userHistClassfList[i].Key.DisplayName);

			UserListKey k = new UserListKey(userHistClassfList[i].Key.JobNumber,
				userHistClassfList[i].Key.DisplayName);

			FilePath<FileNameSimple> f;

			// bool result = FhMgr.FindClassf(k, userHistClassfList,  out f);
			bool result = FhMgr.FindClassfInHist(k, out f);

			ListResult1(result, f);
		}

		public static void TestHistPair()
		{
			int i = 2;

			Console.WriteLine("");
			Console.WriteLine("looking for| history pair");
			Console.WriteLine("looking for| " + userHistPairList[i].Key.JobNumber);
			Console.WriteLine("looking for| " + userHistPairList[i].Key.DisplayName);

			UserListKey k = new UserListKey(userHistPairList[i].Key.JobNumber,  userHistPairList[i].Key.DisplayName);

			FilePath<FileNameSimple> f;

			// bool result = FhMgr.FindClassf(k, userHistPairList,  out f);
			bool result = FhMgr.FindClassfInHistPair(k, out f);

			ListResult1(result, f);

			// result = FhMgr.FindSmpl(k, userHistPairList,  out f);
			result = FhMgr.FindSmplInHistPair(k, out f);

			ListResult1(result, f);
		}

		public static void ListResult1(bool result, FilePath<FileNameSimple> f)
		{
			if (result)
			{
				if (f.Exists)
				{
					Console.WriteLine("found| " + f?.FullFilePath ?? "empty path");

					string s = CsXmlUtilities.ScanXmlForElementValue(f.FullFilePath, "Description", 0);

					Console.WriteLine("file description| " + s ?? "description not found");
				}
				else
				{
					Console.WriteLine("** file not found **");
				}
			}
			else
			{
				Console.WriteLine("** value not found **");
			}
		}

		public static void ListUserList(int idx)
		{
			switch (idx)
			{
			case 1:
				{
					listUserList(FhMgr.UserFavClassfList, FhMgr.ClassifFileList);
					break;
				}
			case 2:
				{
					listUserList(FhMgr.UserFavPairList, FhMgr.ClassifFileList);
					break;
				}
			case 3:
				{
					break;
				}
			case 4:
				{
					break;
				}
			}
		}

		private static void listUserList<T>(ObservableDictionary<UserListKey, T> dict,
			ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileList
			) where T : IFavAndHistoryClassfValue
		{
			int col = 20;
			int[] widths = new [] {45, 0};
			string[] msgs = new [] {"Key", "File Name"};

			Console.WriteLine("");
			Console.WriteLine(
				logMsgParams("User List", widths, col, msgs)
				);

			int i = 0;

			foreach (KeyValuePair<UserListKey, T> kvp in dict)
			{
				msgs = new []
				{
					kvp.Key.ToString(),
					kvp.Value.FilePathClassf(fileList).FileName
				};

				Console.WriteLine(
					logMsgParams("item " + i++, widths, col, msgs)
					);
			}
		}


		public static void Process()
		{
			string fileName = "(" + Environment.UserName + ") " + FAV_HIST_FOLDER;

			FilePath<FileNameSimple> path = new FilePath<FileNameSimple>(
				SuiteSettings.Path.SettingFolderPath);

			dataFile =
				new DataManager<FavAndHistoryDataStore>(path);
				// new BaseDataFile<FavAndHistoryDataStore>();

			dataFile.Create(path, fileName, new [] {FAV_HIST_FOLDER});

			Console.WriteLine("data file created|    path| " + dataFile.Path.SettingFolderPath);
			Console.WriteLine("data file created| exists?| " + dataFile.Path.Exists);

			FhMgr.Configure();

			classfSampleData(dataFile);
			smplSampleData(dataFile);
			// pairSampleData(dataFile);
			histClasfSampleData(dataFile);
			histSmplSampleData(dataFile);

			dataFile.Admin.Write();
		}

	#endregion

	#region private methods

		// private static void classfSampleData(BaseDataFile<FavAndHistoryDataStore> datafile)
		private static void classfSampleData(DataManager<FavAndHistoryDataStore> datafile)
		{
			FilePath<FileNameSimple> pathClassf;

			for (var i = 0; i < favFilenamelist.GetLength(0); i++)
			{
				// pathClassf = new FilePath<FileNameSimple>(
				// 	@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\" + favFilenamelist[i, 0]
				// 	+ ".xml");

				pathClassf = new FilePath<FileNameSimple>(FileLocationSupport.ClassifFileLocationUser +"\\" + favFilenamelist[i, 0]
					+ "." + FileLocationSupport.DATA_FILE_EXT);

				if (FhMgr.AddFavClassf(jobNum, favFilenamelist[i, 1], favFilenamelist[i, 2], pathClassf).Value == false)
				{
					Console.WriteLine("** Add Failed **");
				}
			}
		}

		// private static void smplSampleData(BaseDataFile<FavAndHistoryDataStore> datafile)
		private static void smplSampleData(DataManager<FavAndHistoryDataStore> datafile)
		{
			FilePath<FileNameSimple> pathClassf;
			FilePath<FileNameSimple> pathSmpl;

			for (var i = 0; i < smplFilenamelist.GetLength(0); i++)
			{
				// pathClassf = new FilePath<FileNameSimple>(
				// 	@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\" + favFilenamelist[i, 0]
				// 	+ ".xml");
				//
				// pathSmpl = new FilePath<FileNameSimple>(
				// 	@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\Sample Files\"
				// 	+ smplFilenamelist[i, 0]
				// 	+ ".sample");

				pathClassf = new FilePath<FileNameSimple>(FileLocationSupport.ClassifFileLocationUser +"\\" + favFilenamelist[i, 0]
					+ "." + FileLocationSupport.DATA_FILE_EXT);

				pathSmpl = new FilePath<FileNameSimple>(FileLocationSupport.ClassifSampleFileLocationUser +"\\"+ smplFilenamelist[i, 0]
					+ "." + FileLocationSupport.SAMPLE_FILE_EXT);

				if (FhMgr.AddFavPair(jobNum, smplFilenamelist[i, 0],
					favFilenamelist[i, 2], pathClassf, smplFilenamelist[i, 2], pathSmpl) == false)
				{
					Console.WriteLine("** Add Failed **");
				}
			}
		}

		// private static void histSmplSampleData(BaseDataFile<FavAndHistoryDataStore> datafile)
		private static void histSmplSampleData(DataManager<FavAndHistoryDataStore> datafile)
		{
			FilePath<FileNameSimple> pathClassf;
			FilePath<FileNameSimple> pathSmpl;

			UserListKey key;
			UserHistPairListValue value;

			for (var i = 0; i < smplFilenamelist.GetLength(0); i++)
			{
				key = new UserListKey(jobNum, favFilenamelist[i, 1]);

				// pathClassf = new FilePath<FileNameSimple>(
				// 	@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\" + favFilenamelist[i, 0]
				// 	+ ".xml");
				//
				// pathSmpl = new FilePath<FileNameSimple>(
				// 	@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\Sample Files\"
				// 	+ smplFilenamelist[i, 0]
				// 	+ ".sample");


				pathClassf = new FilePath<FileNameSimple>(FileLocationSupport.ClassifFileLocationUser +"\\" + favFilenamelist[i, 0]
					+ "." + FileLocationSupport.DATA_FILE_EXT);

				pathSmpl = new FilePath<FileNameSimple>(FileLocationSupport.ClassifSampleFileLocationUser +"\\"+ smplFilenamelist[i, 0]
					+ "." + FileLocationSupport.SAMPLE_FILE_EXT);

				if (FhMgr.AddToHistPair(jobNum, favFilenamelist[i, 1],  favFilenamelist[i, 2],
					pathClassf, smplFilenamelist[i, 2], pathSmpl) == false)
				{
					Console.WriteLine("** Add Failed **");
				}
			}
		}

		// private static void histClasfSampleData(BaseDataFile<FavAndHistoryDataStore> datafile)
		private static void histClasfSampleData(DataManager<FavAndHistoryDataStore> datafile)
		{
			FilePath<FileNameSimple> pathClassf;

			UserListKey key;
			UserHistClassfListValue value;

			for (var i = 0; i < favFilenamelist.GetLength(0); i++)
			{
				key = new UserListKey(jobNum, favFilenamelist[i, 1]);

				// pathClassf = new FilePath<FileNameSimple>(
				// 	@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\" + favFilenamelist[i, 0]
				// 	+ ".xml");

				pathClassf = new FilePath<FileNameSimple>(FileLocationSupport.ClassifFileLocationUser +"\\" + favFilenamelist[i, 0]
					+ "." + FileLocationSupport.DATA_FILE_EXT);

				if (FhMgr.AddHistClassf(jobNum, favFilenamelist[i, 1], favFilenamelist[i, 2], pathClassf) == false)
				{
					Console.WriteLine("** Add Failed **");
				}
			}
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FavAndHistoryManager";
		}

	#endregion
	}
}