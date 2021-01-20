#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   1/17/2021 3:42:48 PM

namespace Tests2.DataStore
{
	[DataContract(Namespace = "")]
	public class SavedFile : INotifyPropertyChanged
	{
		private string name;
		private FilePath<FileNameSimple> filePath;

		public SavedFile(string name, FilePath<FileNameSimple> filePath)
		{
			this.name = name;
			this.filePath = filePath;
		}

		[DataMember(Order = 1)]
		public string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChanged();
			}
		}

		[DataMember(Order = 2)]
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
