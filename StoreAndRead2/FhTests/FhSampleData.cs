#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SettingsManager;
using StoreAndRead.Annotations;
using StoreAndRead2.FavHistoryAdmin;
using UtilityLibrary;

#endregion


// projname: $projectname$
// itemname: FhSampleData
// username: jeffs
// created:  2/10/2021 10:16:00 PM

namespace StoreAndRead2.FhTests
{
	public class FhSampleData : INotifyPropertyChanged
	{
	#region private fields

		private const string JOB_NUMBER = "2099-999";


		private static readonly Lazy<FhSampleData> instance =
			new Lazy<FhSampleData>(() => new FhSampleData());

		private FhManager fhMgr;

		private FhAdministrator<FhClassfValue> favs;
		private FhAdministrator<FhPairValue> favPairs;
		private FhAdministrator<FhClassfValue> history;
		private FhAdministrator<FhPairValue> histPairs;


		private bool initialized;


		private static string[,] favFilenamelist = new string[,]
		{
			{"(jeffs) PdfSample 1",   "Classification File 1",   "Icon fav 1" },
			{"(jeffs) PdfSample 1A",  "Classification File 1A",  "Icon 1A" },
			{"(jeffs) PdfSample 2",   "Classification File 2",   "Icon 2" },
			{"(jeffs) PdfSample 3",   "Classification File 3",   "Icon 3" },
			{"(jeffs) PdfSample 901", "Classification File 901", "Icon 901" },
		};

		private static string[,] smplFilenamelist = new string[,]
		{
			// [0] filename   [1] display name    [2] icon name
			{"PdfSample A",   "Sample file A",    "Icon fav A" },
			{"PdfSample A1",  "Sample file A1",   "Icon fav A1" },
			{"PdfSample B",   "Sample file B",    "Icon fav B" },
			{"PdfSample C",   "Sample file C",    "Icon fav C" },
		};

	#endregion

	#region ctor

		private FhSampleData() { }

	#endregion

	#region public properties

		public static FhSampleData Instance => instance.Value;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void TestMakeSampleData1()
		{

			Initialize();

			fhMgr.Create();


			SampleDataFavs();
			SampleDataFavPairs();
			SampleDataHist();
			SampleDataHistPairs();
		}
		
		public void TestMakeSampleData2()
		{
			Initialize();

			fhMgr.Read();

			Debug.WriteLine("test read done");

		}


		public void Initialize()
		{
			fhMgr = FhManager.Instance;

			favs = fhMgr.Favorites;
			favPairs = fhMgr.FavoritePairs;
			history = fhMgr.History;
			histPairs = fhMgr.HistoryPairs;

			initialized = true;
		}

		public void SampleDataFavs()
		{
			if (!initialized) return;

			int index = 1;
			string description;


			FilePath<FileNameSimple> path;

			FhKey key;
			FhClassfValue value;

			for (int i = 0; i < favFilenamelist.GetLength(0); i++)
			{
				key = FhKey.ClassfKey(JOB_NUMBER, favFilenamelist[1, 0]);
				path = new FilePath<FileNameSimple>(
					@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\" + favFilenamelist[i, 0]
					+ ".xml");
				description = "Favorite for| " + favFilenamelist[1, 1];
				value = new FhClassfValue(favFilenamelist[1, 1], description, favFilenamelist[i, 2], path );

				favs.Add(key, value);
			}
		}

		public void SampleDataFavPairs()
		{
			if (!initialized) return;

			int index = 1;
			string description;


			FilePath<FileNameSimple> pathClassf;
			FilePath<FileNameSimple> pathSmpl;

			FhKey key;
			FhPairValue value;

			for (int i = 0; i < smplFilenamelist.GetLength(0); i++)
			{
				key = FhKey.PairfKey(JOB_NUMBER,favFilenamelist[i, 0], smplFilenamelist[1, 0]);

				pathClassf = new FilePath<FileNameSimple>(
					@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\" + favFilenamelist[i, 0]
					+ ".xml");

				pathSmpl = new FilePath<FileNameSimple>(
					@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\Sample Files\"
					+ smplFilenamelist[i, 0]
					+ ".sample");


				description = "Favorite pair for| " + smplFilenamelist[1, 1];
				value = new FhPairValue(smplFilenamelist[1, 1], description,
					favFilenamelist[i, 2], pathClassf ,
					smplFilenamelist[i, 2], pathSmpl);

				favPairs.Add(key, value);
			}
		}
		
		public void SampleDataHist()
		{
			if (!initialized) return;

			int index = 1;
			string description;


			FilePath<FileNameSimple> path;

			FhKey key;
			FhClassfValue value;

			for (int i = 0; i < favFilenamelist.GetLength(0) / 2; i++)
			{
				key = FhKey.ClassfKey(JOB_NUMBER, favFilenamelist[1, 0]);
				path = new FilePath<FileNameSimple>(
					@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\" + favFilenamelist[i, 0]
					+ ".xml");
				description = "Favorite for| " + favFilenamelist[1, 1];
				value = new FhClassfValue(favFilenamelist[1, 1], description, favFilenamelist[i, 2], path );

				history.Add(key, value);
			}
		}

		public void SampleDataHistPairs()
		{
			if (!initialized) return;

			int index = 1;
			string description;

			FilePath<FileNameSimple> pathClassf;
			FilePath<FileNameSimple> pathSmpl;

			FhKey key;
			FhPairValue value;

			for (int i = 0; i < smplFilenamelist.GetLength(0) / 2; i++)
			{
				key = FhKey.PairfKey(JOB_NUMBER, favFilenamelist[1, 0], smplFilenamelist[1, 0]);

				pathClassf = new FilePath<FileNameSimple>(
					@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\" + favFilenamelist[i, 0]
					+ ".xml");

				pathSmpl = new FilePath<FileNameSimple>(
					@"C:\ProgramData\CyberStudio\Andy\User Classification Files\jeffs\Sample Files\"
					+ smplFilenamelist[i, 0]
					+ ".sample");


				description = "Favorite pair for| " + smplFilenamelist[1, 1];
				value = new FhPairValue(smplFilenamelist[1, 1], description,
					favFilenamelist[i, 2], pathClassf ,
					smplFilenamelist[i, 2], pathSmpl);

				histPairs.Add(key, value);
			}
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
			return "this is FhSampleData";
		}

	#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}