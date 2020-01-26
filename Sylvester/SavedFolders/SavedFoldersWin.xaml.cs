using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Sylvester.FileSupport;
using Sylvester.Settings;
using Sylvester.Support;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

using static Sylvester.SavedFolders.SavedFolderType;

namespace Sylvester.SavedFolders
{
	/// <summary>
	/// Interaction logic for SavedFoldersWin.xaml
	/// </summary>
	public partial class SavedFoldersWin : Window
	{
		// functions needed
		// add project
		// add revision pair
		// remove revision pair
		// remove project
		// show / edit / select current folder
		// show / edit / select revision folder

		private SavedFoldersDebugSupport sfds = SavedFoldersDebugSupport.Instance;

		public SavedFoldersWin()
		{
			InitializeComponent();
		}

		public List<Dictionary<string, SavedFolder>> SavedFolders => SetgMgr.Instance.SavedFolders;


		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@savedfolderWin| debug");
		}

		private void BtnTest_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@savedfolderWin| test");

			tbxMain.Clear();

			sfds.Test_02(CURRENT);

		}

		public void Append(string msg)
		{
			tbxMain.AppendText(msg);
		}

		public void AppendLine(string msg)
		{
			Append(msg + nl);
		}

		public void AppendLineFmt(string msg1, string msg2 = "")
		{
			AppendLine(logMsgDbS(msg1, msg2));
		}
	}
}
