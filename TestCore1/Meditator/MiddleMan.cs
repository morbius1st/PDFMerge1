#region using

using System;
using System.Collections.Generic;


#endregion

// username: jeffs
// created:  9/4/2020 6:25:11 AM

namespace TestCore1.Meditator
{
	public static class Architect
	{
		private static Dictionary<string, ConfRoom> conferenceCenter = new Dictionary<string, ConfRoom>();

		public static int Count => conferenceCenter.Count;

		public static string[,] ConferenceRooms()
		{
			string[,] rooms = new string[Count,4];

			int i = 0;

			foreach (KeyValuePair<string, ConfRoom> kvp in conferenceCenter)
			{
				rooms[i, 0] = kvp.Key;
				rooms[i, 1] = kvp.Value.Description;
				rooms[i, 2] = kvp.Value.Listeners.ToString();
				rooms[i, 3] = kvp.Value.Annoncers.ToString();
				i++;
			}

			return rooms;
		}

		public static void Description(string room, string description)
		{
			if (conferenceCenter.ContainsKey(room))
			{
				conferenceCenter[room].Description = description;
			}
		}

		public static void Listen(string room, 
			ConfRoom.MeditatorEventHandler evt, string description = null)
		{
			if (!conferenceCenter.ContainsKey(room))
			{
				conferenceCenter.Add(room, new ConfRoom() {Description = description});
			}

			conferenceCenter[room].MeditatorEvent += evt;
			conferenceCenter[room].Listeners += 1;
			
		}

		public static void UnListen(string room, ConfRoom.MeditatorEventHandler evt)
		{
			if (conferenceCenter.ContainsKey(room))
			{
				conferenceCenter[room].MeditatorEvent -= evt;
				if (conferenceCenter[room].Listeners > 0) conferenceCenter[room].Listeners -= 1;
			}
		}

		// public static void Present(string room, object package)
		// {
		// 	if (!conferenceCenter.ContainsKey(room))
		// 	{
		// 		conferenceCenter.Add(room, new ConfRoom());
		// 	}
		//
		// 	conferenceCenter[room].RaiseMeditatorEvent(package);
		// }

		public static ConfRoom.Announcer Announcer(  object sender, string room, string description = null)
		{
			if (!conferenceCenter.ContainsKey(room))
			{
				conferenceCenter.Add(room, new ConfRoom() {Description = description});
			}

			conferenceCenter[room].Annoncers += 1;

			return new ConfRoom.Announcer((sender, value) => conferenceCenter[room].RaiseMeditatorEvent(sender, value));
		}

		public static void UnAnnounce(string room)
		{
			if (conferenceCenter.ContainsKey(room))
			{
				if (conferenceCenter[room].Annoncers > 0) conferenceCenter[room].Annoncers -= 1;
			}
		}

		public class ConfRoom
		{
			public int Listeners { get; set; }

			public int Annoncers { get; set; }

			public string Description { get; set; }

			public delegate void MeditatorEventHandler(object sender, object value);

			public event ConfRoom.MeditatorEventHandler MeditatorEvent;

			public void RaiseMeditatorEvent(object sender, object value)
			{
				MeditatorEvent?.Invoke(sender, value);

			}

			public class Announcer
			{
				private ConfRoom.MeditatorEventHandler evt { get; set; }

				public Announcer(ConfRoom.MeditatorEventHandler evt)
				{
					this.evt = evt;
				}

				public void Announce(object sender, object package)
				{
					evt?.Invoke(sender, package);
				}

			}

		}
	}


	public class MiddleMan
	{
		public delegate void IsModifiedEventHandler(object sender, bool value);

		public static event MiddleMan.IsModifiedEventHandler IsModified;

		public static void RaiseIsModifiedEvent(object who, bool value)
		{
			Console.WriteLine("@ middleman| ismodified event raised| value| " + value);
			IsModified?.Invoke(who, value);
		}

		public delegate void OutgoingEventHandler(object sender, bool value);

		public static event MiddleMan.OutgoingEventHandler Outgoing;

		public static void RaiseOutgoingEvent(object who, bool value)
		{
			Console.WriteLine("@ middleman| outgoing event raised| value| " + value);
			Outgoing?.Invoke(who, value);
		}

		public delegate void InitializeEventHandler(object sender, bool value);

		public static event MiddleMan.InitializeEventHandler Initialize;

		public static void RaiseInitializeEvent(object who, bool value)
		{
			Console.WriteLine("@ middleman| initialized event raised| value| " + value);
			Initialize?.Invoke(who, value);
		}

		public delegate void StartTestEventHandler(object sender, bool value);

		public static event MiddleMan.StartTestEventHandler StartTest;

		public static void RaiseStartTestEvent(object who, bool value)
		{
			Console.WriteLine("@ middleman| StartTestd event raised| value| " + value);
			StartTest?.Invoke(who, value);
		}


		public delegate void MonitoredOutEventHandler(object sender, string value);

		public static event MiddleMan.MonitoredOutEventHandler MonitoredOut;

		public static void RaiseMonitoredOutEvent(object who, string value)
		{
			Console.WriteLine("@ middleman| monitoredout event raised| value| " + value);
			MonitoredOut?.Invoke(who, value);
		}


		public delegate void NonMonitoredOutEventHandler(object sender, string value);

		public static event MiddleMan.NonMonitoredOutEventHandler NonMonitoredOut;

		public static void RaiseNonMonitoredOutEvent(object who, string value)
		{
			Console.WriteLine("@ middleman| nonmonitoredout event raised| value| " + value);
			NonMonitoredOut?.Invoke(who, value);
		}


	#region system overrides

		public override string ToString()
		{
			return "this is MiddleMan";
		}

	#endregion
	}
}