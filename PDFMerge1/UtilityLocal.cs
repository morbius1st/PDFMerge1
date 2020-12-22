using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using iText.Kernel.Pdf;
using System.IO;


namespace PDFMerge1
{
	public class UtilityLocal
	{
		static public int defColumn = 35;

		static public string nl = Environment.NewLine;

		static public RichTextBox txInfo { private get; set; }

		static public int output { get; set; } = 0;

		static public string[] splitPath(string fileAndPath)
		{
			return fileAndPath.Split('\\');
		}
//
//		private static string fmtInt(int int1)
//		{
//			return $"{int1,-4}";
//		}

		static public string fmt<T>(T var)
		{
			if (var is int)
			{
				return $"{var,-4}";
			}

			return var.ToString();
		}

		static public void logMsgFmtln<T>(string msg1, T var1, int shift = 0, int column = -1)
		{
			logMsgFmt(msg1, fmt(var1), shift: shift, column: column);
			logMsg(nl);
		}

		static public void logMsgFmt<T>(string msg1, T var1, int shift = 0, int column = -1)
		{
			logMsg(fmtMsg(msg1, fmt(var1), shift: shift, column: column));
		}

		static public void logMsgFmt<T>(string msg1, T var1, Color color, Font font, int shift = 0, int column = -1)
		{
			logMsg(fmtMsg(msg1, fmt(var1), shift: shift, column: column), color, font);
		}

		static public string fmtMsg<T>(string msg1, T var1, int shift = 0, int column = -1)
		{
			if (column < 0) { column = defColumn; }

			return string.Format(" ".Repeat(shift) + "{0," + column + "}{1}", msg1, fmt(var1));
		}

		static public void logMsgln<T1, T2>(T1 var1, T2 var2)
		{
			logMsg(fmt(var1));
			logMsgln(fmt(var2));
		}

		static public void logMsgln<T>(T var)
		{
			sendMsg(fmt(var));
			sendMsg(nl);
		}

		static public void logMsg<T1, T2>(T1 var1, T2 var2)
		{
			logMsg(fmt(var1));
			logMsg(fmt(var2));
		}

		static public void logMsg<T>(T var)
		{
			sendMsg(fmt(var));
		}

		static public void logMsg<T>(T var, Color color, Font font)
		{
			sendMsg(fmt(var), color, font);
		}

		static public void sendMsg(string msg, Color color, Font font)
		{
			if (output == 0)
			{
				txInfo.AppendText(msg, color, font);
			}
			else
			{
				Console.Write(msg);
			}
		}

		static private void sendMsg(string msg)
		{
			if (output == 0)
			{
				txInfo.AppendText(msg);
			}
			else
			{
				Console.Write(msg);
			}
		}

		static public void clearConsole()
		{
			DTE2 ide = (DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE");

			OutputWindow w = ide.ToolWindows.OutputWindow;

			w.ActivePane.Activate();
			w.ActivePane.Clear();

		}
	}

	public static class StringExtensions
	{
		public static string Repeat(this string s, int quantity)
		{
			if (quantity <= 0) return "";

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < quantity; i++)
			{
				sb.Append(s);
			}

			return sb.ToString();
		}

		public static int CountSubstring(this string s, string substring)
		{
			int count = 0;
			int i = 0;
			while ((i = s.IndexOf(substring, i)) != -1)
			{
				i += substring.Length;
				count++;
			}

			return count;
		}

		public static int IndexOfToOccurance(this string s, string substring,
			int occurance, int start = 0)
		{
			if (s.Trim().Length == 0) { return -1; }
			if (occurance < 0) { return -1; }

			int pos = start;

			for (int count = 0; count < occurance; count++)
			{
				pos = s.IndexOf(substring, pos);

				if (pos == -1) { return pos; }

				pos += substring.Length;
			}
			return pos - substring.Length;
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

		// problem - will provide the filename
		public static string GetSubDirectoryName(this string path, int requestedDepth)
		{
			requestedDepth++;

			path = path.TrimEnd('\\');

			if (requestedDepth > path.CountSubstring("\\")) { return ""; }

			string result = path.GetSubDirectoryPath(requestedDepth - 1);

			if (result.Length == 0) { return ""; }

			int pos = path.IndexOfToOccurance("\\", requestedDepth) + 1;

			return result.Substring(pos);
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

			if (pos < 0) return path;  // no sub-folders - path is first directory

			return path.Substring(1, pos);
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
							return ((PdfNumber)obj).IntValue() + 1;
						}
						else if (obj is PdfDictionary)
						{
							return doc.GetPageNumber((PdfDictionary)obj);
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
#pragma warning disable CS0168 // The variable 'e' is declared but never used
			catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
			{
				return -2;
			}
		}
	}
}
