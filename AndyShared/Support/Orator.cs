// Solution:     PDFMerge1
// Project:       Tests
// File:             Orator.cs
// Created:      2020-09-06 (8:12 AM)

using System.Collections.Generic;

namespace AndyShared.Support
{
	public class OratorRooms
	{
		// // from parent to children to initialize
		// public const string CF_INIT = "CfInit";

		// from parent to children to initialize
		public const string TN_INIT = "TnInit";

		// from children to parent, i have been modified
		// for treenodes only
		public const string MODIFIED = "Modified";

		// parent to children - modifications saved
		public const string SAVED = "Saved";
		
		// parent to children - modifications saved
		public const string SAVING = "Saving";

		// parent to children - modifications saved
		public const string TN_REM_EXCOLLAPSE_STATE = "TnRem_EC_State";
	}

	// ver	2.0		adjusted announcer to hold ref to owner and reduce
	//					parameter count when announcing

	public static class Orator
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
			ConfRoom.MeditatorEventHandler onAnnounce, string description = null)
		{
			if (!conferenceCenter.ContainsKey(room))
			{
				conferenceCenter.Add(room, new ConfRoom() {Description = description});
			}

			conferenceCenter[room].MeditatorEvent += onAnnounce;
			conferenceCenter[room].Listeners += 1;
			
		}

		public static void UnListen(string room, ConfRoom.MeditatorEventHandler onAnnounce)
		{
			if (conferenceCenter.ContainsKey(room))
			{
				conferenceCenter[room].MeditatorEvent -= onAnnounce;
				if (conferenceCenter[room].Listeners > 0) conferenceCenter[room].Listeners -= 1;
			}
		}

		public static ConfRoom.Announcer GetAnnouncer(object owner, string room, string description = null)
		{
			if (!conferenceCenter.ContainsKey(room))
			{
				conferenceCenter.Add(room, new ConfRoom() {Description = description});
			}

			conferenceCenter[room].Annoncers += 1;

			return new ConfRoom.Announcer(owner, (sender,value) => conferenceCenter[room].RaiseMeditatorEvent(sender, value));
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

			public event MeditatorEventHandler MeditatorEvent;

			public void RaiseMeditatorEvent(object sender, object value)
			{
				MeditatorEvent?.Invoke(sender, value);

			}

			public class Announcer
			{
				private object owner;

				private MeditatorEventHandler _Anevent { get; set; }

				public Announcer(object owner, MeditatorEventHandler anEvent)
				{
					this.owner = owner;
					_Anevent = anEvent;
				}

				public void Announce(object value)
				{
					_Anevent?.Invoke(owner, value);
				}

			}

		}
	}
}