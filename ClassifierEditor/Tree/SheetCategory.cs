#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using ClassifierEditor.FilesSupport;

#endregion

// username: jeffs
// created:  5/2/2020 9:18:14 AM

/*
	notes:
	1. phase / building adds a description to the category but does not create a tree depth
	2. phase / building cannot be a condition
	3. the category list's top level starts with discipline

	adding conditions

	at treenode: 
	> replace pattern with a condition list
	> condition list has a maximum of 5 conditions
	> treenode depth defines to which sheet name component the conditions apply
		top = discipline / -1 = category / -2 = subcategory / -3 = modifier / -4 = submodifier
	> conditions
		all tests are case sensitive a != A
		{} = the value of the sheet component

		value conditions (vc)
			match conditions (mc)
				{} contains value, {} does not contain value, {} starts with value, {} does not start with value, {} ends with value, {} does not end with value

			comparison conditions (cc)
				{} == value, {} != value, {} > value, {} >= value, {} < value, {} <= value
		logical conditions (lc)
			condition AND condition, condition OR condition
	> condition list
		> condition, comparison value (for value conditions and for comparison values)
		> condition, null value (for logical conditions)
		> min list length = 1
		> list length will always be a odd number
		> ex lists: A) (vc); B) (vc) (lc) (vc)
	> list processing
		> evaluate resultA = (vc)
		> loop
		> has an (lc) no -> return resultA
		> evaluate resultB = next (vc)
		> evaluate resultA = resultA (lc) resultB
			
*/

namespace ClassifierEditor.Tree
{
	[DataContract(Name = "SheetCategoryDescription", Namespace = "", IsReference = true)]
	public class SheetCategory : INotifyPropertyChanged, ICloneable
	{
	#region private fields

		// static fields
		private static int tempIdx;
		private static int copyIdx;

		// not static fields
		private string title;
		private string description;
		private int depth;
		private Regex pattern;
		private ObservableCollection<ComparisonOperation> compareOps;

	#endregion

	#region ctor

		public SheetCategory(string title, string description, string pattern)
		{
//			this.keyCode = keyCode;
			this.title = title;
			this.description = description;
			this.pattern = pattern == null ? new Regex("") : new Regex(pattern);

			CompareOps = new ObservableCollection<ComparisonOperation>();
		}

	#endregion

	#region public properties

		[DataMember(Order = 1)]
		public string Title
		{
			get => title;

			set
			{
				title = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 2)]
		public string Description
		{
			get => description;

			set
			{
				description = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 3)]
		public string Pattern
		{
			get => pattern.ToString();

			set
			{
				try
				{
					pattern = new Regex(value);
				}
				catch 
				{
					pattern = new Regex("invalid");
				}
				OnPropertyChange();
			}
		}

		[DataMember(Order = 3)]
		public ObservableCollection<ComparisonOperation> CompareOps
		{
			get => compareOps;
			set
			{
				compareOps = value;
				OnPropertyChange();
			}
		}
		
		[IgnoreDataMember]
		public int Depth
		{
			get => depth;
			set
			{
				depth = value;

				OnPropertyChange("ComponentName");
			}
		}


		[IgnoreDataMember]
		public string ComponentName
		{
			get { return FileNameSheetPdf.SheetNumberComponentTitles[Depth]; }
		}





	#endregion

	#region private properties

	#endregion

	#region public methods

		public void NotifyChange()
		{
			OnPropertyChange("Title");
			OnPropertyChange("Description");
			OnPropertyChange("Pattern");
		}

		public static SheetCategory TempSheetCategory()
		{
			return new SheetCategory($"{tempIdx++:D2} New Node Title", "New Node Description", "");
		}

	#endregion

	#region private methods



	#endregion

	#region event processing

	#endregion

	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides


		public object Clone()
		{
			copyIdx++;

			string t = title + $" (copy {copyIdx})";
			string d = description + $" (copy {copyIdx})";
			string p = pattern.ToString();

			return new SheetCategory(t, d, p);
		}

		public override string ToString()
		{
			return "this is SheetCategory| " + title;
		}


	#endregion
	}
}