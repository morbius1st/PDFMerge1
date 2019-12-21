using static Test3.MainWindow;
using System.Collections.ObjectModel;

// Solution:     PDFMerge1
// Project:       Test3
// File:             IEvt.cs
// Created:      -- ()

namespace Test3 {

	public abstract class Evt<T>
	{
		protected string name;
		protected SelectState selectState = SelectState.UNCHECKED;
		protected NodeType nodeType = NodeType.LEAF;

		public abstract void Configure(int depth, int parentBranchId, 
			int branchId, int LeafId, int uniqueId, bool asBranch = false);

		public ObservableCollection<T> childList { get; set; } =
			new ObservableCollection<T>();

		public NodeType NodeType { get; set; }

		public abstract void Reset();

	#if DEBUG

		public void AppendLine(string msg)
		{
			Instance.AppendLineTbk1(msg);
		}

		public void AppendMessage(string msg)
		{
			Instance.AppendMessageTbk1(msg);
		}

	#endif


	#region system overrides

		public override string ToString()
		{
			return nodeType + "::" + name + "::" + selectState;
		}

	#endregion

	#region Event Processing

		// event 2 - state change - notify parent
		public abstract void StateChangeFromChild(SelectState newState, SelectState fromState, bool useTriState);

		public delegate void StateChangeNotifyParent(SelectState newState, SelectState fromState, bool useTriState);
		public StateChangeNotifyParent OnStateChangeNotifyParentEvent { get; set; }

		public void NotifyParentOfStateChange(SelectState newState, SelectState fromState, bool useTriState)
		{
			if (OnStateChangeNotifyParentEvent != null)
				OnStateChangeNotifyParentEvent(newState, fromState, useTriState);
		}

		// event 2 - state change - notify child
		public abstract void StateChangeFromParent(SelectState toThis, bool useTriState);

		public delegate void StateChangedNotifyChild(SelectState newState, bool useTriState);
		public StateChangedNotifyChild OnStateChangeNotifyChildrenEvent { get; set; }


		public void NotifyChildrenOfStateChange(SelectState newState, bool useTriState)
		{
			if (OnStateChangeNotifyChildrenEvent != null) 
				OnStateChangeNotifyChildrenEvent(newState, useTriState);
		}


	#endregion

	}
}