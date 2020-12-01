#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion

// username: jeffs
// created:  11/28/2020 6:32:28 PM

namespace ClassifySheets.Tests
{
	public class Test01
	{
	#region private fields

		private Progress<double> p2Double;
		private Progress<string> p2String;
		private Progress<string> p2msg;

	#endregion

	#region ctor

		public Test01() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure(Progress<double> p2Double, Progress<string> p2msg,Progress<string> p2String)
		{
			this.p2Double = p2Double;
			this.p2msg = p2msg;
			this.p2String = p2String;
		}

		public async void Go(int min, int max, int delay)
		{
			await Task.Run(delegate { go(min, max, delay); });
		}


		private void go(int min, int max, int delay)
		{
			string msg;
			Thread.Sleep(delay);

			for (int i = min+1; i < max+1; i++)
			{
				msg = "this is " + i.ToString("00");

				((IProgress<string>) p2msg)?.Report("** D ** " + msg + "\n");
				((IProgress<string>) p2String)?.Report(msg);
				((IProgress<double>) p2Double)?.Report(i);

				Thread.Sleep(delay);
			}
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is Test01";
		}

	#endregion
	}
}