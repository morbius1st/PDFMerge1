#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using AODeliverable.FileSelection;
using AODeliverable.Support;

using static UtilityLibrary.MessageUtilities;

#endregion


// projname: AODeliverable.OutlineSupport
// itemname: OutlineSettings
// username: jeffs
// created:  11/3/2019 1:10:40 PM


namespace AODeliverable.OutlineSupport
{
	public class OutlineSettings
	{
		private List<OutlineDefinition> Outlines = new List<OutlineDefinition>();

		public OutlineSettings()
		{
			TempData();
		}

		public FileItem AdjustOutlineByFilter(FileItem fi)
		{
			string firstDir = fi.outlinePath.GetFirstDirectoryName();
			string remainDir = fi.outlinePath.Substring(firstDir.Length);
			string newPath = fi.outlinePath;

			for (int i = 0; i < Outlines.Count; i++)
			{
				logMsgDbLn2("test pattern", Outlines[i].Regex);
				if (Outlines[i].Regex.IsMatch(fi.getName()))
				{
					newPath = "\\" + Outlines[i].OutlinePath
						+ remainDir;
					logMsgDbLn2("found| new path", newPath);

					fi.outlinePath = newPath;

					// depth adjustment is not needed - this is
					// calculated on the fly

					logMsgDbLn2("");

					break;
				}
			}

			return fi;
		}


//		public string AdjustOutlineByFilter(string outlinePath, string fileName)
//		{
//			string firstDir = outlinePath.GetFirstDirectoryName();
//			string remainDir = outlinePath.Substring(firstDir.Length);
//			string newPath = outlinePath;
//
//			for (int i = 0; i < Outlines.Count; i++)
//			{
//				if (Outlines[i].Regex.IsMatch(fileName))
//				{
//					newPath = "\\" + Outlines[i].OutlinePath
//						+ remainDir;
//				}
//			}
//
//			return newPath;
//		}



		private struct OutlineDefinition : IComparable<OutlineDefinition>
		{
			public OutlineDefinition(  Regex regex, string outlinePath, int depthAdjustment, string description)
			{
				this.Regex = regex;
				OutlinePath = outlinePath;
				DepthAdjustment = depthAdjustment;
				Description = description;

			}

			public Regex Regex { get; private set; }
			public string OutlinePath { get; private set; }
			public int DepthAdjustment { get; private set; }
			public string Description { get; private set; }


			public int CompareTo(OutlineDefinition other)
			{
				return other.DepthAdjustment - DepthAdjustment;
			}
		}

		private void TempData()
		{
			Outlines.Add(new OutlineDefinition(
					new Regex(@"^[A-Z] ?A", RegexOptions.Compiled),
					"070 Arch", 0, "Typical Architectural"));

			Outlines.Add(new OutlineDefinition(
					new Regex(@"^[A-Z] ?A1", RegexOptions.Compiled),
					"070 Arch\\01 Site", 1, "Architectural Site Sheets")
				);

			Outlines.Add(new OutlineDefinition(
					new Regex(@"^[A-Z] ?A2", RegexOptions.Compiled),
					"070 Arch\\02 Plans", 1, "Architectural Plan Sheets")
				);

			Outlines.Add(new OutlineDefinition(
					new Regex(@"^[A-Z] ?A3", RegexOptions.Compiled),
					"070 Arch\\03 Vert Circ", 1, "Architectural Vertical Circulation Sheets")
				);

			Outlines.Add(new OutlineDefinition(
					new Regex(@"^[A-Z] ?S", RegexOptions.Compiled),
					"110 Struct", 0, "Structural Sheets")
				);

			Outlines.Add(new OutlineDefinition(
					new Regex(@"^[A-Z] ?M", RegexOptions.Compiled),
					"150 Mech", 0, "Mechanical Sheets")
				);

			Outlines.Add(new OutlineDefinition(
					new Regex(@"^[A-Z] ?P", RegexOptions.Compiled),
					"130 Plumb", 0, "Plumbing Sheets")
				);

			Outlines.Add(new OutlineDefinition(
					new Regex(@"^[A-Z] ?T", RegexOptions.Compiled),
					"000 Title", 0, "Architectural Title Sheets")
				);

			Outlines.Add(new OutlineDefinition(
					new Regex(@"^[A-Z] ?C", RegexOptions.Compiled),
					"000 Cover", 0, "Architectural Cover Sheets")
				);

			Outlines.Add(new OutlineDefinition(
					new Regex(@"^C ?\d", RegexOptions.Compiled),
					"010 Civil", 0, "Civil For Reference Sheets")
				);

			Outlines.Sort();
		}
	}
}
