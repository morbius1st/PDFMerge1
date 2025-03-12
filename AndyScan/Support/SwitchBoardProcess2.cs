
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;


// user name: jeffs
// created:   12/7/2024 7:09:12 AM

namespace AndyScan.Support
{
	//
	// public interface ISbProcess
	// {
	// 	public Dictionary<string, Tuple<string, bool?>>[] Menus { get; }
	// 	public string[] TwoCharOptions { get; }
	//
	// 	public bool ProcessSbOption(string option, int index, Tuple<string, bool?> selOpt);
	// }
	//
	// public class SwitchBoardProcess2 : ISbProcess
	// {
	// 	// objects
	// 	private Switchboard2 sbs;
	//
	// 	private AndyScanSupport sps;
	//
	// 	// values
	// 	public IWin iw;
	//
	// 	private Tuple<string, bool?> selOpt;
	//
	// 	// ctor
	// 	public SwitchBoardProcess2(IWin iw)
	// 	{
	// 		this.iw = iw;
	//
	// 		sps = new AndyScanSupport(iw);
	// 	}
	//
	// 	// methods
	// 	public bool ProcessSbOption(string option, int index, Tuple<string, bool?> selOpt)
	// 	{
	// 		this.selOpt = selOpt;
	//
	// 		return processOptiopn(option, index);
	// 	}
	//
	// 	// primary switch
	//
	// 	// true = good
	// 	// false = fail
	// 	// null = exit
	// 	private bool processOptiopn(string option, int index)
	// 	{
	// 		bool repeat = true;
	//
	// 		if (index == 0)
	// 		{
	// 			repeat = sbMainProcess(option);
	// 		}
	// 		else if (index == 1)
	// 		{
	// 			repeat = sbMainSubProcess1(option);
	// 		}
	//
	// 		return repeat;
	// 	}
	//
	//
	// 	// process switches
	//
	// 	// main switch
	//
	// 	// true == worked
	// 	// false == did not work
	// 	// null = exit (not allowed)
	// 	private bool sbMainProcess(string option)
	// 	{
	// 		bool result = true;
	//
	// 		switch (option)
	// 		{
	// 		case "SS":
	// 			{
	// 				result = sps.SelectData();
	// 				break;
	// 			}
	// 		case "RD" :
	// 			{
	// 				result = sps.PrepForScan();
	// 				break;
	// 			}
	// 		case "SC" :
	// 			{
	// 				result = sps.Scan();
	// 				break;
	// 			}
	// 		}
	//
	// 		return result;
	// 	}
	//
	// 	// main sub-switch
	//
	// 	// true == worked
	// 	// false == did not work
	// 	// null = exit
	// 	private bool sbMainSubProcess1(string option)
	// 	{
	// 		bool result = true;
	//
	// 		switch (option)
	// 		{
	// 		case "SC":
	// 			{
	// 				result = sps.Scan(); // ? null : false;
	//
	// 				break;
	// 			}
	// 		}
	//
	// 		return result;
	// 	}
	// 	
	//
	// 	// data
	// 	public string[] TwoCharOptions { get; set; } = new []
	// 	{
	// 		"RS",
	// 		"S"
	// 	};
	//
	// 	// formatter: off
	// 	public Dictionary<string, Tuple<string, bool?>>[] Menus { get; set; } = new []
	// 	{
	// 		//                                                             v - repeat upon true | false = no | true = yes | null = na (same as true)
	// 		new Dictionary<string, Tuple<string, bool?>>() //              ------
	// 		{
	// 			{ ">01", new Tuple<string, bool?>("Primary Menu"          , null) },
	// 			{ ">02", new Tuple<string, bool?>("Data to Scan"          , null) },
	// 			{ "SS",  new Tuple<string, bool?>("Select data >"         , true) },
	// 			{ "RD",  new Tuple<string, bool?>("Read Files"            , true) },
	// 			{ "SC",  new Tuple<string, bool?>("Scan"                  , null) },
	// 			{ ">03", new Tuple<string, bool?>("Completion Options"    , null) },
	// 			{ "!X",  new Tuple<string, bool?>("Exit"                  , null) },
	// 		},
	// 		new Dictionary<string, Tuple<string, bool?>>()
	// 		{
	// 			{ ">01", new Tuple<string, bool?>("Process Data"          , null) },
	// 			{ "SC",  new Tuple<string, bool?>("Scan"                  , false) },
	// 			{ "!X",  new Tuple<string, bool?>("Exit"                  , null) },
	// 		},
	// 	} ;
	// 	// formatter: on
	//
	// 	public override string ToString()
	// 	{
	// 		return $"this is {nameof(SwitchBoardProcess2)}";
	// 	}
	//
	// }
}
