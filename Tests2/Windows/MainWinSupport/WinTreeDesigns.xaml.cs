using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace Tests2.Windows.MainWinSupport
{
    class WinTreeDesigns
    {
		// user settings
		public static readonly double ToggleBtnSize = 11;

		public static readonly double TvTreeMarginLeft = 6;
		public static readonly double TvTreeMarginRight = 3;
		private static readonly double TvToggBtnColWidthRightPerferred = 10;

		public static readonly double TvVertConnThickness = 2;
		public static readonly double TvHorizConnThickness = 1;

		public static readonly double ToggleBtnHorizLineHeightChecked = 2;


		// calculated settings
//		public static GridLength TvRowHeight = new GridLength(TvBranchFontSize * 1.3);
//		public static readonly double TvRowHeightHalf = TvRowHeight.Value / 2;



		public static readonly double ToggleBtnWidth =
			ToggleBtnSize > TOGG_BTN_MIN_SIZE ? ToggleBtnSize : TOGG_BTN_MIN_SIZE;
		public static readonly double ToggleBtnHeight = ToggleBtnWidth;
		public static readonly double ToggleBtnPlusWidth = ToggleBtnWidth - 5;
		public static readonly double ToggleBtnPlusHeight = ToggleBtnHeight - 5;


		private static readonly double TvToggBtnColWidthRightMin = ToggleBtnWidth / 2 + 4;
		public static readonly double TvToggBtnColWidthRight = 
			TvToggBtnColWidthRightMin > TvToggBtnColWidthRightPerferred ? TvToggBtnColWidthRightMin : TvToggBtnColWidthRightPerferred;

		public static readonly double TvToggBtnColWidthLeft = (TvToggBtnColWidthRight / 2);


		public static readonly GridLength TvTreeMargLeft = new GridLength(TvTreeMarginLeft);
		public static readonly GridLength TvTreeMargRight = new GridLength(TvTreeMarginRight);

		public static readonly Thickness TvVertConnThk = new Thickness(0, 0, TvVertConnThickness, 0);
		public static readonly Thickness TvHorizConnThk = new Thickness(0, 0, 0, TvHorizConnThickness);

		public static readonly Thickness ToggleBtnMargin = 
			new Thickness(TvToggBtnColWidthLeft - ToggleBtnWidth / 2, 0, 0, 0);


		// fixed values
		private const int TOGG_BTN_MIN_SIZE = 9;

	}
}
