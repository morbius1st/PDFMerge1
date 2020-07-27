#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

// username: jeffs
// created:  7/12/2020 5:07:52 PM

namespace WpfShared.Windows
{
	public class WindowManager
	{
	#region private fields

		private App app = null;
		private MainWindow mainWin = null;
		private ClassificationFileSelector ClsFileMgr = null;

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

		public void Start()
		{
			int which = 1;

			switch (which)
			{
			case 0:
				{
					ShowMainWindow();
					break;
				}
			case 1:
				{
					ShowClassificationFileManager();
					break;
				}
			}
		}

	#endregion

	#region private methods

		private void Initialize()
		{
			app = new App();
			app.InitializeComponent();

			mainWin = new MainWindow();
			ClsFileMgr = new ClassificationFileSelector();
		}


		private void ShowMainWindow()
		{
			// app.Run(mainWin);

			mainWin.ShowDialog();
		}

		private void ShowClassificationFileManager()
		{
			ClsFileMgr.ShowDialog();
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