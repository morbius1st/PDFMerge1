// Solution:     PDFMerge1
// Project:       ClassifierEditor
// File:             MwClassifEdTemplateSelectors.cs
// Created:      2024-10-14 (10:19 PM)

using System.Windows;
using System.Windows.Controls;
using AndyShared.ClassificationDataSupport.TreeSupport;

namespace ClassifierEditor.Windows
{

	public class Lv1ConditionTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;

			if (element != null && item != null && item is ComparisonOperation)
			{
				ComparisonOperation taskitem = item as ComparisonOperation;

				if ((taskitem.ValueCompOpDef?.OpCodeValue ?? (int) ValueComparisonOp.VALUE_NO_OP)
					== (int) ValueComparisonOp.VALUE_NO_OP)
				{
					return
						element.FindResource("Lv1DataTemplate3") as DataTemplate;
				}

				return
					element.FindResource("Lv1DataTemplate0") as DataTemplate;


				// if (taskitem.ValueCompOpDef is LogicalCompOpDef)
				// {
				// 	return
				// 		element.FindResource("Lv1DataTemplate2") as DataTemplate;
				// }
				// else if (taskitem.ValueCompOpDef.OpCodeValue == (int) NO_OP)
				// {
				// 	return
				// 		element.FindResource("Lv1DataTemplate3") as DataTemplate;
				// }
				// else
				// {
				// 	return
				// 		element.FindResource("Lv1DataTemplate1") as DataTemplate;
				// }
			}

			return null;
		}
	}

	public class Lv2ConditionTemplateSelector : DataTemplateSelector
	{
		public static int MasterIdIdx;

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;

			if (element != null && item != null && item is ComparisonOperation)
			{
				ComparisonOperation taskitem = item as ComparisonOperation;

				taskitem.Id = MasterIdIdx++;

				// if (taskitem.ValueCompOpCode == (int) VALUE_NO_OP)
				if (taskitem.ValueCompOpCode == (int) ValueComparisonOp.VALUE_NO_OP)
				{
					return
						element.FindResource("Lv2DataTemplate3") as DataTemplate;
				}

				return
					element.FindResource("Lv2DataTemplate0") as DataTemplate;
			}

			return null;
		}
	}

	public class Lv3TemplateSelector3 : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;

			if (element != null && item != null && item is ComparisonOperation)
			{
				ComparisonOperation taskitem = item as ComparisonOperation;

				if ((taskitem.ValueCompOpDef?.OpCodeValue ?? (int) ValueComparisonOp.VALUE_NO_OP)
					== (int) ValueComparisonOp.VALUE_NO_OP)
				{
					return
						element.FindResource("Lv3DataTemplate33") as DataTemplate;
				}

				return
					element.FindResource("Lv3DataTemplate30") as DataTemplate;


				// if (taskitem.ValueCompOpDef is LogicalCompOpDef)
				// {
				// 	return
				// 		element.FindResource("Lv1DataTemplate2") as DataTemplate;
				// }
				// else if (taskitem.ValueCompOpDef.OpCodeValue == (int) NO_OP)
				// {
				// 	return
				// 		element.FindResource("Lv1DataTemplate3") as DataTemplate;
				// }
				// else
				// {
				// 	return
				// 		element.FindResource("Lv1DataTemplate1") as DataTemplate;
				// }
			}

			return null;
		}
	}


}