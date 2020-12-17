#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestWpf2.Windows;

#endregion

// username: jeffs
// created:  12/6/2020 4:07:17 PM

namespace TestWpf2.Modify
{
	public class Modify
	{
	#region private fields

		private MainWinTestWpf2 win;

		private IProgress<double> progressDouble;
		private IProgress<string> progressString;

	#endregion

	#region ctor

		public Modify(MainWinTestWpf2 win, IProgress<double> progressDouble,
			IProgress<string> progressString)
		{
			this.win = win;
			this.progressDouble = progressDouble;
			this.progressString = progressString;
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public async void TestProgress(double qty)
		{
			await Task.Run(() =>
				{
					workTask(qty);
				}
				);
		}

	#endregion

	#region private methods

		private void workTask(double qty)
		{
			for (double i = 0; i < qty; i++)
			{
				Debug.WriteLine("work task item| " + i);

				progressDouble.Report(i+1);
				progressString.Report("working on| " + (i+1) + "\n");

				Thread.Sleep(50);
			}
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is Modify";
		}

	#endregion
	}
}