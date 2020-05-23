using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ClassifierEditor.DataRepo;
using Point = System.Windows.Point;
using Color = System.Windows.Media.Color;

namespace ClassifierEditor.Windows.TreeViewSelector
{
	/// <summary>
	/// Interaction logic for TreeViewSelector.xaml
	/// </summary>
	public partial class TreeViewSelector : Window, INotifyPropertyChanged
	{
	#region + Preface

		public enum WindowPosition
		{
			BOTTOM = -3,
//			BOTTOM_RIGHT = -2,
//			BOTTOM_LEFT = -1,
//			TOP_LEFT = 1,
//			TOP_RIGHT = 2,
			TOP = 3

		}

		private Window             win;
		private FrameworkElement   fe;
		private ResourceDictionary r;

		private const int MAX_TIME = 10000;

		private WindowPosition _orientation;

		private double _screenXScaleFactor = 1.0;
		private double _screenYScaleFactor = 1.0;

		private Vector _userAdjustment = new Vector();

		private int _fadeInDuration  = 3000;
		private int _fadeOutDuration = 200;
#pragma warning disable CS0414 // The field 'TreeViewSelector._pauseTime' is assigned but its value is never used
		private int _pauseTime       = 1000;
#pragma warning restore CS0414 // The field 'TreeViewSelector._pauseTime' is assigned but its value is never used

		// keep at least 5 pixles between the screen edge and the balloon
		private const double LEFT_OR_RIGHT_MARGIN = 5.0;
		private Vector _internalAdjustment;

		private DataManagerCategories categories;

	#endregion

	#region + Constructor

		public TreeViewSelector(Window win, FrameworkElement fe)
		{
			InitializeComponent();

			this.win = win;
			this.fe  = fe;
			this.Owner = win;

			r = this.Resources;

			DefinedHeight = Height;

			// this is the default orientation
			Orientation = WindowPosition.BOTTOM;

			GetScreenScaleFactors();
		}

	#endregion

	#region + Properties

		public DataManagerCategories Categories
		{
			get => categories;
			set
			{ 
				categories = value;
				OnPropertyChange();
			}
		}

		public WindowPosition Orientation
		{
			get => _orientation;

			set
			{
				if (_orientation != value)
				{
					_orientation = value;
//					AdjustPointerForOrientation();
				}
			}
		}

		public double DefinedHeight { get; set; }


		public int FadeInDuration
		{
			get => _fadeInDuration;
			set
			{
				if (value > 0 && value < MAX_TIME)
				{
					FiDuration = new Duration(GetTimeSpan(value));
				}
			}
		}

		public Duration FiDuration
		{
			get => new Duration(GetTimeSpan(FadeInDuration));
			set
			{
				int millisecs = GetMilliSecs(value.TimeSpan);

				if (millisecs > 0 && millisecs < MAX_TIME)
				{
					_fadeInDuration = millisecs;
					OnPropertyChange();
				}
			}
		}

		public int FadeOutDuration
		{
			get => _fadeOutDuration;
			set
			{
				if (value > 0 && value < MAX_TIME)
				{
					FoDuration = new Duration(GetTimeSpan(value));
				}
			}
		}

		public Duration FoDuration
		{
			get => new Duration(GetTimeSpan(FadeOutDuration));
			set
			{
				int millisecs = GetMilliSecs(value.TimeSpan);

				if (millisecs > 0 && millisecs < MAX_TIME)
				{
					_fadeOutDuration = millisecs;
					OnPropertyChange();
				}
			}
		}

	#region + Private

	#endregion

	#endregion

	#region + Methods

	#region + Private

		// calculate the number of milli seconds
		// using only the seconds and milliseconds
		// values from the timespan
		private int GetMilliSecs(TimeSpan timeSpan)
		{
			return timeSpan.Seconds * 1000 + timeSpan.Milliseconds;
		}

		private TimeSpan GetTimeSpan(int millisecs)
		{
			return new TimeSpan(0, 0, 0, 0, millisecs);
		}

		private Rectangle GetScaledScreenSize()
		{
			Rectangle result = new Rectangle();

			Point p1 = fe.TranslatePoint(new Point(0, 0), win);

			Point p2 =  win.PointToScreen(new Point(0, 0));

			Screen screen = Screen.FromPoint(new System.Drawing.Point(
				(int) (p1.X + p2.X + fe.ActualWidth / 2),
				(int) (p1.Y + p2.Y + fe.ActualHeight / 2)));

			result = screen.Bounds.Scale(_screenXScaleFactor, _screenYScaleFactor);

			return result;
		}

		private Point CalcWinPosition()
		{
			// get infor for the screen that the main window
			// is currently positioned
			Rectangle screen = GetScaledScreenSize();

			Point winCorner = CalcControlTlCorner();

			Point winPosition = Point.Add(winCorner, CalcOrientationAdjust());

			winPosition = Point.Add(winPosition, _userAdjustment);

			// determine if the balloon is off the screen.
			// adjust and re-calculate
			if (IsWinOffScreen(winPosition, screen))
			{
				winPosition = Point.Add(winCorner, CalcOrientationAdjust());

				winPosition = Point.Add(winPosition, _userAdjustment);

				winPosition = Point.Add(winPosition, _internalAdjustment);
			}

			return winPosition;
		}

		private void GetScreenScaleFactors()
		{
			// get the screen scaling factors
			PresentationSource src = PresentationSource.FromVisual(fe);
			Matrix mx  = src.CompositionTarget.TransformFromDevice;

			_screenXScaleFactor = mx.M11;
			_screenYScaleFactor = mx.M22;
		}

		private Point CalcControlTlCorner()
		{
			// p is the control's TL corner relative to the 
			// window's client area
			Point p = fe.TranslatePoint(new Point(0, 0), win);

			// p2 is the TL parent window corner in screen coordinates (un-scaled)
			Point p2 = win.PointToScreen(new Point(0, 0));

			// calculate the scaled screen coordinate for the parent's
			// TL corner
			double winX = p2.X * _screenXScaleFactor;
			double WinY = p2.Y * _screenYScaleFactor;

			return new Point(winX + p.X, WinY + p.Y);
		}

		private Vector CalcOrientationAdjust()
		{
			Vector vector = new Vector(0.0, 0.0);
			// this method only works after the loaded event is called
			// as the actual height width are not valid otherwise

			// in all cases, there is no x adjustment

			if (_orientation == WindowPosition.BOTTOM)
			{
				vector.Y = fe.ActualHeight;
			}
			else
			{
				vector.Y = -1.0 * this.ActualHeight;
			}

			return vector;
		}

		private bool IsWinOffScreen(Point winPosition, Rectangle screen)
		{
			bool result = false;

			_internalAdjustment = new Vector(0.0, 0.0);

			// negative numbers mean it is past the screen edge;
			double top = winPosition.Y - screen.Top;
			double left = winPosition.X - screen.Left;
			double right = screen.Right - (winPosition.X + ActualWidth);
			double bottom = screen.Bottom - (winPosition.Y + ActualWidth);


			// the below assumes that the window cannot be 
			// above and below the screen edges and cannot be
			// past the left and right screen edges

			// is the window past the screens left edge
			if (top < 0 || bottom < 0)
			{
				// window is above screen's top edge
				Orientation = FlipOrientation(_orientation);
				this.UpdateLayout();
				result = true;
			}

			if (left < 0)
			{
				// window is past screen's left edge
				// move the window to the right (make a positive amount)
				_internalAdjustment.X = -1 * left + LEFT_OR_RIGHT_MARGIN;
				result = true;
			}
			else if (right < 0)
			{
				// window is past screen's right edge
				// move the window to the left (use a negative amount)
				_internalAdjustment.X = right - LEFT_OR_RIGHT_MARGIN;
				result = true;
			}

			return result;
		}

		private WindowPosition FlipOrientation(WindowPosition orientation)
		{
			return (WindowPosition) (-1 * (int) orientation);
		}


		private void closeWin(object o, EventArgs e)
		{
			fadeCompleted = true;
			Close();
		}

	#endregion

	#endregion

	#region + Events

		private void BtnTitle_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			Close();
		}

		private void Tvx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			
			Close();
		}

		private bool fadeCompleted = false;

		private void FloatingWin_Closing(object sender, CancelEventArgs e)
		{
			if (!fadeCompleted)
			{
				DoubleAnimation a = new DoubleAnimation
				{
					From = 1.0, To = 0.0,
					FillBehavior = FillBehavior.Stop,
					BeginTime = TimeSpan.Zero,
					Duration = new Duration(TimeSpan.FromMilliseconds(_fadeOutDuration))
					
				};

				Storyboard storyboard = new Storyboard();

				storyboard.Children.Add(a);
				storyboard.Completed += closeWin;
				Storyboard.SetTarget(a, this);
				Storyboard.SetTargetProperty(a, new PropertyPath(OpacityProperty));
				storyboard.Begin();

				e.Cancel = true;
			}
		}

		private void TreeViewSelector_OnDeactivated(object sender, EventArgs e)
		{
			Close();
		}


		private void FloatingWin_Loaded(object sender, RoutedEventArgs e)
		{
			Point TL = CalcWinPosition();

			Left = TL.X;
			Top = TL.Y;

		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public void CloseWindow(object sender, EventArgs args)
		{
			Close();
		}



		#endregion


			
	}

	static class EBalloonxtensions
	{
		public static Rectangle Scale(this Rectangle rc, double scaleFactorX, double scaleFactorY)
		{
			return new Rectangle(
				(int) (rc.Left * scaleFactorX),
				(int) (rc.Top * scaleFactorY),
				(int) (rc.Width * scaleFactorX),
				(int) (rc.Height * scaleFactorY));
		}
	}
}

namespace DesignTimeProperties
{
	public static class d
	{
		static bool? inDesignMode;

		/// <summary>
		/// Indicates whether or not the framework is in design-time mode. (Caliburn.Micro implementation)
		/// </summary>
		private static bool InDesignMode
		{
			get
			{
				if (inDesignMode == null)
				{
					var prop = DesignerProperties.IsInDesignModeProperty;
					inDesignMode = (bool) DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement))
					.Metadata.DefaultValue;

					if (!inDesignMode.GetValueOrDefault(false) && System.Diagnostics.Process.GetCurrentProcess()
					.ProcessName.StartsWith("devenv", System.StringComparison.Ordinal))
						inDesignMode = true;
				}

				return inDesignMode.GetValueOrDefault(false);
			}
		}

	}
}