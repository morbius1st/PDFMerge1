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
	public class UserFavPairListValue : AFavAndHistPair, INotifyPropertyChanged, IFavAndHistoryPairValue
	{
	#region private fields

		private FileListKey referenceKeyClassf;
		private FileListKey referenceKeySmpl;
		private string iconNameClassf;
		private string iconNameSmpl;

	#endregion

	#region ctor

		public UserFavPairListValue(string iconNameClassf, FileListKey referenceKeyClassf,
			string iconNameSmpl,
			FileListKey referenceKeySmpl)
		{
			// if (!IsConfigured) throw new InvalidOperationException("UserFavClassfListValue not configured");

			this.referenceKeyClassf = referenceKeyClassf;
			this.referenceKeySmpl = referenceKeySmpl;
			this.iconNameClassf = iconNameClassf;
			this.iconNameSmpl = iconNameSmpl;
		}

	#endregion

	#region public properties

		[DataMember(Order = 1)]
		public FileListKey ReferenceKeyClassf
		{
			get => referenceKeyClassf;
			protected set
			{
				referenceKeyClassf = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 2)]
		public FileListKey ReferenceKeySmpl
		{
			get => referenceKeySmpl;
			protected set
			{
				referenceKeySmpl = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 5)]
		public string IconNameClassf
		{
			get => iconNameClassf;
			set
			{ 
				iconNameClassf = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 6)]
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

		public void Configure(ObservableDictionary<FileListKey, FilePath<FileNameSimple>> filelistClassf,
			ObservableDictionary<FileListKey, FilePath<FileNameSimple>> filelistSmpl)
		{
			fileListClassf = filelistClassf;
			fileListSmpl = filelistSmpl;
		}

		public FilePath<FileNameSimple> FilePathClassf(ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileList)
		{
			if (fileList == null) return FilePath<FileNameSimple>.Invalid;
		
			FilePath<FileNameSimple> file;
		
			bool result = fileList.TryGetValue(referenceKeyClassf, out file);
		
			return result ? file : FilePath<FileNameSimple>.Invalid;
		}
		
		public FilePath<FileNameSimple> FilePathSmpl(ObservableDictionary<FileListKey, FilePath<FileNameSimple>> fileList)
		{
			if (fileList == null) return FilePath<FileNameSimple>.Invalid;
		
			FilePath<FileNameSimple> file;
		
			bool result = fileList.TryGetValue(referenceKeySmpl, out file);
		
			return result ? file : FilePath<FileNameSimple>.Invalid;
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