// Solution:     PDFMerge1
// Project:       Sylvester
// File:             SheetIdBase.cs
// Created:      -- ()

using System;

namespace Sylvester.FileSupport {
	public class BaseFile : SheetId, ICloneable
	{
		public BaseFile() { }


		public object Clone()
		{
			return this.Clone<BaseFile>();
		}

		public override void UpdateSelectStatus() { }
	}
}