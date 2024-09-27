namespace AndyShared.ClassificationDataSupport.TreeSupport
{
	public interface ITreeNodeItem : ICloneable
	{
		bool IsInitialized { get; set; }

		bool IsModified { get; set; }

		bool CanSelect { get; set; }

		int Depth { get; set; }

		int Count { get; }

		void UpdateProperties();

		new object Clone();

	}
}


