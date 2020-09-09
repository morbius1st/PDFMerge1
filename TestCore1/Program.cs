using System;
using System.Reflection.Metadata;
using TestCore1.Children;
using TestCore1.Children.SubChildren;
using TestCore1.Meditator;

namespace TestCore1
{
	public class Program
	{
		public static int count = 0;

		private static Child c0;

		public const string INIT_EVT_NAME = "Init";
		public const string MODIFY_EVT_NAME = "Modify";


		[STAThread]
		static void Main(string[] args)
		{
			Program p = new Program();

			c0 = new Child();

			// p.Test1();
			// p.Test2();
			p.Test3();

			Console.WriteLine("Waiting...\n");
			Console.ReadKey();
		}

		public void Test3()
		{
			Console.WriteLine("\n@ program| Test 2\n");

			Orator.Listen(MODIFY_EVT_NAME, OnModifiedEvent);

			// send message to children
			Orator.ConfRoom.Announcer A = Orator.GetAnnouncer(INIT_EVT_NAME);
			Orator.ConfRoom.Announcer2 A2 = Orator.GetAnnouncer2(this, INIT_EVT_NAME);

			AnnounceInit("init event sent");

			A.Announce(this, true);
			A2.Announce("this is an A2 package");

		}

		private void AnnounceInit(object package)
		{
			Orator.Announcer(this, INIT_EVT_NAME, package);
		}


		public void Test2()
		{
			Console.WriteLine("\n@ program| Test 2\n");

			Orator.ConfRoom.Announcer A1 = Orator.GetAnnouncer("Alpha");

			Console.WriteLine("@ program| raise event Alpha1\n");
			A1.Announce(this, 1000);

			Orator.Listen("Alpha", OnAlphaEvent, "Alpha Description");
			Orator.Listen("Alpha", OnAlpha2Event, "Alpha Description");
			Orator.Listen("Beta", OnBetaEvent, "Beta Description");

			Orator.ConfRoom.Announcer A2 = Orator.GetAnnouncer("Alpha");

			Console.WriteLine("@ program| raise event Alpha1 & Alpha2\n");
			A1.Announce(this, 1000);
			A2.Announce(this, 2000);

			Orator.ConfRoom.Announcer B = Orator.GetAnnouncer("Beta");

			Console.WriteLine("@ program| raise event B\n");
			B.Announce(this, 500);

			string[,] rooms = Orator.ConferenceRooms();

			Console.WriteLine("@ program| list ********\n");
			for (int i = 0; i < rooms.GetLength(0); i++)
			{
				Console.Write("@ program| name| " + rooms[i,0]);
				Console.Write(" desc| " + rooms[i,1]);
				Console.Write(" List #| " + rooms[i,2]);
				Console.WriteLine(" Ann #| " + rooms[i,3]);
			}
		}

		private void OnModifiedEvent(object sender, object value)
		{
			Console.WriteLine("@ program| Modified event received|  \"" 
				+ value.ToString() 
				+ "\" sent by| " + sender.ToString());

			if (sender.GetType() == typeof(SubChild))
			{
				SubChild sc = sender as SubChild;

				Console.WriteLine("@ program| subchild message| " + sc.Message);
			}

			Console.WriteLine("\n");
		}
		
		private void OnAlphaEvent(object sender, object value)
		{
			Console.WriteLine("@ program| Alpha event received| " 
				+ value.ToString() + "\n");

		}
				
		private void OnAlpha2Event(object sender, object value)
		{
			Console.WriteLine("@ program| Alpha 2 event received| " 
				+ value.ToString() + "\n");

		}

		private void OnBetaEvent(object sender, object value)
		{
			Console.WriteLine("@ program| Beta event received| "
				+ value.ToString() + "\n");

		}


		public void Test1()
		{
			Console.WriteLine("\n@ program| Test 1\n");

			MiddleMan.IsModified += middleManOnIsModified;

			Console.WriteLine("@ program| initialized to| true\n");

			MiddleMan.RaiseInitializeEvent(this, true);

			Console.WriteLine("@ program| outgoing sending| true\n");

			MiddleMan.RaiseOutgoingEvent(this, true);

			Console.WriteLine("@ program| outgoing sending| false\n");

			MiddleMan.RaiseOutgoingEvent(this, false);

			Console.WriteLine("\n\n@ program| ******************");
			Console.WriteLine("@ program| sending| monitored event\n");
			MiddleMan.RaiseMonitoredOutEvent(this, "monitored space message");

			Console.WriteLine("\n@ program| ******************");
			Console.WriteLine("@ program| sending| nonmonitored event\n");
			MiddleMan.RaiseNonMonitoredOutEvent(this, "nonmonitored space message");


			Console.WriteLine("\n@ program| ******************");
			Console.WriteLine("@ program| initialized to| false\n");

			MiddleMan.RaiseInitializeEvent(this, false);

			Console.WriteLine("\n@ program| ******************");
			Console.WriteLine("@ program| sending| monitored event\n");
			MiddleMan.RaiseMonitoredOutEvent(this, "monitored space message");

			Console.WriteLine("\n@ program| ******************");
			Console.WriteLine("@ program| sending| nonmonitored event\n");
			MiddleMan.RaiseNonMonitoredOutEvent(this, "nonmonitored space message");
		}

		private static void middleManOnIsModified(object sender, bool value)
		{
			Console.WriteLine("\n@ program| ******************");
			Console.WriteLine("@ program| ismodified event received");
			Console.WriteLine("@ program| from sender| " + sender.ToString());
			Console.WriteLine("@ program| from value | " + value);

			Console.WriteLine("");
		}

		public override string ToString()
		{
			return "This is Program";
		}
	}
}
