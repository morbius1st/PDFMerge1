using System;

namespace AndyShared.ClassificationDataSupport.TreeSupport
{
	public interface ITreeNodeItem : ICloneable
	{
		bool IsInitialized { get; set; }

		bool ShtCatModified { get; set; }

		bool CannotSelect { get; set; }

		int Depth { get; set; }

		int MergeItemCount { get; }

		void UpdateProperties();

		new object Clone();

	}
}


