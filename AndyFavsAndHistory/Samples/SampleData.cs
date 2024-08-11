#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AndyFavsAndHistory.FavHistoryMgr;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  1/31/2021 6:50:30 AM

namespace AndyFavsAndHistory.Samples
{
	public class SampleData   //: INotifyPropertyChanged
	{
	#region private fields

	#endregion

	#region ctor

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static void FavSamples(FavHistoryManager favMgr)
		{
			string[,] filenamelist = new string[,]
			{
				{"(jeffs) PdfSample 1", "displayname 1" },
				{"(jeffs) PdfSample 1A", "displayname 1A" },
				{"(jeffs) PdfSample 2", "displayname 2" },
				{"(jeffs) PdfSample 3", "displayname 3" },
				{"(jeffs) PdfSample 901", "displayname 901" },

			};
				

			FileListKey key;
			FileListItem item;
			FilePath<FileNameSimple> file;



			for (var i = 0; i < filenamelist.GetLength(0); i++)
			{
				file = new FilePath<FileNameSimple>(
					@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\" + filenamelist[i,0]
					+ ".xml");

				key = new FileListKey("2099999", FileListKeyTypes.CLASSF_FILE);
				item = new FileListItem( filenamelist[i,1], file);

				favMgr.Add(key, item);				
			}
		}

		public static void SmplSamples(FavHistoryManager favMgr)
		{
			string[,] filenamelist = new string[,]
			{
				{"PdfSample A", "displayname A" },
				{"PdfSample A1", "displayname A1" },
				{"PdfSample B", "displayname B" },
				{"PdfSample C", "displayname C" },
			};
				

			FileListKey key;
			FileListItem item;
			FilePath<FileNameSimple> file;



			for (var i = 0; i < filenamelist.Length; i++)
			{
				file = new FilePath<FileNameSimple>(
					@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\Sample Files" + filenamelist[i,0]
					+ ".xml");

				key = new FileListKey("2099999", FileListKeyTypes.CLASSF_FILE);
				item = new FileListItem( filenamelist[i,1], file);

				favMgr.Add(key, item);				
			}
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		// public event PropertyChangedEventHandler PropertyChanged;
		//
		// private void OnPropertyChange([CallerMemberName] string memberName = "")
		// {
		// 	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		// }

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleData";
		}

	#endregion
	}
}