using System;
using System.Text;
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

			p.Process();

			Console.Write("Waiting ... : ");
			Console.ReadKey();

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
			string compare;
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
			
			
				if (fo.isPhaseBldg == true)
				{
					sb.AppendLine("PB Info");
			
					for (int i = COMP_PB_IDX_MIN; i < COMP_PB_IDX_COUNT; i++)
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

				for (int i = COMP_SHTID_IDX_MIN; i < COMP_SHTID_IDX_COUNT; i++)
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
				
					for (int i = COMP_IDENT_IDX_MIN; i < COMP_IDENT_IDX_COUNT; i++)
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
				
				
						sb.Append((ShtIds.CompTitle2(ShtCompTypes.IDENT, i) + "| ").PadLeft(tabStops[0]))
						.Append(test.PadRight(tabStops[2]));
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
			}
			else
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
						// general info
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