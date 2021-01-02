#region + Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AODeliverable.Settings;
using iText.Kernel.Pdf;
using UtilityLibrary;
using AndyShared;
using static UtilityLibrary.MessageUtilities;

#endregion


// projname: AODeliverable.Support
// itemname: Utilities
// username: jeffs
// created:  11/3/2019 8:33:55 AM


namespace AODeliverable.Support
{
	public static class Utilities
	{
		internal static bool VerifyOutputFile(string outputPathFile)
		{
			string[] message;

			if (outputPathFile == null ||
				outputPathFile.Length < 3) return false;

			if (File.Exists(outputPathFile))
			{
				if (UserSettings.Data.AllowOverwriteOutputFile)
				{
					try
					{
						var f = File.OpenWrite(outputPathFile);
						f.Close();
						File.Delete(outputPathFile);

						Program.win.AppendStatusMessage(
							new [] {logMsgDbS("output file", "exists: overwrite allowed")});

						return true;
					}
					catch (Exception ex)
					{
						Program.win.AppendStatusMessage(
							new [] {"result", "fail - file is not accessible or is open elsewhere"});

						return false;
					}
				}
				else
				{
					Program.win.AppendStatusMessage(
						new [] {logMsgDbS("output file", "exists: overwrite disallowed")});
					return false;
				}
			}

			Program.win.AppendStatusMessage(
				new [] {logMsgDbS("output file", "does not exist")});

			return true;
		}


		// provide the name of the first directory in the path
		// cases:
		// c:/filename.ext => return ""
		// c:/directory/filename.ext => return directory
		// /filename.ext => return ""
		// /directory/filename.ext => return directory
		public static string GetFirstDirectoryName(this string path)
		{
			string name = Path.GetFileName(path);

			if (name != String.Empty)
			{
				path = path.Substring(0, path.Length - name.Length);
			}

			path = path.TrimEnd('\\');

			int pos = path.IndexOf(':');

			if (pos > -1)
			{
				path = path.Substring(pos + 1);
			}

			if (path.Length == 0) return path;

			// ignore the first character which is a slash
			pos = path.IndexOf('\\', 1);

			if (pos < 0) return path; // no sub-folders - path is first directory

			return path.Substring(1, pos);
		}

		// problem - will provide the filename
		// includes the slash prefix
		public static string GetSubDirectoryPath(this string path, int requestedDepth)
		{
			requestedDepth++;
			if (requestedDepth == 0) { return "\\"; }

			path = path.TrimEnd('\\');

			int depth = path.CountSubstring("\\");

			if (requestedDepth > depth) { requestedDepth = depth; }

			int pos = path.IndexOfToOccurance("\\", requestedDepth);

			if (pos < 0) { return ""; }

			pos = path.IndexOfToOccurance("\\", requestedDepth + 1);

			if (pos < 0) { pos = path.Length; }

			return path.Substring(0, pos);
		}
	}

		public static class OutlineExtension
		{
			// extension method to the outlines 
			// in the itext package
			public static int GetPageNumber(this PdfOutline outline, PdfDocument doc)
			{
				try
				{
					PdfDictionary dict = outline.GetContent().GetAsDictionary(PdfName.A);

					if (dict == null)
					{
						PdfArray array = outline.GetContent().GetAsArray(PdfName.Dest);

						if (array != null)
						{
							PdfObject obj = array.SubList(0, 1)[0];

							if (obj is PdfNumber)
							{
								return ((PdfNumber) obj).IntValue() + 1;
							}
							else if (obj is PdfDictionary)
							{
								return doc.GetPageNumber((PdfDictionary) obj);
							}
							else
							{
								return -3;
							}
						}
						else
						{
							return -1;
						}
					}

					dict = dict.GetAsArray(PdfName.D).GetAsDictionary(0);

					return doc.GetPageNumber(dict);

				}
				catch (Exception e)
				{
					return -2;
				}
			}
		}

}
