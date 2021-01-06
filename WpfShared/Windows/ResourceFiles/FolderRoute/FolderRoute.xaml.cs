using System;
using System.Collections.Generic;
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

namespace WpfShared.Windows.ResourceFiles.FolderRoute
{
	/// <summary>
	/// Interaction logic for FolderRoute.xaml
	/// </summary>
	public partial class FolderRoute : UserControl
	{
		public FolderRoute()
		{
			InitializeComponent();
		}


	#region SkewAngle

		public static readonly DependencyProperty SkewAngleProperty = DependencyProperty.Register(
			"SkewAngle", typeof(double), typeof(FolderRoute), new PropertyMetadata(20.0));

		public double SkewAngle
		{
			get => (double) GetValue(SkewAngleProperty);
			set => SetValue(SkewAngleProperty, value);
		}

	#endregion



	}
}
