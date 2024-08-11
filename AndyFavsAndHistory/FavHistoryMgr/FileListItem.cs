#region + Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   1/17/2021 3:42:48 PM

namespace AndyFavsAndHistory.FavHistoryMgr
{
	
	[DataContract(Namespace = "")]
	public class FileListItem :  INotifyPropertyChanged
	{
		private string displayName;
		private FilePath<FileNameSimple> filePath;

		public FileListItem() { }

		public FileListItem(string displayName, FilePath<FileNameSimple> filePath)
		{
			this.displayName = displayName;
			this.filePath = filePath;
		}

		[DataMember]
		public string DisplayName
		{
			get => displayName;
			set
			{
				displayName = value;
				OnPropertyChanged();
			}
		}

		[DataMember]
		public FilePath<FileNameSimple> FilePath
		{
			get => filePath;
			private set => filePath = value;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}