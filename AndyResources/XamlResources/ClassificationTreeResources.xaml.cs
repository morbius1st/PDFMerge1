#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#endregion

// user name: jeffs
// created:   11/18/2024 10:47:44 PM

namespace AndyResources.XamlResources
{
	public partial class ClassificationTreeResources
	{

		private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				MouseWheelEventArgs eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
				eventArg.RoutedEvent = UIElement.MouseWheelEvent;
				eventArg.Source = sender;
				var parent = ((Control)sender).Parent as UIElement;
				parent.RaiseEvent(eventArg);
			}
		}

		public override string ToString()
		{
			return $"this is {nameof(ClassificationTreeResources)}";
		}
	}
}
