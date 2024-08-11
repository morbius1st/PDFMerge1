
#region + Using Directives

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   6/21/2020 9:58:53 PM


namespace AndyShared.FileSupport
{

	public class FileNameSimpleSelectable : FileNameSimple, 
		/*AFileName,*/ INotifyPropertyChanged
	{
		private bool selected;

		public bool Selected
		{
			get => selected;
			set
			{
				Debug.WriteLine("at selected| " + value);

				selected = value;
				OnPropertyChange();
			}
		}

	#region event handling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	}
}
