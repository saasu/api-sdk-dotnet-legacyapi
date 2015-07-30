using System.Collections.Generic;

namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml.Serialization;

	[XmlRoot(ElementName = "inventoryItem")]
	public class InventoryItemDto : EntityDto
	{
		[XmlElement(ElementName = "code")]
		public string	Code;

		[XmlElement(ElementName = "description")]
		public string	Description;

		[XmlElement(ElementName = "isActive")]
		public bool		IsActive = true;

		[XmlElement(ElementName = "notes")]
		public string	Notes;

		[XmlElement(ElementName = "isInventoried")]
		public bool		IsInventoried;

		[XmlElement(ElementName = "assetAccountUid")]
		public int		AssetAccountUid;

		[XmlElement(ElementName = "stockOnHand")]
		public decimal	StockOnHand;

		[XmlElement(ElementName = "currentValue")]
		public decimal	CurrentValue;

	    [XmlElement(ElementName = "averageCost")] 
        public decimal AverageCost;

		[XmlElement(ElementName = "quantityOnOrder")]
		public decimal QuantityOnOrder;

		[XmlElement(ElementName = "quantityCommitted")]
		public decimal QuantityCommitted;

		[XmlElement(ElementName = "isBought")]
		public bool		IsBought;

		[XmlElement(ElementName = "purchaseExpenseAccountUid")]
		public int		PurchaseExpenseAccountUid;

		[XmlElement(ElementName = "purchaseTaxCode")]
		public string	PurchaseTaxCode;

		[XmlElement(ElementName = "minimumStockLevel")]
		public decimal	MinimumStockLevel;

		[XmlElement(ElementName = "primarySupplierContactUid")]
		public int		PrimarySupplierContactUid;

		[XmlElement(ElementName = "primarySupplierItemCode")]
		public string	PrimarySupplierItemCode;

		[XmlElement(ElementName = "defaultReOrderQuantity")]
		public decimal	DefaultReOrderQuantity;

		[XmlElement(ElementName = "isSold")]
		public bool		IsSold;

		[XmlElement(ElementName = "saleIncomeAccountUid")]
		public int		SaleIncomeAccountUid;

		[XmlElement(ElementName = "saleTaxCode")]
		public string	SaleTaxCode;

		[XmlElement(ElementName = "saleCoSAccountUid")]
		public int		SaleCoSAccountUid;

		/// <summary>
		/// Deprecated. Don't use this. Use "SellingPrice" and "IsSellingPriceIncTax" instead.
		/// </summary>
		[XmlElement(ElementName = "rrpInclTax")]
		public decimal	RrpInclTax;

		[XmlElement(ElementName = "sellingPrice")]
		public decimal SellingPrice;

		[XmlElement(ElementName = "isSellingPriceIncTax")]
		public bool IsSellingPriceIncTax;

		[XmlElement(ElementName = "buyingPrice")]
		public decimal BuyingPrice;

		[XmlElement(ElementName = "isBuyingPriceIncTax")]
		public bool IsBuyingPriceIncTax;

		[XmlElement(ElementName = "isVoucher")]
		public bool IsVoucher;

		[XmlElement(ElementName = "validFrom")]
		public DateTime ValidFrom;

		[XmlElement(ElementName = "validTo")]
		public DateTime ValidTo;

		[XmlElement(ElementName = "isVirtual")]
		public bool IsVirtual;

		[XmlElement(ElementName = "vType")]
		public string VType;

		[XmlElement(ElementName = "isVisible")]
		public bool IsVisible; 
	}
}
