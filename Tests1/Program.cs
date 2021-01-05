using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;
using AndyShared.FileSupport.FileNameSheetPDF;
using UtilityLibrary;

namespace Tests1
{
	class Program
	{
		private static FileNameSheetPdf fo;

		[STAThread]
		static void Main(string[] args)
		{
			Program p = new Program();

			p.Process2();

			Console.Write("\nWaiting ... : ");
			Console.ReadKey();

			Environment.Exit(0);
		}

		private void Process2()
		{
			CultureInfo c = new CultureInfo("en-US");
			// CultureInfo c = new CultureInfo("fr_CA");
			decimal d = 0;

			string rs = @"_balance_";

			Regex r = new Regex(rs);

			string test = "this is a test _balance_ substitution";

			string bal = @"\" + d.ToString("c2", c);

			string result = r.Replace(test, bal);

			Console.WriteLine("    rs| " + rs);
			Console.WriteLine("  test| " + test);
			Console.WriteLine("   bal| " + bal);
			Console.WriteLine("result| " + result);

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
				f = new FilePath<FileNameSheetPdf>(td[i][0, 0]);

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
			int[] tabStops = new [] {20, 25, 7, 9};

			fo = f.FileNameObject;

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("");
			sb.Append("test data| ".PadLeft(tabStops[0])).AppendLine(td[0, 0]);

			sb.Append("file type| ".PadLeft(tabStops[0])).AppendLine(fo.FileType.ToString());
			sb.Append("sht num type| ".PadLeft(tabStops[0])).AppendLine(fo.ShtCompTypeName.ToString());
			sb.Append("sheet num| ".PadLeft(tabStops[0])).AppendLine(fo.SheetNumber);
			sb.Append("isPhBld| ".PadLeft(tabStops[0])).AppendLine(fo.IsPhaseBldg?.ToString() ?? "unknown");
			sb.Append("hasIdent| ".PadLeft(tabStops[0])).AppendLine(fo.HasIdentifier?.ToString() ?? "unknown");

			sb.Append("sht number| ".PadLeft(tabStops[0])).AppendLine(fo.SheetNumber);
			sb.Append("sht Title| ".PadLeft(tabStops[0])).AppendLine(fo.SheetTitle);

			sb.AppendLine();

			string test;
			string test2 = null;
			// string test3 = null;
			string compare;

			test = fo.SheetID;
			test2 = fo.SheetId();

			sb.Append(("sht id's match" + "| ").PadLeft(tabStops[0]));

			sb.Append(test.PadRight(tabStops[2]));
			sb.Append(("(" + test2 + ")").PadRight(tabStops[3]));
			sb.Append("(match?| ").Append((test.Equals(test2)).ToString());
			sb.AppendLine(")");

			int idx = 0;

			test = fo.SheetComponentType.ToString();
			compare = td[0, 1];

			sb.Append(("sht comp type" + "| ").PadLeft(tabStops[0]))
			.Append(test.PadRight(tabStops[2]));
			sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
			sb.Append("(match?| ").Append((test.Equals(compare)).ToString());
			sb.AppendLine(")");

			if (fo.FileType == FileTypeSheetPdf.SHEET_PDF)
			{
				sb.AppendLine("Comparisons based on indexed values");
			
				if (fo.isPhaseBldg == true)
				{
					sb.AppendLine("PB Info");
			
					for (int i = 0; i < 2; i++)
					{
						idx = ShtIds.CompValueIdx2(ShtCompTypes.PHBLDG, i);

						test = fo.SheetComps[idx] ?? "";
						
						if (i % 2 == 0)
						{
							// idx = i / 2;
							test2 = fo[idx];
						}
			
			
						compare = td[1, i];
			
						if(compare.IsVoid() && test.IsVoid()) continue;
			
						sb.Append((ShtIds.CompTitle2(ShtCompTypes.PHBLDG, i) + "| ").PadLeft(tabStops[0]))
						.Append(test.PadRight(tabStops[2]));
						sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
						sb.Append("(match?| ").Append((test.Equals(compare)).ToString()).Append(")");
						sb.AppendLine();
			
						if (i % 2 == 0)
						{
							sb.Append(("[" + idx + "] | ").PadLeft(tabStops[0]));
							sb.Append(test2.PadRight(tabStops[2]));
							sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
							sb.Append("(match?| ").Append((test2.Equals(compare)).ToString()).Append(")");
							sb.AppendLine();
						}
			
					}
				}


				sb.AppendLine("TYPE Info");

				for (int i = 0; i < 9; i++)
				{
					if (!ShtIds.CompIsUsed2(fo.shtCompType, i)) continue;

					idx = ShtIds.CompValueIdx2(fo.shtCompType, i);

					test = fo.SheetComps[idx] ?? "";

					if (i % 2 == 0)
					{
						idx = (idx) / 2;
						test2 = fo[idx];
					}

					compare = td[2, i];

					if (compare.IsVoid() && test.IsVoid()) continue;

					sb.Append((ShtIds.CompTitle2(fo.shtCompType, i) + "| ").PadLeft(tabStops[0]))
					.Append(test.PadRight(tabStops[2]));
					sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
					sb.Append("(match?| ").Append((test.Equals(compare)).ToString()).Append(")");
					sb.AppendLine();

					if (i % 2 == 0)
					{
						sb.Append(("[" + idx + "] | ").PadLeft(tabStops[0]));
						sb.Append(test2.PadRight(tabStops[2]));
						sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
						sb.Append("(match?| ").Append((test2.Equals(compare)).ToString()).Append(")");
						sb.AppendLine();
					}
				}

				if (fo.hasIdentifier == true)
				{
					sb.AppendLine("IDENT Info");
				
					for (int i = 0; i < 5; i++)
					{
						idx = ShtIds.CompValueIdx2(ShtCompTypes.IDENT, i);

						test = fo.SheetComps[idx] ?? "";
						
						if (i % 2 == 1)
						{
							// idx = (i + 1) / 2 + 5;
							idx = idx / 2;
							test2 = fo[idx];
						}
				
						compare = td[3, i];
				
						if (compare.IsVoid() && test.IsVoid()) continue;
				
				
						sb.Append((ShtIds.CompTitle2(ShtCompTypes.IDENT, i) + "| ").PadLeft(tabStops[0])).Append(test.PadRight(tabStops[2]));
						sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
						sb.Append("(match?| ").Append((test.Equals(compare)).ToString()).Append(")");
						sb.AppendLine();
				
						if (i % 2 == 1)
						{
							sb.Append(("[" + idx + "] | ").PadLeft(tabStops[0]));
							sb.Append(test2.PadRight(tabStops[2]));
							sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
							sb.Append("(match?| ").Append((test2.Equals(compare)).ToString()).Append(")");
							sb.AppendLine();
						}
					}
				}


				sb.AppendLine("Comparisons based on array values");

				if (!td[1, 0].IsVoid()) {sb.Append(formatValue("Ph/Bldg| "      , fo.PhaseBldg    , td[1, 0], tabStops));}
				if (!td[1, 0].IsVoid()) {sb.Append(formatValue("Ph/Bldg sep| "  , fo.PhaseBldgSep , td[1, 1], tabStops));}
				if (!td[2, 0].IsVoid()) {sb.Append(formatValue("Discipline| "   , fo.Discipline   , td[2, 0], tabStops));}
				if (!td[2, 1].IsVoid()) {sb.Append(formatValue("sep| "          , fo.Seperator0   , td[2, 1], tabStops));}
				if (!td[2, 2].IsVoid()) {sb.Append(formatValue("Category| "     , fo.Category     , td[2, 2], tabStops));}
				if (!td[2, 3].IsVoid()) {sb.Append(formatValue("sep| "          , fo.Seperator1   , td[2, 3], tabStops));}
				if (!td[2, 4].IsVoid()) {sb.Append(formatValue("SubCategory| "  , fo.Subcategory  , td[2, 4], tabStops));}
				if (!td[2, 5].IsVoid()) {sb.Append(formatValue("sep| "          , fo.Seperator2   , td[2, 5], tabStops));}
				if (!td[2, 6].IsVoid()) {sb.Append(formatValue("Modifier| "     , fo.Modifier     , td[2, 6], tabStops));}
				if (!td[2, 7].IsVoid()) {sb.Append(formatValue("sep| "          , fo.Seperator3   , td[2, 7], tabStops));}
				if (!td[2, 8].IsVoid()) {sb.Append(formatValue("SubModifier| "  , fo.Submodifier  , td[2, 8], tabStops));}
				if (!td[3, 0].IsVoid()) {sb.Append(formatValue("sep| "          , fo.Seperator4   , td[3, 0], tabStops));}
				if (!td[3, 1].IsVoid()) {sb.Append(formatValue("Identifier| "   , fo.Identifier   , td[3, 1], tabStops));}
				if (!td[3, 2].IsVoid()) {sb.Append(formatValue("sep| "          , fo.Seperator5   , td[3, 2], tabStops));}
				if (!td[3, 3].IsVoid()) {sb.Append(formatValue("SubIdentifier| ", fo.Subidentifier, td[3, 3], tabStops));}
				if (!td[3, 4].IsVoid()) {sb.Append(formatValue("sep| "          , fo.Seperator6   , td[3, 4], tabStops));}


			}
			else
			{
				sb.Append("\n*** file type: ").Append(fo.FileType).AppendLine(" ***\n");
			}

			Console.WriteLine(sb.ToString());
		}

		private string formatValue(string title, string test, string compare, int[] tabStops)
		{
			StringBuilder sb = new StringBuilder();

			test = test == null ? "{null}" : test;

			sb.Append(title.PadLeft(tabStops[0])).Append(test.PadRight(tabStops[2]));
			sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
			sb.Append("(match?| ").Append((test.Equals(compare)).ToString()).Append(")");
			sb.AppendLine();

			return sb.ToString();
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
						@"C:\A A2.1-3 Sheet Name.pdf", "TYPE10", "Sheet Name", "", "", "", "", "", ""
					},
					{
						// PB
						"A", " ", "", "", "", "", "", "", ""
					},
					{
						// TYPE 
						"A", "", "2", ".",  "1",   "-",  "3",   "", ""
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
						@"C:\A A2.1-3.4 Sheet Name.pdf", "TYPE10", "Sheet Name", "", "", "", "", "", ""
					},
					{
						// PB
						"A", " ", "", "", "", "", "", "", ""
					},
					{
						// TYPE 
						"A", "", "2", ".",  "1", "-",  "3",   ".", "4"
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
						@"C:\A A2.1-3.4(right) Sheet Name.pdf", "TYPE10", "Sheet Name", "", "", "", "", "", ""
					},
					{
						// PB
						"A", " ", "", "", "", "", "", "", ""
					},
					{
						// TYPE 
						"A", "", "2", ".",  "1", "-",  "3",   ".", "4"
					},
					{
						// IDENT
						"(",  "right", "",  "",   ")", "", "", "", ""
					}
				},
				new [,]
				{
					{
						// general info  alt code = Alt+179
						@"C:\A A2.1-4.5(right│left) Sheet Name.pdf", "TYPE10", "Sheet Name", "", "", "", "", "", ""
					},
					{
						// PB
						"A", " ", "", "", "", "", "", "", ""
					},
					{
						// TYPE 
						"A", "", "2", ".",  "1", "-",  "4",   ".", "5"
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