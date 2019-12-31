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

	#region preface

		public struct PartIndex : IComparable<PartIndex>
		{

		#region ctor

			public PartIndex(int order, 
				string libraryId
				)
			{
				Order = order;

				LibraryId = libraryId;
			}

		#endregion

		#region public properties

			public int Order { get; private set; }
			public string LibraryId { get; private set; }

		#endregion

		#region public methods

			public int CompareTo(PartIndex other)
			{
				return Order.CompareTo(other.Order);
			}

		#endregion
		}

	#endregion

	#region ctor

		public SheetPartsDescriptor(string description)
		{
			Description = description;

			SheetNumberPartIndicies = new List<PartIndex>();

			SheetNamePartIndicies = new List<PartIndex>();
		}

	#endregion

	#region public properties

		public string Description { get; private set; }
		public List<PartIndex> SheetNumberPartIndicies { get; set; }
		public List<PartIndex> SheetNamePartIndicies { get; private set; }

	#endregion

	#region public properties

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

	#endregion

	}
}