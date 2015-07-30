namespace Ola.RestClient.Dto
{
	public class TransactionType
	{
		private TransactionType() { }
		
		public static readonly string	Sale						= "S";
		public static readonly string	Purchase					= "P";
		public static readonly string	SalePayment					= "SP";
		public static readonly string	PurchasePayment				= "PP";
		public static readonly string	GeneralJournal				= "GJ";
		public static readonly string	BankTransfer				= "BT";
		public static readonly string	PayrollEntry				= "PE";
		public static readonly string	AccountOpeningBalance		= "AOB";
		public static readonly string	InventoryAdjustment			= "IA";
		public static readonly string	InventoryOpeningBalance		= "IOB";
		public static readonly string	InventoryTransfer			= "IT";
	}
}
