#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



#endregion


// projname: Test3
// itemname: Leaf
// username: jeffs
// created:  12/23/2019 11:16:20 PM


namespace Test3
{
	public class Leaf //: ATreeNode, INotifyPropertyChanged
	{


	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	}
}
