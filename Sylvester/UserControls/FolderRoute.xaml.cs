using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sylvester.FileSupport;

namespace Sylvester.UserControls
{
	/// <summary>
	/// Interaction logic for FolderRoute.xaml
	/// </summary>
	public partial class FolderRoute : UserControl
	{
		private string[] path = new []
		{
			@"C:\path",
			@"C:\path"
		};

		private int nextIndex;

		private Color pathFontColor = Color.FromArgb(0xFF, 0xCC, 0xCC, 0xCC);

		public FolderRoute()
		{
			InitializeComponent();
		}

		public int SelectedIndex { get; private set; }
		public string SelectedFolder { get; private set; }
		public string SelectedPath { get; private set; }

		public Route Path
		{
			get => new Route(path);
			set { SetPath(value); }
		}

		private void clearObliqueButtons()
		{
			foreach (ObliqueButton spPathChild in SpPath.Children)
			{
				spPathChild.InnerButton.Click -= InnerButton_Click;
			}

			SpPath.Children.Clear();
		}


		public void SetPath(Route newPath)
		{
			clearObliqueButtons();

			SelectedFolder = null;

			if (newPath == null || !newPath.IsValid)
			{
				// when null, reset and show the select folder button
				SelectedPath = null;
				SelectedFolder = null;
				path = null;
			}
			else
			{
				AddPath(newPath.FullPathNames);
			}
		}

		private void AddPath(string[] path)
		{
			this.path = path;
			nextIndex = 0;

			foreach (string s in this.path)
			{
				Add(s, nextIndex++);
			}
		}

		private void Add(string text, int index)
		{

			ObliqueButton ob = new ObliqueButton();
			ob.Name = $"obx_{index:D3}";
			ob.Text = text;
			ob.Index = index;
			ob.Style = (Style) folderRoute.FindResource("ObText");

			ob.InnerButton.Click += InnerButton_Click;

			SpPath.Children.Add(ob);
		}


		private void InnerButton_Favorites(object sender, RoutedEventArgs e)
		{
			RaiseFavoritesEvent();
		}

		private void InnerButton_SelectFolder(object sender, RoutedEventArgs e)
		{
			RaiseSelectFolderEvent();
		}

		private void InnerButton_Click(object sender, RoutedEventArgs e)
		{
			Button b = (Button) sender;

			ObliqueButton ob = b.Tag as ObliqueButton;

			SelectedIndex = (int) ob.Tag;
			SelectedFolder = ob.Text;
			SelectedPath = path[0] + @"\";

			if (SelectedIndex > 0)
			{
				StringBuilder sb = new StringBuilder(path[0]);

				for (int i = 1; i < SelectedIndex + 1; i++)
				{
					sb.Append(@"\").Append(path[i]);
				}

				SelectedPath = sb.ToString();

				RaisePathChangeEvent();
			}
			else
			{
				RaiseSelectFolderEvent();
			}
		}

	#region event handeling

		// event 2, a folder was selected
		public delegate void FavoritesEventHandler(object sender, EventArgs e);

		public event FolderRoute.FavoritesEventHandler Favorites;

		protected virtual void RaiseFavoritesEvent()
		{
			Favorites?.Invoke(this, new EventArgs());
		}

		// event 2, a folder was selected
		public delegate void PathChangedEventHandler(object sender, PathChangeArgs e);

		public event FolderRoute.PathChangedEventHandler PathChange;

		protected virtual void RaisePathChangeEvent()
		{
			PathChange?.Invoke(this, new PathChangeArgs(SelectedIndex, SelectedFolder, SelectedPath));
		}

		// event 1, the select folder button was pressed
		public delegate void SelectFolderEventHandler(object sender, EventArgs e);

		public event FolderRoute.SelectFolderEventHandler SelectFolder;

		protected virtual void RaiseSelectFolderEvent()
		{
			SelectFolder?.Invoke(this, new EventArgs());
		}

	#endregion




		public static readonly DependencyProperty FontBrushProperty = DependencyProperty.Register(
			"FontBrush", typeof(SolidColorBrush), typeof(FolderRoute), new PropertyMetadata(Brushes.White));

		public SolidColorBrush FontBrush
		{
			get { return (SolidColorBrush) GetValue(FontBrushProperty); }
			set { SetValue(FontBrushProperty, value); }
		}


		public static readonly DependencyProperty ProposedObliqueButtonTypeProperty = DependencyProperty.Register(
			"ProposedObliqueButtonType", typeof(int), typeof(FolderRoute), new PropertyMetadata(7));

		public int ProposedObliqueButtonType
		{
			get { return (int) GetValue(ProposedObliqueButtonTypeProperty); }
			set { SetValue(ProposedObliqueButtonTypeProperty, value); }
		}


		public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
			"TextMargin", typeof(Thickness), typeof(FolderRoute), new PropertyMetadata(new Thickness(0)));

		public Thickness TextMargin
		{
			get { return (Thickness) GetValue(TextMarginProperty); }
			set { SetValue(TextMarginProperty, value); }
		}
	}

	public class PathTypeVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			int proposedPathType = (int) values[0];
			if (values[1] == DependencyProperty.UnsetValue) return true;

			int buttonType = (int) values[1];

			if (proposedPathType == 0 || buttonType == 0) return Visibility.Visible;

			int x = (proposedPathType & buttonType);

			Visibility v =  x == 0 ? Visibility.Collapsed : Visibility.Visible;

			return v;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}


}
