#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SettingsManager;
using Tests2.Support;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  1/16/2021 3:07:28 PM

namespace Tests2.TestData
{
	public class SampleData
	{
	#region private fields

		private DataManager<DataSet> dm;

	#endregion

	#region ctor

		public SampleData(DataManager<DataSet> dm)
		{
			this.dm = dm;

			MakeSampleData();
		}

	#endregion

	#region public properties



	#endregion

	#region private properties

	#endregion

	#region public methods

		private void MakeSampleData()
		{
			ObservableDictionary<string, subDataClass> od = dm.Data.Od;
			Dictionary<string, subDataStruct> d = dm.Data.D;

			od.Add("fox", new subDataClass("the quick red fox", 1.0, 1));
			od.Add("bear", new subDataClass("the brown bear in the forest", 2.0, 2));
			od.Add("wolf", new subDataClass("the grey wolf and the moon", 3.0, 3));
			od.Add("hawk", new subDataClass("the sienna hawk in the sky", 4.0, 4));

			d.Add("fox",  new subDataStruct("the quick red fox", 1.0, 1));
			d.Add("bear", new subDataStruct("the brown bear in the forest", 2.0, 2));
			d.Add("wolf", new subDataStruct("the grey wolf and the moon", 3.0, 3));
			d.Add("hawk", new subDataStruct("the sienna hawk in the sky", 4.0, 4));

			

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
			return "this is SampleData";
		}

	#endregion
	}
}