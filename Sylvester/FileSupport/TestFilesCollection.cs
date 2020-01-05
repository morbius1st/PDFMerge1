#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

#endregion


// projname: Sylvester.FileSupport
// itemname: TestFilesCollection
// username: jeffs
// created:  1/4/2020 10:20:45 PM


namespace Sylvester.FileSupport
{
	public class TestFilesCollection
	{
		public ObservableCollection<TestFile>
			TestFiles { get; private set; }

		public TestFilesCollection()
		{
			TestFiles = new ObservableCollection<TestFile>();
		}

		public void Add(TestFile tf)
		{
			TestFiles.Add(tf);
		}

		public void Add(Route r)
		{
//			TestFile tf = Support.ParseFileName<TestFile>(r);
			TestFile tf = new TestFile(r);

			SheetIdBase baseFile = new SheetIdBase();
			baseFile.FullFileRoute = new Route("NONE 1.0 - none.pdf");

			tf.BaseFile = baseFile;
			

			Add(tf);
		}
	}
}