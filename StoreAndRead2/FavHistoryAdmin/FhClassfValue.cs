#region using directives

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using StoreAndRead.Annotations;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  2/8/2021 11:04:56 PM

namespace StoreAndRead2.FavHistoryAdmin
{
	[DataContract(Namespace = "")]
	public class FhClassfValue : AFavAndHistValue, INotifyPropertyChanged
	{
	#region private fields

		private string displayName;
		private string description;
		private string iconNameClassf;

	#endregion

	#region ctor

		static FhClassfValue()
		{
			pathType = FilePathType.CLASSF;
		}


		public FhClassfValue(string displayName, string description,
			string iconNameClassf,
			FilePath<FileNameSimple> filePathClassf)
		{
			if (filePathClassf == null || !filePathClassf.IsValid)
			{
				throw new InvalidOperationException("Invalid Path");
			}

			filePaths = new ObservableCollection<FilePath<FileNameSimple>>();
			filePaths.Add(filePathClassf);

			this.displayName = displayName;
			this.description = description;
			this.iconNameClassf = iconNameClassf;
		}


		[OnDeserializing]
		private void initFilePaths(StreamingContext context)
		{
			filePaths = new ObservableCollection<FilePath<FileNameSimple>>();
			filePaths.Add(new FilePath<FileNameSimple>());
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

		[NotifyPropertyChangedInvocator]
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