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
	public class UserHistClassfListValue : AFavAndHistClassf, INotifyPropertyChanged, IFavAndHistoryClassfValue
	{
	#region private fields

		private FilePath<FileNameSimple> filePath;
		private string iconName;

	#endregion

	#region ctor

		public UserHistClassfListValue(FilePath<FileNameSimple> filePath, string iconName)
		{
			// if (!IsConfigured) throw new InvalidOperationException("UserFavClassfListValue not configured");

			this.filePath = filePath;
			this.iconName = iconName;
		}

	#endregion

	#region public properties

		[DataMember(Order = 1)]
		public FilePath<FileNameSimple> ClassfFilePath
		{
			get => filePath;
			private set
			{ 
				filePath = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 2)]
		public string IconName
		{
			get => iconName;
			set
			{ 
				iconName = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		// public void Configure(ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileList)
		// {
		// 	fileListClassf = fileList;
		// }

		public FilePath<FileNameSimple> FilePathClassf(
			ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileList = null)
		{
			// if (fileList != null) return FilePath<FileNameSimple>.Invalid;

			return filePath;
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