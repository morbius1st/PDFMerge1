
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using iText.Commons.Bouncycastle.Tsp;
using UtilityLibrary;


// user name: jeffs
// created:   12/6/2024 7:02:35 PM

namespace AndyScan.Support
{
	//
	// public class Switchboard2
	// {
	// 	public static string myname => nameof(Switchboard2);
	//
	// 	// objects
	// 	private IWin iw;
	//
	// 	private UIElement input;
	//
	// 	private AndyScanSupport sps;
	//
	// 	// switchboard support
	// 	private ISbProcess sbp;
	// 	private static SwitchBoardProcess2 sbProcess2;
	//
	// 	private int acceptCharCount;
	// 	private string acceptedKeys;
	//
	// 	// values
	// 	private int sbIdx;
	//
	// 	private List<Tuple<string, bool?>> selOpption;
	//
	// 	static Switchboard2()
	// 	{
	// 		sbProcess2= new SwitchBoardProcess2(null);
	// 	}
	//
	// 	public Switchboard2(IWin iw, IWinInput iwi)
	// 	{
	// 		this.iw = iw;
	// 		input = iwi.GetInputWindow();
	// 		
	//
	// 		sbp = new SwitchBoardProcess2(iw);
	// 		sbProcess2.iw = iw;
	// 		sps = new AndyScanSupport(iw);
	//
	// 		selOpption = new List<Tuple<string, bool?>>();
	// 	}
	//
	// 	
	//
	// 	// menu
	//
	// 	public void Proceed()
	// 	{
	// 		SwitchBoard(0);
	// 	}
	//
	// 	// public methods
	//
	// 	/* process
	// 	 * SB (idx 0) -> get option
	// 	 *    if (option "X") -> exit -> null
	// 	 *		else
	// 	 *    processOption()
	// 	 *
	// 	 * processOption()
	// 	 *    if (idx 0) -> process 0 == null -> exit -> null
	// 	 *    if (idx 1) -> process 1 == null -> exit -> null
	// 	 *
	// 	 * process 0
	// 	 *
	// 	 *    
	// 	 */
	// 	public bool SwitchBoard(int index)
	// 	{
	// 		bool? repeat = true;
	// 		string option;
	//
	// 		iw.DebugMsg("\n");
	//
	// 		do
	// 		{
	// 			sps.ShowStatus();
	//
	// 			sbOptionList(index);
	//
	// 			option = chooseOption();
	//
	// 			iw.DebugMsg("\n\n");
	//
	// 			if (option.Equals("X"))
	// 			{
	// 				repeat = null;
	// 			}
	// 			else
	// 			{
	// 				findMenuInfo(option, index);
	//
	// 				if (selOpption[index].Item1 == null)
	// 				{
	// 					iw.DebugMsgLine($"the value entered | {option} is not valid.  try again\n");
	// 				}
	// 				else
	// 				{
	// 					repeat = sbp.ProcessSbOption(option, index, selOpption[index]);
	//
	// 					if (repeat == false)
	// 					{
	// 						iw.DebugMsgLine($"the last operation failed.  try again\n");
	// 					} 
	// 					else
	// 					{
	// 						// returned is true
	// 						if (selOpption[index].Item2 == false) repeat = null;
	// 					}
	// 				}
	// 			}
	// 		}
	// 		while (repeat != null);
	//
	// 		// return	(repeat.HasValue && repeat == true) || !repeat.HasValue;
	// 		return	repeat != false;
	// 	}
	//
	// 	public void findMenuInfo(string selected, int index)
	// 	{
	// 		Tuple<string, bool?> outValue;
	//
	// 		bool result = sbp.Menus[sbIdx].TryGetValue(selected, out outValue);
	//
	// 		if (result)
	// 		{
	// 			addSelOpt(outValue, index);
	// 		} 
	// 		else
	// 		{
	// 			addSelOpt(new Tuple<string, bool?>(null, null), index);
	// 		}
	// 	}
	//
	// 	// private methods
	//
	//
	// 	private string chooseOption()
	// 	{
	// 		
	// 		// iw.DebugMsg("\n ? > ");
	// 	
	// 		// TextBlock t = new TextBlock();
	// 		//
	// 		// ConsoleKeyInfo key = Console.ReadKey(false);
	// 	
	// 		// string c = key.KeyChar.ToString().ToUpper();
	// 		//
	// 		// if (sbp.TwoCharOptions[sbIdx].Contains(c))
	// 		// {
	// 		// 	key = Console.ReadKey(false);
	// 		// 	c += key.KeyChar.ToString().ToUpper();
	// 		// }
	// 	
	// 		// iw.DebugMsg("\n");
	// 	
	// 		// return c;
	// 		return "";
	// 	}
	//
	// 	private void sbOptionList(int idx)
	// 	{
	// 		sbIdx = idx;
	//
	// 		foreach (KeyValuePair<string, Tuple<string, bool?>> kvp in sbp.Menus[sbIdx])
	// 		{
	// 			if (kvp.Key.StartsWith('>'))
	// 			{
	// 				Console.WriteLine($"\n{"",-6}+ --- {kvp.Value} ---");
	// 			}
	// 			else if (kvp.Key.StartsWith('!'))
	// 			{
	// 				Console.WriteLine($"> {kvp.Key.Substring(1),-4}| *> {kvp.Value} <*");
	// 			}
	// 			else
	// 			{
	// 				Console.WriteLine(
	// 					$"> {kvp.Key,-4}| *> {kvp.Value}");
	// 			}
	// 		}
	// 	}
	//
	// 	private void addSelOpt(Tuple<string, bool?> sel, int index)
	// 	{
	// 		int repeat = index - selOpption.Count + 1;
	//
	// 		if (index >= selOpption.Count)
	// 		{
	// 			for (int i = 0; i < repeat; i++)
	// 			{
	// 				selOpption.Add(null);
	// 			}
	// 		}
	//
	// 		selOpption[index] = sel;
	// 	}
	//
	// 	// overrides
	// 	public override string ToString()
	// 	{
	// 		return $"this is {nameof(Switchboard2)}";
	// 	}
	//
	// }
}