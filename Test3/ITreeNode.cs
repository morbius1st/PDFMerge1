#region + Using Directives

#endregion


// projname: Test3
// itemname: ITreeNode
// username: jeffs
// created:  12/23/2019 11:17:47 PM


namespace Test3
{
	public interface ITreeNode
	{

	#region properties

		string Name { get; set; }

		string SortCode { get; set; }
		
		int Id { get; }
		int Depth { get; }


		NodeType NodeType { get; }

		CheckedState CheckedState { get; }
		CheckedState ChkState { set; }
		CheckedState TriState { get;  set; }

		bool? Checked { get; set; }
		bool IsExpanded { get; set; }

	#endregion

	#region methods

		void StateChangeFromParent(CheckedState newState, CheckedState oldState, bool useTristate);

		void Reset();

		void TriStateReset();

	#endregion

	#region system overrides

		string ToString();

	#endregion

	}
}
