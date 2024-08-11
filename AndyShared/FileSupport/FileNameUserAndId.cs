
#region + Using Directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   6/21/2020 9:58:53 PM


namespace AndyShared.FileSupport
{

	public class FileNameUserAndId : FileNameSimple, /*AFileName,*/ INotifyPropertyChanged
	{
		private const string FILE_EXT = "xml";

		private const int USER_NAME_IDX = 1;
		private const int FILE_ID_IDX = 2;
		private const int FILE_EXT_IDX = 3;

		// private const string PARSE_PATTERN = @"^\((.{3,})\) (.{3,})\.([xX][mM][lL])";
		private const string PARSE_PATTERN = @"^\((.{3,})\) (.{3,})";


		// private bool isValid;
		private string userName;
		private string fileId;

		/// <summary>
		/// The filename and extension
		/// </summary>
		public new string FileName => AssembleFileName(UserName, FileId, ExtensionNoSep);

		/// <summary>
		/// The filename sans extension
		/// </summary>
		public override string FileNameNoExt
		{
			get => AssembleFileNameNoExt(UserName, FileId);

			set
			{
				if (value.IsVoid())
				{
					this.fileNameNoExt = null;
					UserName = null;
					FileId = null;

				} 
				else
				{
					this.fileNameNoExt = value.TrimStart();

					parseFileName();
				}

				OnPropertyChange();
			}
		}

		public override string ExtensionNoSep
		{
			get => this.extensionNoSep;

			set
			{
				if (value.Equals(FILE_EXT, StringComparison.OrdinalIgnoreCase))
				{
					this.extensionNoSep = value;
				}
				else
				{
					extensionNoSep = null;
				}
			}
		}

		public string UserName
		{
			get => userName;
			private set
			{
				userName = value;

				OnPropertyChange();
			}
		}
		public string FileId
		{
			get => fileId;
			private set
			{
				fileId = value;
				OnPropertyChange();
			}
		}

		public override bool IsValid
		{
			get
			{
				
				if (userName == null || fileId == null ||
					userName.Length < 3 || fileId.Length < 4) return false;

				return true;
			}
		}

		private void parseFileName()
		{
			Regex r = new Regex(PARSE_PATTERN, RegexOptions.IgnoreCase);

			Match m = r.Match(this.fileNameNoExt);

			if (!m.Success ||
				m.Groups.Count != 3) return;

			UserName = m.Groups[USER_NAME_IDX].Value;
			FileId = m.Groups[FILE_ID_IDX].Value;
			// Extension = m.Groups[FILE_EXT_IDX].Value;

			OnPropertyChange("IsValid");
		}

		public static string AssembleFileNameNoExt(string userName, string fileId)
		{
			return $"({userName}) {fileId}";
		}
		
		public static string AssembleFileName(string userName, string fileId, string extensionNoSep)
		{
			return AssembleFileNameNoExt(userName, fileId) + FilePathUtil.EXT_SEPARATOR + extensionNoSep;
		}

	#region event handling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	}
}
