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

namespace Sylvester
{
	/// <summary>
	/// Interaction logic for FolderPath.xaml
	/// </summary>
	public partial class FolderPath : UserControl
	{
		private string[] names = new [] {"p:", "first name", "second name", "final name"};
		private int nextIndex;

		public FolderPath()
		{
			InitializeComponent();

//			((SkewedButton) PathDockPanel.Children[0]).InnerButton.Click += InnerButton_Click;

//			AddPath(names);

		}

		public void AddPath(string[] path)
		{
			PathDockPanel.Children.Clear();
			nextIndex = 0;

			foreach (string s in path)
			{
				Add(s, nextIndex++);
			}
		}



		private void InnerButton_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			SkewedButton sb = b.Tag as SkewedButton;

			int i = (int) sb.Tag;
			string s = sb.InnerSp.Tag as string;

//			Debug.WriteLine("button pressed| " + s + " index| " + i);
		}

		private void Add(string text, int index)
		{
			SkewedButton sk = new SkewedButton();
			sk.Text = text;
			sk.Index = index;
			sk.InnerButton.Click += InnerButton_Click;

			PathDockPanel.Children.Add(sk);
		}
	}
}
