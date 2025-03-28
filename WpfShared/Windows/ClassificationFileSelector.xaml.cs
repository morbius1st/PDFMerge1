﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using UtilityLibrary;
using AndyShared.ClassificationFileSupport;
using WpfShared.Dialogs;
using WpfShared.SampleData;
using AndyShared.SampleFileSupport;
using SettingsManager;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;
using static AndyShared.ClassificationFileSupport.ClassificationFile;
using System.Diagnostics;
using AndyShared.Support;


namespace WpfShared.Windows
{
	/// <summary>
	/// Interaction logic for ClassificationFileSelector.xaml <br/>
	/// </summary>
	/// <remarks>
	/// Be aware that this window will be used by other assemblies.  This <br/>
	/// means that the user and app setting file read will be from referencing <br/>
	/// assembly and not from this assembly<br/>
	/// </remarks>
	public partial class ClassificationFileSelector : Window, INotifyPropertyChanged
	{
		public static string NO_SAMPLE_FILE { get; } = "A Sample File has not been Configured";

	#region private fields
		
		private int beforeIdx;
		private int cbxSelIdx = 0;

		private WindowId winId = WindowId.DIALOG_SELECT_CLASSF_FILE;
		private WindowManager winMgr = new WindowManager();

		private ClassificationFiles cfgClsFiles = null;

		private ClassificationFile selected;

		private SampleFiles sampleFiles = null;

		private SampleFile selectedSampleFile;

		private ICollectionView view;

		private Balloon b;

		private bool tbxKeyProcessingFlag;

		public static ClassfFileSelectorSampleData SampleData { get; set; } = new ClassfFileSelectorSampleData();

	#endregion

	#region ctor

		static ClassificationFileSelector()
		{
			ClassificationFiles cfgClsFiles = ClassfFileSelectorSampleData.CfgClsFiles;
		}

		public ClassificationFileSelector()
		{
			InitializeComponent();

			WinHandle = ScreenParameters.GetWindowHandle(Common.GetCurrentWindow());

			UserSettings.Admin.Read();

			initialize();

			initWindowLocation();

		}

	#endregion

	#region public properties

		public static IntPtr WinHandle { get; private set; }

		public ClassificationFiles CfgClsFiles => cfgClsFiles;

		public ICollectionView View => view;

		public ClassificationFile Selected
		{
			get => selected;
			set
			{
				if (selected != null)  selected.FileIdChanged -= selected_FileIdChanged;

				selected = value;

				if (selected != null)
				{
					selected.FileIdChanged += selected_FileIdChanged;

					selected.Initialize();

					selectedSampleFile = null;

					SearchText = selected.SampleFileNameNoExt;
				}

				OnPropertyChange();
				OnPropertyChange(nameof(SelectedSample));

				
			}
		}

		public SampleFile SelectedSample
		{
			get => selectedSampleFile;
			set
			{
				if (value?.SortName != null &&
					selectedSampleFile?.SortName != null &&
					!selectedSampleFile.SortName.Equals(value.SortName))
				{
					if (value.SortName.Equals(SampleFiles.SAMPLE_CBX_FIRST_ITEM))
					{
						selected.UpdateSampleFile(null);
					}
					else
					{
						selected.UpdateSampleFile(value.SortName);
						OnPropertyChange("Selected");
					}
				}

				selectedSampleFile = value;

				OnPropertyChange();
			}
		}

		public string UserName => Environment.UserName;

		public string ListViewTitle => UserName + "'s Classification Files";

		public bool FileIdPopupIsOpen { get; set; } = false;

		public SampleFiles SampleFiles => sampleFiles;

		public int CbxSelIdx
		{
			get => cbxSelIdx;
			set
			{
				cbxSelIdx = value;
				OnPropertyChange();
			}
		}

		public string SelectedFileId => selected.FileId;

		private string searchText;

		public string SearchText
		{
			get => searchText;
			set
			{
				if (selected?.CanEdit ?? false)
				{
					if (value.IsVoid())
					{
						searchText = SampleFiles.SAMPLE_CBX_FIRST_ITEM;
					}
					else
					{
						searchText = value;
					}
				}
				else
				{
					searchText = "*";
				}

				OnPropertyChange();
			}
		}


	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void initWindowLocation()
		{
			WindowLayout layout = UserSettings.Data.GetWindowLayout(winId);

			winMgr.RestoreWinLayout(this, UserSettings.Data.GetWindowLayout(winId));
		}

		private void initialize()
		{
			cfgClsFiles = ClassificationFiles.Instance;
			cfgClsFiles.Initialize();

			sampleFiles = SampleFiles.Instance;
			sampleFiles.Initialize(cfgClsFiles.UserClassfFolderPath);

			reinitialize();
		}

		private void reinitialize()
		{
			cfgClsFiles.Reinitialize();

			sampleFiles.reinitialize();

			OnPropertyChange("CfgClsFiles");
			OnPropertyChange("SampleFiles");

			// CbxSampleFiles.SelectedIndex = 0;
			CbxSelIdx = 0;

			initializeView();
		}

		private void initializeView()
		{
			view = CollectionViewSource.GetDefaultView(cfgClsFiles.UserClassificationFiles);

			OnPropertyChange("View");
		}

		private bool rename(string newFileId)
		{
			string newFileName = Rename(selected.FilePathLocal, newFileId);

			if (!newFileName.IsVoid())
			{
				selected.FilePathLocal.ChangeFileName(newFileName, "");

				OnPropertyChange("Selected");

				selected.UpdateProperties();

				return true;
			}

			return false;
		}

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

		private void winClsFileMgr_Closing(object sender, CancelEventArgs e)
		{
			UserSettings.Data.SetWindowLayout(winId, winMgr.GetWinLayout(this, winId));
		}


		private void CbxSampleFiles_DropDownOpened(object sender, EventArgs e)
		{
			object s = sender;
			ComboBox c = sender as ComboBox;
			beforeIdx = c.SelectedIndex;
		}

		private void CbxSampleFiles_DropDownClosed(object sender, EventArgs e)
		{
			object s = sender;
			ComboBox c = sender as ComboBox;
			int idx = c.SelectedIndex;
		}

		private void CbxSampleFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SampleFile s = e.AddedItems[0] as SampleFile;
		}

		private void BtnInstallSample_OnClick(object sender, RoutedEventArgs e)
		{
			int currIdx = Lb1.SelectedIndex;

			FilePath<FileNameSimple> newFilePath =
				SampleFileAssist.InstallSampleFile(cfgClsFiles.AllClassifFolderPath,
					sampleFiles.SampleFileFolderPath);

			if (newFilePath != null && newFilePath.IsValid) reinitialize();

			Lb1.SelectedIndex = currIdx;
		}

		private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsMgr<UserSettingPath, UserSettingInfo<UserSettingDataFile>, UserSettingDataFile> u = UserSettings.Admin;

			selected = null;
			DialogResult = false;

			this.Close();
		}

		private void BtnSelect_OnClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;

			this.Close();
		}

		private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
		{
			string selectedFilePath = selected.FullFilePath;

			Lb1.SelectedIndex = -1;

			if (Delete(selectedFilePath))
			{
				reinitialize();
			}
		}

		private void BtnNew_OnClick(object sender, RoutedEventArgs e)
		{
			ClassificationFile cf = Create(null, cfgClsFiles.AllClassifFolderPath);

			if (cf == null) return;

			cfgClsFiles.UserClassificationFiles.Add(cf);

			Lb1.SelectedItem = cf;
		}

		private void BtnCopy_OnClick(object sender, RoutedEventArgs e)
		{
			bool result = true;

			if (selected == null || !selected.IsValid) return;

			while (result)
			{
				DialogGetFileId dlg = new DialogGetFileId("File Id for the Duplicate Classification File");

				dlg.Owner = this;

				if (dlg.ShowDialog() == true)
				{
					string fileId = dlg.FileId;

					if (!Duplicate(selected.FilePathLocal, fileId)) return;

					reinitialize();

					result = false; // done - exit loop
				}
				else
				{
					result = false; // canceled dialog - exit loop
				}
			}
		}

		private void BtnRename_OnClick(object sender, RoutedEventArgs e)
		{
			if (selected == null || !selected.IsValid) return;

			bool result = true;

			// loop until done or cancel
			while (result)
			{
				DialogGetFileId dlg =
					new DialogGetFileId("New File Id for an Existing Classification File",
						selected.FileId);

				dlg.Owner = this;

				if (dlg.ShowDialog() == true)
				{
					string fileId = dlg.FileId;

					result = !rename(fileId);
				}
				else // selected cancel in the dialog box - exit loop
				{
					result = false;
				}
			}
		}

		private void TextBox_OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (tbxKeyProcessingFlag)
				{
					TextBox_UpdateSource((TextBox) sender);

					tbxKeyProcessingFlag = false;
				}
			}
			else
			{
				// non-enter pressed, start editing
				// flag editing the textbox
				tbxKeyProcessingFlag = true;
			}
		}

		private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
		{
			if (tbxKeyProcessingFlag)
			{
				TextBox_UpdateSource((TextBox) sender);

				tbxKeyProcessingFlag = false;
			}
		}

		private void TextBox_UpdateSource(TextBox tbx)
		{
			// textbox changed - about to process change
			// validate change
			// if change is OK, tell textbox to update its source
			// if not OK return
			if (!rename(tbx.Text))
			{
				// rename failed
				// restore the textbox's value
				tbx.Text = selected.FileId;

				return;
			}
			else
			{
				// rename worked - show balloon
				ShowRenameBalloon();
			}

			tbx.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
		}

		private void selected_FileIdChanged(object sender, PropertyChangedEventArgs e)
		{
			ShowRenameBalloon();
		}

		private void ShowRenameBalloon()
		{
			b = new Balloon(this, TbxFileId, "Classification file was renamed");
			b.X = 20;
			b.Orientation = Balloon.BalloonOrientation.BOTTOM_RIGHT;
			b.ShowDialog();
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ClassificationFileSelector";
		}

		#endregion


	}
}