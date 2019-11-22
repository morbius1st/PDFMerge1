#region + Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilityLibrary;

#endregion


// projname: AODeliverable.FileSelection
// itemname: Route
// username: jeffs
// created:  11/2/2019 5:18:09 PM


namespace AODeliverable.FileSelection
{
	public class Route
	{
		public string FullRoute { get; private set; }

		public string volumeName { get; set; } = null;
		public string root { get; set; } = null;
		public List<string> folderNames  { get; set; } = new List<string>();
		public string path { get; set; } = null;
		public string directoryPath  { get; set; }
		public string fileName  { get; set; } = null;

		public bool? IsUnc { get; set; } = null;

		public Route(string fullRoute)
		{
			if (string.IsNullOrWhiteSpace(fullRoute))
			{
				FullRoute = "";
				return;
			}

			fullRoute.Replace('/', '\\');

			FullRoute = fullRoute.Trim();

			divide();
		}

		public bool IsRooted => string.IsNullOrWhiteSpace(root);
		public bool IsValid => FullRoute.Length > 0;
		public bool HasFileName => !string.IsNullOrWhiteSpace(fileName);

		public int FolderCount => folderNames.Count;

		public string LeftFolders(int count)
		{
			if (folderNames.Count == 0) return root;
			if (count >= folderNames.Count) count = folderNames.Count;

			string path;

			if (IsUnc == true)
			{
				path = root;
			}
			else
			{
				path = volumeName;
			}

			for (int i = 0; i < count; i++)
			{
				path += @"\" + folderNames[i];
			}

			return path;
		}

		private void divide()
		{
			SetFileNameAndPath();

			SetVolume();

			directoryPath = directoryPath.Substring(volumeName.Length);

			if (!string.IsNullOrWhiteSpace(directoryPath))
			{
				if (directoryPath.StartsWith("\\"))
				{
					folderNames.AddRange(directoryPath.Substring(1).Split('\\'));
				}
				else
				{
					folderNames.AddRange(directoryPath.Split('\\'));
				}
			}

			SetRoot();
		}

		private void SetRoot()
		{
			int pos;

			if (IsUnc == true)
			{
				root = volumeName + "\\";

				if (folderNames != null && folderNames.Count > 0)
				{
					root += folderNames[0];

					folderNames.RemoveAt(0);

					if (folderNames.Count == 1)
					{
						pos = 0;
					}
					else
					{
						pos = directoryPath.IndexOf('\\', 1);
					}

					if (pos > -1)
					{
						directoryPath = directoryPath.Substring(pos);
					}
				}
			}
			else if (IsUnc == false)
			{
				if (!string.IsNullOrWhiteSpace(volumeName))
				{
					root = volumeName + "\\";
				}
			}
		}

		private void SetFileNameAndPath()
		{
			fileName = Path.GetFileName(FullRoute);

			if (!Path.HasExtension(fileName))
			{
				fileName = "";
				directoryPath = FullRoute;
			}
			else
			{
				directoryPath = Path.GetDirectoryName(FullRoute);
			}

			path = directoryPath;
		}

		private void SetVolume()
		{
			int pos = 0;

			if (directoryPath.StartsWith(@"\\"))
			{
				IsUnc = true;

				pos = directoryPath.IndexOf('\\', 2);

				if (pos == -1)
				{
					volumeName = directoryPath;
				}
				else
				{
					volumeName = directoryPath.Substring(0, pos);
				}
			}
			else
			{
				pos = directoryPath.IndexOf(":");

				if (pos == -1)
				{
					volumeName = "";
				}

				IsUnc = false;

				volumeName = directoryPath.Substring(0, ++pos);
			}
		}

		//
//		public string GetFileName(string path)
//		{
//			List<string> parts = null;
//			return getFileName(path, ref parts);
//		}
//
//		private string getFileName(string path, ref List<string> parts)
//		{
//			if (parts == null) 
//			{
//				parts = new List<string>(FullRoute.Split('\\'));
//			}
//
//			string fileName = "";
//
//			if (parts[parts.Count - 1].Contains("."))
//			{
//				fileName = parts[parts.Count - 1];
//
//				parts.RemoveAt(parts.Count - 1);
//			}
//
//			return fileName;
//		}
//
//		public string GetVolume(string path)
//		{
//			List<string> parts = null;
//			return getVolume(path, ref parts);
//		}
//
//		private string getVolume(string path, ref List<string> parts)
//		{
//			if (parts == null) 
//			{
//				parts = new List<string>(FullRoute.Split('\\'));
//			}
//
//			string vol = "";
//
//			if (FullRoute.StartsWith(@"\\"))
//			{
//				vol = @"\" + AssembleParts(parts, 3);
//				parts = RemoveParts(parts,0, 3);
//			}
//
//			if (parts[0].Contains(":"))
//			{
//				vol = parts[0];
//				parts = RemoveParts(parts, 0, 1);
//			}
//
//			return vol;
//		}
//
//		public string GetPath(string Path)
//		{
//			List<string> parts = new List<string>(FullRoute.Split('\\'));
//
//			string filename = getFileName(path, ref parts);
//			string volume = getVolume(path, ref parts);
//
//			return AssembleParts(parts, parts.Count);
//		}
//
//
//		private void Divide2()
//		{
//			Debug.WriteLine("");
//			Debug.WriteLine("at divide2");
//			Debug.WriteLine("full route  | " + FullRoute);
//			Debug.WriteLine("volume      | " + GetVolume(FullRoute));
//			Debug.WriteLine("filename    | " + GetFileName(FullRoute));
//			Debug.WriteLine("path        | " + GetPath(FullRoute));
//			Debug.WriteLine("\n\n");
//		}
//
//		// depth is NOT zero based
//		private string AssembleParts(List<string> parts, int depth)
//		{
//			if (depth == 0) return "";
//
//			if (depth > parts.Count)
//			{
//				depth = parts.Count;
//			}
//
//			string answer = "";
//
//			for (int i = 0; i < depth; i++)
//			{
//				answer = @"\" + parts[i];
//			}
//
//			return answer;
//		}
//
//		private List<string> RemoveParts(List<string> parts, int start, int quantity)
//		{
//			if (quantity == 0) return parts;
//			if (start < 0) start = 0;
//
//			int begin = start - 1;
//			int end = begin + quantity;
//
//			if (end >= parts.Count) end = parts.Count - 1;
//
//			for (int i = end; i > begin; i--)
//			{
//				parts.RemoveAt(i);
//			}
//
//			return parts;
//		}
//

	}
}