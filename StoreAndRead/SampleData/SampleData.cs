#region using directives

using System.Collections.ObjectModel;
using System.Windows.Input;

#endregion

// projname: $projectname$
// itemname: SampleData
// username: jeffs
// created:  4/11/2020 10:14:11 AM

namespace StoreAndRead.SampleData
{
	class SampleData
	{
	#region private fields

		private string dataName;

	#endregion

	#region ctor

		public SampleData(string dataName)
		{
			this.dataName = dataName;
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public ObservableCollection<SampleItem> LoadSampleData()
		{
			ObservableCollection<SampleItem> root = new ObservableCollection<SampleItem>();

//			SampleItem SubSubBranch;
//			SampleItem subBranch;
//			SampleItem branch;

			// leaf 1
			root.Add(MakeItem(1.0, "leaf"));

			// branch 2
			root.Add(SubLeaves(2.0, 3, 0.1, 0));

			// leaf 3
			root.Add(MakeItem(3.0, "leaf"));

			// branch 4
			root.Add(SubLeaves(4.0, 3, 0.1, 2));

			return root;
		}

	#endregion

	#region private methods

		private SampleItem SubLeaves(double branchId, int qty, double increment, int depth)
		{
			ObservableCollection<SampleItem> branch;

			SampleItem item;

			double leafId = branchId;

			branch = new ObservableCollection<SampleItem>();

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
			item = new SampleItem($"branch{dataName} {branchId:N}", $"branch{dataName} {branchId:N} data");

			item.Leaves = branch;

			return item;
		}

		private SampleItem MakeItem(double leafId, string name)
		{
			return new SampleItem($"{name}{dataName} {leafId:G}", $"{name}{dataName} {leafId:G} data");
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