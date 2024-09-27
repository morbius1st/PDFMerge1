#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

using System.Text;
using System.Threading.Tasks;
using AndyShared.ClassificationFileSupport;
using AndyShared.ClassifySupport;
using AndyShared.FileSupport.FileNameSheetPDF;
using AndyShared.FileSupport.FileNameSheetPDF4;
using Test4.Support;
using UtilityLibrary;

#endregion


// projname: $projectname$
// itemname: SheetPdfManager
// username: jeffs
// created:  9/8/2024 5:56:30 PM

namespace Test4.SheetMgr
{
	public class SheetPdfManager
	{
	#region private fields

		bool? result;
		string failed = "";

		private Classify4 cf4;

		// private List<int> compLengths;

		// public static List<FilePath<FileNameSheetPdf2>> SheetFiles = new List<FilePath<FileNameSheetPdf2>>();

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

		// public List<int> CompLengths
		// {
		// 	get => compLengths;
		// 	set => compLengths = value;
		// }

	#endregion

	#region private properties

	#endregion

	#region public methods

		private int start = 0;
		private int last;
		private int end;

		public void ParseSheetNames4()
		{
			Test4.Support.Samples4.MakeSamples();

			last =  Samples4.Sheets.Count;
			end = last;
			start = 0;

			FileNameSheetParser3.Instance.Config();
			FileNameSheetParser3.Instance.CreateSpecialDisciplines(SpecialDisciplines);
			FileNameSheetParser3.Instance.CreateFileNamePattern();


			for (var i = start; i < end; i++)
			{
				Samples4.Sheets[i].SheetPdf3 =
					new FilePath<FileNameSheetPdf3>(Samples4.Sheets[i].FileName);
			}
		}


		public void FilterSheetNames4(ClassificationFile cf)
		{
			last =  Samples4.fileList.Files.Count;
			end = 10;
			start = 0;

			CreateFilterList();

			cf4 = new Classify4();

			cf4.Configure(cf.TreeBase, Samples4.fileList);

			cf4.PreProcess();

			cf4.Process3();
		}



		public void CreateFilterList()
		{
			Samples4.MakeSamples();

			for (var i = start; i < end; i++)
			{
				Samples4.fileList.Files.Add(new FilePath<FileNameSheetPdf4>(Samples4.Sheets[i].FileName));
			}

		}

		// version 2 + 3
		private void showShtNumber4(ShtNumber sn, List<string> sheetComps)
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
		public void ShowShtNameResults4()
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

				s = Samples4.Sheets[i];

				result = showSheet4(s);

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
				compValue = FileNameSheetParser3.Instance.CompLengths[j].ToString();

				Debug.Write($"{compName} {$"({compValue})",-5}| ");
			}

			compName = SheetIdentifiers3.SheetNumComponents[j].Name;
			compValue = FileNameSheetParser3.Instance.CompLengths[j].ToString();

			Debug.Write($"{compName} {$"({compValue})",-5}");
			
			Debug.Write("\n");

		}

		// version 3a
		private bool? showSheet4(SheetPdfSample s)
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

			bool? temp = showShtnumber4(s);

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
		private bool? showShtnumber4(SheetPdfSample s)
		{
			ShtNumber sn = s.SheetPdf3.FileNameObject.ShtNumber;

			showShtNumber4(sn, s.SheetComps);

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