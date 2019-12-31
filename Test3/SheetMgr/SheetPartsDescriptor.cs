// Solution:     PDFMerge1
// Project:       Test3
// File:             SheetPartsDescriptor.cs
// Created:      -- ()

using System;
using System.Collections.Generic;
using Test3.SheetMgr;

namespace Test3
{
	public class SheetPartsDescriptor
	{
		public struct PartIndex : IComparable<PartIndex>
		{
			public int Order { get; private set; }
			public int SheetPartDataIndex { get; private set; }

			public PartIndex(int order, int sheetPartDataIndex)
			{
				Order = order;
				SheetPartDataIndex = sheetPartDataIndex;
			}

			public int CompareTo(PartIndex other)
			{
				return Order.CompareTo(other.Order);
			}
		}

		public string Description { get; private set; }
		public List<PartIndex> SheetNumberPartIndicies { get; set; }
		public List<PartIndex> SheetNamePartIndicies { get; private set; }

		public SheetPartsDescriptor(string description)
		{
			Description = description;

			SheetNumberPartIndicies = new List<PartIndex>();

			SheetNamePartIndicies = new List<PartIndex>();
		}

		public void AddPartIndex(PartIndex idx, PartIdType partIdType)
		{
			if (partIdType == PartIdType.SHEETNUMBER)
			{
				SheetNumberPartIndicies.Add(idx);
			}
			else
			{
				SheetNamePartIndicies.Add(idx);
			}
		}

		public void SortIndicies()
		{
			SheetNumberPartIndicies.Sort();
			SheetNamePartIndicies.Sort();
		}

	}
}