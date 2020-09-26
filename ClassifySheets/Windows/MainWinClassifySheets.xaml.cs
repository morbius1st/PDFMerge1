#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using UtilityLibrary;
using AndyShared;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.ClassificationFileSupport;
using UtilityLibrary;
using AndyShared.FileSupport;
using AndyShared.SampleFileSupport;
using AndyShared.Support;
using static UtilityLibrary.MessageUtilities;

#endregion

// projname: ClassifySheets
// itemname: MainWindow
// username: jeffs
// created:  9/24/2020 1:11:53 PM

namespace ClassifySheets.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWinClassifySheets : Window, INotifyPropertyChanged
	{
	#region private fields

		private ClassificationFile classificationFile;

		private static TreeNode userSelected;

		private Dictionary<string, string> cmdLineArgs = new Dictionary<string, string>();

		private string classfFileArg;

		private string tbx1Message;

	#endregion

	#region ctor

		public MainWinClassifySheets()
		{
			InitializeComponent();
		}

	#endregion

	#region public properties

		public SampleFileList TestFileList { get; private set; }

		public ClassificationFile ClassificationFile
		{
			get => classificationFile;
			private set
			{
				if (value == null) return;

				classificationFile = value;

				if (Common.SHOW_DEBUG_MESSAGE1) Debug.WriteLine("@ MainWinClassifySheets|@ ClassificationFile");

				classificationFile.Initialize();

				if (!classificationFile.SampleFilePath.IsVoid())
				{
					TestFileList = new SampleFileList(classificationFile.SampleFilePath);
				}

				string desc = classificationFile.HeaderDescFromMemory;

				OnPropertyChange();
				OnPropertyChange("BaseOfTree");
				OnPropertyChange("TestFileList");
				OnPropertyChange("UserSelected");
			}
		}

		public BaseOfTree BaseOfTree
		{
			get => classificationFile?.TreeBase ?? null;
		}

		public TreeNode UserSelected
		{
			get => userSelected;
			set
			{
				userSelected = value;

				OnPropertyChange();
				OnPropertyChange("HasSelection");
			}
		}

		public string Tbx1Message
		{
			get => tbx1Message;

			private set
			{
				tbx1Message += value;

				OnPropertyChange();
			}
		}


	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			getCmdLineArgs();

			try
			{
				if (classfFileArg.IsVoid())
				{
					ClassificationFile = ClassificationFileAssist.GetUserClassfFile("PdfSample 1A");
				}
				else
				{
					ClassificationFile = ClassificationFileAssist.GetUserClassfFile(classfFileArg);
				}
			}
			catch (Exception ex)
			{
				Debug.Write("Outer Exception| ");
				Debug.WriteLine(ex);

				Debug.Write("Inner Exception| ");
				Debug.WriteLine(ex.InnerException?.Message ?? "None");

				Environment.Exit(1);
			}

			classificationFile.Initialize();

			ListSampleFileList();

		}

		private void getCmdLineArgs()
		{
			string[] args = Environment.GetCommandLineArgs();

			int count = args.Length;

			if (count >= 3)
			{
				count -= 1;

				if (count % 2 == 0)
				{
					for (var i = 1; i < args.Length; i += 2)
					{
						if (args.Length == i+1 || args[i + 1].StartsWith("-"))
						{
							cmdLineArgs.Add(args[i], string.Empty);
						}
						else
						{
							cmdLineArgs.Add(args[i], args[i + 1]);
						}
					}

					foreach (KeyValuePair<string, string> kvp in cmdLineArgs)
					{
						if (kvp.Key == "-classification_file")
						{
							classfFileArg = kvp.Value;

							Debug.WriteLine("classf file cmd line arg found| " + classfFileArg);
						}
					}
				}
			}

		}

		private void ListSampleFileList()
		{
			int i = 0;

			tbx1Message = "";

			foreach (FilePath<FileNameSheetPdf> filePath in TestFileList.Files)
			{
				FileNameSheetPdf fp = filePath.FileNameObject;

				StringBuilder sb = new StringBuilder();

				sb.Append(i);
				sb.Append(" |  ").Append(fp.PhaseBldg);
				sb.Append(" |  ").Append(fp.Discipline);
				sb.Append(" |  ").Append(fp.Category);
				sb.Append(" |  ").Append(fp.Subcategory);
				sb.Append(" |  ").Append(fp.Modifier);
				sb.Append(" |  ").Append(fp.Submodifier);
				sb.Append(" |  component| ").Append(fp.SheetIdByComponent);
				sb.Append(" |  sht title| ").Append(fp.SheetTitle);

				sb.Append(nl);

				tbx1Message += sb.ToString();
			}

			OnPropertyChange("Tbx1Message");
		}


	#endregion

	#region event consuming

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@ Debug");
		}

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);

		}
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
			return "this is MainWindow";
		}

		#endregion

			
	}
}