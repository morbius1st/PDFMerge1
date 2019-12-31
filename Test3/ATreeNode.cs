#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion


// projname: Test3
// itemname: ATreeNode
// username: jeffs
// created:  12/23/2019 11:16:47 PM


namespace Test3
{
	public abstract class ATreeNode : ITreeNode, IComparable<ATreeNode> , IEquatable<ATreeNode>, INotifyPropertyChanged
	{

	#region common fields

		private string name;
		protected int id;
		protected int depth;
		protected bool isExpanded;
		protected bool informedOfMixedState;
		protected CheckedState checkedState = CheckedState.UNCHECKED;
		protected CheckedState triState = CheckedState.UNSET;
		protected ATreeNode parent;

	#endregion

	#region ctor

		public void Configure(ATreeNode parent, string name,
			int depth, int id, bool isExpanded)
		{
			this.Name = name;
			this.parent = parent;
			this.depth = depth;
			this.id = id;

			IsExpanded = isExpanded;

			ConfigureNodeType();
		}

	#endregion

	#region properties

		public string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChange();
			}
		}

		public string SortCode { get; set; }

		public int Id => id;
		public int Depth => depth;

		public NodeType NodeType { get; protected set; }

		public CheckedState CheckedState => checkedState;

		public CheckedState ChkState
		{
			set
			{
				if (value == CheckedState.UNSET)
				{
					checkedState = value;
				}
				else if (value != checkedState)
				{
					checkedState = value;
					OnPropertyChange("SelectState");
					OnPropertyChange("Checked");
				}
			}
		}

		public CheckedState TriState
		{
			get => triState;
			set
			{
				if (triState == value) return;
				triState = value;

				OnPropertyChange();
			}
		}

		public bool? Checked
		{
			get => checkedState.AsBool;
			set
			{
				ProcessStateChange(CheckedState.Parse(value));

				OnPropertyChange();
			}
		}

		public bool IsExpanded
		{
			get => isExpanded;
			set
			{
				if (isExpanded == value) return;

				isExpanded = value;
				OnPropertyChange();
			}
		}

		public abstract bool IsBranch { get; }
		public abstract bool IsLeaf { get; }

		public ATreeNode Parent => parent;

	#endregion

	#region public methods

		public abstract void StateChangeFromParent(CheckedState newState, 
			CheckedState oldState, bool useTristate);

		public void TriStateReset()
		{
			TriState = CheckedState.UNSET;
		}

		public void Reset()
		{
			ChkState = CheckedState.UNCHECKED;
			TriState = CheckedState.UNSET;

		}

	#if DEBUG
		public abstract string MakeName();
	#endif

	#endregion

	#region protected methods

		protected abstract void ConfigureNodeType();

		protected abstract void NotifyParentOfStateChange(CheckedState newState, 
			CheckedState oldState, bool useTriState);

		protected abstract void ProcessStateChange(CheckedState newState);

	#endregion

	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public bool Equals(ATreeNode other)
		{
			return this.CheckedState.Equals(other.CheckedState);
		}

		public int CompareTo(ATreeNode other)
		{
			return this.CheckedState.AsInt.CompareTo(other.CheckedState.AsInt);
		}

		public override string ToString()
		{
			return name;
		}

	#endregion
	}
}