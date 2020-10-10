#region using

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UtilityLibrary;
using AndyShared.FileSupport.FileNameSheetPDF;

#endregion

// username: jeffs
// created:  9/26/2020 7:17:45 AM

namespace AndyShared.SampleFileSupport
{
	/// <summary>
	/// holds a list of sheet files to be classified
	/// </summary>
	public class SheetFileList : INotifyPropertyChanged
	{
	#region private fields

	#endregion
	
	#region ctor

	#endregion
	
	#region public properties
	
		public ObservableCollection<FilePath<FileNameSheetPdf>> Files { get; private set; }
			= new ObservableCollection<FilePath<FileNameSheetPdf>>();
	
	#endregion
	
	#region private properties
	
	#endregion
	
	#region public methods

		public void ReadSampleSheetFileList(string filePath)
		{
			if (!File.Exists(filePath)) throw new FileNotFoundException();

			SampleFileListData sampleFileData = new SampleFileListData();

			try
			{
				bool result = CsXmlUtilities.ReadXmlFile(filePath, out sampleFileData);

				if (!result) return;

				sampleFileData.Data.Parse();
			}
			catch (Exception e)
			{
				Debug.WriteLine("exception message");
				Debug.WriteLine(e.Message);
				Debug.WriteLine("inner exception message");
				Debug.WriteLine(e.InnerException?.Message);

				return;
			}

			foreach (string s in sampleFileData.Data.SheetList)
			{
				if (s.IsVoid()) continue;

				FilePath<FileNameSheetPdf> fileName = new FilePath<FileNameSheetPdf>(s);

				Files.Add(fileName);
			}
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
			return "this is SampleFileList";
		}
	
	#endregion
	}
}