namespace Ola.RestClient.Dto
{
	using System;

	public class AccountType
	{
		public static readonly string Income		= "Income";
		public static readonly string Expense		= "Expense";
		public static readonly string Asset			= "Asset";
		public static readonly string Equity		= "Equity";
		public static readonly string Liability		= "Liability";
		public static readonly string OtherIncome	= "Other Income";
		public static readonly string OtherExpense	= "Other Expense";
		public static readonly string CostOfSales	= "Cost of Sales";


		private AccountType()
		{
		}
	}
}
