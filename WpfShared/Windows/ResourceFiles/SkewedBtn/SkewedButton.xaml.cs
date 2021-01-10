using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace WpfShared.Windows.ResourceFiles.SkewedBtn
{
	public partial class SkewedButton : Button, INotifyPropertyChanged
	{
		public SkewedButton()
		{
			InitializeComponent();

			this.Click += SkewedButton_Click;
		}

		private void SkewedButton_Click(object sender, RoutedEventArgs e)
		{
			IsChecked = !IsChecked;
		}


		/*
			properties
			index		(int)			arbitrary number assigned to the control
			textblk		(textblock)		a textblock to be displayed on the right side of the button
			Icon		(path)	        path to display as the icon

				┏━━━━━━━━━━━━━━━━━━━┓                                       
						  █            <<-- icon / display when not null ┓ <<- both can be displayed a the same time
					t   e   x   t      <<-- text displayed when not null ┛
				┗━━━━━━━━━━━━━━━━━━━┛										

			rules: 
				if icon is not unll, show the icon 
				if text is not null, show the text (above icon)

		 */

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


	#region ButtonSkewAngle

		public static readonly DependencyProperty ButtonSkewAngleProperty = DependencyProperty.Register(
			"ButtonSkewAngle", typeof(double), typeof(SkewedButton), new PropertyMetadata(20.0));

		public double ButtonSkewAngle
		{
			get => (double) GetValue(ButtonSkewAngleProperty);
			set => SetValue(ButtonSkewAngleProperty, value);
		}

	#endregion

	#region IsChecked

		public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
			"IsChecked", typeof(bool), typeof(SkewedButton), new PropertyMetadata(default(bool)));

		public bool IsChecked
		{
			get => (bool) GetValue(IsCheckedProperty);
			set => SetValue(IsCheckedProperty, value);
		}

	#endregion


	#region index

		public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(
			"Index", typeof(int), typeof(SkewedButton), new PropertyMetadata(-1));

		public int Index
		{
			get => (int) GetValue(IndexProperty);
			set => SetValue(IndexProperty, value);
		}

	#endregion

	#region SkewAngle

		public static readonly DependencyProperty SkewAngleProperty = DependencyProperty.Register(
			"SkewAngle", typeof(double), typeof(SkewedButton), new PropertyMetadata(20.0));

		public double SkewAngle
		{
			get => (double) GetValue(SkewAngleProperty);
			set => SetValue(SkewAngleProperty, value);
		}

	#endregion


	#region TextBlk

		private static TextBlock t = new TextBlock(new Run("Empty"));

		public static readonly DependencyProperty TextBlkProperty = DependencyProperty.Register(
			"TextBlk", typeof(TextBlock), typeof(SkewedButton), new PropertyMetadata(null));

		public TextBlock TextBlk
		{
			get => (TextBlock) GetValue(TextBlkProperty);
			set => SetValue(TextBlkProperty, value);
		}

	#endregion


	#region Icon

		public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
			"Icon", typeof(Path), typeof(SkewedButton), new PropertyMetadata(null));

		public Path Icon
		{
			get => (Path) GetValue(IconProperty);
			set => SetValue(IconProperty, value);
		}

	#endregion

	}

}