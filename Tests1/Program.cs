using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static AndyShared.FileSupport.FileNameSheetPDF.FileNameSheetIdentifiers;
using AndyShared.FileSupport.FileNameSheetPDF;
using Tests1.FaveHistoryMgr;
using Tests1.FavHistoryMgr;
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

			p.Process1();
			// p.Process2();
			// p.Process3();

			Console.Write("\nWaiting ... : ");
			char a =	Console.ReadKey().KeyChar;


			Environment.Exit(0);
		}

		private void Process3()
		{
			FavAndHistoryTests.Process();
			// FavAndHistoryTests.TestFavClassf();
			// FavAndHistoryTests.TestFavPair();
			// FavAndHistoryTests.TestHistClassf();
			// FavAndHistoryTests.TestHistPair();

			FavAndHistoryTests.ListUserList(1);
			FavAndHistoryTests.ListUserList(2);
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

		private void Process1()
		{
			int choice = 0;

			switch (choice)
			{
			case 0:
				{
					TestB();
					break;
				}
			}
		}

		private void TestA()
		{
			Console.WriteLine("Starting TestA");

			string[][,] td = TestData1();
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


		private void TestB()
		{
			int testListIdx = 1;

			Console.WriteLine("Starting TestA");

			string[][][,] td = TestData2();
			string[] names = TestDataNames();

			FilePath<FileNameSheetPdf> f;

			for (int i = 0; i < td[testListIdx].Length; i++)
			{
				f = new FilePath<FileNameSheetPdf>(td[testListIdx][i][0, 0]);

				if (f.IsValid)
				{
					ListSheetPdfInfo(td[testListIdx][i], names, f);
				}
				else
				{
					Console.WriteLine();
					Console.WriteLine("this sheet file is not valid| " + td[testListIdx][i][0, 0]);
					Console.WriteLine();
				}
			}
		}

		private void ListSheetPdfInfo(string[,] td, string[] names, FilePath<FileNameSheetPdf> f)
		{
			bool result = true;


			int[] tabStops = new [] { 20, 25, 7, 9 };

			fo = f.FileNameObject;

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("");
			sb.Append("test data| ".PadLeft(tabStops[0])).AppendLine(td[0, 0]);

			sb.Append("file type| ".PadLeft(tabStops[0])).AppendLine(fo.FileType.ToString());
			sb.Append("sht num type| ".PadLeft(tabStops[0])).AppendLine(fo.SheetIdType.ToString());
			sb.Append("sheet num| ".PadLeft(tabStops[0])).AppendLine(fo.SheetNumber);
			sb.Append("isPhBld| ".PadLeft(tabStops[0])).AppendLine(fo.IsPhaseBldg?.ToString() ?? "unknown");
			sb.Append("hasIdent| ".PadLeft(tabStops[0])).AppendLine(fo.hasIdentifier?.ToString() ?? "unknown");

			sb.Append("sht number| ".PadLeft(tabStops[0])).AppendLine(fo.SheetNumber);
			sb.Append("sht Title| ".PadLeft(tabStops[0])).AppendLine(fo.SheetTitle);

			sb.AppendLine();

			string test;
			string test2 = null;
			// string test3 = null;
			string compare;

			bool answer;

			test = fo.SheetID;
			// test2 = fo.originalSheetTitle();

			sb.Append(("sht id's match" + "| ").PadLeft(tabStops[0]));

			sb.Append(test.PadRight(tabStops[2]));
			sb.Append(("(" + test2 + ")").PadRight(tabStops[3]));
			answer = test.Equals(test2);
			result &= answer;
			sb.Append("(match?| ").Append(answer.ToString());
			sb.AppendLine(")");

			int idx = 0;

			test = fo.shtCompType.ToString();
			compare = td[0, 1];

			sb.Append(("sht comp type" + "| ").PadLeft(tabStops[0]))
			.Append(test.PadRight(tabStops[2]));
			sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
			answer = test.Equals(test2);
			result &= answer;
			sb.Append("(match?| ").Append(answer.ToString());
			sb.AppendLine(")");

			if (fo.FileType == FileTypeSheetPdf.SHEET_PDF)
			{
				sb.AppendLine("Comparisons based on indexed values");

				if (fo.isPhaseBldg == true)
				{
					sb.AppendLine("PB Info");

					for (int i = 0; i < 2; i++)
					{
						// idx = ShtIds.CompValueIdx2(ShtCompTypes.PHBLDG, i);
						idx = SheetNumComponentData[VI_PHBLDG].Index;

						test = fo.SheetComps[idx] ?? "";

						if (i % 2 == 0)
						{
							// idx = i / 2;
							test2 = fo[idx];
						}


						compare = td[1, i];

						if (compare.IsVoid() && test.IsVoid()) continue;

						// sb.Append((ShtIds.CompTitle2(ShtCompTypes.PHBLDG, i) + "| ").PadLeft(tabStops[0]));
						sb.Append(SheetNumComponentData[VI_PHBLDG].Name + "| ".PadLeft(tabStops[0]))
							.Append(test.PadRight(tabStops[2]));

						sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
						answer = test.Equals(test2);
						result &= answer;
						sb.Append("(match?| ").Append(answer.ToString());
						sb.AppendLine(")");
						sb.AppendLine();

						if (i % 2 == 0)
						{
							sb.Append(("[" + idx + "] | ").PadLeft(tabStops[0]));
							sb.Append(test2.PadRight(tabStops[2]));
							sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
							answer = test.Equals(test2);
							result &= answer;
							sb.Append("(match?| ").Append(answer.ToString());
							sb.AppendLine(")");
							sb.AppendLine();
						}
					}
				}


				sb.AppendLine("TYPE Info");

				for (int i = 0; i < 9; i++)
				{
					// if (!ShtIds.CompIsUsed2(fo.shtCompType, i)) continue;

					// idx = ShtIds.CompValueIdx2(fo.shtCompType, i);
					idx = SheetNumComponentData[(int) fo.shtCompType].Index;
					
					test = fo.SheetComps[idx] ?? "";

					if (i % 2 == 0)
					{
						idx = (idx) / 2;
						test2 = fo[idx];
					}

					compare = td[2, i];

					if (compare.IsVoid() && test.IsVoid()) continue;

					// sb.Append((ShtIds.CompTitle2(fo.shtCompType, i) + "| ").PadLeft(tabStops[0]))

					sb.Append(SheetNumComponentData[(int) fo.shtCompType].Name + "| ".PadLeft(tabStops[0]))
						.Append(test.PadRight(tabStops[2]));


					sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
					answer = test.Equals(test2);
					result &= answer;
					sb.Append("(match?| ").Append(answer.ToString());
					sb.AppendLine(")");
					sb.AppendLine();

					if (i % 2 == 0)
					{
						sb.Append(("[" + idx + "] | ").PadLeft(tabStops[0]));
						sb.Append(test2.PadRight(tabStops[2]));
						sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
						answer = test.Equals(test2);
						result &= answer;
						sb.Append("(match?| ").Append(answer.ToString());
						sb.AppendLine(")");
						sb.AppendLine();
					}
				}

				if (fo.hasIdentifier == true)
				{
					sb.AppendLine("IDENT Info");

					for (int i = 0; i < 5; i++)
					{
						// idx = ShtIds.CompValueIdx2(ShtCompTypes.IDENT, i);
						idx = SheetNumComponentData[VI_IDENTIFIER].Index;

						test = fo.SheetComps[idx] ?? "";

						if (i % 2 == 1)
						{
							// idx = (i + 1) / 2 + 5;
							idx = idx / 2;
							test2 = fo[idx];
						}

						compare = td[3, i];

						if (compare.IsVoid() && test.IsVoid()) continue;


						// sb.Append((ShtIds.CompTitle2(ShtCompTypes.IDENT, i) + "| ").PadLeft(tabStops[0])).Append(test.PadRight(tabStops[2]));
						sb.Append((SheetNumComponentData[VI_IDENTIFIER].Name + "| ").PadLeft(tabStops[0])).Append(test.PadRight(tabStops[2]));

						sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
						answer = test.Equals(test2);
						result &= answer;
						sb.Append("(match?| ").Append(answer.ToString());
						sb.AppendLine(")");
						sb.AppendLine();

						if (i % 2 == 1)
						{
							sb.Append(("[" + idx + "] | ").PadLeft(tabStops[0]));
							sb.Append(test2.PadRight(tabStops[2]));
							sb.Append(("(" + compare + ")").PadRight(tabStops[3]));
							answer = test.Equals(test2);
							result &= answer;
							sb.Append("(match?| ").Append(answer.ToString());
							sb.AppendLine(")");
							sb.AppendLine();
						}
					}
				}

				sb.AppendLine($"** the overall result of the above| {result}");

				sb.AppendLine("Comparisons based on array values");

				if (!td[1, 0].IsVoid()) { sb.Append(formatValue("Ph/Bldg| "      , fo.PhaseBldg    , td[1, 0], tabStops)); }

				if (!td[1, 0].IsVoid()) { sb.Append(formatValue("Ph/Bldg sep| "  , "?" , td[1, 1], tabStops)); }

				if (!td[2, 0].IsVoid()) { sb.Append(formatValue("Discipline| "   , fo.Discipline   , td[2, 0], tabStops)); }

				if (!td[2, 1].IsVoid()) { sb.Append(formatValue("sep| "          , fo.Seperator0   , td[2, 1], tabStops)); }

				if (!td[2, 2].IsVoid()) { sb.Append(formatValue("Category| "     , fo.Category     , td[2, 2], tabStops)); }

				if (!td[2, 3].IsVoid()) { sb.Append(formatValue("sep| "          , fo.Seperator1   , td[2, 3], tabStops)); }

				if (!td[2, 4].IsVoid()) { sb.Append(formatValue("SubCategory| "  , fo.Subcategory  , td[2, 4], tabStops)); }

				if (!td[2, 5].IsVoid()) { sb.Append(formatValue("sep| "          , fo.Seperator2   , td[2, 5], tabStops)); }

				if (!td[2, 6].IsVoid()) { sb.Append(formatValue("Modifier| "     , fo.Modifier     , td[2, 6], tabStops)); }

				if (!td[2, 7].IsVoid()) { sb.Append(formatValue("sep| "          , fo.Seperator3   , td[2, 7], tabStops)); }

				if (!td[2, 8].IsVoid()) { sb.Append(formatValue("SubModifier| "  , fo.Submodifier  , td[2, 8], tabStops)); }

				if (!td[3, 0].IsVoid()) { sb.Append(formatValue("sep| "          , fo.Seperator4   , td[3, 0], tabStops)); }

				if (!td[3, 1].IsVoid()) { sb.Append(formatValue("Identifier| "   , fo.Identifier   , td[3, 1], tabStops)); }

				if (!td[3, 2].IsVoid()) { sb.Append(formatValue("sep| "          , fo.Seperator5   , td[3, 2], tabStops)); }

				if (!td[3, 3].IsVoid()) { sb.Append(formatValue("SubIdentifier| ", fo.Subidentifier, td[3, 3], tabStops)); }

				// if (!td[3, 4].IsVoid()) { sb.Append(formatValue("sep| "          , fo.Seperator6   , td[3, 4], tabStops)); }

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

		private string[][,] TestData1()
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


		private string[][][,] TestData2()
		{
			string[][][,] data =
					new []
					{
						new []
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
						},
						new []
						{
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\A100 Sheet Name.pdf",   "TYPE30", "Sheet Name", "", "", "", "", "", ""
								},
								{
//						PB
//					                PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE
//						           disc sep0 cat  sep1  subcat  sep2 mod sep3 submod
									"A", "",  "100",   "",  "",   "",  "",   "", ""
								},
								{
//						IDENT
//						            sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
									@"C:\L1.0 Sheet Name.pdf",   "TYPE30", "Sheet Name", "", "", "", "", "", ""
								},
								{
//						PB
//					                PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE
//						           disc sep0 cat  sep1  subcat  sep2 mod sep3 submod
									"L", "",  "1",   ".",  "0",   "",  "",   "", ""
								},
								{
//						IDENT
//						            sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
									@"C:\A2 A1.0 Sheet Name.pdf",   "TYPE30", "Sheet Name", "", "", "", "", "", ""
								},
								{
//						PB
//					                PB   PBSEP
									"A", "", "2", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE
//						           disc sep0 cat  sep1  subcat  sep2 mod sep3 submod
									"A", "",  "1",   ".",  "0",   "",  "",   "", ""
								},
								{
//						IDENT
//						            sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
									@"C:\2 A1.0 Sheet Name.pdf",   "TYPE30", "Sheet Name", "", "", "", "", "", ""
								},
								{
//						PB
//					                PB   PBSEP
									"2", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE
//						           disc sep0 cat  sep1  subcat  sep2 mod sep3 submod
									"A", "",  "1",   ".",  "0",   "",  "",   "", ""
								},
								{
//						IDENT
//						            sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\C01 Cover Sheet.pdf",   "TYPE30", "Cover Sheet", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0 cat  sep1  subcat  sep2 mod sep3 submod
									"C", "",  "01",   "",  "",   "",  "",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\M01 Cover Sheet.pdf",   "TYPE30", "Cover Sheet", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0 cat  sep1  subcat  sep2 mod sep3 submod
									"M", "",  "01",   "",  "",   "",  "",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\T001 Title Sheet.pdf",   "TYPE30", "Title Sheet", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0 cat  sep1  subcat  sep2 mod sep3 submod
									"T", "",  "001",   "",  "",   "",  "",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
									@"C:\DA0.0 General Notes.pdf",   "TYPE30", "General Notes", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"DA", "",  "0", ".",  "0",   "",  "",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\FP10 Floor Plan.pdf",   "TYPE30", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"FP", "",  "10", "",  "",   "",  "",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\GRN 1 Floor Plan.pdf",   "TYPE20", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"GRN", " ",  "1", "",  "",   "",  "",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\N_ID-2.0 Floor Plan.pdf",   "TYPE30", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"N_ID", "-",  "2", ".",  "0",   "",  "",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\L01 - Floor Plan.pdf",   "TYPE30", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"L", "",  "01", "",  "",   "",  "",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
									@"C:\SH-1A - Floor Plan.pdf",   "TYPE40", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"SH", "-",  "1A", "",  "",   "",  "",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\SH-2.1 - Floor Plan.pdf",   "TYPE30", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"SH", "-",  "2", ".",  "1",   "",  "",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\SH 1.0-0 - Floor Plan.pdf",   "TYPE30", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"SH", " ",  "1", ".",  "0",   "-",  "0",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\T24-1.0 - Floor Plan.pdf",   "TYPE30", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"T24", "-",  "1", ".",  "0",   "",  "",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
									@"C:\A2.2-R1S - Floor Plan.pdf",   "TYPE10", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"A", "",  "2", ".",  "2",   "-",  "R1S",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// FAILED
									@"C:\EBM2.2-RF - Floor Plan.pdf",   "TYPE10", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"EBM", "",  "2", ".",  "2",   "-",  "RF",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
									@"C:\LS2.2-P4 - Floor Plan.pdf",   "TYPE10", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"LS", "",  "2", ".",  "2",   "-",  "P4",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
									@"C:\S2.2-P1NB - Floor Plan.pdf",   "TYPE10", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"S", "",  "2", ".",  "2",   "-",  "P1NB",   "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\A7.1.2.1 - Floor Plan.pdf",   "TYPE30", "Floor Plan", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"A", "",  "7", ".",  "1",   ".",  "2", ".", "1"
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},
							new [,]
							{
								{
//						general info                              type      sheet name
// *INVALID
									@"C:\A2-2-2A Bldg A - Floor Plan - Level 2 - East.pdf",   "TYPE30", "Bldg A - Floor Plan - Level 2 - East", "", "", "", "", "", ""
								},
								{
//						PB         PB   PBSEP
									"", "", "", "",  "",    "",  "", "",  ""
								},
								{
//						TYPE       disc sep0   cat  sep1  subcat  sep2 mod sep3 submod
									"A", "",  "2", "-",  "2",  "-",  "2A", "", ""
								},
								{
//						IDENT      sep4 id  sep5 subid sep6
									"",  "", "",  "",   "", "", "", "", ""
								}
							},

						}
					}
				;
			return data;
		}
	}
}