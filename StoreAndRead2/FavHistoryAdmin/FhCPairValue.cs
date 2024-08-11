#region using directives

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  2/8/2021 11:04:56 PM

namespace StoreAndRead2.FavHistoryAdmin
{
	[DataContract(Namespace = "")]
	public class FhPairValue :  AFavAndHistValue, INotifyPropertyChanged
	{

	#region private fields

		private string displayName;
		private string description;
		private string iconNameClassf;
		private string iconNameSmpl;

	#endregion

	#region ctor

		static FhPairValue()
		{
			pathType = FilePathType.SAMPLE;
		}

		public FhPairValue(string displayName, string description, 
			string iconNameClassf, 
			FilePath<FileNameSimple> filePathClassf, 
			string iconNameSmpl,
			FilePath<FileNameSimple> filePathSmpl)
		{
			if (filePathClassf == null || !filePathClassf.IsValid)
			{
				throw new InvalidOperationException("Invalid Path");
			}

			if (filePathSmpl == null || !filePathSmpl.IsValid)
			{
				throw new InvalidOperationException("Invalid Path");
			}

			filePaths = new ObservableCollection<FilePath<FileNameSimple>>();
			filePaths.Add(filePathClassf);
			filePaths.Add(filePathSmpl);

			this.displayName = displayName;
			this.description = description;
			this.iconNameClassf = iconNameClassf;
			this.iconNameSmpl = iconNameSmpl;
		}

		[OnDeserializing]
		private void initFilePaths(StreamingContext context)
		{
			filePaths = new ObservableCollection<FilePath<FileNameSimple>>();
			filePaths.Add(new FilePath<FileNameSimple>());
			filePaths.Add( new FilePath<FileNameSimple>());
		}

	#endregion

	#region public properties

		[DataMember(Order = 10)]
		public FilePath<FileNameSimple> FilePathClassf
		{
			get => filePaths[(int) FilePathType.CLASSF];
			set
			{
				filePaths[(int) FilePathType.CLASSF] = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 11)]
		public FilePath<FileNameSimple> FilePathSmpl
		{
			get => filePaths[(int) FilePathType.SAMPLE];
			set
			{
				filePaths[(int) FilePathType.SAMPLE] = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 1)]
		public string DisplayName
		{
			get => displayName;
			set
			{
				if (value == displayName) return;
				displayName = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 2)]
		public string Description
		{
			get => description;
			set
			{
				description = value;
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
			return "this is FhPairValue";
		}

	#endregion
	}
}