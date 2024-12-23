﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sylvester.FileSupport;
using Sylvester.UserControls;


namespace Sylvester
{
	/// <summary>
	/// Interaction logic for FolderPath.xaml
	/// </summary>
	public partial class FolderPath : UserControl
	{
		private string[] path = new []
		{
			@"C:\path",
			@"C:\path"
		};

		private int nextIndex;

		private Color pathFontColor = Color.FromArgb(0xFF, 0xCC, 0xCC, 0xCC);

		public FolderPath()
		{
			InitializeComponent();
		}

		public int SelectedIndex { get; private set; }
		public string SelectedFolder { get; private set; }
		public string SelectedPath { get; private set; }

		public Route Path
		{
			get => new Route(path);
			set
			{
				SetPath(value);
			}
		}

		public void SetPath(Route newPath)
		{
			clearSkewedButtons();

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
			SkewedButton sk = new SkewedButton();
			sk.Text = text;
			sk.Index = index;
			sk.FontColor = new SolidColorBrush(pathFontColor);
			sk.InnerButton.Click += InnerButton_Click;

			SpPath.Children.Add(sk);
		}

		private void clearSkewedButtons()
		{
			foreach (SkewedButton spPathChild in SpPath.Children)
			{
				spPathChild.InnerButton.Click -= InnerButton_Click;
			}

			SpPath.Children.Clear();
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
//			SkewedButton sbx = ((Button) sender).Parent;

			SkewedButton skb = b.Tag as SkewedButton;

			SelectedIndex = (int) skb.Tag;
			SelectedFolder = skb.Text;
//			SelectedFolder = skb.InnerSp.Tag as string;
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

		public event FavoritesEventHandler Favorites;

		protected virtual void RaiseFavoritesEvent()
		{
			Favorites?.Invoke(this, new EventArgs());
		}

		// event 2, a folder was selected
		public delegate void PathChangedEventHandler(object sender, PathChangeArgs e);

		public event PathChangedEventHandler PathChange;

		protected virtual void RaisePathChangeEvent()
		{
			PathChange?.Invoke(this, new PathChangeArgs(SelectedIndex, SelectedFolder, SelectedPath));
		}

		// event 1, the select folder button was pressed
		public delegate void SelectFolderEventHandler(object sender, EventArgs e);

		public event SelectFolderEventHandler SelectFolder;

		protected virtual void RaiseSelectFolderEvent()
		{
			SelectFolder?.Invoke(this, new EventArgs());
		}

	#endregion


		public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register(
			"FontColor", typeof(SolidColorBrush), typeof(FolderPath), new PropertyMetadata(Brushes.White));

		public SolidColorBrush FontColor
		{
			get { return (SolidColorBrush) GetValue(FontColorProperty); }
			set { SetValue(FontColorProperty, value); }
		}


		public static readonly DependencyProperty ProposedSkewedButtonTypeProperty = DependencyProperty.Register(
			"ProposedSkewedButtonType", typeof(int), typeof(FolderPath), new PropertyMetadata(7));

		public int ProposedSkewedButtonType
		{
			get { return (int) GetValue(ProposedSkewedButtonTypeProperty); }
			set { SetValue(ProposedSkewedButtonTypeProperty, value); }
		}
	}

	public class PathTypeVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			int proposedPathType = (int) values[0];
			if (values[1] == DependencyProperty.UnsetValue) return true;

			int skewedButtonType = (int) values[1];

			if (proposedPathType == 0 || skewedButtonType == 0) return Visibility.Visible;

			int x = (proposedPathType & skewedButtonType);

			Visibility v =  x == 0 ? Visibility.Collapsed : Visibility.Visible;

			return v;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}