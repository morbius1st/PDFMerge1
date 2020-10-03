using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndyShared.FileSupport.SheetPDF;
using static AndyShared.FileSupport.SheetPDF.FileNameSheetIdentifiers;
using UtilityLibrary;

namespace Tests1
{
	class Program
	{
		private FileNameSheetIdentifiers ids = new FileNameSheetIdentifiers();

		[STAThread]
		static void Main(string[] args)
		{
			Program p = new Program();

			p.Process();

			Console.Write("Waiting ... : ");
			Console.Read();

			Environment.Exit(0);
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

			string[][,] td = TestData();
			string[] names = TestDataNames();

			FilePath<FileNameSheetPdf> f;

			for (int i = 0; i < td.Length; i++)
			{
				f = new FilePath<FileNameSheetPdf>(td[i][0,0]);

				if (f.IsValid)
				{
					ListSheetPdfInfo(td[i], names, f);
				}
				else
				{
					Console.WriteLine();
					Console.WriteLine("this sheet file is not valid| " + td[i][0, 0]);
					Console.WriteLine();
				}
			}
		}


		private void ListSheetPdfInfo(string[,] td, string[] names, FilePath<FileNameSheetPdf> f)
		{
			int[] tabStops = new [] {20, 25, 6, 8};

			FileNameSheetPdf fo = f.FileNameObject;

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("");
			sb.Append("test data|".PadLeft(tabStops[0])).AppendLine(td[0,0]);

			sb.Append("file type|".PadLeft(tabStops[0])).AppendLine(fo.FileType.ToString());
			sb.Append("sht comp type|".PadLeft(tabStops[0])).AppendLine(fo.shtCompType.ToString());
			sb.Append("sheet num|".PadLeft(tabStops[0])).AppendLine(fo.SheetNumber);
			sb.Append("ph bldg|".PadLeft(tabStops[0])).AppendLine(fo.ShtIdComps[PHBLDGIDX]);
			sb.Append("sht id|".PadLeft(tabStops[0])).AppendLine(fo.sheetID);
			sb.Append("sht Title|".PadLeft(tabStops[0])).AppendLine(fo.SheetTitle);
			sb.Append("isPhBld|".PadLeft(tabStops[0])).AppendLine(fo.isPhaseBldg?.ToString() ?? "unknown");
			sb.Append("hasIdent|".PadLeft(tabStops[0])).AppendLine(fo.hasIdentifier?.ToString() ?? "unknown");

			if (fo.FileType == FileTypeSheetPdf.SHEET_PDF)
			{
				string test;
				string compare;

				if (fo.isPhaseBldg == true)
				{
					sb.AppendLine("PB Info");

					for (int i = PBSTART; i < PBEND; i++)
					{
						test = fo.PbComps[ids.CompIdx(ShtCompTypes.PHBLDG, i)] ?? "";
						compare = td[1, i];

						sb.Append((ids.CompTitle(ShtCompTypes.PHBLDG, i) + "|").PadLeft(tabStops[0]))
						.Append(test.PadRight(tabStops[2]));
						sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
						sb.Append("(match?| ").AppendLine((test.Equals(compare)).ToString());
					}
				}


				sb.AppendLine("TYPE Info");

				for (int i = TYPESTART; i < TYPEEND; i++)
				{
					if (!ids.CompIsUsed(fo.shtCompType, i)) continue;

					test = fo.ShtIdComps[ids.CompIdx(fo.shtCompType, i)]?? "";
					compare = td[2, i];

					sb.Append((ids.CompTitle(fo.shtCompType, i) + "|").PadLeft(tabStops[0]))
					.Append(test.PadRight(tabStops[2]));
					sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
					sb.Append("(match?| ").AppendLine((test.Equals(compare)).ToString());
				}

				if (fo.hasIdentifier == true)
				{
					sb.AppendLine("IDENT Info");

					for (int i = IDENTSTART; i < IDENTEND; i++)
					{
						test = fo.IdentComps[ids.CompIdx(ShtCompTypes.IDENT, i)] ?? "";
						compare = td[3, i];

						sb.Append((ids.CompTitle(ShtCompTypes.IDENT, i) + "|").PadLeft(tabStops[0]))
						.Append(test.PadRight(tabStops[2]));
						sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
						sb.Append("(match?| ").AppendLine((test.Equals(compare)).ToString());
					}
				}
			} else
			{
				sb.Append("\n*** file type: ").Append(fo.FileType).AppendLine(" ***\n");
			}

			Console.WriteLine(sb.ToString());
		}


		private string[] TestDataNames()
		{
			string[] names = new []
			{
				"test data"
			};

			return names;
		}

		private string[][,] TestData()
		{
			string[][,] data = new []
			{
				new [,]
				{
					{
						// general info
						@"C:\A A-101A Sheet Name.pdf", "TYPE40", "Sheet Name", "", "", "", "", "", ""
					},
					{
						// PB
						"A", " ", "", "", "", "", "", "", ""
					},
					{
						// TYPE 
						"A", "-", "101A", "",  "",   "",  "",   "", ""
					},
					{
						// IDENT
						"",  "", "",  "",   "", "", "", "", ""
					}
				},
				new [,]
				{
					{
						// general info
						@"C:\A A2.1-1 Sheet Name.pdf", "TYPE10", "Sheet Name", "", "", "", "", "", ""
					},
					{
						// PB
						"A", " ", "", "", "", "", "", "", ""
					},
					{
						// TYPE 
						"A", "", "2", ".",  "1",   "-",  "1",   "", ""
					},
					{
						// IDENT
						"",  "", "",  "",   "", "", "", "", ""
					}
				},
				new [,]
				{
					{
						// general info
						@"C:\A A2.1-1.1 Sheet Name.pdf", "TYPE10", "Sheet Name", "", "", "", "", "", ""
					},
					{
						// PB
						"A", " ", "", "", "", "", "", "", ""
					},
					{
						// TYPE 
						"A", "", "2", ".",  "1", "-",  "1",   ".", "1"
					},
					{
						// IDENT
						"",  "", "",  "",   "", "", "", "", ""
					}
				},
				new [,]
				{
					{
						// general info
						@"C:\A A2.1-1.1(right) Sheet Name.pdf", "TYPE10", "Sheet Name", "", "", "", "", "", ""
					},
					{
						// PB
						"A", " ", "", "", "", "", "", "", ""
					},
					{
						// TYPE 
						"A", "", "2", ".",  "1", "-",  "1",   ".", "1"
					},
					{
						// IDENT
						"(",  "right", "",  "",   ")", "", "", "", ""
					}
				},
				new [,]
				{
					{
						// general info
						@"C:\A A2.1-1.1(right│left) Sheet Name.pdf", "TYPE10", "Sheet Name", "", "", "", "", "", ""
					},
					{
						// PB
						"A", " ", "", "", "", "", "", "", ""
					},
					{
						// TYPE 
						"A", "", "2", ".",  "1", "-",  "1",   ".", "1"
					},
					{
						// IDENT
						"(",  "right", "│",  "left",  ")", "", "", "", ""
					}
				},
				new [,]
				{
					{
//						general info                  type      sheet name
						@"C:\A GRN.1 Sheet Name.pdf",   "TYPE20", "Sheet Name", "", "", "", "", "", ""
					},
					{
//						PB
//					    PB   PBSEP
						"A", " ", "", "",  "",    "",  "", "",  ""
					},
					{
//						TYPE
//						disc   sep0 cat sep1 subcat sep2 mod sep3 submod
						"GRN", ".", "1", "",  "",   "",  "",   "", ""
					},
					{
//						IDENT
//						sep4 id  sep5 subid sep6
						"",  "", "",  "",   "", "", "", "", ""
					}
				},
				new [,]
				{
					{
//						general info                  type      sheet name
						@"C:\A2.1a Sheet Name.pdf",   "TYPE30", "Sheet Name", "", "", "", "", "", ""
					},
					{
//						PB
//					    PB   PBSEP
						"", "", "", "",  "",    "",  "", "",  ""
					},
					{
//						TYPE
//						disc sep0 cat  sep1  subcat  sep2 mod sep3 submod
						"A", "",  "2", ".",  "1a",   "",  "",   "", ""
					},
					{
//						IDENT
//						sep4 id  sep5 subid sep6
						"",  "", "",  "",   "", "", "", "", ""
					}
				},
				new [,]
				{
					{
//						general info                  type      sheet name
						@"C:\A A2.1a Sheet Name.pdf",   "TYPE30", "Sheet Name", "", "", "", "", "", ""
					},
					{
//						PB
//					    PB   PBSEP
						"A", " ", "", "",  "",    "",  "", "",  ""
					},
					{
//						TYPE
//						disc sep0 cat  sep1  subcat  sep2 mod sep3 submod
						"A", "",  "2", ".",  "1a",   "",  "",   "", ""
					},
					{
//						IDENT
//						sep4 id  sep5 subid sep6
						"",  "", "",  "",   "", "", "", "", ""
					}
				},
				new [,]
				{
					{
//						general info                  type      sheet name
						@"C:\A A-1 Sheet Name.pdf",   "TYPE40", "Sheet Name", "", "", "", "", "", ""
					},
					{
//						PB
//					    PB   PBSEP
						"A", " ", "", "",  "",    "",  "", "",  ""
					},
					{
//						TYPE
//						disc sep0 cat sep1 subcat sep2 mod sep3 submod
						"A", "-", "1", "",  "",   "",  "",   "", ""
					},
					{
//						IDENT
//						sep4 id  sep5 subid sep6
						"",  "", "",  "",   "", "", "", "", ""
					}
				},
			};

			return data;
		}
	}
}