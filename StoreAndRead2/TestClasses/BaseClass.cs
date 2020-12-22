#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion

// username: jeffs
// created:  12/20/2020 9:52:30 PM

namespace StoreAndRead.TestClasses
{
	public interface IBaseClass
	{
		void RaiseHand();
	}

	public class BaseClass : IBaseClass
	{
		protected string name;

		private string testString;

		public static BaseClass bc = new BaseClass("root");

		public BaseClass(string name)
		{
			this.name = name;
			Init();
		}

		public void Init()
		{
			if (bc == null) return;

			bc.CommonFunction += bcCommonFunction;
		}

		public string Name
		{
			get => name;
			set
			{
				name = value;
			}
		}

		public virtual void RaiseHand()
		{
			Debug.WriteLine("I can do this BaseClass| " + name);
		}

		private void bcCommonFunction(object sender)
		{
			this.RaiseHand();
		}

		public delegate void CommonFunctionEventHandler(object sender);

		public event BaseClass.CommonFunctionEventHandler CommonFunction;

		public virtual void RaiseCommonFunctionEvent()
		{
			CommonFunction?.Invoke(this);
		}

		public override string ToString()
		{
			return "this is BaseClass| " + name;
		}
	}

}