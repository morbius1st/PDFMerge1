// Solution:     PDFMerge1
// Project:       Test3
// File:             SheetPartData.cs
// Created:      -- ()

using System;
using Test3.SheetMgr;

namespace Test3 {

	// held by SheetPartsDescriptor
	public class SheetPartData : IComparable<SheetPartData>
	{
		public string LibraryId        { get; private set; }
		public string PartDescription  { get; private set; }
		public string PartPattern      { get; private set; }
		public int PartPatternGroup    { get; private set; }
		public int PartMinLen          { get; private set; } = 1;
		public int PartMaxLen          { get; private set; } = -1;
		public string PartFormat       { get; private set; }
		public PartIdType PartIdType   { get; private set; }
		public int PartIdOrder         { get; private set; }

		public SheetPartData(string libraryId, 
			PartIdType partIdType,
			int partIdOrder,
			string partDescription,
			string partPattern,
			int partPatternGroup,
			int partMinLen,
			int partMaxLen,
			string partFormat)
		{
			LibraryId         = libraryId;
			PartIdType        = partIdType;
			PartIdOrder       = partIdOrder;
			PartDescription   = partDescription;
			PartPattern       = partPattern;
			PartPatternGroup  = partPatternGroup;
			PartMinLen        = partMinLen;
			PartMaxLen        = partMaxLen;
			PartFormat        = partFormat;
		}

		public SheetPartData() { }

		public bool IsValid			=> PartMaxLen >= 0;
		public string FormatString	=> PartFormat;

		public int CompareTo(SheetPartData other)
		{
			return PartIdOrder.CompareTo(other.PartIdOrder);
		}

		public override string ToString()
		{
			return PartDescription;
		}
	}
}