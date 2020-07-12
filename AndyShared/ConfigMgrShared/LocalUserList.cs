#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  7/11/2020 6:02:05 PM

namespace AndyShared.ConfigMgrShared
{
	public class LocalUserList
	{
	#region private fields

		private Dictionary<string, FilePath<FileNameSimple>> userList = 
			new Dictionary<string, FilePath<FileNameSimple>>();


	#endregion

	#region ctor

		public LocalUserList() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void AddUser(string username, string folderPath)
		{
			FilePath<FileNameSimple> path = new FilePath<FileNameSimple>(folderPath);

			if (path.IsFolderPath)
			{
				userList.Add(username, path);
			}
		}

		public bool FindUser(string username, out string path)
		{
			path = null;

			bool result = userList.ContainsKey(username);

			if (!result) return false;

			path = userList[username].GetPath;

			return true;
		}

	#endregion

	#region private methods

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is LocalUserList";
		}

	#endregion
	}
}