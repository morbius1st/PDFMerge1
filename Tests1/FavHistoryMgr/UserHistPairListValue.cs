#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Tests1.FavHistoryMgr;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  1/31/2021 5:06:46 PM

namespace Tests1.FaveHistoryMgr
{
	[DataContract(Namespace = "")]
	public class UserHistPairListValue : AFavAndHistPair, INotifyPropertyChanged, IFavAndHistoryPairValue
	{
	#region private fields

		private FilePath<FileNameSimple> filePathClassf;
		private FilePath<FileNameSimple> filePathSample;
		private string iconNameClassf;
		private string iconNameSmpl;

	#endregion

	#region ctor

		public UserHistPairListValue(string iconNameClassf,FilePath<FileNameSimple> filePathClassf, 
			string iconNameSmpl, FilePath<FileNameSimple> filePathSample)
		{
			// if (!IsConfigured) throw new InvalidOperationException("UserFavClassfListValue not configured");

			this.filePathClassf = filePathClassf;
			this.filePathSample = filePathSample;
			this.iconNameClassf = iconNameClassf;
			this.iconNameSmpl = iconNameSmpl;
		}

	#endregion

	#region public properties

		[DataMember(Order = 1)]
		public FilePath<FileNameSimple> ClassfFilePath
		{
			get => filePathClassf;
			set
			{ 
				filePathClassf = value;
				OnPropertyChange();
			}
		}
		
		[DataMember(Order = 2)]
		public FilePath<FileNameSimple> SampleFilePath
		{
			get => filePathSample;
			set
			{ 
				filePathSample = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 3)]
		public string IconNameClassf
		{
			get => iconNameClassf;
			set
			{ 
				iconNameClassf = value;
				OnPropertyChange();
			}
		}
		
		[DataMember(Order = 4)]
		public string IconNameSmpl
		{
			get => iconNameSmpl;
			set
			{ 
				iconNameSmpl = value;
				OnPropertyChange();
			}
		}



	#endregion

	#region private properties

	#endregion

	#region public methods

		// public void Configure(ObservableDictionary<FileListKey, FilePath<FileNameSimple>> filelistClassf,
		// 	ObservableDictionary<FileListKey, FilePath<FileNameSimple>> filelistSmpl)
		// {
		// 	fileListClassf = filelistClassf;
		// 	fileListSmpl = filelistSmpl;
		// }

		public FilePath<FileNameSimple> FilePathClassf(
			ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileList = null)
		{
			// if (fileList != null) return FilePath<FileNameSimple>.Invalid;

			return filePathClassf;
		}

		public FilePath<FileNameSimple> FilePathSmpl(ObservableDictionary<FileListKey, 
			FilePath<FileNameSimple>> fileList = null)
		{
			// if (fileList == null) return FilePath<FileNameSimple>.Invalid;

			return filePathSample;
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FavAndHistoryManager";
		}

	#endregion
	}
}