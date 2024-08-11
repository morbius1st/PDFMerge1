#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Tests1.FaveHistoryMgr;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   2/6/2021 6:12:32 AM

namespace Tests1.FavHistoryMgr
{
	[DataContract(Namespace = "")]
	public abstract class AFavAndHist
	{
		[IgnoreDataMember]
		public FilePath<FileNameSimple> DefaultValue => FilePath<FileNameSimple>.Invalid;

	}

	[DataContract(Namespace = "")]
	public abstract class AFavAndHistClassf : AFavAndHist
	{
		protected AFavAndHistClassf()
		{
			if (!IsConfigured) throw new InvalidOperationException("UserFavClassfListValue not configured");
		}

		[IgnoreDataMember]
		protected static ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileListClassf;

		[IgnoreDataMember]
		public bool IsConfigured => fileListClassf != null;


	}


	[DataContract(Namespace = "")]
	public abstract class AFavAndHistPair : AFavAndHistClassf
	{
		[IgnoreDataMember]
		protected static ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileListSmpl;

		[IgnoreDataMember]
		public new bool IsConfigured => fileListClassf != null && fileListSmpl != null;

	}


	public interface IFavAndHistory
	{
		bool IsConfigured { get; }
	}


	public interface IFavAndHistoryClassfValue : IFavAndHistory
	{
		FilePath<FileNameSimple> FilePathClassf(
			ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileList = null);
	}

	// public interface IFavAndHistoryClassf : IFavAndHistoryClassfValue
	// {
	// 	void Configure(ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileListClassf);
	// }


	public interface IFavAndHistoryPairValue : IFavAndHistoryClassfValue
	{
		FilePath<FileNameSimple> FilePathSmpl(
			ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileList = null);
	}

	// public interface IFavAndHistoryPair : IFavAndHistoryClassfValue, IFavAndHistorySmplValue
	// {
	// 	void Configure(
	// 		ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileListClassf,
	// 		ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileListSmpl
	// 		);
	// }


	// [DataContract(Namespace = "")]
	// public abstract class AFavAndHistClassf : AFavAndHist, IFavAndHistoryClassfValue
	// {
	// 	protected FileListKey referenceKeyClassf;
	//
	// 	public abstract FileListKey ReferenceKeyClassf { get; protected set; }
	//
	// 	public FilePath<FileNameSimple> FilePathClassf(
	// 		ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileList)
	// 	{
	// 		if (fileList == null) return FilePath<FileNameSimple>.Invalid;
	//
	// 		FilePath<FileNameSimple> file;
	//
	// 		bool result = fileList.TryGetValue(referenceKeyClassf, out file);
	//
	// 		return result ? file : FilePath<FileNameSimple>.Invalid;
	// 	}
	//
	// }
	//
	//
	// [DataContract(Namespace = "")]
	// public abstract class AFavAndHistPair : AFavAndHistClassf, IFavAndHistorySmplValue
	// {
	// 	protected FileListKey referenceKeySmpl;
	//
	// 	public abstract FileListKey ReferenceKeySmpl { get; protected set; }
	//
	// 	public FilePath<FileNameSimple> FilePathSmpl(
	// 		ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileList)
	// 	{
	// 		if (fileList == null) return FilePath<FileNameSimple>.Invalid;
	//
	// 		FilePath<FileNameSimple> file;
	//
	// 		bool result = fileList.TryGetValue(referenceKeySmpl, out file);
	//
	// 		return result ? file : FilePath<FileNameSimple>.Invalid;
	// 	}
	//
	// }


}