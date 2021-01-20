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
	[DataContract(Namespace = "")]
	public class SavedFile : INotifyPropertyChanged
	{
		private string name;
		private FilePath<FileNameSimple> filePath;

		public SavedFile() {}

		public SavedFile(string name, FilePath<FileNameSimple> filePath)
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
			set => filePath = value;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}
}
