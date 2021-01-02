#region + Using Directives

using System.Windows;
using SettingsManager;
// using Sylvester.Settings;

using static UtilityLibrary.ScreenParameters;

#endregion

// projname: Sylvester.Windows
// itemname: WindowManager
// username: jeffs
// created:  2/22/2020 11:04:44 AM

namespace Sylvester.Windows
{
	public enum WindowId
	{
		WINDOW_MAIN = 0,
		DIALOG_SAVED_FOLDERS = 1,
		COUNT = 2
	}

	public class WindowManager
	{
		public WindowLayout SaveWinLayout(Window win, WindowLayout layout)
		{
			layout.Width = (int) win.Width;
			layout.Height = (int) win.Height;
			layout.LeftEdge = (int) win.Left;
			layout.TopEdge = (int) win.Top;

			return layout;
		}

		public void RestoreWinPosition(Window win, WindowLayout layout)
		{
			double meleft = layout.LeftEdge;
			double meTop = layout.TopEdge;

			if (meTop <= 0 || meleft <= 0)
			{
				CenterToParent(win);
			}
			else
			{
				win.Top = meTop;
				win.Left = meleft;
			}
		}

		public void RestoreWinLayout(Window win, WindowLayout layout)
		{
			double meleft = layout.LeftEdge;
			double meTop = layout.TopEdge;

			double meWidth = layout.Width;
			double meHeight = layout.Height;

			if (meTop <= 0 || meleft <= 0)
			{
				CenterToParent(win);
			}
			else
			{
				win.Top = meTop;
				win.Left = meleft;
			}

			if (meWidth < win.MinWidth || meHeight < win.MinHeight)
			{
				win.Width = win.MinWidth;
				win.Height = win.MinHeight;
			}
			else
			{
				win.Width = meWidth;
				win.Height = meHeight;
			}
		}

		public void CenterToParent(Window win)
		{
			if (win.Owner == null)
			{
				CenterToScreen(win);
				return;
			}

			double parentLeft = win.Owner.Left;
			double parentTop = win.Owner.Top;
			double parentWidth = win.Owner.Width;
			double parentHeight = win.Owner.Height;

			double left = parentLeft;
			double top = parentTop;

			if (win.ActualWidth < parentWidth)
			{
				left = parentLeft + (parentWidth - win.Width) / 2;
			}

			if (win.ActualHeight < parentHeight)
			{
				top = parentTop + (parentHeight = win.Height) / 2;
			}

			win.Top = top;
			win.Left = left;

		}

		public void CenterToScreen(Window win)
		{
			System.Drawing.Rectangle r = GetScaledScreenSize(win);

			win.Left = (r.Width - win.Width) / 2;
			win.Top = (r.Height - win.Height) / 2;

		}


	}
}
