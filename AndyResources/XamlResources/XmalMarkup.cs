#region + Using Directives

using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

#endregion

// user name: jeffs
// created:   5/9/2020 10:23:38 PM


namespace AndySharedResources.XamlResources
{

	[MarkupExtensionReturnType(typeof(System.Windows.Media.Color))]
	public class XmalColor : MarkupExtension
	{
		private System.Windows.Media.Color? c;
		private byte? a;
		private byte? r;
		private byte? g;
		private byte? b;

		public XmalColor() { }

		public System.Windows.Media.Color Color
		{
			get => c.Value;
			set
			{
				c = value;
			}
		}

		public byte R
		{
			get
			{
				return ((byte) (r.HasValue ? r.Value : c.HasValue ? c.Value.R : 255));
			}
			set
			{
				r = value;
			}
		}

		public byte G
		{
			get
			{
				return ((byte) (g.HasValue ? g.Value : c.HasValue ? c.Value.G : 255));
			}
			set
			{
				g = value;
			}
		}
		
		public byte B
		{
			get
			{
				return ((byte) (b.HasValue ? b.Value : c.HasValue ? c.Value.B : 255));
			}
			set
			{
				b = value;
			}
		}
		
		public byte A
		{
			get
			{
				return ((byte) (a.HasValue ? a.Value : c.HasValue ? c.Value.A : 255));
			}
			set
			{
				a = value;
			}
		}

		public System.Windows.Media.Color ToColor()
		{
			// return new SolidColorBrush(Color.FromArgb(
			// 	(byte) (A.HasValue ? A.Value : 255), R, G, B));

			return System.Windows.Media.Color.FromArgb(A, R, G, B);


		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return (System.Windows.Media.Color) ToColor();
		}
	}


	// <Setter Property="Background" Value="{custom:ScBrush color={StaticResource Gray.150}, A=#2f}"></Setter>

	[MarkupExtensionReturnType(typeof(Brush))]
	public class ScBrush : MarkupExtension
	{
		private Color c;

		public ScBrush() { }

		public Color color
		{
			get => c;
			set
			{
				c = value;
				R = c.R;
				G = c.G;
				B = c.B;
			}
		}

		public byte R { get; set; }

		public byte G { get; set; }

		public byte B { get; set; }

		public byte? A { get; set; }

		public System.Windows.Media.Brush ToBrush()
		{
			return new SolidColorBrush(Color.FromArgb(
				(byte) (A.HasValue ? A.Value : 255), R, G, B));
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return ToBrush();
		}
	}
}