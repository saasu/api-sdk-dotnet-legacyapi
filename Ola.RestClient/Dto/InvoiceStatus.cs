using System;

namespace Ola.RestClient.Dto
{
	public class InvoiceStatus
	{
		public static readonly string Quote = "Q";
		public static readonly string Order = "O";
		public static readonly string Invoice = "I";


		private InvoiceStatus()
		{
		}
	}
}
