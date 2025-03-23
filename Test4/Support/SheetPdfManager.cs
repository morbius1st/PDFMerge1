#region using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

using System.Text;
using System.Threading.Tasks;
using AndyClassifSupport.Samples;
using AndyShared.ClassificationFileSupport;

using AndyShared.FileSupport.FileNameSheetPDF;

using AndyShared.MergeSupport;
using AndyShared.SampleFileSupport;

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

		// private Classify4 cf4;

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

		public bool ParseSheetNames4()
		{
			bool result = true;

			Samples3.MakeSamples();

			last =  Samples3.Sheets3.Count;
			end = last;
			start = 0;

			ConfigFileNameParser();

			for (var i = start; i < end; i++)
			{
				

				Samples3.Sheets3[i].SheetPdf =
					new FilePath<FileNameSheetPdf>(Samples3.Sheets3[i].FileName);

				result &= Samples3.Sheets3[i].SheetPdf.FileNameObject.IsParseGood;
			}

			return result;
		}

		public void ConfigFileNameParser()
		{
			FileNameSheetParser.Instance.Config();
			FileNameSheetParser.Instance.CreateSpecialDisciplines(SpecialDisciplines);
			FileNameSheetParser.Instance.CreateFileNamePattern();
		}

		public bool FilterSheetNames4(ClassificationFile cf, Classify cf4)
		{
			Samples3.MakeSamples();

			last =  Samples3.Sheets3.Count;
			end = last;
			start = 0;

			ConfigFileNameParser();

			CreateFilterList();

			if (cf4.Configure(cf.TreeBase, Samples3.fileList))
			{
				Debug.WriteLine("config worked");
			}
			else
			{
				Debug.WriteLine("config failed");

				return false;
			}

			showFileList(cf4.fileList);

			cf4.PreProcess();

			cf4.Process3();

			return true;
		}

		public void CreateFilterList()
		{
			Samples3.fileList = new SheetFileList();
			Samples3.fileList.Files = new ObservableCollection<FilePath<FileNameSheetPdf>>();

			for (var i = start; i < end; i++)
			{
				// FilePath<FileNameSheetPdf> x = new FilePath<FileNameSheetPdf>(Samples3.Sheets3[i].FileName);

				Samples3.fileList.Files.Add(new FilePath<FileNameSheetPdf>(Samples3.Sheets3[i].FileName));
			}
		}


		private void showFileList(SheetFileList fileList)
		{
			Debug.WriteLine($"\n\nfile list");

			foreach (FilePath<FileNameSheetPdf> file in fileList.Files)
			{
				Debug.WriteLine($"{file.FileNameObject.SheetNumber,-10} {file.FileNameObject.SheetName}");
			}
		}



		// version 2 + 3
		private void showShtNumber4(ShtNumber sn, List<string> sheetComps, int idx)
		{
			string compName;
			string nameObjStr = "";
			string sampStr = "";

			Debug.WriteLine(
				$"\t{$"index {idx}",-31}|  +10      0               1               2               3              4                5               6");
			Debug.WriteLine(
				               $"\t{" ", -31}|  pb/0    di/2    sp0/3   cat/4   sp1/5   scat/6  sp2/7   mod/8   sp3/9  smod/10  sp4/11  idf/12  sp5/13  sid/14");
			Debug.Write($"\t{$">{sn.OrigSheetNumber}<",-31}| ");

			foreach (KeyValuePair<int, FileNameSheetIdentifiers.ShtNumComps2> kvp in
					FileNameSheetIdentifiers.SheetNumComponentData)
			{
				if (kvp.Value.ItemClass == FileNameSheetIdentifiers.ShtIdClass2.SC_IGNORE) continue;

				nameObjStr = sn.ShtNumComps[kvp.Key] ?? "";

				if (kvp.Key > FileNameSheetIdentifiers.VI_SEP0 && nameObjStr.IsVoid())
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

				s = Samples3.Sheets3[i];

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

			for (j = 0; j < FileNameSheetIdentifiers.VI_SUBIDENTIFIER; j++)
			{
				if (FileNameSheetIdentifiers.SheetNumComponentData[j].ItemClass== FileNameSheetIdentifiers.ShtIdClass2.SC_COMP_ID_SEP || 
					FileNameSheetIdentifiers.SheetNumComponentData[j].ItemClass== FileNameSheetIdentifiers.ShtIdClass2.SC_IGNORE) continue;

				compName = FileNameSheetIdentifiers.SheetNumComponentData[j].Name;
				compValue = FileNameSheetParser.Instance.CompLengths[j].ToString();

				Debug.Write($"{compName} {$"({compValue})",-5}| ");
			}

			compName = FileNameSheetIdentifiers.SheetNumComponentData[j].Name;
			compValue = FileNameSheetParser.Instance.CompLengths[j].ToString();

			Debug.Write($"{compName} {$"({compValue})",-5}");
			
			Debug.Write("\n");

		}

		// version 3a
		private bool? showSheet4(SheetPdfSample s)
		{
			failed = "";
			result = true;

			if (!s.SheetPdf.FileNameObject.IsValid)
			{
				failed += "| file name is not valid";
				result = null;
			}

			if (!s.SheetPdf.FileNameObject.IsParseGood)
			{
				failed += "| parse is not valid";
				result = null;
			}

			if (!s.SheetPdf.FileName.Equals(s.FileName))
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
			Debug.Write($"{$"file name >{s.SheetPdf.FileName}<",-60}");
			Debug.Write($"{$"title >{s.SheetPdf.FileNameObject.SheetTitle}<",-60}");
			Debug.Write("\n");

			Debug.Write($"\t        actual| ");
			Debug.Write($"{$"is valid >{s.SheetPdf.IsValid}<",-18}");
			Debug.Write($"{$"status >{s.SheetPdf.FileNameObject.StatusCode}<",-30}");
			Debug.Write($"{$"full path >{s.SheetPdf.FullFilePath}<"}");
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
			ShtNumber sn = s.SheetPdf.FileNameObject.ShtNumberObj;

			showShtNumber4(sn, s.SheetComps, s.SheetPdf.FileNameObject.Index);

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