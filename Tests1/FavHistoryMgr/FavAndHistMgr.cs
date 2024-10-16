#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SettingsManager;
using Tests1.FaveHistoryMgr;
using UtilityLibrary;

#endregion


// projname: $projectname$
// itemname: FavAndHistoryManager
// username: jeffs
// created:  2/6/2021 10:16:50 AM

namespace Tests1.FavHistoryMgr
{

	public class FavAndHistMgr
	{
	#region private fields

		private static readonly Lazy<FavAndHistMgr> instance =
			new Lazy<FavAndHistMgr>(() => new FavAndHistMgr());

		// private static BaseDataFile<FavAndHistoryDataStore> dataFile;
		private static DataManager<FavAndHistoryDataStore> dataFile;

		public ObservableDictionary<FileListKey, FilePath<FileNameSimple>>		ClassifFileList
		{
			get => dataFile.Data.ClassfFileList;
			set => dataFile.Data.ClassfFileList = value;
		}

		public ObservableDictionary<FileListKey, FilePath<FileNameSimple>>		SampleFileList
		{
			get => dataFile.Data.SampleFileList;
			set => dataFile.Data.SampleFileList = value;
		}


		// favs
		public ObservableDictionary<UserListKey, UserFavClassfListValue>		UserFavClassfList
		{
			get => dataFile.Data.UserFavClassfList;
			set => dataFile.Data.UserFavClassfList = value;
		}

		public ObservableDictionary<UserListKey, UserFavPairListValue>			UserFavPairList
		{
			get => dataFile.Data.UserFavPairList;
			set => dataFile.Data.UserFavPairList = value;
		}


		// history
		public ObservableDictionary<UserListKey, UserHistClassfListValue>		UserHistClassfList
		{
			get => dataFile.Data.UserHistClassfList;
			set => dataFile.Data.UserHistClassfList = value;
		}

		public ObservableDictionary<UserListKey, UserHistPairListValue>			UserHistPairList
		{
			get => dataFile.Data.UserHistPairList;
			set => dataFile.Data.UserHistPairList = value;
		}

	#endregion

	#region ctor

		private FavAndHistMgr() { }

	#endregion

	#region public properties

		public static FavAndHistMgr FhMgr => instance.Value;

		// public BaseDataFile<FavAndHistoryDataStore> DataFile
		public DataManager<FavAndHistoryDataStore> DataFile
		{
			get => dataFile;
			set => dataFile = value;
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure()
		{
			ClassifFileList = new ObservableDictionary<FileListKey, FilePath<FileNameSimple>>();
			SampleFileList = new ObservableDictionary<FileListKey, FilePath<FileNameSimple>>();
			UserFavClassfList = new ObservableDictionary<UserListKey, UserFavClassfListValue>();
			UserFavPairList = new ObservableDictionary<UserListKey, UserFavPairListValue>();
			UserHistClassfList = new ObservableDictionary<UserListKey, UserHistClassfListValue>();
			UserHistPairList = new ObservableDictionary<UserListKey, UserHistPairListValue>();

			// UserFavClassfListValue.Configure(ClassifFileList);
			//
			// UserFavPairListValue.Configure(ClassifFileList, SampleFileList);

		}


		public bool FindClassfInFavs(UserListKey k,out FilePath<FileNameSimple> filePath)
		{
			return FindClassf(k, UserFavClassfList,  out filePath);
		}
		
		public bool FindClassfInFavPair(UserListKey k,out FilePath<FileNameSimple> filePath)
		{
			return FindClassf(k, UserFavPairList,  out filePath);
		}

		public bool FindClassfInHist(UserListKey k,out FilePath<FileNameSimple> filePath)
		{
			return FindClassf(k, UserHistClassfList,  out filePath);
		}
		
		public bool FindClassfInHistPair(UserListKey k,out FilePath<FileNameSimple> filePath)
		{
			return FindClassf(k, UserHistPairList,  out filePath);
		}

		public bool FindSmplInFavPair(UserListKey k,out FilePath<FileNameSimple> filePath)
		{
			return FindSmpl(k, UserFavPairList,  out filePath);
		}

		public bool FindSmplInHistPair(UserListKey k,out FilePath<FileNameSimple> filePath)
		{
			return FindSmpl(k, UserHistPairList,  out filePath);
		}


		public bool FindClassf<T>(UserListKey k,
			ObservableDictionary<UserListKey, T> dict,
			out FilePath<FileNameSimple> filePath
			) where T : IFavAndHistoryClassfValue
		{
			bool result;
			filePath = null;

			result = dict.TryGetValue(k, out var value);

			filePath = value.FilePathClassf(ClassifFileList);

			return true;
		}
		
		public bool FindSmpl<T>(UserListKey k,
			ObservableDictionary<UserListKey, T> dict,
			out FilePath<FileNameSimple> filePath
			) where T : IFavAndHistoryPairValue
		{
			bool result;
			filePath = null;

			result = dict.TryGetValue(k, out var value);

			filePath = value.FilePathSmpl(SampleFileList);

			return true;
		}






		public bool? AddFavClassf(String jobNumber, string displayName, string iconName,
			FilePath<FileNameSimple> filePathClassf)
		{
			UserListKey favKey;
			UserFavClassfListValue favValue;

			FileListKey listKey = new FileListKey(jobNumber, filePathClassf.FileNameNoExt);

			if (!ClassifFileList.ContainsKey(listKey))
			{
				ClassifFileList.Add(listKey, filePathClassf);
			}

			favKey = new UserListKey(jobNumber, displayName);
			favValue = new UserFavClassfListValue(listKey, iconName);

			return AddToFavs(favKey, favValue, UserFavClassfList);
		}

		public bool? AddHistClassf(string jobNumber, string displayName,  string iconName,
			FilePath<FileNameSimple> filePathClassf)
		{
			UserListKey histKey = new UserListKey(jobNumber, displayName);
			UserHistClassfListValue histValue;

			histValue = new UserHistClassfListValue(filePathClassf, iconName);

			return AddToFavs(histKey, histValue, UserHistClassfList);

		}

		public bool? AddFavPair(string jobNumber, string displayName, string iconNameClassf,
			FilePath<FileNameSimple> filePathClassf,
			string iconNameSmpl,
			FilePath<FileNameSimple> filePathSmpl)
		{
			UserListKey pairKey;
			UserFavPairListValue pairValue;

			FileListKey keyClassf = new FileListKey(jobNumber, filePathClassf.FileNameNoExt);
			FileListKey keySmpl = new FileListKey(jobNumber, filePathSmpl.FileNameNoExt);

			if (!ClassifFileList.ContainsKey(keyClassf))
			{
				ClassifFileList.Add(keyClassf, filePathClassf);
			}

			if (!SampleFileList.ContainsKey(keySmpl))
			{
				SampleFileList.Add(keySmpl, filePathSmpl);
			}

			pairKey = new UserListKey(jobNumber, displayName);
			pairValue = new UserFavPairListValue(iconNameClassf, keyClassf, iconNameSmpl, keySmpl);

			return AddToFavs(pairKey, pairValue, UserFavPairList);
		}



		public bool? AddToHistPair(string jobNumber, string displayName,  string iconNameClassf,
			FilePath<FileNameSimple> filePathClassf,
			string iconNameSmpl,
			FilePath<FileNameSimple> filePathSmpl
			)
		{
			UserListKey histKey = new UserListKey(jobNumber, displayName);
			UserHistPairListValue histValue;

			if (UserHistPairList.ContainsKey(histKey))
			{
				return null;
			}

			histValue = new UserHistPairListValue(iconNameClassf,filePathClassf, iconNameSmpl,  filePathSmpl);

			return AddToFavs(histKey, histValue, UserHistPairList);

		}


		public bool? AddToFavs<T>(UserListKey key, T value, ObservableDictionary<UserListKey, T> dict) where T: IFavAndHistoryClassfValue
		{
			if (!key.IsValid) return false;

			if (dict.ContainsKey(key))
			{
				return null;
			}

			dict.Add(key, value);

			return true;
		}






		// public bool? AddFavClassf(String jobNumber, string displayName, string iconName,
		// 	FilePath<FileNameSimple> filePathClassf)
		// {
		// 	FileListKey listKey = new FileListKey(jobNumber, filePathClassf.FileNameNoExt);
		//
		// 	if (!ClassifFileList.ContainsKey(listKey))
		// 	{
		// 		ClassifFileList.Add(listKey, filePathClassf);
		// 	}
		//
		// 	return AddToFavClassf(jobNumber, displayName, iconName, listKey);
		// }

		// public bool? AddToFavClassf(string jobNumber, string displayName,  string iconName,
		// 	FileListKey listKeyClassf)
		// {
		// 	UserListKey key = new UserListKey(jobNumber, displayName);
		//
		// 	if (UserFavClassfList.ContainsKey(key))
		// 	{
		// 		return null;
		// 	}
		//
		// 	UserFavClassfListValue value = new UserFavClassfListValue(listKeyClassf, iconName);
		//
		// 	UserFavClassfList.Add(key, value);
		//
		// 	return true;
		// }



		// public bool? AddFavPair(string jobNumber, string displayName, string iconNameClassf,
		// 	FilePath<FileNameSimple> filePathClassf,
		// 	string iconNameSmpl,
		// 	FilePath<FileNameSimple> filePathSmpl)
		// {
		// 	FileListKey keyClassf = new FileListKey(jobNumber, filePathClassf.FileNameNoExt);
		// 	FileListKey keySmpl = new FileListKey(jobNumber, filePathSmpl.FileNameNoExt);
		//
		// 	if (!ClassifFileList.ContainsKey(keyClassf))
		// 	{
		// 		ClassifFileList.Add(keyClassf, filePathClassf);
		// 	}
		//
		// 	if (!SampleFileList.ContainsKey(keySmpl))
		// 	{
		// 		SampleFileList.Add(keySmpl, filePathSmpl);
		// 	}
		//
		// 	return AddToFavPair(jobNumber, displayName, iconNameClassf, keyClassf, iconNameSmpl, keySmpl);
		// }

		// public bool? AddToFavPair(string jobNumber, string displayName,  string iconNameClassf,
		// 	FileListKey listKeyClassf, string iconNameSmpl, FileListKey listKeySmpl)
		// {
		//
		// 	UserListKey key = new UserListKey(jobNumber, displayName);
		//
		// 	if (UserFavPairList.ContainsKey(key))
		// 	{
		// 		return null;
		// 	}
		//
		// 	UserFavPairListValue value = new UserFavPairListValue(iconNameClassf, listKeyClassf, iconNameSmpl, listKeySmpl);
		//
		// 	UserFavPairList.Add(key, value);
		//
		// 	return true;
		// }





		
		// public bool? AddHistClassf(string jobNumber, string displayName,  string iconName,
		// 	FilePath<FileNameSimple> filePathClassf)
		// {
		// 	UserListKey key = new UserListKey(jobNumber, displayName);
		//
		// 	if (UserHistClassfList.ContainsKey(key))
		// 	{
		// 		return null;
		// 	}
		//
		// 	UserHistClassfListValue value = new UserHistClassfListValue(filePathClassf, iconName);
		//
		// 	UserHistClassfList.Add(key, value);
		//
		// 	return true;
		// }









		// public bool? AddToHistPair(string jobNumber, string displayName,  string iconName,
		// 	FilePath<FileNameSimple> filePathClassf,
		// 	FilePath<FileNameSimple> filePathSmpl
		// 	)
		// {
		// 	UserListKey key = new UserListKey(jobNumber, displayName);
		//
		// 	if (UserHistPairList.ContainsKey(key))
		// 	{
		// 		return null;
		// 	}
		//
		// 	UserHistPairListValue value = new UserHistPairListValue(filePathClassf, filePathSmpl, iconName);
		//
		// 	UserHistPairList.Add(key, value);
		//
		// 	return true;
		// }













	#endregion

	#region private methods



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