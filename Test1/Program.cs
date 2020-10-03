using System;
using System.Text;
using AndyShared.FileSupport;
using UtilityLibrary;

namespace Test1
{
	class Program
	{
		static void Main(string[] args)
		{
			Program p = new Program();

		}

		private void Process()
		{
			int choice = 0;

			switch (choice)
			{
			case 0:
				{
					TestA();
					break;
				}
			}

		}

		private void TestA()
		{
			Console.WriteLine("Starting TestA");

			string[][] td = TestData();
			string[] names = TestDataNames();

			FilePath<FileNameSheetPdf> f;

			for (int i = 0; i < td.Length; i++)
			{
				f = new FilePath<FileNameSheetPdf>(td[i][0]);

				ListSheetPdfInfo(td[i], names, f);
			}

		}



		private void ListSheetPdfInfo(string[] td, string[] names, FilePath<FileNameSheetPdf> f)
		{
			int[] tabStops = new [] {15, 25};

			FileNameSheetPdf fo = f.FileNameObject;

			StringBuilder sb = new StringBuilder();

			sb.Append("test data| ".PadLeft(tabStops[0])).AppendLine(td[0]);

			sb.Append("sheet num| ".PadLeft(tabStops[0])).AppendLine(fo.SheetNumber);
			sb.Append("bldg| ".PadLeft(tabStops[0])).Append(fo.PhaseBldg).Append(" (").Append(td[1]).AppendLine(")");
			sb.Append("disc| ".PadLeft(tabStops[0])).Append(fo.PhaseBldgSep).Append(" (").Append(td[2]).AppendLine(")");



			for (var i = 0; i < td.Length; i++)
			{
				
			}







		}



		private string[] TestDataNames()
		{
			string[] names = new []
			{
				"test data"
			};

			return names;
		}

		private string[][] TestData()
		{
			string[][] data = new []
			{
				//                            bldg disc cat  sep10 subcat sep11 mod sep12 submod sep1 id  sep2 subid sep3  sht name
				new []{"A A2.1-1 Sheet Name", "A", "A", "2", ".",  "1",   "-",  "1", "",   "",   "",  "", "",  "",   "", "Sheet Name"},
				//                            bldg disc sep41 cat40 sep10 subcat sep11 mod sep12 submod sep1 id  sep2 subid sep3  sht name
				new []{"A A-1 Sheet Name",    "A", "A", "-",  "2",  "",   "",    "",   "", "",   "",    "",  "", "",  "",   "", "Sheet Name"}
			};

			return data;
		}

	}
}
