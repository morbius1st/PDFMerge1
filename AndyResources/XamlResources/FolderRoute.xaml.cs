﻿using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using UtilityLibrary;
using WpfShared.Dialogs.DialogSupport;
using AndyResources.XamlResources;

namespace AndyResources.XamlResources
{
	public enum ObliqueButtonType
	{
		TEXT = 1,
		SELECTFOLDER = 2,
		FAVORITES = 4,
		HISTORY = 8
	}


	/// <summary>
	/// Interaction logic for FolderRoute.xaml
	/// </summary>
	public partial class FolderRoute : UserControl
	{
		private FilePath<FileNameSimple> path;

		private int nextIndex;

		public FolderRoute()
		{
			InitializeComponent();

			BtnText.Text = "Hello";
			// Path = new FilePath<FileNameSimple>(@"P:\2099-900 Sample Project\Publish\Bulletins\2017-07-00 flat");
		}

		public bool IsPathValid => Path?.IsValid ?? false;
		public int SelectedIndex { get; private set; }
		public string SelectedFolder { get; private set; }

		//		public FilePath<FileNameSimple> SelectedPath => path;

		public FilePath<FileNameSimple> Path
		{
			get => path;
			set
			{
				path = value;
				SetPath(value);
				RaisePathChangeEvent();
			}
		}

		//		public FilePath<FileNameSimple> Path
		//		{
		//			get => Path;
		//			set
		//			{
		//				SetPath(value);
		//			}
		//		}

		//		public void AssignEvents(FolderManager fm)
		//		{
		//			PathChange += fm.onPathPathChangeEvent;
		//			SelectFolder += fm.onPathSelectFolderEvent;
		//			Favorites += fm.onPathFavoriteEvent;
		//			History += fm.onPathHistoryEvent;
		//
		//		}

		private void clearObliqueButtons()
		{
			foreach (ObliqueButton spPathChild in SpPath.Children)
			{
#pragma warning disable CS1061 // 'ObliqueButton' does not contain a definition for 'Ib' and no accessible extension method 'Ib' accepting a first argument of type 'ObliqueButton' could be found (are you missing a using directive or an assembly reference?)
				spPathChild.Ib.Click -= InnerButton_Click;
#pragma warning restore CS1061 // 'ObliqueButton' does not contain a definition for 'Ib' and no accessible extension method 'Ib' accepting a first argument of type 'ObliqueButton' could be found (are you missing a using directive or an assembly reference?)
			}

			SpPath.Children.Clear();
		}


		private void SetPath(FilePath<FileNameSimple> newPath)
		{
			clearObliqueButtons();

			SelectedFolder = null;

			if (newPath == null || !newPath.IsValid)
			{
				// when null, reset and show the select folder button
				path = FilePath<FileNameSimple>.Invalid;
				SetValue(FilePathProperty, path);
				SelectedFolder = null;
			}
			else
			{
				//				path = newPath;
				//				SetValue(FilePathProperty, path);

				AddPath(newPath);

				SelectedFolder = newPath[-1.1];
				SelectedIndex = newPath.Depth;
			}

			//			RaisePathChangeEvent();
		}

		private void AddPath(FilePath<FileNameSimple> path)
		{
			nextIndex = 0;

			foreach (string s in path.GetPathNames)
			{
				Add(s, nextIndex++);
			}

			ScrollBar.ScrollToRightEnd();
		}

		private void Add(string text, int index)
		{
			ObliqueButton ob = new ObliqueButton();
			ob.Name = $"obx_{index:D3}";
			ob.Text = text;
			ob.Index = index;

			if (index == 0)
			{
				ob.Style = (Style)folderRoute.FindResource("ObBtn");
			}
			else
			{
				ob.Style = (Style)folderRoute.FindResource("ObText");
			}

#pragma warning disable CS1061 // 'ObliqueButton' does not contain a definition for 'Ib' and no accessible extension method 'Ib' accepting a first argument of type 'ObliqueButton' could be found (are you missing a using directive or an assembly reference?)
			ob.Ib.Click += InnerButton_Click;
#pragma warning restore CS1061 // 'ObliqueButton' does not contain a definition for 'Ib' and no accessible extension method 'Ib' accepting a first argument of type 'ObliqueButton' could be found (are you missing a using directive or an assembly reference?)

			SpPath.Children.Add(ob);
		}


		private void InnerButton_Favorites(object sender, RoutedEventArgs e)
		{
			RaiseFavoritesEvent();
		}

		private void InnerButton_History(object sender, RoutedEventArgs e)
		{
			RaiseHistoryEvent();
		}

		private void InnerButton_SelectFolder(object sender, RoutedEventArgs e)
		{
			RaiseSelectFolderEvent();
		}

		private void InnerButton_Click(object sender, RoutedEventArgs e)
		{


			// Button b = (Button)sender;

			// ObliqueButton ob = b.Tag as ObliqueButton;
			ObliqueButton ob = sender as ObliqueButton;

			SelectedIndex = (int)ob.Tag;
			SelectedFolder = ob.Text;
			//			Path = se + @"\";

			if (SelectedIndex > 0)
			{
				StringBuilder sb = new StringBuilder(path[0]);

				for (int i = 1; i < SelectedIndex + 1; i++)
				{
					sb.Append(@"\").Append(path[i]);
				}

				Path = new FilePath<FileNameSimple>(sb.ToString());
			}
			else
			{
				RaiseSelectFolderEvent();
			}
		}

		#region event handeling

		// event 1, a folder was selected
		public delegate void PathChangedEventHandler(object sender, PathChangeArgs e);

#pragma warning disable CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.
		public event FolderRoute.PathChangedEventHandler PathChange;
#pragma warning restore CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.

		protected virtual void RaisePathChangeEvent()
		{
			PathChange?.Invoke(this, new PathChangeArgs(SelectedIndex, SelectedFolder, Path));
		}

		// event 2, the select folder button was pressed
		public delegate void SelectFolderEventHandler(object sender, EventArgs e);

#pragma warning disable CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.
		public event FolderRoute.SelectFolderEventHandler SelectFolder;
#pragma warning restore CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.

		protected virtual void RaiseSelectFolderEvent()
		{
			SelectFolder?.Invoke(this, new EventArgs());
		}

		// event 3, favorites was selected
		public delegate void FavoritesEventHandler(object sender, EventArgs e);

#pragma warning disable CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.
		public event FolderRoute.FavoritesEventHandler Favorites;
#pragma warning restore CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.

		protected virtual void RaiseFavoritesEvent()
		{
			Favorites?.Invoke(this, new EventArgs());
		}

		// event 4, history was selected
		public delegate void HistoryEventHandler(object sender, EventArgs e);

#pragma warning disable CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.
		public event FolderRoute.HistoryEventHandler History;
#pragma warning restore CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.

		protected virtual void RaiseHistoryEvent()
		{
			History?.Invoke(this, new EventArgs());
		}

		#endregion

		#region control properties

		public static readonly DependencyProperty FontBrushProperty = DependencyProperty.Register(
#pragma warning disable CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.
			"FontBrush", typeof(SolidColorBrush), typeof(FolderRoute), new PropertyMetadata(Brushes.White));
#pragma warning restore CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.

		public SolidColorBrush FontBrush
		{
			get { return (SolidColorBrush)GetValue(FontBrushProperty); }
			set { SetValue(FontBrushProperty, value); }
		}


		public static readonly DependencyProperty ProposedObliqueButtonTypeProperty = DependencyProperty.Register(
#pragma warning disable CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.
			"ProposedObliqueButtonType", typeof(int), typeof(FolderRoute), new PropertyMetadata(7));
#pragma warning restore CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.

		public int ProposedObliqueButtonType
		{
			get { return (int)GetValue(ProposedObliqueButtonTypeProperty); }
			set { SetValue(ProposedObliqueButtonTypeProperty, value); }
		}

		public static readonly DependencyProperty ObliqueButtonMarginProperty = DependencyProperty.Register(
#pragma warning disable CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.
			"ObliqueButtonMargin", typeof(Thickness), typeof(FolderRoute), new PropertyMetadata(new Thickness(0)));
#pragma warning restore CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.

		public Thickness ObliqueButtonMargin
		{
			get { return (Thickness)GetValue(ObliqueButtonMarginProperty); }
			set { SetValue(ObliqueButtonMarginProperty, value); }
		}


		public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
#pragma warning disable CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.
			"TextMargin", typeof(Thickness), typeof(FolderRoute), new PropertyMetadata(new Thickness(0)));
#pragma warning restore CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.

		public Thickness TextMargin
		{
			get { return (Thickness)GetValue(TextMarginProperty); }
			set { SetValue(TextMarginProperty, value); }
		}

		public static readonly DependencyProperty TextFontSizeProperty = DependencyProperty.Register(
#pragma warning disable CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.
			"TextFontSize", typeof(double), typeof(FolderRoute), new PropertyMetadata(8.0));
#pragma warning restore CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.

		public double TextFontSize
		{
			get { return (double)GetValue(TextFontSizeProperty); }
			set { SetValue(TextFontSizeProperty, value); }
		}

		public static readonly DependencyProperty ObliqueButtonHeightProperty = DependencyProperty.Register(
#pragma warning disable CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.
			"ObliqueButtonHeight", typeof(double), typeof(FolderRoute), new PropertyMetadata(13.0));
#pragma warning restore CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.

		public double ObliqueButtonHeight
		{
			get { return (double)GetValue(ObliqueButtonHeightProperty); }
			set { SetValue(ObliqueButtonHeightProperty, value); }
		}

		public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath",
#pragma warning disable CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.
				typeof(FilePath<FileNameSimple>), typeof(FolderRoute), new PropertyMetadata(null));
#pragma warning restore CS0436 // The type 'FolderRoute' in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'FolderRoute' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\AndyResources\XamlResources\FolderRoute.xaml.cs'.

		public FilePath<FileNameSimple> FilePath
		{
			get => (FilePath<FileNameSimple>)GetValue(FilePathProperty);
			set
			{
				Path = value;
				SetValue(FilePathProperty, value);
			}
		}

		#endregion
	}

	public class PathTypeVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
#pragma warning disable CS0436 // The type 'ObliqueButtonType' in 'B:\Programming\VisualStudioProjects\PDFMerge1\Andy\..\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'ObliqueButtonType' in 'WpfShared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\Andy\..\AndyResources\XamlResources\FolderRoute.xaml.cs'.
			int proposedPathType = (int)values[0];
#pragma warning restore CS0436 // The type 'ObliqueButtonType' in 'B:\Programming\VisualStudioProjects\PDFMerge1\Andy\..\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'ObliqueButtonType' in 'WpfShared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\Andy\..\AndyResources\XamlResources\FolderRoute.xaml.cs'.
			if (values[1] == DependencyProperty.UnsetValue) return true;

#pragma warning disable CS0436 // The type 'ObliqueButtonType' in 'B:\Programming\VisualStudioProjects\PDFMerge1\Andy\..\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'ObliqueButtonType' in 'WpfShared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\Andy\..\AndyResources\XamlResources\FolderRoute.xaml.cs'.
			int buttonType = (int)(ObliqueButtonType)values[1];
#pragma warning restore CS0436 // The type 'ObliqueButtonType' in 'B:\Programming\VisualStudioProjects\PDFMerge1\Andy\..\AndyResources\XamlResources\FolderRoute.xaml.cs' conflicts with the imported type 'ObliqueButtonType' in 'WpfShared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\Andy\..\AndyResources\XamlResources\FolderRoute.xaml.cs'.

			if (proposedPathType == 0 || buttonType == 0) return Visibility.Visible;

			int x = (proposedPathType & buttonType);

			Visibility v = x == 0 ? Visibility.Collapsed : Visibility.Visible;

			return v;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}