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
// created:  11/8/2019 6:09:14 PM


namespace Tests.Sequence
{
	public class SeqParts : INotifyPropertyChanged
	{
//		private static SeqParts _instance;
//
//		public static SeqParts Instance
//		{
//			get
//			{
//				if (_instance == null) _instance = new SeqParts();
//
//				return _instance;
//			}
//		}
//
		public Dictionary<string, SeqPart> Parts { get; private set; }

		public string Name { get; private set; }

		public SeqParts(string name)
		{
			Name = name;

			Parts = new Dictionary<string, SeqPart>(4);
		}

		public void AddPart(SeqPart part)
		{
			Parts.Add(part.Code, part);
			OnPropertyChange("Parts");
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
