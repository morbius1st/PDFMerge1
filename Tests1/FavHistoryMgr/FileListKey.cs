#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  1/31/2021 4:30:28 PM

namespace Tests1.FaveHistoryMgr
{

	[DataContract(Namespace = "")]
	public class FileListKey : INotifyPropertyChanged, IEquatable<FileListKey>, IComparable<FileListKey>
	{

	#region private fields

		private string jobNumber;
		private string filename;

	#endregion

	#region ctor

		public FileListKey(string jobNumber, string filename)
		{
			this.jobNumber = jobNumber;
			this.filename = filename;
		}

	#endregion

	#region public properties

		[DataMember(Order = 1)]
		public string JobNumber
		{
			get => jobNumber;
			private set
			{ 
				jobNumber = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 2)]
		public string Filename
		{
			get => filename;
			private set
			{ 
				filename = value;
				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public bool IsValid => !jobNumber.IsVoid() && !filename.IsVoid();

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public int CompareTo(FileListKey other)
		{
			return FileListKeyComparer.Compare(this, other);
		}

		public static IComparer<FileListKey> FileListKeyComparer { get; } = new FileListKeyRelationalComparer();

		private sealed class FileListKeyRelationalComparer : IComparer<FileListKey>
		{
			public int Compare(FileListKey x, FileListKey y)
			{
				if (ReferenceEquals(x, y)) return 0;
				if (ReferenceEquals(null, y)) return 1;
				if (ReferenceEquals(null, x)) return -1;
				int jobNumberComparison = string.Compare(x.jobNumber, y.jobNumber, StringComparison.Ordinal);
				if (jobNumberComparison != 0) return jobNumberComparison;
				return string.Compare(x.filename, y.filename, StringComparison.Ordinal);
			}
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (jobNumber.GetHashCode() * 397) ^ filename.GetHashCode();
			}
		}

		public bool Equals(FileListKey other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return jobNumber == other.jobNumber && filename == other.filename;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((FileListKey) obj);
		}


		public override string ToString()
		{
			return jobNumber+filename;
		}

	#endregion
	}
}