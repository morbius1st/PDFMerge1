#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndyShared.FileSupport;
using AndyShared.Support;
using Microsoft.WindowsAPICodePack.Dialogs;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   9/27/2020 6:28:45 PM

namespace AndyShared.Support
{
	public class CommonTaskDialogs
	{

		public static void CommonErrorDialog(
			string title, string main, string message)
		{
			TaskDialog td = new TaskDialog();
			td.Caption = title;
			td.InstructionText = main;
			td.Text = message;
			td.Icon = TaskDialogStandardIcon.Error;
			td.Cancelable = false;
			td.OwnerWindowHandle = ScreenParameters.GetWindowHandle(Common.GetCurrentWindow());
			td.StartupLocation = TaskDialogStartupLocation.CenterOwner;
			td.Opened += Common.TaskDialog_Opened;
			td.Show();
		}


		public static TaskDialogResult CommonWarningDialog(string title, 
			string main, string message, TaskDialogStandardButtons buttons =
				TaskDialogStandardButtons.Ok | TaskDialogStandardButtons.Cancel)
		{
			TaskDialog td = new TaskDialog();
			td.Caption = title;
			td.InstructionText = main;
			td.Text = message;
			td.Icon = TaskDialogStandardIcon.Warning;
			td.Cancelable = false;
			td.StandardButtons = buttons;
			td.OwnerWindowHandle = ScreenParameters.GetWindowHandle(Common.GetCurrentWindow());
			td.StartupLocation = TaskDialogStartupLocation.CenterOwner;
			td.Opened += Common.TaskDialog_Opened;

			return td.Show();
		}


	}
}
