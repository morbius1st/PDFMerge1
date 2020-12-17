#region using

using System;
using System.Collections.Generic;
using System.Windows.Controls;

#endregion

// username: jeffs
// created:  12/13/2020 6:10:08 AM

namespace ClassifySheets.Windows.Support
{

	public class ProgressBarManager
	{
		public class PbData
		{
			public double Value{ get; set; }
			public double Min{ get; private set; }
			public double Max { get; private set; }
			public bool Reverse { get; private set; }
			public string Name { get; private set; }

			private PbData(string name, double initValue, 
				double maxValue, bool reverse = false)
			{
				Name = name;
				Reverse = reverse;
				Min = initValue;
				Max = maxValue;
				Reset();
			}

			public void Reset()
			{
				Value = Reverse ? Min : Max;
			}
		}


	#region private fields

		private List<PbData> progBars;

		private ProgressBar pb;

		private double pbValue;
		private double pbMaxValue;

		private IProgress<double> pbDouble;

	#endregion

	#region ctor

		public ProgressBarManager() { }

	#endregion

	#region public properties

		public int Index { get; set; }

		public double Value
		{
			get => progBars[Index].Value;
			set
			{
				progBars[Index].Value = value;
				
				pbDouble?.Report(value);
			}
		}


	#endregion

	#region private properties

	#endregion

	#region public methods

		public int Create(PbData pbData)
		{
			progBars.Add(pbData);

			return progBars.Count - 1;
		}

		public bool Assign(PbData pbData, int index)
		{
			if (index < 0 || index > progBars.Count)
				return false;

			progBars[index] = pbData;

			return true;
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
			return "this is ProgressBarManager";
		}

	#endregion
	}
}