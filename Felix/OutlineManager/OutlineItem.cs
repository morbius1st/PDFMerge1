#region + Using Directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

#endregion


// projname: Tests
// itemname: OutlineItem
// username: jeffs
// created:  11/6/2019 10:07:37 PM


namespace Felix.OutlineManager
{
	[DataContract(Name = "OutlineItem")]
	public class OutlineItem : INotifyPropertyChanged, IComparable<OutlineItem>
	{

	#region private fields - primary data

		private string sequenceCode;
		private string outlinePath;
		private string description;
		private string pattern;

	#endregion

		// has this item been changed / needs to be saved?
		private bool modified;

		public OutlineItem(string sequenceCode, string pattern, string outlinePath, 
			string description, bool modified = false)
		{
			this.description = description;
			this.sequenceCode = sequenceCode;
			this.pattern = pattern;
			this.outlinePath = outlinePath;
			this.modified = modified;
		}

		[DataMember]
		public string SequenceCode
		{
			get => sequenceCode;
			set
			{
				OnPropertyChange();
				sequenceCode = value;
			}
		}

		[DataMember]
		public string OutlinePath
		{
			get => outlinePath;
			set
			{
				OnPropertyChange();
				outlinePath = value;
			}
		}

		[DataMember]
		public string Description
		{
			get => description;
			set
			{
				OnPropertyChange();
				description = value;
			}
		}

		[DataMember]
		public string Pattern
		{
			get => pattern;
			set
			{
				OnPropertyChange();
				pattern = value;
			}
		}

		public bool Modified
		{
			get => modified;
			set
			{
				if (value != modified)
				{
					modified = value;
					OnPropertyChange();
				}
			}
		}

		public int CompareTo(OutlineItem other)
		{
			return sequenceCode.CompareTo(other.SequenceCode);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return sequenceCode;
		}

		public OutlineItem Clone()
		{
			return new OutlineItem(sequenceCode, pattern, outlinePath, description);
		}
	}
}
