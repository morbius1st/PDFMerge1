#region using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
#endregion

// username: jeffs
// created:  8/17/2020 6:17:55 AM

namespace AndyShared.SampleFileSupport
{

	public class SampleFiles : INotifyPropertyChanged
	{
		#region private fields



		#endregion

		#region ctor

		public SampleFiles() { }

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
			return "this is SampleFiles";
		}

		#endregion

	}
}
