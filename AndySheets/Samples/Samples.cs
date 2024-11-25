#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   11/24/2024 10:00:51 PM

namespace AndySheets.Samples
{
	public class Samples
	{
		private string sampleRootPath = @"C:\Users\jeffs\Documents\Programming\VisualStudioProjects\PDF SOLUTIONS\_Samples\TestBoxes\";

		// order matters
		private string[] filesNames = new []
		{
			"TestBoxes.pdf",                               // 0
			"A0.1.0 - COVER SHEET.pdf",                    // 1
			"A2.2-G - LEVEL G - OVERALL FLOOR PLAN.pdf",   // 2
			"TestBoxes 2.pdf",                             // 3
			"A0.1.0 - COVER SHEET b.pdf",                  // 4
			"A2.2-G - LEVEL G - OVERALL FLOOR PLAN b.pdf", // 5
		};



		public override string ToString()
		{
			return $"this is {nameof(Samples)}";
		}
	}
}
