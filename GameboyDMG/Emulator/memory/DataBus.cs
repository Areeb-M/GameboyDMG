using System;

namespace Emulator
{
	public class DataBus<T>
	{
		T data;
		
		public DataBus(T val)
		{
			data = val;
		}
		
		public virtual T Data
		{
			get { return data; }
			set { data = value;}
		}
	}
}