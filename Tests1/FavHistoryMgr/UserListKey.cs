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
	public class UserListKey : INotifyPropertyChanged, IEquatable<UserListKey>, IComparable<UserListKey>
	{

	#region private fields

		private const int MAX_DISPLAY_NAME_LEN = 30;
		private const string NAME_DIVIDER = ">|<";

		private string jobNumber;
		private string padding;
		private string displayName;

	#endregion

	#region ctor

		public UserListKey(string jobNumber, string displayName)
		{
			this.jobNumber = jobNumber;
			this.displayName = displayName.Length <= MAX_DISPLAY_NAME_LEN ?
				displayName : displayName.Substring(0, MAX_DISPLAY_NAME_LEN);

			this.padding = padDisplayName();
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
		public string DisplayName
		{
			get => displayName;
			private set
			{ 
				displayName = value;
				OnPropertyChange();
			}
		}

		[IgnoreDataMember]
		public bool IsValid => !jobNumber.IsVoid() && !displayName.IsVoid();

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private string padDisplayName()
		{
			return ".".Repeat(MAX_DISPLAY_NAME_LEN - displayName.Length);
		}

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

		public int CompareTo(UserListKey other)
		{
			return FavListKeyComparer.Compare(this, other);
		}

		public static IComparer<UserListKey> FavListKeyComparer { get; } = new FavListKeyRelationalComparer();

		private sealed class FavListKeyRelationalComparer : IComparer<UserListKey>
		{
			public int Compare(UserListKey x, UserListKey y)
			{
				if (ReferenceEquals(x, y)) return 0;
				if (ReferenceEquals(null, y)) return 1;
				if (ReferenceEquals(null, x)) return -1;
				int jobNumberComparison = string.Compare(x.jobNumber, y.jobNumber, StringComparison.Ordinal);
				if (jobNumberComparison != 0) return jobNumberComparison;
				return string.Compare(x.displayName, y.displayName, StringComparison.Ordinal);
			}
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (jobNumber.GetHashCode() * 397) ^ displayName.GetHashCode();
			}
		}

		public bool Equals(UserListKey other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return jobNumber == other.jobNumber && displayName == other.displayName;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((UserListKey) obj);
		}


		public override string ToString()
		{
			return jobNumber + NAME_DIVIDER + padding + displayName;
		}

	#endregion
	}
}