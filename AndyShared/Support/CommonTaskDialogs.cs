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

using UtilityLibrary;

#endregion

// user name: jeffs
// created:   9/27/2020 6:28:45 PM

namespace AndyShared.Support
{
	public class CommonTaskDialogs
	{

		public static void CommonErrorDialog(
			string title, string main, string message, IntPtr handle =  default)
		{
			TaskDialog td = new TaskDialog();
			td.Caption = title;
			td.InstructionText = main;
			td.Text = message;
			td.Icon = TaskDialogStandardIcon.Error;
			td.Cancelable = false;

			if (handle != IntPtr.Zero)
			{
				// add WinHandle to a static IntPtr to the main window and update usage
				// see MainWindowClassifierEditor for example
				td.OwnerWindowHandle = handle;
				td.StartupLocation = TaskDialogStartupLocation.CenterOwner;
			}

			td.Opened += Common.TaskDialog_Opened;
			td.Show();
		}

		/// <summary>
		/// common dialog that provides a warning notification and has
		/// two button options (default) - OK or Cancel
		/// </summary>
		public static TaskDialogResult CommonWarningDialog(string title, 
			string main, string message, IntPtr handle =  default, 
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

			if (handle != IntPtr.Zero)
			{
				// add WinHandle to a static IntPtr to the main window and update usage
				// see MainWindowClassifierEditor for example
				td.OwnerWindowHandle = handle;
				td.StartupLocation = TaskDialogStartupLocation.CenterOwner;
			}

			td.Opened += Common.TaskDialog_Opened;

			return td.Show();
		}

		/// <summary>
		/// common dialog that provides a warning notification and has
		/// one button option (default) - OK
		/// </summary>
		public static TaskDialogResult CommonNotificationDialog(string title, 
			string main, string message, IntPtr handle =  default,
			TaskDialogStandardButtons buttons = TaskDialogStandardButtons.Ok)
		{
			return CommonWarningDialog(title, main, message, handle, buttons);
		}

		/// <summary>
		/// common dialog that provides a error notification and allows
		/// two button choices (by default) - Yes or No
		/// </summary>
		public static TaskDialogResult CommonErrorRequestDialog(string title, 
			string main, string message, string footer = "", IntPtr handle =  default, 
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

			if (handle != IntPtr.Zero)
			{
				// add WinHandle to a static IntPtr to the main window and update usage
				// see MainWindowClassifierEditor for example
				td.OwnerWindowHandle = handle;
				td.StartupLocation = TaskDialogStartupLocation.CenterOwner;
			}

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
			string main, string message, IntPtr handle =  default, 
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

			if (handle != IntPtr.Zero)
			{
				// add WinHandle to a static IntPtr to the main window and update usage
				// see MainWindowClassifierEditor for example
				td.OwnerWindowHandle = handle;
				td.StartupLocation = TaskDialogStartupLocation.CenterOwner;
			}

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
