#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AndyShared.FileSupport;
using AndyShared.Support;

using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
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

		/// <summary>
		/// common dialog that provides a warning notification and has
		/// two button options (default) - OK or Cancel
		/// </summary>
		public static TaskDialogResult CommonWarningDialog(string title, 
			string main, string message, 
			TaskDialogStandardButtons buttons =
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

		/// <summary>
		/// common dialog that provides a warning notification and has
		/// one button option (default) - OK
		/// </summary>
		public static TaskDialogResult CommonNotificationDialog(string title, 
			string main, string message, 
			TaskDialogStandardButtons buttons = TaskDialogStandardButtons.Ok)
		{
			return CommonWarningDialog(title, main, message, buttons);
		}

		/// <summary>
		/// common dialog that provides a error notification and allows
		/// two button choices (by default) - Yes or No
		/// </summary>
		public static TaskDialogResult CommonErrorRequestDialog(string title, 
			string main, string message, string footer = "", 
			TaskDialogStandardButtons buttons =
				TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No)
		{
			TaskDialog td = new TaskDialog();

			td.Caption = title;
			td.InstructionText = main;
			td.Text = message;
			td.Icon = TaskDialogStandardIcon.Error;
			td.Cancelable = false;
			td.StandardButtons = buttons;
			td.OwnerWindowHandle = ScreenParameters.GetWindowHandle(Common.GetCurrentWindow());
			td.StartupLocation = TaskDialogStartupLocation.CenterOwner;

			if (!footer.IsVoid())
			{
				// td.FooterIcon = TaskDialogStandardIcon.Information;
				td.FooterText = footer;
			}

			
			return td.Show();
		}

		/// <summary>
		/// common dialog that provides a error notification and allows
		/// two button choices (by default) - Yes or No
		/// </summary>
		public static TaskDialogResult CommonTestDialog(string title, 
			string main, string message, 
			TaskDialogStandardButtons buttons =
				TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No)
		{
			TaskDialog td = new TaskDialog();

			td.Caption = title;
			td.InstructionText = main;
			td.Text = message;
			td.Icon = TaskDialogStandardIcon.Error;
			td.Cancelable = false;
			td.StandardButtons = buttons;
			td.OwnerWindowHandle = ScreenParameters.GetWindowHandle(Common.GetCurrentWindow());
			td.StartupLocation = TaskDialogStartupLocation.CenterOwner;
			// td.Opened += Common.TaskDialog_Opened;

			td.FooterIcon = TaskDialogStandardIcon.Shield;
			td.FooterCheckBoxChecked = false;
			td.FooterCheckBoxText = "check box text";
			td.FooterText = "this is footer text";

			td.DefaultButton = TaskDialogDefaultButton.Yes;

			td.DetailsCollapsedLabel = "collapsed label";
			td.DetailsExpanded = true;
			td.DetailsExpandedLabel = "expanded label";
			td.DetailsExpandedText = "this is expanded text\nthis text is on two lines";

			td.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter;

			return td.Show();
		}
	}
}
