#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Documents;
using System.Xml;
using AndyShared.FilesSupport;
using UtilityLibrary;
using static UtilityLibrary.CsXmlUtilities;

#endregion

// username: jeffs
// created:  5/25/2020 12:15:18 PM

namespace ClassifierEditor.SampleData
{
	public class SampleFileList : INotifyPropertyChanged
	{
	#region private fields

		private string fileName;

		public ObservableCollection<FilePath<FileNameSheetPdf>> Files { get; private set; }
			= new ObservableCollection<FilePath<FileNameSheetPdf>>();

	#endregion

	#region ctor

		public SampleFileList()
		{
			OnPropertyChange("Files");
		}


		public SampleFileList(string fileName)
		{
			// WriteXml();

			if (!fileName.IsVoid())
			{
				this.fileName = fileName;

				GetFiles();
			}

			OnPropertyChange("Files");
		}

	#endregion

	#region public properties

		public void AddPath(FilePath<FileNameSheetPdf> path)
		{
			Files.Add(path);
			OnPropertyChange("Files");
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		// public void WriteXml()
		// {
		// 	SampleFileData sd = new SampleFileData();
		//
		// 	sd.Header.Description = "this is a description";
		//
		// 	string nl = Environment.NewLine;
		// 	sd.Data.Sheets = nl + "this is a file string 01"
		// 		+ nl + "this is a file string 01"
		// 		+ nl + "this is a file string 02"
		// 		+ nl + "this is a file string 03"
		// 		+ nl + "this is a file string 04"
		// 		+ nl + "this is a file string 05"
		// 		+ nl;
		//
		// 	DataContractSerializer ds = new DataContractSerializer(typeof(SampleFileData));
		//
		// 	XmlWriterSettings xs = new XmlWriterSettings() {Indent = true} ;
		//
		// 	try
		// 	{
		// 		bool result = WriteXmlFile(
		// 			@"C:\ProgramData\CyberStudio\Andy\User Classification Files\test.xml", sd);
		//
		//
		// 		// using (XmlWriter w = XmlWriter.Create(@"C:\ProgramData\CyberStudio\Andy\User Classification Files\test.xml",
		// 		// 	xs))
		// 		// {
		// 		// 	ds.WriteObject(w,sd);
		// 		// }
		// 	}
		// 	catch (Exception e)
		// 	{
		// 		Console.WriteLine(e);
		// 		throw;
		// 	}
		//
		// 	sd = new SampleFileData();
		//
		// 	bool answer = ReadXmlFile(@"C:\ProgramData\CyberStudio\Andy\User Classification Files\test.xml", out sd);
		//
		// 	// using (FileStream fs = new FileStream(@"C:\ProgramData\CyberStudio\Andy\User Classification Files\test.xml", FileMode.Open))
		// 	// {
		// 	// 	sd = (SampleFileData) ds.ReadObject(fs);
		// 	// }
		// 	if (answer)
		// 	{
		// 		sd.Data.Parse();
		// 	}
		// 	
		//
		// 	Debug.WriteLine("xml read");
		// }


//		public void GetFiles()
//		{
//			string pattern = "*.pdf";
//
//			foreach (string file in
//				Directory.EnumerateFiles(fileName, pattern,
//					SearchOption.AllDirectories))
//			{
//				Files.Add(new FilePath<FileNameSheetPdf>(file));
//			}
//		}


		public void GetFiles2()
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}

			string file;

			StreamReader fileStream = new StreamReader(fileName);

			while ((file = fileStream.ReadLine()) != null)
			{
				file = file.Trim();

				if (file.Length == 0 || file.Substring(0, 1).Equals(@"<")) continue;

				Files.Add(new FilePath<FileNameSheetPdf>(file));
			}
		}

		public void GetFiles()
		{
			if (!File.Exists(fileName))throw new FileNotFoundException();

			SampleFileData sampleFileData = new SampleFileData();

			try
			{
				bool result = CsXmlUtilities.ReadXmlFile(fileName, out sampleFileData);
				
				if (!result) return;

				sampleFileData.Data.Parse();

			}
			catch (Exception e)
			{
				string m = e.Message;
				string im = e.InnerException?.Message;

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

	#region event processing

	#endregion

	#region event handeling

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

	[DataContract(Name = "SampleFile", Namespace = "")]
	public class SampleFileData
	{
		[DataMember(Order = 1)]
		public Hdr Header { get; set; } = new Hdr();

		[DataMember(Order = 2)]
		public Shts Data { get; set; } = new Shts();


		[DataContract(Namespace = "")]
		public class Hdr
		{
			[DataMember]
			public string Description { get; set; }
		}

		// [DataContract]
		// public class Shts
		// {
		// 	[DataMember]
		// 	public List<string> Sheets { get; set; } = new List<string>();
		// }

		[DataContract(Namespace = "")]
		public class Shts
		{
			[DataMember]
			public string Sheets { get; set; }

			[IgnoreDataMember]
			public string[] SheetList { get; set; }

			public bool Parse()
			{
				SheetList = Sheets?.Split(new [] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);

				return (SheetList?.Length ?? 0) > 0;
			}
		}
	}
}