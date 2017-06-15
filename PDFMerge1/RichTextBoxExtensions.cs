using System;
using System.Drawing;
using System.Windows.Forms;

namespace PDFMerge1
{
	public static class RichTextBoxExtensions
	{

		public static void AppendText(this RichTextBox rtb, string text, Color color, Font font)
		{
			rtb.SuspendLayout();

			if (color != null || font != null)
			{
				rtb.SelectionStart = rtb.TextLength;
				rtb.SelectionLength = 0;

				if (color != null)
				{
					rtb.SelectionColor = color;
				}
				if (font != null)
				{
					rtb.SelectionFont = font;
				}
				rtb.AppendText(text);
				rtb.SelectionColor = rtb.ForeColor;
				
			} else
			{
				rtb.AppendText(text);
			}

			rtb.ScrollToCaret();
			rtb.ResumeLayout();
		}
	}

}