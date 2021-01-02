// Solution:     PDFMerge1
// Project:       WpfShared
// File:             ClassfFileSelectorSampleData.cs
// Created:      2020-09-14 (12:28 PM)

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using AndyShared.ClassificationFileSupport;
using AndyShared.SampleFileSupport;

namespace WpfShared.SampleData
{
	public class ClassfFileSelectorSampleData : INotifyPropertyChanged
	{

		private static ClassificationFiles cfgClsFiles = null;

		private static ClassificationFile selected;

		// private static SampleFiles sampleFiles = null;

		private static SampleFile selectedSampleFile;

		static ClassfFileSelectorSampleData()
		{

			cfgClsFiles = new ClassificationFiles();
			
			cfgClsFiles.UserClassificationFiles = new ObservableCollection<ClassificationFile>();
			// cfgClsFiles.Initialize();

			selected = new ClassificationFile(@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\(jeffs) PdfSample 1.xml");
			selected.SampleFilePath =
				@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\Sample Files\PdfSample A.sample";
			

			cfgClsFiles.UserClassificationFiles.Add(selected);

			selected = new ClassificationFile(@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\(jeffs) PdfSample 1A.xml");
			selected.SampleFilePath =
				@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\Sample Files\PdfSample B.sample";

			cfgClsFiles.UserClassificationFiles.Add(selected);

			selected = new ClassificationFile(@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\(jeffs) PdfSample 2.xml");
			selected.SampleFilePath =
				@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\Sample Files\PdfSample C.sample";

			cfgClsFiles.UserClassificationFiles.Add(selected);
			
			selected = new ClassificationFile(@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\(jeffs) PdfSample 3.xml");
			selected.SampleFilePath =
				@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\Sample Files\PdfSample B.sample";

			cfgClsFiles.UserClassificationFiles.Add(selected);
			


			// cfgClsFiles.UpdateView();

			// sampleFiles = SampleFiles.Instance;
			// sampleFiles.Initialize(cfgClsFiles.UserClassfFolderPath);
			// reinitialize();
		}

		public static ClassificationFile Selected => selected;

		public static ClassificationFiles CfgClsFiles => cfgClsFiles;

		public static int Count => cfgClsFiles.UserClassificationFiles.Count;

		private static void reinitialize()
		{
			cfgClsFiles.Reinitialize();

			// sampleFiles.reinitialize();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}
}