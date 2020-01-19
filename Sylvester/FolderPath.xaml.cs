using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Sylvester
{
	/// <summary>
	/// Interaction logic for FolderPath.xaml
	/// </summary>
	public partial class FolderPath : UserControl
	{
		private string[] path;
		private int nextIndex;

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
			set { SetPath(value); }
		}

		public void SetPath(Route newPath)
		{
			SelectedFolder = null;

			ClearSkewedButtons();

			if (newPath == null || !newPath.IsValid)
			{
				// when null, reset and show the select folder button
				SelectedPath = null;
				SelectedFolder = null;
				path = null;

				AddSelectFolderButton();
			}
			else
			{
				path = newPath.FullPathNames;

				AddPath();
			}
		}

		private void AddPath()
		{
			ClearSkewedButtons();
			nextIndex = 0;

			foreach (string s in path)
			{
				Add(s, nextIndex++);
			}
		}

		private void Add(string text, int index)
		{
			SkewedButton sk = new SkewedButton();
			sk.Text = text;
			sk.Index = index;
			sk.InnerButton.Click += InnerButton_Click;

			PathDockPanel.Children.Add(sk);
		}

		private void ClearSkewedButtons()
		{
			if (PathDockPanel.Children.Count <= 0) return;

			for (int i = PathDockPanel.Children.Count - 1; i >= 0 ; i--)
			{
				((SkewedButton) PathDockPanel.Children[i]).InnerButton.Click -= InnerButton_Click;
			}

			PathDockPanel.Children.Clear();
		}

		private void AddSelectFolderButton()
		{
			SkewedButton sk = new SkewedButton();
			sk.Text = "Select Folder";
			sk.Index = -1;
			sk.InnerButton.Click += InnerButton_SelectFolder;

			PathDockPanel.Children.Add(sk);
		}

		private void InnerButton_SelectFolder(object sender, RoutedEventArgs e)
		{
			RaiseSelectFolderEvent();
		}

		private void InnerButton_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			SkewedButton skb = b.Tag as SkewedButton;

			SelectedIndex = (int) skb.Tag;
			SelectedFolder = skb.InnerSp.Tag as string;
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
		public delegate void PathChangedEventHandler(object sender, PathChangeArgs e);

		public event PathChangedEventHandler PathChange;

		protected virtual void RaisePathChangeEvent()
		{
			PathChange?.Invoke(this, new PathChangeArgs(SelectedIndex, SelectedFolder, SelectedPath));
		}

		// event 1, the select folder button was pressed
		public delegate void SelectFolderEventHandler(object sender);

		public event SelectFolderEventHandler SelectFolder;

		protected virtual void RaiseSelectFolderEvent()
		{
			SelectFolder?.Invoke(this);
		}

	#endregion
	}

	public class PathChangeArgs
	{
		public int Index;
		public string SelectedFolder;
		public Route SelectedPath;

		public PathChangeArgs(int index, string selectedFolder, string selectedPath)
		{
			Index = index;
			SelectedFolder = selectedFolder;
			SelectedPath = new Route(selectedPath);
		}
	}
}