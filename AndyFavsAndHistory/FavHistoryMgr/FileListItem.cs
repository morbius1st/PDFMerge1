#region + Using Directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   1/17/2021 3:42:48 PM

namespace AndyFavsAndHistory.FavHistoryMgr
{

	// public interface IFileListItem
	// {
	// 	FilePath<FileNameSimple> FilePath { get; }
	// 	string Name { get; set; }
	//
	// }
	//

	[DataContract(Namespace = "")]
	public class FileListItem :  INotifyPropertyChanged
	{
		private string name;
		private FilePath<FileNameSimple> filePath;

		public FileListItem() { }

		public FileListItem(string name, FilePath<FileNameSimple> filePath)
		{
			this.name = name;
			this.filePath = filePath;
		}

		[DataMember]
		public string Name
		{
			get => name;
			set
			{
				name = value;
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