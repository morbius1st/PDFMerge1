#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  2/8/2021 10:55:25 PM

namespace StoreAndRead2.FavHistoryAdmin
{
	[DataContract(Namespace = "")]
	
	public class FhKey : INotifyPropertyChanged
	{
	#region private fields

		private const int MAX_DISPLAY_NAME_LEN = 30;
		private const string NAME_DIVIDER = " >|< ";

		private FileStorageType type;
		private string jobNumber;
		private string[] fileName;

	#endregion

	#region ctor

		private FhKey(FileStorageType type, string jobNumber, string[] fileName)
		{
			this.type = type;
			this.jobNumber = jobNumber;
			this.fileName = fileName;
		}

	#endregion

	#region public properties

		[DataMember(Order = 1)]
		public string JobNumber
		{
			get => jobNumber;
			private set
			{ 
				jobNumber = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 2)]
		public string[] FileNames
		{
			get => fileName;
			private set
			{ 
				fileName = value;
				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public bool IsValid => !jobNumber.IsVoid() && 
			fileName != null && fileName.Length == (int) type;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static FhKey ClassfKey(string jobNumber, string fileName)
		{
			return new FhKey(FileStorageType.SOLO, jobNumber, new []{fileName});
		}

		public static FhKey PairfKey(string jobNumber, string fileNameClassf, string fileNameSmpl)
		{
			return new FhKey(FileStorageType.PAIR, jobNumber, new []{fileNameClassf, fileNameSmpl});
		}


	#endregion

	#region private methods

		private string padDisplayName()
		{
			return ".".Repeat(MAX_DISPLAY_NAME_LEN - fileName.Length);
		}

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
			string result = jobNumber;

			foreach (string fName in FileNames)
			{
				result += NAME_DIVIDER + fName;
			}

			return result;
		}

	#endregion
	}
}