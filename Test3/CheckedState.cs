#region + Using Directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static Test3.CheckedState;

#endregion


// projname: Test3
// itemname: CheckedState
// username: jeffs
// created:  12/23/2019 11:22:33 PM


namespace Test3
{
	public class CheckedState
	{
	#region preface

		public enum Checked
		{
			MIXED = 0,
			CHECKED = 1,
			UNCHECKED = 2,
			UNSET = 3
		}

	#endregion

	#region fields

		private static CheckedState[] StateList = new CheckedState[4];

		private Checked state;
		private bool? asBool;
		private int asInt;

	#endregion

	#region ctor

		static CheckedState()
		{
			StateList[(int) Checked.MIXED] = new CheckedState(Checked.MIXED, null, (int) Checked.MIXED);
			StateList[(int) Checked.CHECKED] = new CheckedState(Checked.CHECKED, true, (int) Checked.CHECKED);
			StateList[(int) Checked.UNCHECKED] = new CheckedState(Checked.UNCHECKED, false, (int) Checked.UNCHECKED);
			StateList[(int) Checked.UNSET] = new CheckedState(Checked.UNSET, null, (int) Checked.UNSET);
		}

		private CheckedState(Checked c, bool? b, int i)
		{
			state = c;
			asBool = b;
			asInt = i;
		}

	#endregion

	#region public properties

		public static CheckedState MIXED => StateList[(int) Checked.MIXED];
		public bool IsMixed => this == MIXED;

		public static CheckedState CHECKED => StateList[(int) Checked.CHECKED];
		public bool IsChecked => this == CHECKED;

		public static CheckedState UNCHECKED => StateList[(int) Checked.UNCHECKED];
		public bool IsUchecked => this == UNCHECKED;

		public static CheckedState UNSET => StateList[(int) Checked.UNSET];
		public bool IsUnset => this == UNSET;

		public Checked State => state;

		public bool? AsBool
		{
			get
			{
				if (state == Checked.UNSET) throw new InvalidOperationException();
				return asBool;
			}
		}

		public int AsInt => asInt;

		public CheckedState Next => next(this);

	#endregion

	#region public methods

		public static CheckedState Parse(bool? b)
		{
			for (int i = 0; i < StateList.Length - 1; i++)
			{
				if (StateList[i].asBool == b) return StateList[i];
			}

			return UNSET;
		}

		public static CheckedState Parse(int i)
		{
			if (i > (int) Checked.UNSET || i < (int) Checked.MIXED) throw new ArgumentOutOfRangeException();

			return StateList[i];
		}

	#endregion

	#region private methods

		private static CheckedState next(CheckedState c)
		{
			int a = c.asInt + 1;

			return StateList[a > 2 ? 0 : a];
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return this.state.ToString();
		}

	#endregion

	}
}