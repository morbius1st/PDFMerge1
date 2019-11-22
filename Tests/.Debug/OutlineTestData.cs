#region + Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tests;

#endregion


// projname: Tests
// itemname: OutlineTestData
// username: jeffs
// created:  11/6/2019 10:16:00 PM


namespace Tests
{
	public class OutlineTestData
	{
		public OutlineTestData()
		{

		}

		public static List<OutlineItem> TestData()
		{
			List<OutlineItem> items = new List<OutlineItem>();

			items.Add( new OutlineItem(  "170 Electrical", 
				"All Electrical Sheets", "A E", OutlineItem.MakeCode("170")));

			items.Add( new OutlineItem(  "150 Mechanical", 
				"All Mechanical Sheets", "A M", OutlineItem.MakeCode("150")));

			items.Add( new OutlineItem(  "130 Plumbing", 
				"All Plumbing Sheets", "A P", OutlineItem.MakeCode("130")));
			items.Add( new OutlineItem(  "132 Plumbing", 
				"All Plumbing Sheets", "A P2", OutlineItem.MakeCode("130")));
			
			items.Add( new OutlineItem(  "110 Structural", 
				"All Structural Sheets", "A S", OutlineItem.MakeCode("110")));
			items.Add( new OutlineItem(  "110 Structural - Plans", 
				"All Structural Sheets", "A S2", OutlineItem.MakeCode("110", "2")));

			items.Add( new OutlineItem(  "073 Architectural - Vertical Views", 
				"Architectural Vertical View Sheets", "A A3", OutlineItem.MakeCode("070", "3")));

			items.Add( new OutlineItem(  "072 Architectural - Slab Plans", 
				"Architectural Plan Sheets", "A A2.0", OutlineItem.MakeCode("070", "2", "0")));

			items.Add( new OutlineItem(  "071 Architectural - Site", 
				"Architectural Site Sheets", "A A1", OutlineItem.MakeCode("070", "1")));

			items.Add( new OutlineItem(  "070 Architectural", 
				"All Architectural Sheets", "A A", OutlineItem.MakeCode("070")));

			items.Add( new OutlineItem(  "072 Architectural - Plans", 
				"Architectural Plan Sheets", "A A2", OutlineItem.MakeCode("070", "2")));

			items.Add( new OutlineItem(  "072 Architectural - Floor Plans", 
				"Architectural Plan Sheets", "A A2.1", OutlineItem.MakeCode("070", "2", "1")));

			items.Add( new OutlineItem(  "074 Architectural - Sections", 
				"Architectural Section Sheets", "A A4", OutlineItem.MakeCode("070", "4")));
			return items;
		}



	}
}
