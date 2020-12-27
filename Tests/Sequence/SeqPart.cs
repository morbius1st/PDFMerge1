#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



#endregion


// projname: Tests.Sequence
// itemname: SeqPart
// username: jeffs
// created:  11/8/2019 6:04:24 PM


namespace Tests.Sequence
{
	public class SeqPart : INotifyPropertyChanged
	{
		private string code;
		private string title;
		private string description;
		private string test;

		public SeqPart(string code, string title, string description)
		{
			Code = code;
			Title = title;
			Description = description;
		}

		public string Code
		{
			get => code;
			set
			{
				code = value;
				OnPropertyChange();
			}
		}

		public string Title
		{
			get => title;
			set
			{
				title = value;
				OnPropertyChange();
			}
		}

		public string Description
		{
			get => description;
			set
			{
				description = value;
				OnPropertyChange();
			}
		}

		public string Test
		{
			get => test;
			set
			{
				test = value;
				OnPropertyChange();
			}
		}

		public override string ToString()
		{
			return Code + " " + Description;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
