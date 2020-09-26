#region + Using Directives

using System;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

#endregion

// user name: jeffs
// created:   8/25/2020 6:06:54 AM

namespace AndyShared.Support
{
	public class Common
	{
		public const bool SHOW_DEBUG_MESSAGE1				= false;

		public static Window GetCurrentWindow()
		{
			WindowCollection wc = Application.Current.Windows;

			return wc[wc.Count - 1];
		}


		public static void TaskDialog_Opened(object sender, EventArgs e)
		{
			TaskDialog td = sender as TaskDialog;
			td.Icon = td.Icon = td.Icon;
			if (td.FooterIcon != TaskDialogStandardIcon.None)
			{
				td.FooterIcon = td.FooterIcon;
			}

			td.InstructionText = td.InstructionText;
		}
	}


	// public static class Meditator
	// {
	// 	public delegate void OutIsModifiedEventHandler();
	//
	// 	public static event Meditator.OutIsModifiedEventHandler OutIsModified;
	//
	// 	public static void RaiseOutIsModifiedEvent()
	// 	{
	// 		OutIsModified?.Invoke();
	// 	}
	//
	// 	public delegate void InIsModifiedEventHandler();
	//
	// 	public static event Meditator.InIsModifiedEventHandler InIsModified;
	//
	// 	public static void RaiseInIsModifiedEvent()
	// 	{
	// 		InIsModified?.Invoke();
	// 	}
	// }
}