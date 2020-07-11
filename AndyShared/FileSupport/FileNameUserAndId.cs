
#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   6/21/2020 9:58:53 PM


namespace AndyShared.FilesSupport
{

	public class FileNameUserAndId : AFileName, INotifyPropertyChanged
	{
		private const int USER_NAME_IDX = 1;
		private const int FILE_ID_IDX = 2;
		private const int FILE_EXT_IDX = 3;

		private const string PARSE_PATTERN = @"^\((.{3,})\) (.{3,})\.([xX][mM][lL])";
		// private bool isValid;
		private string userName;
		private string fileId;

		public override string FileNameNoExt
		{
			get => formatFileName();
			set
			{
				if (value.IsVoid())
				{
					this.fileNameNoExt = null;
					UserName = null;
					FileId = null;

					return;
				}

				this.fileNameNoExt = value.Trim();

				parseFileName();

				OnPropertyChange();
			}
		}

		/// <summary>
		/// The filename and extension
		/// </summary>
		public string FileName => fileNameNoExt + FilePathUtil.EXT_SEPARATOR + Extension;

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

		public bool IsValid { get; private set; }

		private string formatFileName()
		{
			return $"({UserName}) {FileId}";
		}


		private void parseFileName()
		{
			

			Regex r = new Regex(PARSE_PATTERN, RegexOptions.IgnoreCase);

			Match m = r.Match(FileName);
			

			if (!m.Success ||
				m.Groups.Count != 4) return;

			UserName = m.Groups[USER_NAME_IDX].Value;
			FileId = m.Groups[FILE_ID_IDX].Value;
			Extension = m.Groups[FILE_EXT_IDX].Value;

			if (userName.Length > 2 && fileId.Length > 3 && Extension.Length == 3)
			{
				IsValid = true;
				OnPropertyChange("IsValid");
			}

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
