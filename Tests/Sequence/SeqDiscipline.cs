#region + Using Directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

#endregion


// projname: Tests.Sequence
// itemname: SeqDiscipline
// username: jeffs
// created:  11/11/2019 6:46:25 PM


namespace Tests.Sequence
{

	public class SeqDisciplines
	{
		private const string VUE_NAME = "DisciplineVue";
		public ICollectionView DisciplineVue { get; private set; }

		private static SeqDisciplines _instance;

		public static SeqDisciplines Instance
		{
			get
			{
				if (_instance == null) _instance = new SeqDisciplines();

				return _instance;
			}
		}

		private static List<SeqDiscipline> Disciplines;

		private SeqDisciplines()
		{
			Disciplines = new List<SeqDiscipline>(4);

			Disciplines.Add(new SeqDiscipline("06a"  , "Life/Safety"      , "Life/Safety Sheets"));
			Disciplines.Add(new SeqDiscipline("07a"  , "Architectural"    , "Architectural Sheets"));
			Disciplines.Add(new SeqDiscipline("00a"  , "Cover Sheet"      , "Cover Sheet"));
			Disciplines.Add(new SeqDiscipline("00b"  , "General"          , "General Sheets"));
			Disciplines.Add(new SeqDiscipline("00c"  , "Title"            , "Title Sheets"));

			DisciplineVue = CollectionViewSource.GetDefaultView(Disciplines);

			Sort();

			OnPropertyChange(VUE_NAME);
		}

		public void Add(SeqDiscipline item)
		{
			if (item == null ) throw new NoNullAllowedException();
			if (Disciplines.Contains(item)) return;

			Disciplines.Add(item);

			OnPropertyChange(VUE_NAME);
		}

		public void Sort()
		{
			DisciplineVue.SortDescriptions.Add(
				new SortDescription("Code", ListSortDirection.Ascending));

			OnPropertyChange(VUE_NAME);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}


	public class SeqDiscipline : SeqPart
	{
		public SeqDiscipline(string code, string title, string description) : base(code, title, description)
		{

		}


//		private string discipline;
//
//		public SeqDiscipline(string discipline)
//		{
//			this.discipline = discipline ?? throw new NoNullAllowedException();
//		}
//
//		public string Discipline
//		{
//			get => discipline;
//			set
//			{
//				if (value != null)
//				{
//					discipline = value;
//					OnPropertyChange();
//				}
//			}
//		}
//
//		public event PropertyChangedEventHandler PropertyChanged;
//
//		private void OnPropertyChange([CallerMemberName] string memberName = "")
//		{
//			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
//		}
		
	}
}
