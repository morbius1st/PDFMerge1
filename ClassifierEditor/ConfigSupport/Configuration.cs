#region using directives

using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

// username: jeffs
// created:  6/11/2020 6:14:19 AM

namespace ClassifierEditor.ConfigSupport
{

	// configures the location of the information (folders and file names)
	// and handles creating a default setup

	public class Configuration : INotifyPropertyChanged
	{
		#region private fields

			private string classificationFile;
			private string masterClassificationFile;

			private string sampleFile;
			private string masterSampleFile;

		#endregion

		#region ctor

		public Configuration() { }

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
			return "this is Configuration";
		}

		#endregion

	}
}
