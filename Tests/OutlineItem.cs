#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



#endregion


// projname: Tests
// itemname: OutlineItem
// username: jeffs
// created:  11/6/2019 10:07:37 PM


namespace Tests
{
	public class OutlineItem : INotifyPropertyChanged, IComparable<OutlineItem>
	{
		private string description;
		private List<string> sequenceCode;
		private string pattern;
		private string title;

		public OutlineItem(   string title, string description, string pattern, List<string> sequenceCode)
		{
			this.description = description;
			this.sequenceCode = sequenceCode;
			this.pattern = pattern;
			this.title = title;
		}

		public string Description
		{
			get => description;
			set
			{
				OnPropertyChange();
				description = value;
			}
		}

		public List<string> SequenceCode
		{
			get => sequenceCode;
			set
			{
				OnPropertyChange();
				sequenceCode = value;
			}
		}

		public string SequenceCodeString
		{
			get => SeqCodeString();
		}

		public string Pattern
		{
			get => pattern;
			set
			{
				OnPropertyChange();
				pattern = value;
			}
		}

		public string Title
		{
			get => title;
			set
			{
				OnPropertyChange();
				title = value;
			}
		}

		public static List<string> MakeCode(params string[] codes)
		{
			List<string> c = new List<string>();

			foreach (string code in codes)
			{
				c.Add(code);
			}

			return c;
		}

		public string SeqCodeString()
		{
			string answer = sequenceCode[0];

			for (int i = 1; i < sequenceCode.Count; i++)
			{
				answer += "." + sequenceCode[i];
			}

			return answer;
		}

		public int CompareTo(OutlineItem other)
		{
			int iterations = Math.Min(sequenceCode?.Count ?? 0,
				other?.sequenceCode.Count ?? 0);

			int result = -1;

			for (int i = 0; i < iterations; i++)
			{
				result = sequenceCode[i].CompareTo(other.sequenceCode[i]);

				if (result != 0) break;
			}

			return result;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return SeqCodeString();
		}
	}
}
