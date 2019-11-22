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
// itemname: SeqItem
// username: jeffs
// created:  11/8/2019 7:16:01 PM


namespace Tests.Sequence
{
	public class SeqItem : INotifyPropertyChanged
	{
		private string description;

		public List<SeqPart> Parts { get; set; }

		public SeqItem(string description)
		{
			this.description = description;

			Parts = new List<SeqPart>(4);
		}

		public string Description
		{
			get { return MakeDescription(); }

		}

		public void AddItem(List<SeqPart> parts)
		{
			Parts = new List<SeqPart>(parts);

			OnPropertyChange("Parts");
		}

		public string MakeID()
		{
			string result = null;

			foreach (SeqPart part in Parts)
			{
				result += part.Code;
			}

			return result;
		}

		private string MakeDescription()
		{
			string result = null;

			foreach (SeqPart sp in Parts)
			{
				result += sp.Title;
			}

			return result;
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
