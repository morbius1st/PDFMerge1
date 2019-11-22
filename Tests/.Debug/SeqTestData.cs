#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Tests.Sequence;

#endregion


// projname: Tests.Debug
// itemname: SeqTestData
// username: jeffs
// created:  11/8/2019 6:28:50 PM


namespace Tests.Debug
{
	public class SeqTestData : INotifyPropertyChanged
	{
		private static SeqTestData _instance;

		private SeqTestData ()
		{
			si = SeqItems.Instance;
			spl = SeqPartsList.Instance;
//			sd = SeqDisciplines.Instance;

			SeqItemsTestData();

		}

		public static SeqTestData Instance
		{
			get
			{
				if (_instance == null) _instance = new SeqTestData();

				return _instance;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		// identifiers database
		public SeqItems si { get; set; } 
		public SeqPartsList spl { get; set; } 
//		public SeqDisciplines sd { get; set; }

		
		public const string ARCH = "Architectural";
		public const string CIVIL = "Civil";
		public const string STRUCT = "Structural";
		public const string MECH = "Mechanical";
		public const string PLUMB = "Plumbing";
		public const string ELEC = "Electrical";
		public const string GREEN = "CalGreen";



		// set up the item types
		public void SeqItemsTestData()
		{
//			SeqDiscipline sdx;
//
//			sd.Add(new SeqDiscipline("06a"  , "Life/Safety"      , "Life/Safety Sheets"));
//			sd.Add(new SeqDiscipline("07a"  , "Architectural"    , "Architectural Sheets"));
//			sd.Add(new SeqDiscipline("00a"  , "Cover Sheet"      , "Cover Sheet"));
//			sd.Add(new SeqDiscipline("00b"  , "General"          , "General Sheets"));
//			sd.Add(new SeqDiscipline("00c"  , "Title"            , "Title Sheets"));

			si.Clear();
			spl.Clear();

			SeqParts sp = new SeqParts(ARCH);
			//                     code     title                description
// primary categories
			sp.AddPart(new SeqPart("06a"  , "Life/Safety"      , "Life/Safety Sheets"));
			sp.AddPart(new SeqPart("07a"  , "Architectural"    , "Architectural Sheets"));
			sp.AddPart(new SeqPart("00a"  , "Cover Sheet"      , "Cover Sheet"));
			sp.AddPart(new SeqPart("00b"  , "General"          , "General Sheets"));
			sp.AddPart(new SeqPart("00c"  , "Title"            , "Title Sheets"));
// secondary categories
			sp.AddPart(new SeqPart("000a" , "Schedules"        , "Schedule Sheets"));
			sp.AddPart(new SeqPart("001b" , "Title"            , "Title Sheets"));
			sp.AddPart(new SeqPart("000b" , "Assembly"         , "Assembly Sheets"));
			sp.AddPart(new SeqPart("001a" , "Site"             , "Site Sheets"));
			sp.AddPart(new SeqPart("002a" , "Plan"             , "Plan Sheets"));
			sp.AddPart(new SeqPart("003a" , "Elev. & Sections" , "Vertical View Sheets"));
			sp.AddPart(new SeqPart("004a" , "Enlarged Plan"    , "Enlarged Sheets"));
			sp.AddPart(new SeqPart("005a" , "Vert. Circulation", "Vertical Circulation Sheets"));
			sp.AddPart(new SeqPart("006a" , "Schedules"        , "Schedule Sheets"));
// tertiary categories
			sp.AddPart(new SeqPart("0001b", "Accessibility"    , "Accessibility Sheets"));
			sp.AddPart(new SeqPart("0020a", "Slab Plan"        , "Plan Sheets"));
			sp.AddPart(new SeqPart("0021a", "Floor Plan"       , "Plan Sheets"));
			sp.AddPart(new SeqPart("0022a", "Roof Plan"        , "Plan Sheets"));
			sp.AddPart(new SeqPart("0023a", "Ref Clg Plan"     , "Plan Sheets"));
			sp.AddPart(new SeqPart("0031a", "Elevations"       , "Elevation Sheets"));
			sp.AddPart(new SeqPart("0032a", "Bldg Sections"    , "Building Section Sheets"));
			sp.AddPart(new SeqPart("0033a", "Wall Sections"    , "Wall Section Sheets"));
			sp.AddPart(new SeqPart("0051a", "Stairs"           , "Stair Sheets"));
			sp.AddPart(new SeqPart("0052a", "Elevators"        , "Elevator Sheets"));
			sp.AddPart(new SeqPart("0053a", "Escalators"       , "Elevator Sheets"));

			spl.Add(sp);


			sp = new SeqParts(CIVIL);
// primary categories
			sp.AddPart(new SeqPart("01a"  , "Civil", "Civil Sheets"));

			spl.Add(sp);


			sp = new SeqParts(STRUCT);
// primary categories
			sp.AddPart(new SeqPart("11a"  , "Structural"     , "Structural Sheets"));
// secondary categories			
			sp.AddPart(new SeqPart("000c" , "General"        , "General Sheets"));
			sp.AddPart(new SeqPart("000a" , "Schedules"      , "Schedule Sheets"));
			sp.AddPart(new SeqPart("000d" , "Typical Details", "Typical Detail Sheets"));
			sp.AddPart(new SeqPart("002a" , "Plan"           , "Plan Sheets"));
// tertiary categories
			sp.AddPart(new SeqPart("0020a", "Slab Plan"      , "Plan Sheets"));
			sp.AddPart(new SeqPart("0021a", "Floor Plan"     , "Plan Sheets"));
			sp.AddPart(new SeqPart("0022a", "Roof Plan"      , "Plan Sheets"));

			spl.Add(sp);


			sp = new SeqParts(MECH);
// primary categories
			sp.AddPart(new SeqPart("13a"  , "Mechanical"  , "Mechanical Sheets"));	
			// secondary categories				
			sp.AddPart(new SeqPart("000a" , "Schedules"   , "Schedule Sheets"));
			sp.AddPart(new SeqPart("001a" , "Site"        , "Site Sheets"));
			sp.AddPart(new SeqPart("002a" , "Plan"        , "Plan Sheets"));
			// tertiary categories
			sp.AddPart(new SeqPart("0021a", "Floor Plan"  , "Plan Sheets"));
			sp.AddPart(new SeqPart("0022a", "Roof Plan"   , "Plan Sheets"));
			sp.AddPart(new SeqPart("0023a", "Ref Clg Plan", "Plan Sheets"));

			spl.Add(sp);


			sp = new SeqParts(PLUMB);
// primary categories
			sp.AddPart(new SeqPart("15a"  , "Plumbing"    , "Plumbing Sheets"));
			// secondary categories				
			sp.AddPart(new SeqPart("000a" , "Schedules"   , "Schedule Sheets"));
			sp.AddPart(new SeqPart("001a" , "Site"        , "Site Sheets"));
			sp.AddPart(new SeqPart("002a" , "Plan"        , "Plan Sheets"));
			// tertiary categories
			sp.AddPart(new SeqPart("0021a", "Floor Plan"  , "Plan Sheets"));
			sp.AddPart(new SeqPart("0022a", "Roof Plan"   , "Plan Sheets"));
			sp.AddPart(new SeqPart("0023a", "Ref Clg Plan", "Plan Sheets"));

			spl.Add(sp);


			sp = new SeqParts(ELEC);
// primary categories
			sp.AddPart(new SeqPart("17a"  , "Electrical"  , "Electrical Sheets"));
			// secondary categories				
			sp.AddPart(new SeqPart("000a" , "Schedules"   , "Schedule Sheets"));
			sp.AddPart(new SeqPart("001a" , "Site"        , "Site Sheets"));
			sp.AddPart(new SeqPart("002a" , "Plan"        , "Plan Sheets"));
			// tertiary categories
			sp.AddPart(new SeqPart("0021a", "Floor Plan"  , "Plan Sheets"));
			sp.AddPart(new SeqPart("0022a", "Roof Plan"   , "Plan Sheets"));
			sp.AddPart(new SeqPart("0023a", "Ref Clg Plan", "Plan Sheets"));

			spl.Add(sp);

			
			sp = new SeqParts(GREEN);
// primary categories
			sp.AddPart(new SeqPart("97a"  , "CalGreen", "CalGreen Sheets"));			
			sp.AddPart(new SeqPart("000c" , "General" , "General Sheets"));

			spl.Add(sp);


			// architectural items
			// a seqitem for: architectural cover sheet
			si.AddItem(MakeItem("Architectural Cover Sheet", ARCH, "00a"));

			// a seqitem for: architectural general / title sheet
			si.AddItem(MakeItem("Architectural General Title Sheet", ARCH, "00b", "001b"));

			// a seqitem for: architectural title / title sheets / accessibility
			si.AddItem(MakeItem("Architectural Title Sheet", ARCH, "00c", "001b", "0001b"));

			// a seqitem with architectural / assembly
			si.AddItem(MakeItem("Architectural Assembly Sheet", ARCH, "07a", "00b"));	

			// a seqitem for: Architectural / plan
			si.AddItem(MakeItem("Architectural Plan Sheet", ARCH, "07a", "002a"));

			// a seqitem for: Architectural / plan / floor plan
			si.AddItem(MakeItem("Architectural Floor Plan Sheet", ARCH, "07a", "002a", "0021a"));


			// structural items


			// a seqitem for: structural / schedules
			si.AddItem(MakeItem("Structural Schedule Sheet", STRUCT, "11a", "000a"));

			// a seqitem for: structural / typical details
			si.AddItem(MakeItem("Structural Standard Details Sheet", STRUCT, "11a", "000d"));

			// a seqitem for: structural / plans / floor plan
			si.AddItem(MakeItem("Structural Floor Plan Sheet", STRUCT, "11a", "002a", "0021a"));


			// civil items
			// a seqitem for: civil
			si.AddItem(MakeItem("Civil Sheets", CIVIL, "01a"));


			// mechanical items
			// a seqitem for: mechanical
			si.AddItem(MakeItem("Mechanical Schedule Sheet", MECH, "13a", "000a"));
			
			// a seqitem for: mechanical / plan / floor plan
			si.AddItem(MakeItem("Mechanical Floor Plan Sheet", MECH, "13a", "002a", "0021a"));


			// plumbing items
			// a seqitem for: plumbing
			si.AddItem(MakeItem("Plumbing Schedule Sheet", PLUMB, "15a", "000a"));
			
			// a seqitem for: plumbing / plan / floor plan
			si.AddItem(MakeItem("Plumbing Floor Plan", PLUMB, "15a", "002a", "0021a"));
			
		}

		private SeqItem MakeItem( string description, string tableName, params string[] partNames)
		{
			SeqParts spx = spl.PartsList[tableName];

			SeqItem sx = new SeqItem("");

			foreach (string name in partNames)
			{
				sx.Parts.Add(spx.Parts[name]);
			}

			return sx;
		}



	}
}
