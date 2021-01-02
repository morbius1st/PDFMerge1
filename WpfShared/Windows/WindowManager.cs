#region using

using System.Windows;
using static UtilityLibrary.ScreenParameters;
using SettingsManager;

#endregion

// username: jeffs
// created:  7/12/2020 5:07:52 PM

namespace WpfShared.Windows
{
	public enum WindowId
	{
		WINDOW_MAIN = 0,
		DIALOG_SAVED_FOLDERS,
		DIALOG_SELECT_CLASSF_FILE,
		COUNT
	}

	public class WindowManager
	{
	#region private fields

		private App app = null;
		private MainWindow mainWin = null;
		private ClassificationFileSelector ClsFileMgr = null;
		private Window1 win1 = null;

	#endregion

	#region ctor

		public WindowManager()
		{
			Initialize();
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Show(WindowId which)
		{
			switch (which)
			{
			case WindowId.WINDOW_MAIN:
				{
					ShowMainWindow();
					break;
				}
			case WindowId.DIALOG_SELECT_CLASSF_FILE:
				{
					ShowClassificationFileManager();
					break;
				}
			case WindowId.DIALOG_SAVED_FOLDERS:
				{
					ShowWin1();
					break;
				}
			}
		}

		public WindowLayout GetWinLayout(Window win, WindowId id)
		{
			WindowLayout layout = new WindowLayout(id);

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



	#endregion

	#region private methods

		private void Initialize()
		{
			// app = new App();
			// app.InitializeComponent();

			// mainWin = new MainWindow();
			// ClsFileMgr = new ClassificationFileSelector();
			// win1 = new Window1();
		}


		private void ShowMainWindow()
		{
			// app.Run(mainWin);
			mainWin = new MainWindow();
			mainWin.ShowDialog();
		}

		private void ShowClassificationFileManager()
		{
			ClsFileMgr = new ClassificationFileSelector();
			ClsFileMgr.ShowDialog();
		}

		private void ShowWin1()
		{
			win1 = new Window1();
			win1.ShowDialog();
		}

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is WindowManager";
		}

	#endregion
	}
}