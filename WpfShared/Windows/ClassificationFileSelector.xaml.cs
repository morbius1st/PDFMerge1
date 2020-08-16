using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AndyShared.ClassificationFileSupport;
using AndyShared.ConfigMgrShared;
using CSLibraryIo.CommonFileFolderDialog;
using Microsoft.WindowsAPICodePack.Dialogs;
using UtilityLibrary;


/*
functions:
primary: select  a classification file

secondary:
re: classification file
1. new:  create a basic / blank file
2. duplicate: make a copy of the selected classification file - must provide a new fileid
	does not duplicate 
3. delete: delete a classification file 



*/


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
		public static string TEST = "";

	#region private fields

		public static string NO_SAMPLE_FILE = "No Sample File Found";

		private ClassificationFiles cfgClsFiles = null;

		private ClassificationFile selected;

		private ICollectionView view;

		Balloon b;

	#endregion

	#region ctor

		public ClassificationFileSelector()
		{
			InitializeComponent();

			initialize();
		}


		#endregion

		#region public properties

		public ClassificationFiles CfgClsFiles => cfgClsFiles;

		public ICollectionView View => view;

		public ClassificationFile Selected
		{
			get => selected;
			set
			{

				if (selected != null)  selected.SelectedPropertyChanged -= Selected_SelectedPropertyChanged;

				selected = value;

				selected.SelectedPropertyChanged += Selected_SelectedPropertyChanged;

				selected.Read();

				OnPropertyChange();
			}
		}

		public string UserName => Environment.UserName;

		public string ListViewTitle => UserName + "'s Classification Files";

		public bool FileIdPopupIsOpen { get; set; } = false;

	#endregion

	#region private properties



	#endregion

	#region public methods



	#endregion

	#region private methods

		private void initialize()
		{
			cfgClsFiles = ClassificationFiles.Instance;

			cfgClsFiles.Initialize();

			initializeView();
		}

		private void initializeView()
		{
			view = CollectionViewSource.GetDefaultView(cfgClsFiles.UserClassificationFiles);

			view.Filter = new Predicate<object>(MatchUser);

			OnPropertyChange("View");

		}

		private bool MatchUser(object usr)
		{
			ClassificationFile user = usr as ClassificationFile;

			return user.UserName.Equals(UserName);
		}

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling


		private bool tbxFlag; // text changed

		private void BtnSelectSampleFile_OnClick(object sender, RoutedEventArgs e)
		{
			if (selected.GetFolderPath == null) return;

			FileAndFolderDialog fd = new FileAndFolderDialog();

			FileAndFolderDialog.FileAndFolderDialogSettings fdSetg = new FileAndFolderDialog.FileAndFolderDialogSettings();

			fdSetg.Filters.Add(new CommonFileDialogFilter("Sample File", "*.dat"));
			
			string file = fd.GetFile("Select a Sample File", selected.GetFolderPath, fdSetg);

			if (fd.HasSelection)
			{
				Selected.InstallSampleFile(file);
			}
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}


		private void TextBox_OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (tbxFlag)
				{
					TextBox_UpdateSource((TextBox) sender);

					tbxFlag = false;
				}
			} 
			else
			{
				tbxFlag = true;
			}
		}

		private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
		{
			if (tbxFlag)
			{
				TextBox_UpdateSource((TextBox) sender);

				tbxFlag = false;
			}
		}

		private void TextBox_UpdateSource(TextBox tbx)
		{
			tbx.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
		}

		private void Selected_SelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
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
