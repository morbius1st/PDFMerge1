using System;
using System.Collections.Generic;
using System.Text;
using TestCore1.Meditator;

namespace TestCore1.Children.SubChildren
{
	class SubChild
	{
		private int index;

		private Architect.ConfRoom.Announcer xModified;

	#region ctor

		public SubChild()
		{
			MiddleMan.Outgoing += MiddleManOnOutgoing;

			index = ++Program.count;

			Console.WriteLine("@ subchild " + index + "| created");

			xModified = Architect.Announcer(this, Program.MODIFY_EVT_NAME);
		}

		public void ConfigListener(string room)
		{
			Architect.Listen(room, OnIntEvent);
		}

		private bool modified;

		public bool Modified
		{
			get => modified;
			set
			{
				if (value == modified) return;

				modified = value;

				Console.WriteLine("@ subchild " + index + "| sending| " + value);

				MiddleMan.RaiseIsModifiedEvent(this, value);
			}
		}

		public string Message => "this is subchild " + index + " talking";


		private void OnIntEvent(object sender, object value)
		{
			Console.WriteLine("@ subchild " + index + "| Init event received| "
				+ value.ToString() + ""
				+ " type| " + value.GetType()
				+ "\n");

			xModified.Announce(this, "@ subchild " + index + "| I am modified");
		}


		private void MiddleManOnOutgoing(object sender, bool value)
		{
			Console.WriteLine("@ subchild " + index + "| event received\n");
			Console.WriteLine("@ subchild " + index + "| from sender| " + sender.ToString());
			Console.WriteLine("@ subchild " + index + "| from value | " + value);

			Console.WriteLine("\n");
			Console.WriteLine("@ subchild " + index + "| setting modified to true ");

			Modified = true;

			Console.WriteLine("");
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SubChild| " + index;
		}

	#endregion
	}
}
