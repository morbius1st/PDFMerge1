#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Sylvester.FileSupport;
using Sylvester.Process;
using Sylvester.UserControls;
using UtilityLibrary;

#endregion


// projname: Sylvester.FolderSupport
// itemname: FolderInfo
// username: jeffs
// created:  2/3/2020 11:14:04 PM


namespace Sylvester.FolderSupport
{
	public class FolderInfoCurrent : FolderInfo
	{
		public FolderInfoCurrent(FolderManager fm, HeaderControl hc) : base(fm, hc) { }

		public override FolderType FolderType => FolderType.CURRENT;
		public override int FolderPathType => FolderType.CURRENT.Value();
		public override int FolderTypeValue => FolderType.Value();
	}
	
	public class FolderInfoRevision : FolderInfo
	{
		public FolderInfoRevision(FolderManager fm, HeaderControl hc) : base(fm, hc){ }

		public override FolderType FolderType => FolderType.REVISION;
		public override int FolderPathType => FolderType.REVISION.Value();
		public override int FolderTypeValue => FolderType.Value();
	}

	public abstract class FolderInfo
	{
		private FilePath<FileNameAsSheet> folder;

		private HeaderControl Hc;

		public FolderInfo(FolderManager fm, HeaderControl hc)
		{
			Hc = hc;
			Hc.AssignEvents(fm);
		}

	#region public properties

		public FilePath<FileNameAsSheet> Folder
		{
			get => folder;

			set
			{
				folder = value;
				Hc.Path = value;
			}
		}

		public bool HasPriorFolder => Folder.IsValid;

		public abstract FolderType FolderType { get; }

		public abstract int FolderTypeValue { get; }

		public abstract int FolderPathType { get; }

	#endregion

	#region private methods

	#endregion


	}
}
