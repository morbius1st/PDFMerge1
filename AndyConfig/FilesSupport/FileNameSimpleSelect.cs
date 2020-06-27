
#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   6/21/2020 9:58:53 PM


namespace AndyConfig.FilesSupport
{

	public class FileNameSimpleSelectable : AFileName, INotifyPropertyChanged
	{
		private bool selected;

		public bool Selected
		{
			get => selected;
			set
			{
				selected = value;
				OnPropertyChange();
			}
		}

//		public FileNameSimple() { }

//		public FileNameSimple(string filename)
//		{
//			this.filename = filename;
//		}

	#region event handling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	}
}
