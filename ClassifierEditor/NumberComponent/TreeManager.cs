#region using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

// username: jeffs
// created:  5/2/2020 9:51:43 AM

namespace ClassifierEditor.NumberComponent
{
	public class TreeManager : INotifyPropertyChanged
	{
	#region private fields
	
		public TreeNode TreeBase { get; private set; } = new TreeNode();

	#endregion

	#region ctor

		public TreeManager()
		{
			SampleData sd = new SampleData();

			sd.Sample(TreeBase);

			OnPropertyChange("TreeBase");

		}

	#endregion

	#region public properties


	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is TreeManager";
		}

	#endregion
	}
}