using System;
using System.Collections.Generic;
using System.Text;
using TestCore1.Meditator;

namespace TestCore1.Children.SubChildren
{
	public class SubChild
	{
		private int index;

		// private Orator.ConfRoom.Announcer xModified;
		private Orator.ConfRoom.Announcer2 xModified2;


	#region ctor

		public SubChild()
		{
			// MiddleMan.Outgoing += MiddleManOnOutgoing;

			index = ++Program.count;

			Console.WriteLine("@ subchild " + index + "| created");

			// xModified = Orator.GetAnnouncer(Program.MODIFY_EVT_NAME);
			xModified2 = Orator.GetAnnouncer2(this, Program.MODIFY_EVT_NAME);
		}

		public Orator.ConfRoom.Announcer2 Ann2 => xModified2;

		public void ConfigListener(string room)
		{
			Orator.Listen(room, OnIntEvent);

			object lv = Orator.GetLastValue(room);

			if (lv != null)
			{
				Console.WriteLine("@ subchild " + index + "| got lastValue| " + lv.ToString());
			}
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

				// MiddleMan.RaiseIsModifiedEvent(this, value);
			}
		}

		public string Message => "this is subchild " + index + " talking";


		private void OnIntEvent(object sender, object value)
		{
			Console.WriteLine("@ subchild " + index + "| Init event received| \""
				+ value.ToString() + "\""
				+ " type| " + value.GetType()
				+ "\n");

			// xModified.Announce(this, "from subchild " + index + "| I am modified");
			xModified2.Announce("from subchild " + index + "| I am modified 2");
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
