#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

#endregion

// username: jeffs
// created:  5/2/2020 9:18:14 AM

namespace ClassifierEditor.Tree
{
	[DataContract(Name = "SheetCategoryDescription", Namespace = "", IsReference = true)]
	public class SheetCategory : INotifyPropertyChanged, ICloneable
	{
	#region private fields

//		private string keyCode;
		private string title;
		private string description;
		private Regex pattern;

		private static int tempIdx;
		private static int copyIdx;

	#endregion

	#region ctor

		public SheetCategory(string title, string description, string pattern)
		{
//			this.keyCode = keyCode;
			this.title = title;
			this.description = description;
			this.pattern = pattern == null ? new Regex("") : new Regex(pattern);
		}

	#endregion

	#region public properties

//		[DataMember(Order = 1)]
//		public string KeyCode
//		{
//			get => keyCode;
//
//			set
//			{
//				keyCode = value;
//				OnPropertyChange();
//			}
//		}

		[DataMember(Order = 2)]
		public string Title
		{
			get => title;

			set
			{
				title = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 3)]
		public string Description
		{
			get => description;

			set
			{
				description = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 4)]
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