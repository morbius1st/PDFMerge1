#region using

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UtilityLibrary;
using AndyShared.FileSupport.FileNameSheetPDF;
using DebugCode;

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

		private SampleFileListData sampleFileData;

	#endregion
	
	#region ctor

	#endregion
	
	#region public properties
	
		public ObservableCollection<FilePath<FileNameSheetPdf>> Files { get; private set; }
			= new ObservableCollection<FilePath<FileNameSheetPdf>>();

		public string Description => sampleFileData?.Header.Description;

		public string Building { 
			
			get => sampleFileData?.Data.Building;

			// set
			// {
			// 	if (sampleFileData == null) return;
			//
			// 	sampleFileData.Data.Building = value;
			//
			// 	OnPropertyChange("Building");
			// }
	}
	
	#endregion
	
	#region private properties
	
	#endregion
	
	#region public methods

		public void ReadSampleSheetFileList(string filePath)
		{
		#if DML1
			DM.Start0();
		#endif

			if (!File.Exists(filePath))
			{
			#if DML1
				DM.End0("end 1");
			#endif
				return;
			}
			// if (!File.Exists(filePath)) throw new FileNotFoundException();

			sampleFileData = new SampleFileListData();

			updateProperties();

			try
			{
				bool result = CsXmlUtilities.ReadXmlFile(filePath, out sampleFileData);

				if (!result)
				{
				#if DML1
					DM.End0("end 2");
				#endif
					return;
				}

				sampleFileData.Data.Parse();
			}
			catch (Exception e)
			{
				Debug.WriteLine("exception message");
				Debug.WriteLine(e.Message);
				Debug.WriteLine("inner exception message");
				Debug.WriteLine(e.InnerException?.Message);

			#if DML1
				DM.End0("end 3");
			#endif
				return;
			}

			foreach (string s in sampleFileData.Data.SheetList)
			{
				if (s.IsVoid()) continue;

				FilePath<FileNameSheetPdf> fileName = new FilePath<FileNameSheetPdf>(s);

				Files.Add(fileName);
			}

		#if DML1
			DM.End0();
		#endif
		}

		public void AddPath(FilePath<FileNameSheetPdf> path)
		{
			Files.Add(path);
			OnPropertyChange("Files");
		}
	
	#endregion

	#region private methods

		private void updateProperties()
		{
			OnPropertyChange("Description");
			OnPropertyChange("Building");
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
			return "this is SampleFileList";
		}
	
	#endregion
	}
}