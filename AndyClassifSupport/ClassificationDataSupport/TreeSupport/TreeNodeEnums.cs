// Solution:     PDFMerge1
// Project:       Test4
// File:             TreeNodeEnums.cs
// Created:      2024-09-26 (6:51 PM)

namespace AndyShared.ClassificationDataSupport.TreeSupport
{
	public enum NodePlacement
	{
		BEFORE = -1,
		AFTER = 1
	}

	public enum SelectMode
	{
		TWO_STATE = 2,
		TRI_STATE = 3
	}

	public enum SelectState
	{
		UNSET = -1,
		UNCHECKED = 0,
		CHECKED = 1,
		MIXED = 2
	}

	public enum NodeType
	{
		ROOT,
		BRANCH,
		LEAF
	}

	public enum CheckedState
	{
		UNSET = -1,
		MIXED = 0,
		CHECKED = 1,
		UNCHECKED = 2
	}
}