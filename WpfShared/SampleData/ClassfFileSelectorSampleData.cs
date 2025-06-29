﻿
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using AndyShared.ClassificationFileSupport;
using AndyShared.ConfigMgrShared;
using AndyShared.FileSupport;
using AndyShared.SampleFileSupport;
using SettingsManager;


// Solution:     PDFMerge1
// Project:       WpfShared
// File:             ClassfFileSelectorSampleData.cs
// Created:      2020-09-14 (12:28 PM)

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


			selected = new ClassificationFile(FileLocationSupport.ClassifFileLocationUser + "\\"+ @"(jeffs) PdfSample 1.xml");
			selected.SampleFilePath =
				FileLocationSupport.ClassifSampleFileLocationUser + "\\"+ @"PdfSample A.sample";

			cfgClsFiles.UserClassificationFiles.Add(selected);

			selected = new ClassificationFile(FileLocationSupport.ClassifFileLocationUser + "\\"+ @"(jeffs) PdfSample 1A.xml");
			selected.SampleFilePath =
				FileLocationSupport.ClassifSampleFileLocationUser + "\\"+ @"PdfSample B.sample";

			cfgClsFiles.UserClassificationFiles.Add(selected);

			selected = new ClassificationFile(FileLocationSupport.ClassifFileLocationUser + "\\"+ @"(jeffs) PdfSample 2.xml");
			selected.SampleFilePath =
				FileLocationSupport.ClassifSampleFileLocationUser + "\\"+ @"PdfSample C.sample";

			cfgClsFiles.UserClassificationFiles.Add(selected);
			
			selected = new ClassificationFile(FileLocationSupport.ClassifFileLocationUser + "\\"+ @"(jeffs) PdfSample 3.xml");
			selected.SampleFilePath =
				FileLocationSupport.ClassifSampleFileLocationUser + "\\"+ @"PdfSample B.sample";

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