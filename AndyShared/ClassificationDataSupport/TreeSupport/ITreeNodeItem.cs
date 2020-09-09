using System;
using System.Collections.Generic;
using System.Text;

namespace AndyShared.ClassificationDataSupport.TreeSupport
{
	public interface ITreeNodeItem : ICloneable
	{
		bool IsInitialized { get; set; }

		bool IsModified { get; set; }

		bool CanSelect { get; set; }

		int Depth { get; set; }

		void UpdateProperties();

		new object Clone();

	}
}
