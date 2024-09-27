#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndyShared.Support;
using Microsoft.WindowsAPICodePack.Dialogs;

#endregion

// user name: jeffs
// created:   8/15/2024 9:31:21 PM

namespace DisciplineEditor.Support
{
	public class TaskDialogManager
	{
		// warnings

		public static bool VerifyOverwriteDisciplineDataFile(string filepath)
		{
			TaskDialogResult result =
				CommonTaskDialogs.CommonWarningDialog(
					"Overwrite Discipline Data File",
					"This will overwrite an existing Discipline Data File",
					"Discipline Data File selected:\n"
					+ $"{filepath}\n"
					+ "already exists.  This will delete the existing "
					+ "Discipline Data File\n"
					+ "Please confirm the deletion of this file");

			return result == TaskDialogResult.Yes;
		}

		public static bool VerifyDeleteDisciplineDataFile()
		{
			TaskDialogResult result =
				CommonTaskDialogs.CommonWarningDialog(
					"Delete Discipline Data File",
					"This will delete the Discipline Data File",
					"Deleting the Discipline Data File may result "
					+ "in the loss of your data.  Deleting this "
					+ "file cannot be undone.\n"
					+ "Please confirm the deletion of this file",
					TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.Cancel
					);

			return result == TaskDialogResult.Yes;
		}

		// errors

		// displinedatafile
		public static void DisciplineDataFileMissing()
		{
			CommonTaskDialogs.CommonErrorDialog(
				"Missing Discipline Data File",
				"The Discipline Data File is missing",
				"The Discipline Data File is required in order "
				+ "to proceed. Since this is missing, the "
				+ "program will exit.");
		}

		public static void DisciplineDataFileDeleteFailed()
		{
			CommonTaskDialogs.CommonErrorDialog(
				"Delete Discipline Data File",
				"Deleting the Discipline Data File failed",
				"For an unknown reason, the Discipline Data File "
				+ "could not be deleted.  Please verify that the "
				+ "file exists and permissions are correctly set");
		}

		public static void DisciplineDataFileSaveAsFailed()
		{
			CommonTaskDialogs.CommonErrorDialog(
				"SaveAs Discipline Data File",
				"SaveAs the Discipline Data File failed",
				"For an unknown reason, the copy of the Discipline Data File "
				+ "could not be saved.  Please verify that the "
				+ "permissions are correctly set");
		}


		public static void CannotCreateUserSettings(string filePath)
		{
			CommonTaskDialogs.CommonErrorDialog(
				"Cannot Make User Settings File",
				"The User Setting file could not be created",
				"The User Setting file is required in order to"
				+ "proceed.  This file is located here:\n"
				+ $"{filePath}\n"
				+ "please resolve make sure that this location is "
				+ "accessible / is not protected or locked");
		}

		public static TaskDialogResult RequestReplaceMissingDiscDataFile(string fileName, string folderPath)
		{
			TaskDialogResult result = CommonTaskDialogs.CommonErrorRequestDialog(
				"Discipline Data File Missing",
				"The Required Discipline Data File Missing",
				"The file's location was saved however, for an unknown reason, it is missing.\n"
				+ $"File Location\n"
				+ $"{folderPath}\n"
				+ $"File Name: {fileName}\n\n"
				+ "Select YES to provide an empty replacement file\n"
				+ "Select NO to do nothing and exit");

			return result;
		}

		public static void GeneralExceptionError(Exception e)
		{
			string msg = "This is an unaccounted for error and is described as:\n"
				+ $"prime error: {e.Message}";

			if (e.InnerException != null)
			{
				msg += $"\nInner error: {e.InnerException}";
			}

			CommonTaskDialogs.CommonErrorDialog(
				"The General Error",
				"The program has received a General Error",
				msg);
		}

	}
}