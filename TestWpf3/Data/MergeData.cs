#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   11/29/2020 12:16:56 PM

namespace TestWpf2.Data
{
	public class MergeData : INotifyPropertyChanged
	{
		public static int masterMergeNumber = 0;

		private string mergeName;
		private int mergeNumber;

		public MergeData(string name)
		{
			mergeNumber = masterMergeNumber++;
			mergeName = name + mergeNumber.ToString(" 000");
		}

		public string MergeName
		{
			get => mergeName;
			set
			{
				mergeName = value;
				OnPropertyChange();
			}
		}

		public int MergeNumber
		{
			get => mergeNumber;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return "This is MergeData| " + mergeName;
		}
	}
}