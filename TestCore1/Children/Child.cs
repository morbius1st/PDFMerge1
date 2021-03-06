﻿#region using

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TestCore1.Children.SubChildren;
using TestCore1.Meditator;

#endregion

// username: jeffs
// created:  9/4/2020 6:44:48 AM

namespace TestCore1.Children
{
	public class Child // : INotifyPropertyChanged
	{
		private SubChild s0;
		private SubChild s1;
		private SubChild s2;

		private bool initialized = false;

		// private Orator.ConfRoom.Announcer Modified;
		private Orator.ConfRoom.Announcer2 Modified2;

	#region ctor

		public Child()
		{
			Console.WriteLine("@ child| created");

			/*
			MiddleMan.Outgoing += MiddleManOnOutgoing;
			MiddleMan.Initialize += MiddleManOnInitialize;
			MiddleMan.MonitoredOut += MiddleManOnMonitoredOut;
			MiddleMan.NonMonitoredOut += MiddleMan_NonMonitoredOut;

			*/

			s0 = new SubChild();
			s0.ConfigListener(Program.INIT_EVT_NAME);

			s1 = new SubChild();
			s1.ConfigListener(Program.INIT_EVT_NAME);

			s2 = new SubChild();
			s2.ConfigListener(Program.INIT_EVT_NAME);

			Orator.Listen(Program.INIT_EVT_NAME, OnIntEvent);

			// Modified = Orator.GetAnnouncer(Program.MODIFY_EVT_NAME);
			Modified2 = Orator.GetAnnouncer2(this, Program.MODIFY_EVT_NAME);
		}

	#endregion

		public SubChild S0 => s0;


		private void OnIntEvent(object sender, object value)
		{
			Console.WriteLine("@ child| Init event received| \""
				+ value.ToString() + "\""
				+ "   type| " + value.GetType()
				+ "\n");

			// Modified.Announce(this, "from child| I am modified");
			Modified2.Announce("from child| I am modified 2");
		}


		/*
		private void MiddleMan_NonMonitoredOut(object sender, string value)
		{
			Console.WriteLine("\n@ child| nonmonitored event received");
			Console.WriteLine("@ child| nonmonitored value | " + value);

			NonMonitoredText = value;

		}

		private void MiddleManOnMonitoredOut(object sender, string value)
		{
			Console.WriteLine("\n@ child| monitored event received");
			Console.WriteLine("@ child| monitored value | " + value);

			MonitoredText = value;

		}

		private bool isModified;

		public bool IsModified
		{
			get => isModified;
			set
			{
				if (value == isModified) return;

				isModified = value;

				Console.WriteLine("\n@ child| @ modified| sending| " + value + "\n-");

				// MiddleMan.RaiseIsModifiedEvent(this, value);

				OnPropertyChange();
			}
		}

		
		private string monitoredText;

		public string MonitoredText
		{
			get => monitoredText;
			set
			{
				if ((monitoredText ?? "").Equals(value)) return;

				monitoredText = value;

				Console.WriteLine("\n@ child| @monitoredtext property| " + value + "\n");

				OnMonitoredProperty01Change();
			}
		}

		private string nonMonitoredText;

		public string NonMonitoredText
		{
			get => nonMonitoredText;
			set
			{
				if ((monitoredText ?? "").Equals(value)) return;

				nonMonitoredText = value;

				Console.WriteLine("\n@ child| nonmonitoredtext property| " + value + "\n");

				OnPropertyChange();
			}
		}

		
		private void MiddleManOnInitialize(object sender, bool value)
		{
			initialized = value;

			Console.WriteLine("\n@ child| @initialized as| " + value);
		}

		private void MiddleManOnOutgoing(object sender, bool value)
		{
			Console.WriteLine("@ child| *************");
			Console.WriteLine("@ child| event received");
			Console.WriteLine("@ child| from sender| " + sender.ToString());
			Console.WriteLine("@ child| from value | " + value);

			Console.WriteLine("");
			Console.WriteLine("@ child| setting modified to " + value.ToString());

			IsModified = value;

			Console.WriteLine("");
		}

		
		private void OnMonitoredProperty01Change([CallerMemberName] string memberName = "")
		{
			Console.WriteLine("@ child| *************");
			Console.WriteLine("@ child| at event");
		
			if (initialized) IsModified = true;
		
			OnPropertyChange(memberName);
		
		}

		


	#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		*/

	#region system overrides

		public override string ToString()
		{
			return "this is Child";
		}

	#endregion
	}
}