﻿#region using

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using static AndyShared.ClassificationDataSupport.TreeSupport.CompareOperations;
using static AndyShared.ClassificationDataSupport.TreeSupport.ComparisonOp;

#endregion

// ReSharper disable CommentTypo

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
// ReSharper restore CommentTypo

namespace AndyShared.ClassificationDataSupport.TreeSupport
{
//	public class MasterRow
//	{
//		public int MasterRowIdx { get; set; } = 0;
//	}


	[DataContract(Name = "SheetCategoryDescription", Namespace = "", IsReference = true)]
	[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
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
		private ObservableCollection<ComparisonOperation> compareOps;

	#endregion

	#region ctor

		public SheetCategory(string title, string description)
		{
			this.title = title;
			this.description = description;

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
			// ReSharper disable once UnusedMember.Global
			get => depth;
			set
			{
				depth = value;

				// ReSharper disable once ExplicitCallerInfoArgument
				OnPropertyChange("ComponentName");
			}
		}


		// [IgnoreDataMember]
		// public string ComponentName
		// {
		// 	get { return FileNameSheetPdf.SheetNumberComponentTitles[Depth]; }
		// }


	#endregion

	#region private properties

	#endregion

	#region public methods

		// ReSharper disable once UnusedMember.Global
		public int FindCompOp(int findId)
		{
			for (var i = 0; i < compareOps.Count; i++)
			{
				if (compareOps[i].Id == findId) return i;
			}

			return -1;
		}

		// ReSharper disable once UnusedMember.Global
		public void RemoveCompOpAt(int idx)
		{
			compareOps.RemoveAt(idx);
			OnPropertyChange("CompareOps");
		}

		public void NotifyChange()
		{
			OnPropertyChange("Title");
			OnPropertyChange("Description");
			OnPropertyChange("Pattern");
		}

		public static SheetCategory TempSheetCategory()
		{
			SheetCategory temp = new SheetCategory($"{tempIdx++:D2} New Node Title", "New Node Description");
			temp.CompareOps.Add(new ValueCompOp(ValueCompareOps[(int) EQUALTO], "1", true));

			return temp;
		}

		#endregion

		#region private methods


		#endregion

		#region event processing

		// ReSharper disable once UnusedMember.Local
		// ReSharper disable once UnusedParameter.Local
		[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
		[SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
		private void OnIsDisabledChanged(object sender) => Debug.WriteLine("at event");

	#endregion

	#region event handeling


		public event PropertyChangedEventHandler PropertyChanged;

		// ReSharper disable once MemberCanBePrivate.Global
		protected void OnPropertyChange([CallerMemberName] string memberName = "") => 
			PropertyChanged?.Invoke(this,  new PropertyChangedEventArgs(memberName));

	#endregion

	#region system overrides


		public object Clone()
		{
			copyIdx++;

			string t = title + $" (copy {copyIdx})";
			string d = description + $" (copy {copyIdx})";

			SheetCategory clone = new SheetCategory(t, d) {depth = depth };

			foreach (ComparisonOperation compOp in compareOps)
			{
				if (compOp.GetType() == typeof(ValueCompOp))
				{
					clone.compareOps.Add((ValueCompOp) compOp.Clone());
				}
				else
				{
					clone.compareOps.Add((LogicalCompOp) compOp.Clone());
				}

			}

			return clone;
		}

		public override string ToString() => "this is SheetCategory| " + title;

	#endregion

	}


}