#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

#endregion

// username: jeffs
// created:  5/2/2020 9:18:14 AM

namespace ClassifierEditor.NumberComponent
{
	public class NumberComponentItem : INotifyPropertyChanged
	{
	#region private fields

		private string keyCode;
		private string title;
		private string description;
		private Regex pattern;

	#endregion

	#region ctor

		public NumberComponentItem(string keyCode, string title, string description, string pattern)
		{
			this.keyCode = keyCode;
			this.title = title;
			this.description = description;
			this.pattern = pattern == null ? null : new Regex(pattern);
		}

	#endregion

	#region public properties

		public string KeyCode
		{
			get => keyCode;

			set
			{
				keyCode = value;
				OnPropertyChange();
			}
		}

		public string Title
		{
			get => title;

			set
			{
				title = value;
				OnPropertyChange();
			}
		}

		public string Description
		{
			get => description;

			set
			{
				description = value;
				OnPropertyChange();
			}
		}

		public string Pattern
		{
			get => pattern.ToString();

			set
			{
				pattern = new Regex(value);
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void NotifyChange()
		{
			OnPropertyChange("keyCode");
			OnPropertyChange("Title");
			OnPropertyChange("Description");
			OnPropertyChange("Pattern");
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

		public override string ToString()
		{
			return "this is NumberComponentItem";
		}

	#endregion
	}
}