using System;

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


