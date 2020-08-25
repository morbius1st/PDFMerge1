#region using

using System;
using System.IO;

#endregion

// username: jeffs
// created:  8/23/2020 10:39:21 PM

namespace AndyShared.FileSupport
{
	public class FileUtilities
	{
	#region private fields

	#endregion

	#region ctor

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		/// <summary>
		/// Copies, and can rename, a file from one location to another<br/>
		/// both parameters are file paths - neither can be a folder
		/// </summary>
		/// <param name="exFilePath"></param>
		/// <param name="nwFilePath"></param>
		/// <returns></returns>
		public static bool CopyFile(string exFilePath, string nwFilePath)
		{
			// step 1 - validate - existing file path cannot equal new file path
			if (exFilePath.Equals(nwFilePath)) return false;

			// step 2 - validate - new file cannot exist
			if (File.Exists(nwFilePath)) return false;

			try
			{
				// step 3 - copy file to new location (and name).
				File.Copy(exFilePath, nwFilePath);
			}
			catch
			{
				// any errors mean a failure
				return false;
			}

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
			return "this is FileUtilities";
		}

	#endregion
	}
}