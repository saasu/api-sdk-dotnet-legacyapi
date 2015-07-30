using Ola.RestClient.Dto.Inventory;

namespace Ola.RestClient.NUnitTests
{
	using NUnit.Framework;

	using Ola.RestClient.Dto;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Proxies;
	using System;
	using System.Collections;


	[TestFixture]
	public class InventoryItemTests : RestClientTests
	{
		[Test]
		public void InsertAndGet()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			proxy.Insert(dto1);
			
			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");
			
			InventoryItemDto dto2 = (InventoryItemDto) proxy.GetByUid(dto1.Uid);
			
			AssertEqual(dto1, dto2);
		}


		[Test]
		public void Update()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			proxy.Insert(dto1);
			
			string lastUpdatedUid = dto1.LastUpdatedUid;
			
			dto1.Description = this.TestUid + "For Test Update - Updated";
			dto1.Notes = "Updated ....";
			dto1.AssetAccountUid = this.AssetInventory2.Uid;
			dto1.SaleTaxCode = TaxCode.SaleExports;
			
			proxy.Update(dto1);
			
			Assert.IsTrue(dto1.LastUpdatedUid != lastUpdatedUid);
			
			InventoryItemDto dto2 = (InventoryItemDto) proxy.GetByUid(dto1.Uid);
			
			AssertEqual(dto1, dto2);
		}


		[Test]
		public void Delete()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			proxy.Insert(dto1);
			
			proxy.DeleteByUid(dto1.Uid);

			try
			{
				proxy.GetByUid(dto1.Uid);
			}
			catch(RestException ex)
			{
				Assert.AreEqual("RecordNotFoundException", ex.Type);
			}
		}


		[Test]
		public void UpdateDeleted()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			proxy.Insert(dto1);

			proxy.DeleteByUid(dto1.Uid);

			dto1.Description = "Try updating deleted item";

			try
			{
				proxy.Update(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException rex)
			{
				if (!rex.Message.Contains("Unable to find inventory item uid"))
				{
					throw new Exception("Possibly incorrect error thrown.");
				}
			}
		}


		[Test]
		public void InsertSellPriceExcTax()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			dto1.SellingPrice = 7.75M;
			dto1.IsSellingPriceIncTax = false;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InventoryItemDto dto2 = (InventoryItemDto)proxy.GetByUid(dto1.Uid);

			Assert.AreEqual(7.75M, dto1.SellingPrice, "Diff. selling price");
			Assert.AreEqual(false, dto1.IsSellingPriceIncTax, "Is selling price exc tax should be false.");
		}


		[Test]
		public void InsertSellPriceIncTax()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			dto1.SellingPrice = 7.75M;
			dto1.IsSellingPriceIncTax = true;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InventoryItemDto dto2 = (InventoryItemDto)proxy.GetByUid(dto1.Uid);

			Assert.AreEqual(7.75M, dto1.SellingPrice, "Diff. selling price");
			Assert.AreEqual(true, dto1.IsSellingPriceIncTax, "Is selling price exc tax should be true.");
		}


		[Test]
		public void InsertUsingDeprecatedRrpInclTax()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			dto1.RrpInclTax = 7.75M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InventoryItemDto dto2 = (InventoryItemDto)proxy.GetByUid(dto1.Uid);

			Assert.AreEqual(7.75M, dto2.RrpInclTax, "Diff. rrp. incl. tax.");
			Assert.AreEqual(7.75M, dto2.SellingPrice, "Diff. selling price.");
			Assert.AreEqual(true, dto2.IsSellingPriceIncTax, "Is selling price exc tax should be true.");
		}


		[Test]
		public void CheckQuantityOnOrderAndQuantityCommitted()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			dto1.RrpInclTax = 7.75M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto saleOrder = this.GetSaleOrder(dto1);
			CrudProxy invoice = new InvoiceProxy();
			invoice.Insert(saleOrder);

			dto1 = (InventoryItemDto) proxy.GetByUid(dto1.Uid);
			Assert.AreEqual(5, dto1.QuantityCommitted, "Incorrect # of stocks committed (used by sale orders)");

			InvoiceDto po = this.GetPurchaseOrder(dto1);
			po.PurchaseOrderNumber = "<Auto Number>";
			invoice.Insert(po);

			dto1 = (InventoryItemDto)proxy.GetByUid(dto1.Uid);
			Assert.AreEqual(5, dto1.QuantityCommitted, "Incorrect # of stocks committed (used by sale orders)");

			dto1 = (InventoryItemDto)proxy.GetByUid(dto1.Uid);
			Assert.AreEqual(20, dto1.QuantityOnOrder, "Incorrect # of stocks on order(used by purchase orders)");
		}


		[Test]
		public void InsertWithDefaultBuyingPrice()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			dto1.BuyingPrice = 250M;
			dto1.IsBuyingPriceIncTax = true;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InventoryItemDto dto2 = (InventoryItemDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2);
		}


		private InvoiceDto GetSaleOrder(InventoryItemDto itemDto)
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Item);

			dto.Date = DateTime.Parse("6-Oct-05");
			dto.ContactUid = this.MrSmith.Uid;
			dto.Summary = "Test Insert Item Sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("6-Nov-05");
			dto.Layout = InvoiceLayout.Item;
			dto.Status = InvoiceStatus.Order;
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO333";

			ItemInvoiceItemDto item = null;

			item = new ItemInvoiceItemDto();
			item.Quantity = 5;
			item.InventoryItemUid = itemDto.Uid;
			item.Description = "Asus Laptop";
			item.TaxCode = TaxCode.SaleInclGst;
			item.UnitPriceInclTax = 1200.75M;
			item.PercentageDiscount = 12.50M;

			dto.Items.Add(item);

			return dto;
		}


		private InvoiceDto GetPurchaseOrder(InventoryItemDto itemDto)
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Purchase, InvoiceLayout.Item);

			dto.Date = DateTime.Parse("6-Oct-05");
			dto.ContactUid = this.MrSmith.Uid;
			dto.Summary = "Test Insert Item Purchase";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("6-Nov-05");
			dto.Layout = InvoiceLayout.Item;
			dto.Status = InvoiceStatus.Order;
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO333";
			
			ItemInvoiceItemDto item = null;

			item = new ItemInvoiceItemDto();
			item.Quantity = 20;
			item.InventoryItemUid = itemDto.Uid;
			item.Description = "Asus Laptop";
			item.TaxCode = TaxCode.SaleInclGst;
			item.UnitPriceInclTax = 1200.75M;
			item.PercentageDiscount = 12.50M;

			dto.Items.Add(item);

			return dto;
		}


		[Test]
		public void InsertVoucherItem()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			dto1.IsVoucher = true;
			dto1.ValidFrom = DateTime.Now.Date;
			dto1.ValidTo = dto1.ValidFrom.AddMonths(3);

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InventoryItemDto dto2 = (InventoryItemDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2);
		}


		[Test]
		public void InsertVirtualItem()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			dto1.IsVirtual = true;
			dto1.VType = "Plan";

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InventoryItemDto dto2 = (InventoryItemDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2); 
		}


		[Test]
		public void InsertWithVisibleSet()
		{
			CrudProxy proxy = new InventoryItemProxy();
			InventoryItemDto dto1 = this.GetInventoryItem();
			dto1.IsVisible = true;

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InventoryItemDto dto2 = (InventoryItemDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2); 
		}


        [Test]
        public void TestAverageCostIsReturnedForInventoryItem()
        {
            CrudProxy proxy = new InventoryAdjustmentProxy();
            InventoryAdjustmentDto dto1 = this.GetInventoryAdjustment();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            proxy = new InventoryItemProxy();
            InventoryItemDto itemDto  = (InventoryItemDto)proxy.GetByUid(HardDisk.Uid);

            Assert.IsTrue(itemDto.AverageCost == 200.50M);
        }


		public static void AssertEqual(InventoryItemDto expected, InventoryItemDto actual)
		{
			Assert.AreEqual(expected.Code, actual.Code, "Incorrect Code.");
			Assert.AreEqual(expected.Description, actual.Description, "Incorrect description.");
			Assert.AreEqual(expected.Notes, actual.Notes, "Incorrect Notes.");
			Assert.AreEqual(expected.IsActive, actual.IsActive, "Incorrect IsActive.");
			Assert.AreEqual(expected.MinimumStockLevel, actual.MinimumStockLevel, "Incorrect MinimumStockLevel");
			Assert.AreEqual(expected.StockOnHand, actual.StockOnHand, "Incorrect StockOnHand");
			Assert.AreEqual(expected.IsInventoried, actual.IsInventoried, "Incorrect IsInventoried.");
			Assert.AreEqual(expected.AssetAccountUid, actual.AssetAccountUid, "Incorrect AssetAccountUid.");
			Assert.AreEqual(expected.IsSold, actual.IsSold, "Incorrect IsSold.");
			Assert.AreEqual(expected.SaleIncomeAccountUid, actual.SaleIncomeAccountUid, "Incorrect SaleIncomeAccountUid.");
			Assert.AreEqual(expected.SaleTaxCode, actual.SaleTaxCode, "Incorrect SaleTaxCode.");
			Assert.AreEqual(expected.SaleCoSAccountUid, actual.SaleCoSAccountUid, "Incorrect SaleCoSAccountUid.");
			Assert.AreEqual(expected.RrpInclTax, actual.RrpInclTax, "Incorrect RrpInclTax.");
			Assert.AreEqual(expected.IsBought, actual.IsBought, "Incorrect IsBought.");
			Assert.AreEqual(expected.PurchaseExpenseAccountUid, actual.PurchaseExpenseAccountUid, "Incorrect PurchaseExpenseAccountUid.");
			Assert.AreEqual(expected.PurchaseTaxCode, actual.PurchaseTaxCode, "Incorrect PurchaseTaxType.");
			Assert.AreEqual(expected.PrimarySupplierContactUid, actual.PrimarySupplierContactUid, "Incorrect PrimarySupplierContactUid.");
			Assert.AreEqual(expected.PrimarySupplierItemCode, actual.PrimarySupplierItemCode, "Incorrect PrimarySupplierItemCode.");
			Assert.AreEqual(expected.DefaultReOrderQuantity, actual.DefaultReOrderQuantity, "Incorrect DefaultReOrderQuantity.");
			Assert.AreEqual(expected.BuyingPrice, actual.BuyingPrice, "Incorrect BuyingPrice");
			Assert.AreEqual(expected.IsBuyingPriceIncTax, actual.IsBuyingPriceIncTax, "Incorrect IsBuyingPriceIncTax.");
			Assert.AreEqual(expected.IsVoucher, actual.IsVoucher, "Incorrect IsVoucher.");
			Assert.AreEqual(expected.ValidFrom, actual.ValidFrom, "Incorrect ValidFrom.");
			Assert.AreEqual(expected.ValidTo, actual.ValidTo, "Incorrect ValidTo.");
		}

		
		private InventoryItemDto GetInventoryItem()
		{
			string code = "CODE_" + System.Guid.NewGuid().ToString().Remove(0, 10);
			
			InventoryItemDto info	= new InventoryItemDto();
			info.Code				= code;
			info.Description		= "Description for " + code;
			info.Notes				= "Notes for " + code;
			info.IsActive			= true;
			info.MinimumStockLevel	= 99;
			
			info.IsInventoried		= true;
			info.AssetAccountUid	= this.AssetInventory.Uid;
			
			info.IsSold					= true;
			info.SaleIncomeAccountUid	= this.IncomeHardwareSales.Uid;
			info.SaleTaxCode			= TaxCode.SaleInclGst;
			info.SaleCoSAccountUid		= this.CoSHardware.Uid;
			info.RrpInclTax				= 19.95M;
			
			info.IsBought							= true;
			info.PurchaseTaxCode					= TaxCode.ExpInclGst;
			info.PrimarySupplierContactUid			= this.MrsSmith.Uid;
			info.PrimarySupplierItemCode			= "S_CODE";
			info.DefaultReOrderQuantity	= 20;
			
			return info;
		}

        private InventoryAdjustmentDto GetInventoryAdjustment()
        {
            InventoryAdjustmentDto dto = new InventoryAdjustmentDto();
            dto.Date = DateTime.Now;
            dto.Summary = "Test Average Cost is returned for inventory item";                      

            InventoryAdjustmentItemDto item = new InventoryAdjustmentItemDto();
            item.Quantity = 2;
            item.InventoryItemUid = this.HardDisk.Uid;
            item.AccountUid = this.ExpenseOffice.Uid;
            item.UnitPriceExclTax = 200.50M;
            item.TotalPriceExclTax = 401.00M;
            dto.Items.Add(item);

            return dto;
        }
	}
}
