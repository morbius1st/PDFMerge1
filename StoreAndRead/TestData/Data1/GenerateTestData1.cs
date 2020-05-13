#region using directives

using System.Collections.ObjectModel;

#endregion

// projname: StoreAndRead
// itemname: SampleData
// username: jeffs
// created:  4/11/2020 10:14:11 AM

namespace StoreAndRead.TestData.Data1
{
	class GenerateTestData1
	{
	#region private fields

		private string dataName;

	#endregion

	#region ctor

		public GenerateTestData1(string dataName)
		{
			this.dataName = dataName;
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public ObservableCollection<TestItem> LoadSampleData()
		{
			ObservableCollection<TestItem> DataRoot1 = new ObservableCollection<TestItem>();

//			TestItem SubSubBranch;
//			TestItem subBranch;
//			TestItem branch;

			// leaf 1
			DataRoot1.Add(MakeItem(1.0, "leaf"));

			// branch 2
			DataRoot1.Add(SubLeaves(2.0, 3, 0.1, 0));

			// leaf 3
			DataRoot1.Add(MakeItem(3.0, "leaf"));

			// branch 4
			DataRoot1.Add(SubLeaves(4.0, 3, 0.1, 2));

			return DataRoot1;
		}

	#endregion

	#region private methods

		private TestItem SubLeaves(double branchId, int qty, double increment, int depth)
		{
			ObservableCollection<TestItem> branch;

			TestItem item;

			double leafId = branchId;

			branch = new ObservableCollection<TestItem>();

			for (int i = 0; i < qty; i++)
			{
				leafId += increment;

				if (i == 1 && depth > 0)
				{
					item = SubLeaves(leafId, qty, increment / 10, depth - 1);
				}
				else
				{
					item = MakeItem(leafId, "leaf");
				}

				branch.Add(item);
			}

			// branch 2
			item = new TestItem($"branch{dataName} {branchId:N}", $"branch{dataName} {branchId:N} data");

			item.Leaves = branch;

			return item;
		}

		private TestItem MakeItem(double leafId, string name)
		{
			return new TestItem($"{name}{dataName} {leafId:G}", $"{name}{dataName} {leafId:G} data");
		}

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleData";
		}

	#endregion
	}
}