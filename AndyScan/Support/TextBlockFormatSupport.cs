#region + Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using iText.Barcodes;
using iText.Commons.Actions;
using iText.IO.Font.Otf.Lookuptype8;
using UtilityLibrary;
// using static UtilityLibrary.TextBlockFormatSupport.FmtTypeCode;

#endregion

// user name: jeffs
// created:   12/10/2024 7:58:28 PM
//
// namespace UtilityLibrary
// {
// 	/* format
// 	 * 0         1         2         3         4         5         6         7         8         9         0   
// 	 * 01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
// 	 * text A text <codeA>formatted Z text text</codeA> text B text <codeB>formatted Y text</codeB> text C text
// 	 *             ^     ^        <- 19 ->     ^      ^                                                        ^
// 	 *             |     + pos2 (18) (+1)      |      + pos4 (47)                                              +> length = 104
// 	 *             +> pos1 (12) len = 6        +> pos3 (40)  len = 7
// 	 *
// 	 * pos1 = 13
// 	 * pos2 = 18
// 	 * pos3 = 42
// 	 * pos4 = 47
// 	 * lenA = 5
// 	 * lenB = 5
// 	 * lenC = 40-18-1 = 21
// 	 * lenD = 104 - 47 = 57
// 	 * text length = 104
// 	 *
// 	 *
// 	 * xxx xxx <> xxx xxx <> xxx xxx | p3 alt= xxx <> xxx <> xxx
// 	 * -------|---|-----|---|------- |        |-----------------|
// 	 * ^       ^   p4        ^		 |         ^
//  	 * +> p1   +> p2         +> p3a	 |         +> p3b
// 	 *
// 	 * case 1: got p1 / p2 & p3a are null
// 	 * case 2: got p1 & p2 / p3a is null (& !p4.isVoid)
// 	 * case 3: got p1, p2, & p3a (& !p4.isVoid)
// 	 * case 4: got p2 / p1 & p3a are null
// 	 * case 5: got p1, p2, & p3b
// 	 *
// 	 * case 1:
// 	 *    span2 = span.add (run(p1))
// 	 *  return span2
// 	 *
// 	 * case 2:
// 	 *   span1 sInner = process(p2)
// 	 *   span2 = span(p1)
// 	 *   span2 add span1
// 	 *  return span2
// 	 *
// 	 * case 3:
// 	 *   span1 = process(p2)
// 	 *   span2 = span(p1)
// 	 *   span2 add span1
// 	 *   span2 add span (p3a)
// 	 *  return span2
// 	 *
// 	 * case 4:
// 	 *   span2 = span(p2)
// 	 *  return span2
// 	 *
// 	 * case 5:
// 	 *   span1 = process(p2)
// 	 *   span3 = process(p3b)
// 	 *   span2 = span(p1)
// 	 *   span2 add span1
// 	 *   span2 add span (p3)
// 	 *  return span2
// 	 *
// 	 *  process(px) -> span
// 	 *    get p1, p2, p3
// 	 *    if (p2 has codes) process(p2) -> span1
// 	 *    if (p1 != null) span2.add span(p1)
// 	 *    if (span1 != null) span2.add span1
// 	 *    if (p3 1= null) span2.add span(p3)
// 	 *   return span2
// 	 *
// 	 *
// 	 * test:
// 	 *                                                                                                     1
// 	 * 0         1         2         3         4         5         6         7         8         9         0         1         2         3         4         5         6 
// 	 * 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012
// 	 * text D text <bold><red>formatted X text text </red></bold>text E text <blue>formatted text W</blue>formatted V text text <green>formatted text U</green>text F text
// 	 * ^           ^    ^^                               ^^     ^
// 	 * 0           12  17|18                           50|51    57
// 	 * l=12        l=6    l=33                            l=7
// 	 */
//
// 	public class TextBlockFormatSupport
// 	{
// 		private static int count = 0;
// 		private const  int MAX_LOOPS = 50;
// 		private const int HAS_ATTRIBUTES = 1000;
// 		private const int HAS_SOLO_ATTRIBUTES = -1000;
//
// 		public enum FmtTypeCode
// 		{
// 			// solo tags < 0
// 			// with parameters
// 			FC_LINE_MARGIN_SP = HAS_SOLO_ATTRIBUTES - 1,
// 			// without parameters
// 			FC_LINE_ADJ = -1,
// 			// plain text == 0
// 			FC_PLAIN_TEXT = 0,
// 			// non-attribute tags 1 > HAS_ATTRIBUTES
// 			FC_SET_COLOR    = 1,
// 			FC_FONT_WEIGHT,
// 			FC_FONT_STYLE,
// 			FC_TEXT_DECORATION,
// 			// attribute tags > HAS_ATTRIBUTES
// 			FC_TEXT_FORMAT     = HAS_ATTRIBUTES + 1,
// 			FC_CUSTOM_FG_COLOR,
// 			FC_CUSTOM_BG_COLOR,
// 		}
//
// 		private struct Attribute
// 		{
// 			public string OptionCode { get; set; }
//
// 			// public FmtTypeCode TypeCode { get; set; }
//
// 			public int MinArgs { get; set; }
// 			public int MaxArgs { get; set; }
//
// 			public Attribute(string optionCode, int minArgs, int maxArgs)
// 			{
// 				OptionCode = optionCode;
//
// 				MinArgs = minArgs;
// 				MaxArgs = maxArgs;
// 			}
// 		}
//
// 		private static Tuple<FmtTypeCode, object> Default => new Tuple<FmtTypeCode, object>(FC_SET_COLOR, Brushes.Red);
//
// 		// format names
// 		public const string FN_DEFAULT = "default";
// 		public const string FN_LINEBREAK = "linebreak";
// 		public const string FN_MARGIN = "margin";
//
// 		private static Dictionary<string, Tuple<FmtTypeCode, object>> formatData =
// 			new Dictionary<string, Tuple<FmtTypeCode, object>>()
// 			{
// 				// quick color 
// 				// first letter lower case == foreground
// 				// first letter upper case == background
// 				{ ""             , new Tuple<FmtTypeCode, object>(FC_PLAIN_TEXT,         null) },
// 				{ "default"      , new Tuple<FmtTypeCode, object>(FC_SET_COLOR,          Brushes.Red) },
// 				{ "red"          , new Tuple<FmtTypeCode, object>(FC_SET_COLOR,          Brushes.Red) },
// 				{ "blue"         , new Tuple<FmtTypeCode, object>(FC_SET_COLOR,          Brushes.Blue) },
// 				{ "green"        , new Tuple<FmtTypeCode, object>(FC_SET_COLOR,          Brushes.ForestGreen) },
// 				{ "magenta"      , new Tuple<FmtTypeCode, object>(FC_SET_COLOR,          Brushes.Magenta) },
// 				{ "dimgray"      , new Tuple<FmtTypeCode, object>(FC_SET_COLOR,          Brushes.DimGray) },
// 				{ "white"        , new Tuple<FmtTypeCode, object>(FC_SET_COLOR,          Brushes.White) },
// 				{ "bold"         , new Tuple<FmtTypeCode, object>(FC_FONT_WEIGHT,        FontWeights.Bold) },
// 				{ "italic"       , new Tuple<FmtTypeCode, object>(FC_FONT_WEIGHT,        FontStyles.Italic) },
// 				{ "underline"    , new Tuple<FmtTypeCode, object>(FC_TEXT_DECORATION,    TextDecorations.Underline) },
// 				{ "strikethrough", new Tuple<FmtTypeCode, object>(FC_TEXT_DECORATION,    TextDecorations.Strikethrough) },
// 				{ "overline"     , new Tuple<FmtTypeCode, object>(FC_TEXT_DECORATION,    TextDecorations.OverLine) },
//
// 				{
// 					"foreground" , new Tuple<FmtTypeCode, object>(FC_CUSTOM_FG_COLOR,
// 						new Attribute[]
// 						{
// 							new Attribute("color", 3, 4),
// 						})
// 				},
// 				{
// 					"background" , new Tuple<FmtTypeCode, object>(FC_CUSTOM_BG_COLOR,
// 						new Attribute[]
// 						{
// 							new Attribute("color", 3, 4),
// 						})
// 				},
// 				{
// 					"textformat" , new Tuple<FmtTypeCode, object>(FC_TEXT_FORMAT,
// 						new Attribute[]
// 						{
// 							new Attribute("foreground", 3, 4),
// 							new Attribute("background", 3, 4),
// 							new Attribute("weight"    , 1, 1),
// 							new Attribute("style"     , 1, 1),
// 							new Attribute("size"      , 1, 1),
// 							new Attribute("family"    , 1, 1)
// 						})
// 				},
// 				// solo tags
// 				{ FN_LINEBREAK   , new Tuple<FmtTypeCode, object>(FC_LINE_ADJ,           null ) },
// 				{ FN_MARGIN   , new Tuple<FmtTypeCode, object>(FC_LINE_MARGIN_SP,
// 					new Attribute[]
// 					{
// 						new Attribute("spaces", 1, 1),
// 						new Attribute("tabs"  , 1, 1),
// 					})
// 				},
// 			};
//
// 		/*
// 			TextDecorationCollection td = TextDecorations.Strikethrough;
//
// 			Pen p = new Pen();
// 			p.Thickness = 1.5;
// 			TextDecoration txd = new TextDecoration(p);
//
// 			td.Add(new TextDecoration(txd));
//
// 		*/
//
// 		/* version 7*/
// 		public static Span processText(string text)
// 		{
// 			string prefaceText;
// 			string suffixText;
// 			string innerText;
// 			string soloTag;
//
// 			Span span1 = null;
// 			Span span2 = null;
// 			Span span3 = null;
//
// 			Span span5 = null;
//
// 			if (count++ > MAX_LOOPS) return new Span(new Run("stack overflow"));
//
// 			FmtCode fc = FmtCode.DivideText(text, out prefaceText, out innerText, out suffixText, out soloTag);
// 			
// 			fc.FormatData = assignFormatData(fc.FmtTag);
//
// 			if (!prefaceText.IsVoid())
// 			{
// 				span1 = new Span(new Run(prefaceText ?? ""));
//
// 				if (fc.FormatData.Item1 < 0)
// 				{
// 					processSoloTag(fc, span1);
// 				}
// 			}
//
// 			if (fc.IsValid && soloTag.IsVoid())
// 			{
// 				if (FmtCode.HasCode(innerText))
// 				{
// 					span2 = processText(innerText);
// 				}
// 				else
// 				{
// 					span2 = new Span(new Run(innerText));
// 				}
//
// 				processStdTag(fc, span2);
// 					
// 			}
//
// 			if (!suffixText.IsVoid())
// 			{
// 				if (FmtCode.HasCode(suffixText))
// 				{
// 					span3 = processText(suffixText);
// 				}
// 				else
// 				{
// 					span3 = new Span(new Run(suffixText));
// 				}
// 			}
//
// 			Span span4 = new Span();
//
// 			if (span1 != null) span4.Inlines.Add(span1);
// 			if (span2 != null) span4.Inlines.Add(span2);
// 			if (span3 != null) span4.Inlines.Add(span3);
//
// 			return span4;
// 		}
//
// 		private static Tuple<FmtTypeCode, object> assignFormatData(string fmtTag)
// 		{
// 			Tuple<FmtTypeCode, object> formatCode;
//
// 			if (fmtTag.IsVoid()) return formatData[""];
//
// 			if (formatData.TryGetValue(fmtTag.ToLower(), out formatCode)) { return formatCode; }
//
// 			return Default;
// 		}
//
// 		private static void processStdTag(FmtCode fc, Span span)
// 		{
// 			if ((int) fc.FormatData.Item1 >= HAS_ATTRIBUTES)
// 			{
// 				processStdAttrs(fc, span);
// 			}
// 			else
// 			{
// 				formatStdText(fc, span);
// 			}
// 		}
//
// 		private static void processSoloTag(FmtCode fc, Span span)
// 		{
// 			if ((int) fc.FormatData.Item1 >= HAS_ATTRIBUTES)
// 			{
// 				processSoloAttrs(fc, span);
// 			}
// 			else
// 			{
// 				formatSoloText(fc, span);
// 			}
// 		}
//
//
// 		// process "simple" options
// 		private static void formatStdText(FmtCode fc, Span span)
// 		{
// 			Tuple<FmtTypeCode, object> formatCode;
//
// 			bool result = formatData.TryGetValue(fc.FmtTag.ToLower(), out formatCode);
//
// 			if (!result)
// 			{
// 				return;
// 			}
//
// 			assignStdFormatting(fc, formatCode, span, null);
// 		}
//
// 		// process "simple" options
// 		private static void formatSoloText(FmtCode fc, Span span)
// 		{
// 			Tuple<FmtTypeCode, object> formatCode;
//
// 			bool result = formatData.TryGetValue(fc.FmtTag.ToLower(), out formatCode);
//
// 			if (!result)
// 			{
// 				return;
// 			}
//
// 			assignSoloFormatting(fc, formatCode, span, null);
// 		}
//
// 		private static void assignStdFormatting(FmtCode fc, Tuple<FmtTypeCode, object> formatCode, Span span,
// 			string[][] args)
// 		{
// 			switch (formatCode.Item1)
// 			{
// 			case FC_SET_COLOR:
// 				{
// 					if (fc.FmtTag[0].IsLowerAlpha())
// 					{
// 						span.Foreground = (SolidColorBrush) formatCode.Item2;
// 					}
// 					else
// 					{
// 						span.Background = (SolidColorBrush) formatCode.Item2;
// 					}
//
// 					break;
// 				}
// 			case FC_FONT_WEIGHT:
// 				{
// 					span.FontWeight = (FontWeight) formatCode.Item2;
// 					break;
// 				}
// 			case FC_FONT_STYLE:
// 				{
// 					span.FontStyle = (FontStyle) formatCode.Item2;
// 					break;
// 				}
//
// 			case FC_TEXT_DECORATION:
// 				{
// 					span.TextDecorations = (TextDecorationCollection) formatCode.Item2;
// 					break;
// 				}
// 			// case FC_TEXT_FORMAT:
// 			// 	{
// 			// 		applyTextFormat(args, span);
// 			// 		break;
// 			// 	}
// 			}
// 		}
//
// 		private static void assignSoloFormatting(FmtCode fc, Tuple<FmtTypeCode, object> formatCode, Span span,
// 			string[][] args)
// 		{
// 			switch (formatCode.Item1)
// 			{
// 			case FC_LINE_ADJ:
// 				{
// 					if (fc.FmtTag.Equals(FN_LINEBREAK))
// 					{
// 						span.Inlines.Add(new LineBreak());
// 						
// 					}
// 					break;
// 				}
// 			}
// 		}
//
// 		// process a tag that has attributes
//
// 		/// <summary>
// 		/// process a tag that has attributes
// 		/// part 1 validate that the actual attribute meets its criteria
// 		///	part 2 assign formatting if the attribute is correct
// 		/// </summary>
// 		private static void processStdAttrs(FmtCode fc, Span span)
// 		{
// 			Attribute[] possibleAttrs = fc.FormatData.Item2 as Attribute[];
//
// 			if (possibleAttrs == null) return;
//
// 			// got more actual attributes thna possible attributes?
// 			if (fc.Attrs.Length > possibleAttrs.Length ) return;
//
// 			// [0][0] is the attribute name
// 			// [1][1] through [n] parameters for this specific attribute
// 			string[][] actAttribute;
//
// 			// validate each actual attribute to determine if
// 			// it is an allowed attribute and check if the
// 			// args provided meet the count criteria
//
// 			// fc has the formatData - the type specific information about
// 			// the current tag - that is,
// 			// it has the type code and an object
// 			// the object can be a specific value to set or
// 			// it can be an array of attribute
// 			// the array of tag attributes is extracted from the formatData
// 			// above
//
// 			// check each attr in fc.Attrs to determine if it is valid
// 			// against the list of possible attributes
//
// 			foreach (string attr in fc.Attrs)
// 			{
// 				// first divide the actual attribute into its parts - the array string[][]
// 				// see above for a definition
// 				actAttribute = FmtCode.DivideParameters(attr);
//
// 				// first - validate the actual attribute against the possible list
//
// 				if (!validateAttr(actAttribute, possibleAttrs)) continue;
//
// 				assignStdAttrFormatting(fc, fc.FormatData, span, actAttribute);
// 			}
// 		}
//
// 		private static void processSoloAttrs(FmtCode fc, Span span)
// 		{
// 			Attribute[] possibleAttrs = fc.FormatData.Item2 as Attribute[];
//
// 			if (possibleAttrs == null) return;
//
// 			// got more actual attributes thna possible attributes?
// 			if (fc.Attrs.Length > possibleAttrs.Length ) return;
//
// 			// [0][0] is the attribute name
// 			// [1][1] through [n] parameters for this specific attribute
// 			string[][] actAttribute;
//
// 			// validate each actual attribute to determine if
// 			// it is an allowed attribute and check if the
// 			// args provided meet the count criteria
//
// 			// fc has the formatData - the type specific information about
// 			// the current tag - that is,
// 			// it has the type code and an object
// 			// the object can be a specific value to set or
// 			// it can be an array of attribute
// 			// the array of tag attributes is extracted from the formatData
// 			// above
//
// 			// check each attr in fc.Attrs to determine if it is valid
// 			// against the list of possible attributes
//
// 			foreach (string attr in fc.Attrs)
// 			{
// 				// first divide the actual attribute into its parts - the array string[][]
// 				// see above for a definition
// 				actAttribute = FmtCode.DivideParameters(attr);
//
// 				// first - validate the actual attribute against the possible list
//
// 				if (!validateAttr(actAttribute, possibleAttrs)) continue;
//
// 				assignSoloAttrFormatting(fc, fc.FormatData, span, actAttribute);
// 			}
// 		}
//
// 		// validate that the actual attribute provided matches:
// 		// that the name is a valid attribute and that the number 
// 		// of parameters is within the range
// 		private static bool validateAttr(string[][] actAttr, Attribute[] possibleAttr)
// 		{
// 			if (actAttr[0][0].IsVoid() || actAttr[1] == null) return false;
//
// 			for (var i = 0; i < possibleAttr.Length; i++)
// 			{
// 				if (actAttr[0][0].Equals(possibleAttr[i].OptionCode))
// 				{
// 					// match found
//
// 					// validate the number of parameters
// 					if (actAttr[1].Length <= possibleAttr[i].MaxArgs && actAttr[1].Length >= possibleAttr[i].MinArgs)
// 					{
// 						return true;
// 					}
//
// 					break;
// 				}
// 			}
//
// 			return false;
// 		}
//
// 		private static void assignStdAttrFormatting(FmtCode fc, Tuple<FmtTypeCode, object> formatCode, Span span, string[][] args)
// 		{
// 			switch (formatCode.Item1)
// 			{
// 			case FC_TEXT_FORMAT:
// 				{
// 					applyTextFormat(args, span);
// 					break;
// 				}
// 			case FC_CUSTOM_FG_COLOR:
// 				{
// 					span.Foreground = getCustomBrush(args[1]);
// 					break;
// 				}
// 			case FC_CUSTOM_BG_COLOR:
// 				{
// 					span.Background = getCustomBrush(args[1]);
// 					break;
// 				}
// 			}
// 		}
//
// 		
// 		private static void assignSoloAttrFormatting(FmtCode fc, Tuple<FmtTypeCode, object> formatCode, Span span, string[][] args)
// 		{
// 			switch (formatCode.Item1)
// 			{
// 			case FC_LINE_MARGIN_SP:
// 				{
// 					applyTextFormat(args, span);
// 					break;
// 				}
//
// 			}
// 		}
//
// 		private static void applyTextFormat(string[][] args, Span span)
// 		{
// 			bool result;
// 			int answer;
//
// 			switch (args[0][0])
// 			{
// 			case "foreground":
// 				{
// 					SolidColorBrush b = getCustomBrush(args[1]);
// 					span.Foreground = b;
// 					break;
// 				}
// 			case "background":
// 				{
// 					SolidColorBrush b = getCustomBrush(args[1]);
// 					span.Background = b;
// 					break;
// 				}
// 			case "weight":
// 				{
// 					FontWeightConverter fc = new FontWeightConverter();
//
// 					try
// 					{
// 						FontWeight f = (FontWeight) fc.ConvertFromString(args[1][0]);
//
// 						span.FontWeight = f;
// 					}
// 					catch { }
//
// 					break;
// 				}
//
// 			case "size":
// 				{
// 					if (int.TryParse(args[1][0], out answer))
// 					{
// 						span.FontSize = answer;
// 					}
//
// 					break;
// 				}
// 			case "style":
// 				{
// 					FontStyleConverter fc = new FontStyleConverter();
//
// 					try
// 					{
// 						FontStyle f = (FontStyle) fc.ConvertFromString(args[1][0]);
//
// 						span.FontStyle = f;
// 					}
// 					catch { }
//
// 					break;
// 				}
//
// 			case "family":
// 				{
// 					FontFamilyConverter fc = new FontFamilyConverter();
//
// 					try
// 					{
// 						FontFamily f = (FontFamily) fc.ConvertFromString(args[1][0]);
//
// 						span.FontFamily = f;
// 					}
// 					catch { }
//
// 					break;
// 				}
// 			}
// 		}
//
// 		private static void applyMargin(string[][] args, Span span)
// 		{
//
// 		}
//
// 		// general routines
// 		private static SolidColorBrush getCustomBrush(string[] args)
// 		{
// 			if (args.Length != 3 && args.Length != 4)
// 			{
// 				return new SolidColorBrush(Colors.DarkGray);
// 			}
//
// 			byte[] a = new byte[4];
//
// 			bool result = true;
// 			byte value;
// 			int j = 0;
//
// 			if (args.Length != 4)
// 			{
// 				j = 1;
// 				a[0] = 255;
// 			}
//
//
// 			for (int i = 0 + j; i < args.Length + j; i++)
// 			{
// 				a[i] = 128;
//
// 				if (byte.TryParse(args[i - j], out value))
// 				{
// 					a[i] = value;
// 				}
// 			}
//
// 			return new SolidColorBrush(Color.FromArgb(a[0], a[1], a[2], a[3]));
// 		}
//
// 		public static void Reset()
// 		{
// 			count = 0;
// 		}
//
// 		// format code structure
// 		public struct FmtCode
// 		{
// 			public const string START_O = "<";
// 			public const string START_C = "</";
// 			public const string END = ">";
//
// 			public const string P_START = "{";
// 			public const string P_END = "}";
//
// 			public const string P_FLAG = "=";
// 			public const string P_DIVIDER = ";";
//
// 			public static readonly int START_O_LEN = START_O.Length;
// 			public static readonly int START_C_LEN = START_C.Length;
// 			public static readonly int END_LEN = END.Length;
//
// 			// public static readonly int P_START_LEN = P_START.Length;
// 			// public static readonly int P_END_LEN = P_END.Length;
// 			// public static readonly int P_FLAG_LEN = P_FLAG.Length;
// 			// public static readonly int P_DIVIDER_LEN = P_DIVIDER.Length;
// 			//
// 			// public static readonly int MIN_START_O_LEN = END_LEN + START_O_LEN + 1;
// 			// public static readonly int MIN_START_C_LEN = END_LEN + START_C_LEN + 1;
// 			//
// 			// public static readonly int MIN_P_LEN = 1;
//
// 			// separate out the 3 code sections
// 			// test "D. ... bad" (no good)                     \s*(?'AA'.*?)(?'F'(?>\s*?)(?><(?'C'.*?)(?>(?>(?>\s*=\s*(?<ZX>{)\s*?((?'A1'.*?(\s*?;|\s*>)))*\s*?(?'A2'.*?)\s*?(?<-ZX>}\s*?)>))|(?>>)))(?'V'.*)(?></\k'C'>))(?'ZZ'.*)|(?'AA'.+)
// 			// test "divide formatted ..."   (no good)            (?'AA'.*?)(?>(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>))))(?'ZZ'.*)|(?'AA'.+)
// 			// test "C. divide formatted text"  (no good)      \s*(?'AA'.*?)(?'F'(?>\s*?)(?><(?'C'.*?)(?>(?>(?>\s*=\s*{\s*(?>(?'A'.*?)\s*;+\s*)*(?'A'.*?)\s*})>)|(?>>)))(?'V'.*)(?></\k'C'>))(?'ZZ'.*)|(?'AA'.+)
// 			// good - before add solo tags                        (?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>)))))(?'ZZ'.*)|(?'AA'.+)
// 			// best time with solo tags                           (?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>)))))(?'ZZ'.+)|(?>(?'AA'.*?)(?'S'<.*?/>)(?'ZZ'.*?|$))|(?'ZZ'.+)
// 			// best?? with solo tags                              (?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>)))))(?'ZZ'.+)|^(?>(?'AA'.*?)(?'S'<.*?/>)(?'ZZ'.+|$))|(?'ZZ'.+)
// 			// with solo tags 2                                   (?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>)))))(?'ZZ'.+)|^(?>(?'AA'.*?)(?><(?'S'.*?)/>)(?'ZZ'.+|$))|(?'ZZ'.+)
// 			private static string fmtCodePatternA =             @"(?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>)))))(?'ZZ'.+)|^(?>(?'AA'.*?)(?><(?'S'.*?)/>)(?'ZZ'.+|$))|(?'ZZ'.+)";
// 			
// 			// // text "x. ..." incorporate solo tags
// 			// private static string fmtCodePatternX =             @"(?'AA'.*?)(?>(?'F'(?>\s*?)(?'a'<(?'C'.*?)\s*?)(?>(?>(?>(?>\s*)(?'A'.*?)>)(?'T'.*)(?'z1'</\k'C'>)))))(?'ZZ'.+)|(?>(?'AA'.*?)(?'S'<.*?/>)(?'ZZ'.+|$))|(?'ZZ'.+)";
// 			
// 			// from what this provides
// 			// use split to get the individual attributes and it's parameters
//
// 			// use this to divide an attribute and it's parameters into separate parts
// 			private static string fmtCodePatternB =             @"^\s*(?>(?'a'.*?)\s*=\s*(?>{\s*(?>(?'p'.*?)\s*,\s*)*\s*(?'p'.*?)})\s*)|(?'z'.+)";
//
// 			// private static string fmtCodePattern =
// 			// 	@"\s*(?'AA'.*?)(?'F'(?>\s*?)(?><(?'C'.*?)(?>(?>(?>\s*=\s*{\s*(?>(?'A'.*?)\s*;+\s*)*?(?'A'.*?)\s*}\s*)>)|(?>>)))(?'V'.*)(?></\k'C'>))(?'ZZ'.*)|(?'AA'.+)";
//
// 			// private static Regex fmtCodeRx = new Regex(fmtCodePattern);
//
// 			private static Regex fmtCodeRxA = new Regex(fmtCodePatternA);
// 			private static Regex fmtCodeRxB = new Regex(fmtCodePatternB);
//
// 			public FmtCode() { }
//
// 			public string FmtTag { get; set; } = null;
// 			public string[] Attrs { get; set; } = null;
// 			public string FmtStr { get; set; } = null;
// 			public int Length => FmtStr.IsVoid() ? -1 : FmtStr.Length;
//
// 			public bool IsValid => Length > 0 && !FmtTag.IsVoid();
//
// 			public Tuple<FmtTypeCode, object> FormatData { get; set; } = null;
//
// 			// static
//
// 			// v7
//
// 			public static FmtCode DivideText(string text, out string p1, out string p2, out string p3, out string p4)
// 			{
// 				p1 = null;
// 				p2 = null;
// 				p3 = null;
// 				p4 = null;
//
// 				Match match = fmtCodeRxA.Match(text);
//
// 				FmtCode fc = new FmtCode();
//
// 				if (!match.Success) return fc;
//
// 				// preface text
// 				p1 = getSingleGroupItem(match.Groups["AA"]);
// 				// suffix text
// 				p3 = getSingleGroupItem(match.Groups["ZZ"]);
// 				// inner text
// 				p2 = getSingleGroupItem(match.Groups["T"]);
//
// 				if (!p2.IsVoid())
// 				{
// 					fc.FmtStr = getSingleGroupItem(match.Groups["F"]);
//
// 					fc.FmtTag = getSingleGroupItem(match.Groups["C"]);
//
// 					// fc.Attrs = getArrayGroupItem(match.Groups["A"]);
// 					fc.Attrs = getArrayGroupItem(match.Groups["A"]);
// 				} 
// 				else
// 				{
// 					// solo tag
// 					p4 = getSingleGroupItem(match.Groups["S"]);
//
// 					if (!p4.IsVoid())
// 					{
// 						fc.FmtTag = p4;
// 						fc.FmtStr = p4;
// 						fc.Attrs = getArrayGroupItem(match.Groups["A"]);
// 					}
// 				}
//
// 				return fc;
// 			}
//
// 			public static string[][] DivideParameters(string attribute)
// 			{
// 				string[][] p = new string[2][];
//
// 				Match match = fmtCodeRxB.Match(attribute);
//
// 				FmtCode fc = new FmtCode();
//
// 				if (!match.Success) return null;
//
// 				// get the attribute
// 				p[0] = new string[1];
// 				p[0][0] = getSingleGroupItem(match.Groups["a"]);
//
// 				p[1] = new string[match.Groups["p"].Captures.Count];
//
// 				if (p[1].Length > 0)
// 				{
// 					for (int i = 0; i < p[1].Length; i++)
// 					{
// 						p[1][i] = match.Groups["p"].Captures[i].Value;
// 					}
// 				}
//
// 				return p;
// 			}
//
// 			public static bool HasCode(string text) => (text?.IndexOf(START_O) ?? -1) >= 0 ;
//
// 			private static string getSingleGroupItem(Group g)
// 			{
// 				if (g.Captures.Count == 0) return null;
//
// 				return g.Captures[0].Value;
// 			}
//
// 			private static string[] getArrayGroupItem(Group g)
// 			{
// 				string [] result;
//
// 				if (g.Captures.Count == 0 || g.Length == 0) return null;
//
// 				result = g.Captures[0].Value.Split(new char[] { ';' });
//
// 				return result;
// 			}
//
// 		}
// 		
// 	}
// }