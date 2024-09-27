#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using AndyShared.FileSupport.FileNameSheetPDF;
using Test3.FileNameSheetPDF;
using UtilityLibrary;

#endregion


// projname: $projectname$
// itemname: SheetPdfManager
// username: jeffs
// created:  9/8/2024 5:56:30 PM

namespace Test3.SheetMgr
{
	public class SheetPdfManager
	{
	#region private fields

		bool? result;
		string failed = "";

		private List<int> compLengths;

		public static List<FilePath<FileNameSheetPdf2>> SheetFiles = new List<FilePath<FileNameSheetPdf2>>();

		private static readonly Lazy<SheetPdfManager> instance =
			new Lazy<SheetPdfManager>(() => new SheetPdfManager());

	#endregion

	#region ctor

		private SheetPdfManager() { }

	#endregion

	#region public properties

		public static SheetPdfManager Instance => instance.Value;

		public List<string> SpecialDisciplines { get; set; } = new List<string>()
		{
			"T24", "X30", "Z31"
		};

		public List<int> CompLengths
		{
			get => compLengths;
			set => compLengths = value;
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		private int start = 0;
		private int last;
		private int end;


		// use this
		// version 3
		public void ParseSheetNames2()
		{
			Samples2.MakeSamples();

			last =  Samples2.Sheets.Count;
			end = last;
			start = 0;

			FileNameSheetParser3.Instance.Config();
			FileNameSheetParser3.Instance.CreateSpecialDisciplines(SpecialDisciplines);
			FileNameSheetParser3.Instance.CreateFileNamePattern();


			for (var i = start; i < end; i++)
			{
				Samples2.Sheets[i].SheetPdf3 =
					new FilePath<FileNameSheetPdf3>(Samples2.Sheets[i].FileName);
			}
		}

		public void ParseSheetNames3()
		{
			Test3.SheetMgr.Samples3.MakeSamples();

			last =  Samples3.Sheets.Count;
			end = last;
			start = 0;

			FileNameSheetParser3.Instance.Config();
			FileNameSheetParser3.Instance.CreateSpecialDisciplines(SpecialDisciplines);
			FileNameSheetParser3.Instance.CreateFileNamePattern();


			for (var i = start; i < end; i++)
			{
				Samples3.Sheets[i].SheetPdf3 =
					new FilePath<FileNameSheetPdf3>(Samples3.Sheets[i].FileName);
			}
		}

		// version 2 + 3
		private void showShtNumber23(ShtNumber sn, List<string> sheetComps)
		{
			string compName;
			string nameObjStr = "";
			string sampStr = "";

			Debug.WriteLine(
				$"\t{$"index {sn.Index}",-31}|  +10      0               1               2               3              4                5               6");
			Debug.WriteLine(
				               $"\t{" ", -31}|  pb/0    di/2    sp0/3   cat/4   sp1/5   scat/6  sp2/7   mod/8   sp3/9  smod/10  sp4/11  idf/12  sp5/13  sid/14");
			Debug.Write($"\t{$">{sn.OrigSheetNumber}<",-31}| ");

			foreach (KeyValuePair<int, SheetIdentifiers3.ShtNumComps2> kvp in
					SheetIdentifiers3.SheetNumComponents)
			{
				if (kvp.Value.ItemClass == SheetIdentifiers3.ShtIdClass2.SC_IGNORE) continue;

				nameObjStr = sn.ShtNumComps[kvp.Key] ?? "";

				if (kvp.Key > SheetIdentifiers3.SI_SEP0 && nameObjStr.IsVoid())
				{
					Debug.Write(" << end");
					break;
				}

				compName = kvp.Value.GroupId;

				sampStr = sheetComps[kvp.Key];
				if (!nameObjStr.Equals(sampStr))
				{
					failed += $" {compName} (A >{nameObjStr}< S >{sampStr}<)";
					result = null;

					Debug.Write("*>");
				}
				else
				{
					Debug.Write(" >");
				}
					
				Debug.Write($"{$"{nameObjStr}<",-6}");

			}

			Debug.Write("\n");
		}

		// version 3
		public void ShowShtNameResults3()
		{
			int pass = 0;
			int hardFail = 0;
			int softFail = 0;

			bool? result;

			SheetPdfSample s;

			Debug.WriteLine("\n\n****************************");
			Debug.WriteLine("*** sheet parse test 3 *****");
			Debug.WriteLine("****************************\n");

			for (int i = start; i < end; i++)
			{
				result = true;
				failed = "";

				s = Samples3.Sheets[i];

				result = showSheet3(s);

				if (result == true)
				{
					pass++;
					Debug.WriteLine("MATCH\n");
				}
				else
				{
					if (result == false)
					{
						hardFail++;
						Debug.WriteLine($"HARD FAILED [{failed}]\n");
					}
					else
					{
						softFail++;
						Debug.WriteLine($"SOFT FAILED [{failed}]\n");
					}
				}
			}

			Debug.WriteLine($"results| pass {pass} | hard fail {hardFail} | soft fail {softFail}");

			Debug.WriteLine("\ncomp lengths");

			int j;
			string compName;
			string compValue;

			for (j = 0; j < SheetIdentifiers3.SI_SUBIDENTIFIER; j++)
			{
				if (SheetIdentifiers3.SheetNumComponents[j].ItemClass== SheetIdentifiers3.ShtIdClass2.SC_COMP_ID_SEP || 
					SheetIdentifiers3.SheetNumComponents[j].ItemClass== SheetIdentifiers3.ShtIdClass2.SC_IGNORE) continue;

				compName = SheetIdentifiers3.SheetNumComponents[j].Name;
				compValue = CompLengths[j].ToString();

				Debug.Write($"{compName} {$"({compValue})",-5}| ");
			}

			compName = SheetIdentifiers3.SheetNumComponents[j].Name;
			compValue = CompLengths[j].ToString();

			Debug.Write($"{compName} {$"({compValue})",-5}");
			
			Debug.Write("\n");

		}

		// version 3a
		private bool? showSheet3(SheetPdfSample s)
		{
			failed = "";
			result = true;

			if (!s.SheetPdf3.FileNameObject.IsValid)
			{
				failed += "| file name is not valid";
				result = null;
			}

			if (!s.SheetPdf3.FileNameObject.ShtNumber.IsParseGood)
			{
				failed += "| parse is not valid";
				result = null;
			}

			if (!s.SheetPdf3.FileName.Equals(s.FileName))
			{
				failed += " | file name mis-match";
				result = false;
			}

			bool? temp = showShtnumber3(s);

			Debug.Write($"\t        sample| ");
			Debug.Write($"{$"file name >{s.FileName}<",-60}");
			Debug.Write($"{$"title >{s.SheetTitle}<",-30}");
			Debug.Write("\n");

			Debug.Write($"\t        actual| ");
			Debug.Write($"{$"file name >{s.SheetPdf3.FileName}<",-60}");
			Debug.Write($"{$"title >{s.SheetPdf3.FileNameObject.SheetTitle}<",-60}");
			Debug.Write("\n");

			Debug.Write($"\t        actual| ");
			Debug.Write($"{$"is valid >{s.SheetPdf3.IsValid}<",-18}");
			Debug.Write($"{$"status >{s.SheetPdf3.FileNameObject.StatusCode}<",-30}");
			Debug.Write($"{$"full path >{s.SheetPdf3.FullFilePath}<"}");
			Debug.Write("\n");


			if (!result.HasValue || !temp.HasValue)
			{
				result = null;
			}
			else
			{
				result = result == true && temp == true;
			}

			return result;
		}

		// version 3b
		private bool? showShtnumber3(SheetPdfSample s)
		{
			ShtNumber sn = s.SheetPdf3.FileNameObject.ShtNumber;

			showShtNumber23(sn, s.SheetComps);

			Debug.Write($"\t^^^ =>  sample| ");
			Debug.Write($"{$"sheet num  >{s.SheetNumber}<",-30}");
			Debug.Write("\n");


			Debug.Write($"\t        actual| ");
			Debug.Write($"{$"parsed num >{sn.ParsedSheetNumber}<",-30}");
			Debug.Write($"{$"orig num >{sn.OrigSheetNumber}<",-18}");
			Debug.Write("\n");


			Debug.Write("\t");
			Debug.Write(" ".Repeat(8));
			Debug.Write(
				$"type {sn.SheetIdType,-10} ({(sn.SheetIdType.Equals(s.SheetType) ? "match" : "fail")}) | parse OK? {sn.IsParseGood,-10} | is PB {sn.IsPhaseBldg,-10}");

			Debug.Write("\n");

			if (!sn.ParsedSheetNumber.Equals(s.SheetNumber))
			{
				failed += $"| sheet number mis-match | (A >{sn.ParsedSheetNumber}< S >{s.SheetNumber}<)";
				// result = false;
			}

			if (sn.SheetIdType != s.SheetType)
			{
				failed +=
					$" | sheet id type mis-match | (A >{sn.SheetIdType}< S >{s.SheetType}<)";
				result = false;
			}

			return result;
		}


		/* voided - do not use

		public void ParseSheets1()
		{
			Samples2.MakeSamples();

			last =  Samples2.Sheets.Count;
			end = last;
			start = 0;

			FileNameSheetParser3.Instance.CreateSpecialDisciplines(SpecialDisciplines);
			FileNameSheetParser3.Instance.CreateFileNamePattern();

			for (var i = start; i < end; i++)
			{
				Samples2.Sheets[i].SheetPdf =
					new FilePath<FileNameSheetPdf2>(Samples2.Sheets[i].FileName);
			}
		}

		public void ParseSheetNumbers2()
		{
			string sample;

			Samples2.MakeSamples();

			last =  Samples2.Sheets.Count;
			end = last;
			start = 0;

			FileNameSheetParser3.Instance.CreateSpecialDisciplines(SpecialDisciplines);
			FileNameSheetParser3.Instance.CreateFileNamePattern();

			for (var i = start; i < end; i++)
			{
				sample = Samples2.Sheets[i].SheetNumber;

				Samples2.Sheets[i].ShtNumber = new ShtNumber(sample, true);
			}
		}

		// version 1
		public void ShowSheetsResults1()
		{
			int pass = 0;
			int fail = 0;

			bool? result;
			// string failed = "";
			// string nameObjStr = "";
			// string sampStr = "";

			SheetPdfSample s;

			Debug.WriteLine("\n\n****************************");
			Debug.WriteLine("*** sheet parse test *******");
			Debug.WriteLine("****************************\n");

			for (int i = 0; i < Samples2.Sheets.Count; i++)
			{
				s = Samples2.Sheets[i];

				// test each sheet versus the expected result
				result = showSheet1(s);

				if (result == true)
				{
					pass++;
					Debug.WriteLine("MATCH\n");
				}
				else
				{
					fail++;
					Debug.WriteLine($"FAILED [{failed}]\n");
				}

				// FileNameSheetPdf2 f = s.SheetPdf.FileNameObject;
			}

			Debug.WriteLine($"results| pass {pass} | fail {fail}");
		}

		// version 1a
		private bool? showSheet1(SheetPdfSample s)
		{
			string compName;
			string nameObjStr = "";
			string sampStr = "";

			failed = "";
			result = true;

			FileNameSheetPdf2 fo;

			if (s.SheetPdf.FileName.Equals(s.FileName))
			{
				fo = s.SheetPdf.FileNameObject;

				Debug.WriteLine(
					$"{$"index {fo.Index}",-31}|  +10      0               1               2               3               4               5               6");
				Debug.WriteLine(
					$"{" ", -31}|  pb/0    di/2    sp0/3   cat/4   sp1/5   scat/6  sp2/7   mod/8   sp3/9  smod/10  sp4/11  idf/12  sp5/13  sid/14");
				Debug.Write($"{$">{fo.SheetNumber}<",-31}| ");

				foreach (KeyValuePair<int, SheetIdentifiers3.ShtNumComps2> kvp in
						SheetIdentifiers3.SheetNumComponents)
				{
					if (kvp.Value.ItemClass == SheetIdentifiers3.ShtIdClass2.SC_IGNORE) continue;

					compName = kvp.Value.GroupId;
					nameObjStr = fo.SheetComps[kvp.Key] ?? "";
					sampStr = s.SheetComps[kvp.Key];

					if (!nameObjStr.Equals(sampStr))
					{
						failed += $" {compName} (A >{nameObjStr}< S >{sampStr}<)";
						result = false;

						Debug.Write("*>");
					}
					else
					{
						Debug.Write(" >");
					}

					Debug.Write($"{$"{nameObjStr}<",-6}");
				}

				Debug.Write("\n");

				Debug.WriteLine(
					$"^^^ =>  {$"sample| name >{s.SampleSheet}<",-56} | num {$">{s.SheetNumber}<",-16} | title >{s.SheetTitle}<");
				Debug.WriteLine(
					$"        {$"actual| name >{fo.SheetName}<",-56} | num {$">{fo.SheetNumber}<",-16} | title >{fo.SheetTitle}<");

				Debug.Write(" ".Repeat(8));
				Debug.Write($"{$"file name    >{fo.FileName}<",-56} | ");
				Debug.Write($"{$"no ext >{fo.FileNameNoExt}<",-40} | ");
				Debug.Write($"ext >{fo.ExtensionNoSep}<\n");

				Debug.Write(" ".Repeat(8));
				Debug.Write($">{fo.SheetIdType}<\n");

				Debug.Write(" ".Repeat(8));
				Debug.Write($"is PB {$">{fo.IsPhaseBldg}<",-10} | ");
				Debug.Write($"is valid >{fo.IsValid.ToString()}\n");


				if (!fo.SheetNumber.Equals(s.SheetNumber))
				{
					failed = $"sheet number mis-match | (A >{fo.SheetID}< S >{s.SheetNumber}<)";
					result = false;
				}


				if (!fo.SheetName.Equals(s.SampleSheet))
				{
					failed +=
						$"sheet name mis-match | (A >{fo.SheetName}< S >{s.SampleSheet}<)";

					result = false;
				}


				if (!fo.SheetTitle.Equals(s.SheetTitle))
				{
					failed +=
						$"sheet title mis-match | (A >{fo.SheetTitle}< S >{s.SheetTitle}<)";

					result = false;
				}

				if (fo.SheetIdType != s.SheetType)
				{
					failed +=
						$"sheet id type mis-match | (A >{fo.SheetIdType}< S >{s.SheetType}<)";
					result = false;
				}
			}
			else
			{
				failed = "file name mis-match";
				result = false;
			}

			return result;
		}

		// version 2
		public void ShowShtNumberResults2()
		{
			int pass = 0;
			int hardFail = 0;
			int softFail = 0;

			bool? result;
			// string failed = "";
			// string nameObjStr = "";
			// string sampStr = "";

			SheetPdfSample s;

			Debug.WriteLine("\n\n****************************");
			Debug.WriteLine("*** sheet parse test 2 *****");
			Debug.WriteLine("****************************\n");

			for (int i = start; i < end; i++)
			{
				s = Samples2.Sheets[i];

				// test each sheet versus the expected result
				result = showShtNumber2(s);

				if (result == true)
				{
					pass++;
					Debug.WriteLine("MATCH\n");
				}
				else
				{
					if (result == false)
					{
						hardFail++;
						Debug.WriteLine($"HARD FAILED [{failed}]\n");
					}
					else
					{
						softFail++;
						Debug.WriteLine($"SOFT FAILED [{failed}]\n");
					}
				}
			}

			Debug.WriteLine($"results| pass {pass} | hard fail {hardFail} | soft fail {softFail}");
		}

		// version 2a
		private bool? showShtNumber2(SheetPdfSample s)
		{
			string compName;
			string nameObjStr = "";
			string sampStr = "";

			failed = "";
			result = true;

			ShtNumber sn = s.ShtNumber;

			showShtNumber23(sn, s.SheetComps);

			//
			// Debug.WriteLine(
			// 	$"{$"index {sn.Index}",-31}|  +10      0               1               2               3               4               5               6");
			// Debug.WriteLine(
			// 	$"{" ", -31}|  pb/0    di/2    sp0/3   cat/4   sp1/5   scat/6  sp2/7   mod/8   sp3/9  smod/10  sp4/11  idf/12  sp5/13  sid/14");
			// Debug.Write($"{$">{sn.OrigSheetNumber}<",-31}| ");
			//
			// foreach (KeyValuePair<int, SheetIdentifiers3.ShtNumComps2> kvp in
			// 		SheetIdentifiers3.SheetNumComponents)
			// {
			// 	if (kvp.Value.ItemClass == SheetIdentifiers3.ShtIdClass2.SC_IGNORE) continue;
			//
			// 	compName = kvp.Value.GroupId;
			// 	nameObjStr = sn.ShtNumComps[kvp.Key] ?? "";
			// 	sampStr = s.SheetComps[kvp.Key];
			// 	if (!nameObjStr.Equals(sampStr))
			// 	{
			// 		failed += $" {compName} (A >{nameObjStr}< S >{sampStr}<)";
			// 		result = null;
			//
			// 		Debug.Write("*>");
			// 	}
			// 	else
			// 	{
			// 		Debug.Write(" >");
			// 	}
			//
			// 	Debug.Write($"{$"{nameObjStr}<",-6}");
			// }
			//
			// Debug.Write("\n");

			Debug.WriteLine(
				$"^^^ =>  {$"sample| num >{s.SheetNumber}<"}");
			Debug.WriteLine(
				$"        {$"actual| num >{sn.ParsedSheetNumber}<"}");

			Debug.Write(" ".Repeat(8));
			Debug.Write(
				$"type {sn.SheetIdType,-10} ({(sn.SheetIdType.Equals(s.SheetType) ? "match" : "fail")}) | parse OK? {sn.IsParseGood,-10} | is PB {sn.IsPhaseBldg,-10}");

			Debug.Write("\n");

			if (!sn.ParsedSheetNumber.Equals(s.SheetNumber))
			{
				failed = $"sheet number mis-match | (A >{sn.ParsedSheetNumber}< S >{s.SheetNumber}<)";
				// result = false;
			}

			if (sn.SheetIdType != s.SheetType)
			{
				failed +=
					$"sheet id type mis-match | (A >{sn.SheetIdType}< S >{s.SheetType}<)";
				result = false;
			}

			return result;
		}
		*/

		// not used?

		// public void ShowSheetsResults1()
		// {
		// 	string nameObjStr = "";
		// 	string sampStr = "";
		//
		// 	SheetPdfSample s;
		//
		// 	Debug.WriteLine("\n\n****************************");
		// 	Debug.WriteLine("*** sheet parse test *******");
		// 	Debug.WriteLine("****************************\n");
		//
		//
		// 	for (int i = start; i < end; i++)
		// 	{
		// 		result = true;
		// 		failed = "";
		// 		string compName;
		// 		int idx;
		//
		// 		s = Samples2.Sheets[i];
		//
		// 		// Debug.Write(">");
		//
		// 		if (s.SheetPdf.FileName.Equals(s.SampleSheet))
		// 		{
		// 			// if (s.SheetPdf.FileNameObject.SheetNumber.Equals(s.SheetNumber))
		// 			// {
		// 			for (int j = 0; j < SheetIdentifiers3.SheetNumComponents.Count; j++)
		// 			{
		// 				if (SheetIdentifiers3.SheetNumComponents[j].ItemClass ==
		// 					SheetIdentifiers3.ShtIdClass2.SC_IGNORE) continue;
		//
		// 				compName = SheetIdentifiers3.SheetNumComponents[j].GroupId;
		//
		// 				nameObjStr = s.SheetPdf.FileNameObject.SheetComps[j] ?? "";
		// 				// compName = FileNameSheetIdentifiers.CompNames[j] ?? "";
		//
		// 				sampStr = s.SheetComps[j];
		//
		// 				Debug.Write($"{$">{nameObjStr}<",-7}");
		//
		// 				if (!nameObjStr.Equals(sampStr))
		// 				{
		// 					failed += $" {j,-3} ({compName}) (A >{nameObjStr}> vs S >{sampStr}<)";
		// 					result = false;
		//
		// 					Debug.Write("***");
		// 					// break;
		// 				}
		// 			}
		// 			// }
		// 			// else
		// 			// {
		// 			// 	failed = $"sheet number mis-match {s.SheetPdf.FileNameObject.SheetNumber} vs {s.SheetNumber}";
		// 			// 	result = false;
		// 			// }
		//
		// 			Debug.Write("\n");
		// 		}
		// 		else
		// 		{
		// 			failed = "file name mis-match";
		// 			result = false;
		// 		}
		//
		// 		Debug.Write($"^^ {s.SampleSheet,-40} - ");
		//
		// 		if (result==true)
		// 		{
		// 			Debug.WriteLine("\nMATCH");
		// 		}
		// 		else
		// 		{
		// 			Debug.WriteLine($"\nFAILED [{failed}]");
		// 		}
		// 	}
		// }

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
			return $"this is {nameof(SheetPdfManager)}";
		}

	#endregion
	}
}