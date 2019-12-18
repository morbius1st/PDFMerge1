#region + Using Directives

using System;
using static Test3.MainWindow.SelectStatus;
using static Test3.MainWindow;




#endregion


// projname: Test3
// itemname: EventTest2
// username: jeffs
// created:  12/11/2019 8:42:22 PM


namespace Test3
{
	public class EventTest2
	{
		private string name;
		private int idx;

		public EventTest2(string name, int idx)
		{
			this.name = name;
			this.idx = idx;
		}

		public MainWindow.SelectStatus Status { get; set; }

		public void EventTestMethod(SelectStatus test)
		{
			AppendLine("i am " + name + ". my status| (" + Status + ")");
		}

		public void TriStateTestMethod(TriState d)
		{
			AppendLine("i am " + name +" (" + idx + "). my status| (" + Status + ")");

		}

		public void AppendLine(string msg)
		{
			MainWindow.Instance.AppendLineTbk1(msg);
		}

		public void AppendMessage(string msg)
		{
			MainWindow.Instance.AppendMessageTbk1(msg);
		}

	}
}
