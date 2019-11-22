#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
				OnPropertyChange();
				code = value;
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

		public string Description
		{
			get => description;
			set
			{
				OnPropertyChange();
				description = value;
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
