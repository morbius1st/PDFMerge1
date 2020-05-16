#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using ClassifierEditor.NumberComponent;

#endregion

// username: jeffs
// created:  5/12/2020 10:06:41 PM

namespace ClassifierEditor.DataRepo
{
	// this is the actual data set saved to the data file


	[DataContract(Name = "SheetCategories", Namespace = "", IsReference = true)]
	public class SheetCategories : INotifyPropertyChanged
	{
	#region public properties

		[DataMember(Order = 1)]
		public string Description { get; private set; } = "This is a full list of sheet organization categories";

		[DataMember(Order = 2)]
		public TreeNode TreeBase { get; set; } = new TreeNode();

	#endregion

//		#region private properties
//		#endregion
//
		#region public methods

			public void NotifyUpdate()
			{
				OnPropertyChange("TreeBase");
			}

		#endregion
//
//		#region private methods
//		#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

//		#region event handeling
//		#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SheetCategories";
		}

	#endregion
	}
}