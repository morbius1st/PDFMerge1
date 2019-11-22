#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

#endregion


// projname: Tests
// itemname: OutlineItem
// username: jeffs
// created:  11/6/2019 10:07:37 PM


namespace Tests2.OutlineManager
{
	[DataContract(Name = "OutlineItem")]
	public class OutlineItem : INotifyPropertyChanged, IComparable<OutlineItem>
	{
		private string sequenceCode;
		private string title;
		private string description;
		private string pattern;

		public OutlineItem(string sequenceCode, string title, string description, string pattern)
		{
			this.description = description;
			this.sequenceCode = sequenceCode;
			this.pattern = pattern;
			this.title = title;
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
		public string Title
		{
			get => title;
			set
			{
				OnPropertyChange();
				title = value;
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
	}
}
