using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Ola.RestClient.Dto;
using Ola.RestClient.Dto.Inventory;
using Ola.RestClient.Exceptions;
using Ola.RestClient.Proxies;
using Ola.RestClient.Tasks;
using Ola.RestClient.Tasks.Inventory;

namespace Ola.RestClient.NUnitTests.Inventory
{
    [TestFixture]
    public class ComboItemTests : RestClientTests
    {
        [Test]
        public void TestGetByUid()
        {
			ComboItemDto comboItemDto = this.GetComboItem01();

			ComboItemProxy proxy = new ComboItemProxy();
			proxy.Insert(comboItemDto);

			Assert.AreNotEqual(0, comboItemDto.Uid);

			ComboItemDto result = (ComboItemDto)proxy.GetByUid(comboItemDto.Uid);

            Assert.AreEqual(2, result.Items.Count);

			proxy.DeleteByUid(comboItemDto.Uid);

			try
			{
				proxy.GetByUid(comboItemDto.Uid);
			}
			catch (RestException ex)
			{
				StringAssert.Contains("Unable to find inventory item uid ", ex.Message);
			}
        }


        [Test]
        public void BuildComboItem()
        {
            ComboItemProxy proxy = new ComboItemProxy();

			ComboItemDto comboItemDto = this.GetComboItem01();
			proxy.Insert(comboItemDto);
			ComboItemDto comboItem = (ComboItemDto)proxy.GetByUid(comboItemDto.Uid);

            // We need to insert stock first using a purchase
			CrudProxy invoiceProxy = new InvoiceProxy();
			InvoiceDto dto = new InvoiceDto(TransactionType.Purchase, InvoiceLayout.Item);
            dto.Date = DateTime.Today.Date;
            dto.ContactUid = this.MrSmith.Uid;
            dto.Summary = "Add stock do we can build ComboItems";
            dto.Notes = "From REST";
            dto.DueOrExpiryDate = dto.Date.AddMonths(1);
            dto.Status = InvoiceStatus.Invoice;
            dto.InvoiceNumber = "I123";
            dto.PurchaseOrderNumber = "<Auto Number>";

        	decimal unitsToBuild = 12.25M;

            foreach(ComboItemLineItemDto itemInCombo in comboItem.Items)
            {   // "purchase" all the items that are part of this combo item so we have valid stock
                ItemInvoiceItemDto item = new ItemInvoiceItemDto();
				item.Quantity = itemInCombo.Quantity * unitsToBuild;
                item.InventoryItemUid = itemInCombo.Uid;
                item.Description = "Purchasing: " + itemInCombo.Code;
                item.TaxCode = TaxCode.ExpInclGst;
                item.UnitPriceInclTax = 99.95M;
                dto.Items.Add(item);
            }
            invoiceProxy.Insert(dto);

            // Download stock info before
            InventoryItemProxy inventoryItemProxy = new InventoryItemProxy();
			InventoryItemDto inventoryItem = (InventoryItemDto)inventoryItemProxy.GetByUid(comboItemDto.Uid);
            decimal stockOnHand = inventoryItem.StockOnHand;


            // Build the item!
			BuildComboItemResult result = proxy.Build(comboItemDto.Uid, unitsToBuild);
            Assert.AreNotEqual(0, result.Uid);
            Assert.IsNotNull(result.LastUpdatedUid);

			inventoryItem = (InventoryItemDto)inventoryItemProxy.GetByUid(comboItemDto.Uid);
            decimal newStockOnHand = inventoryItem.StockOnHand;
			Assert.AreEqual(stockOnHand + unitsToBuild, newStockOnHand, "We have one extra item in stock");

            // Read Inventory Transfer details
            InventoryTransferProxy transferProxy = new InventoryTransferProxy();
            InventoryTransferDto transfer = (InventoryTransferDto)transferProxy.GetByUid(result.Uid);
            Assert.AreEqual(comboItem.Items.Count+1, transfer.Items.Count); // +1 as we have the combo item the first one

            // confirm first item is the combo with +1
            InventoryTransferItemDto comboItemTransfer = (InventoryTransferItemDto)transfer.Items[0];
			Assert.AreEqual(comboItemDto.Uid, comboItemTransfer.InventoryItemUid);
			Assert.AreEqual(unitsToBuild, comboItemTransfer.Quantity);

            for (int i = 0; i < comboItem.Items.Count; i++)
            {
                ComboItemLineItemDto line = comboItem.Items[i];
                InventoryTransferItemDto item = (InventoryTransferItemDto)transfer.Items[i+1];
                Assert.AreEqual(line.Uid, item.InventoryItemUid);
				Assert.AreEqual(line.Quantity * unitsToBuild, -item.Quantity);
            }
        }

        [Test]
        public void BuildComboItemFailsWithNotEnoughtStock()
        {
            ComboItemProxy proxy = new ComboItemProxy();

			ComboItemDto comboItemDto = this.GetComboItem01();
			proxy.Insert(comboItemDto);
			ComboItemDto comboItem = (ComboItemDto)proxy.GetByUid(comboItemDto.Uid);
			BuildComboItemResult result = proxy.Build(comboItemDto.Uid, 1000);
            
            Assert.AreEqual(1, result.Errors.Count);
            ErrorInfo error = (ErrorInfo)result.Errors[0];

            Assert.AreEqual("InvalidInventoryItemStockOnHandException", error.Type);
            StringAssert.StartsWith("Unable to complete the requested operation as it will cause negative stock-on-hand for ", error.Message);
        }


		[Test]
		public void InsertAndGetByUid()
		{
			ComboItemDto comboItemDto = this.GetComboItem01();

			ComboItemProxy proxy = new ComboItemProxy();
			proxy.Insert(comboItemDto);

			ComboItemDto fromDB = (ComboItemDto)proxy.GetByUid(comboItemDto.Uid);
			this.AssertEqual(comboItemDto, fromDB);
		}


		[Test]
		public void Update()
		{
			ComboItemDto comboItemDto = this.GetComboItem01();
			ComboItemProxy proxy = new ComboItemProxy();
			proxy.Insert(comboItemDto);

			ComboItemLineItemDto lineItemDto = new ComboItemLineItemDto();
			lineItemDto.Uid = this.HardDisk.Uid;
			lineItemDto.Code = this.HardDisk.Code;
			lineItemDto.Quantity = 1;
			comboItemDto.Items.Add(lineItemDto);

			proxy.Update(comboItemDto);

			ComboItemDto fromDB = (ComboItemDto)proxy.GetByUid(comboItemDto.Uid);
			this.AssertEqual(comboItemDto, fromDB);
		}


		[Test]
		public void Delete()
		{
			ComboItemDto comboItemDto = this.GetComboItem01();
			ComboItemProxy proxy = new ComboItemProxy();
			proxy.Insert(comboItemDto);

			proxy.DeleteByUid(comboItemDto.Uid);

			try
			{
				ComboItemDto fromDB = (ComboItemDto)proxy.GetByUid(comboItemDto.Uid);
				Assert.Fail("The Combo Item was not deleted successfully.");
			}
			catch(RestException ex)
			{
				Assert.IsTrue(ex.Type == "RecordNotFoundException", "Expected exception RecordNotFoundException not thrown.");
			}
		} 


		[Test]
		public void InsertWithItemsNotInventoriedOrMakedAsVirtual()
		{
			ComboItemDto comboItemDto = this.GetComboItem02();
			ComboItemProxy proxy = new ComboItemProxy();

			try
			{
				proxy.Insert(comboItemDto);
				Assert.Fail("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("Combo Item's line item with code " + comboItemDto.Items[0].Code + " is not inventoried or not marked as virtual.", ex.Message.Substring(0, 104)); 
			}
		}


		[Test]
		public void InsertWithNoItems()
		{
			ComboItemDto comboItemDto = this.GetComboItem01();
			ComboItemProxy proxy = new ComboItemProxy();
			comboItemDto.Items.Clear();

			try
			{
				proxy.Insert(comboItemDto);
				Assert.Fail("Expected exception was not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("Please select at least one item for the combo item.", ex.Message.Substring(0, 51));
			}
		}


		[Test]
		public void InsertWithComboItemAsItem()
		{
			ComboItemProxy proxy = new ComboItemProxy();
			ComboItemDto comboItemDto = this.GetComboItem01();
			ComboItemLineItemDto lineItemComboItem = new ComboItemLineItemDto();
			lineItemComboItem.Uid = this.CPU.Uid; 
			lineItemComboItem.Code = this.CPU.Code; 
			lineItemComboItem.Quantity = 1;
			comboItemDto.Items.Add(lineItemComboItem);

			proxy.Insert(comboItemDto);

			ComboItemDto fromDB = (ComboItemDto)proxy.GetByUid(comboItemDto.Uid);
			this.AssertEqual(comboItemDto, fromDB); 
		}


		[Test]
		public void InsertWithItemsFromDifferentFile()
		{
			ComboItemDto comboItemDto = this.GetComboItem01();
			ComboItemProxy proxy = new ComboItemProxy();

			ComboItemLineItemDto lineItem = new ComboItemLineItemDto();
			lineItem.Uid = 14605;
			lineItem.Code = "100";
			lineItem.Quantity = 2;
			comboItemDto.Items.Add(lineItem);

			try
			{
				proxy.Insert(comboItemDto);
				Assert.Fail("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("One or more line items of the Combo Item was not found. Please make sure correct items are selected for creating the Combo Item.", ex.Message.Substring(0, 128)); 
			}
		}


		[Test]
		public void UpdateWithItemsFromDifferentFile()
		{
			ComboItemDto comboItemDto = this.GetComboItem01();
			ComboItemProxy proxy = new ComboItemProxy();
			proxy.Insert(comboItemDto);

			ComboItemLineItemDto lineItem = new ComboItemLineItemDto();
			lineItem.Uid = 14605;
			lineItem.Code = "100";
			lineItem.Quantity = 2;
			comboItemDto.Items.Add(lineItem);

			try
			{
				proxy.Update(comboItemDto);
				Assert.Fail("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("One or more line items of the Combo Item was not found. Please make sure correct items are selected for creating the Combo Item.", ex.Message.Substring(0, 128));
			}
		}


		[Test]
		public void SyncCheck()
		{
			ComboItemDto comboItemDto = this.GetComboItem01();
			ComboItemProxy proxy = new ComboItemProxy();
			proxy.Insert(comboItemDto);

			ComboItemDto fromDB = (ComboItemDto)proxy.GetByUid(comboItemDto.Uid);
			fromDB.Description = fromDB.Description + " - Updated";
			fromDB.Notes = "Updated today.";

			proxy.Update(fromDB);

			comboItemDto.Notes = "Late update.";

			try
			{
				proxy.Update(comboItemDto);
				Assert.Fail("Expected exception not thrown.");

			}
			catch (RestException ex)
			{
				Assert.AreEqual("Record to be updated has changed since last read.", ex.Message);
			}
		}


		[Test]
		public void DeleteReferenced()
		{
			ComboItemProxy proxy = new ComboItemProxy();
			ComboItemDto comboItem = this.GetComboItem01();
			proxy.Insert(comboItem);

			// We need to insert stock first using a purchase
			CrudProxy invoiceProxy = new InvoiceProxy();
			InvoiceDto dto = new InvoiceDto(TransactionType.Purchase, InvoiceLayout.Item);
			dto.Date = DateTime.Today.Date;
			dto.ContactUid = this.MrSmith.Uid;
			dto.Summary = "Using a combo item.";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = dto.Date.AddMonths(1);
			dto.Status = InvoiceStatus.Invoice;
			dto.InvoiceNumber = "I123";
			dto.PurchaseOrderNumber = "<Auto Number>";

			decimal unitsToBuild = 12.25M;
			
			// "purchase" all the items that are part of this combo item so we have valid stock
			ItemInvoiceItemDto item = new ItemInvoiceItemDto();
			item.Quantity = 5;
			item.InventoryItemUid = comboItem.Uid;
			item.Description = "Purchasing: " + comboItem.Description;
			item.TaxCode = TaxCode.ExpInclGst;
			item.UnitPriceInclTax = 99.95M;
			dto.Items.Add(item);
			invoiceProxy.Insert(dto);

			try
			{
				proxy.DeleteByUid(comboItem.Uid);
				Assert.Fail("Expected exception was not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("The specified inventory item is referenced by some Transactions and/or Combo Items  and therefore cannot be deleted.", ex.Message);
			}
		}


		[Test]
		[ExpectedException(ExpectedException = typeof(RestException),  MatchType = MessageMatch.Contains, ExpectedMessage = "The save operation could not be completed. Changing a combo item to an inventory item is not allowed.")]
		public void UpdateAsInventoryItem()
		{
			ComboItemProxy proxy = new ComboItemProxy();
			ComboItemDto comboItem = this.GetComboItem01();
			proxy.Insert(comboItem);

			ComboItemDto fromDB = (ComboItemDto)proxy.GetByUid(comboItem.Uid);
			fromDB.Description = fromDB.Description + " - Update as Inventory Item";
			fromDB.Items.Clear();
			
			InventoryItemProxy itemProxy = new InventoryItemProxy();
			itemProxy.Update(fromDB);
		}

		#region Helper Methods 
		private void AssertEqual(ComboItemDto expected, ComboItemDto actual)
		{
			InventoryItemTests.AssertEqual(expected, actual);
			this.AssertEqual(expected.Items, actual.Items);
		}


		private void AssertEqual(List<ComboItemLineItemDto> expectedItems, List<ComboItemLineItemDto> actualItems)
		{
			Assert.AreEqual(expectedItems.Count, actualItems.Count, "Incorrect number of line items.");

			foreach (ComboItemLineItemDto expectedItem in expectedItems)
			{
				Assert.True(actualItems.Find(delegate(ComboItemLineItemDto item) { return item.Uid == expectedItem.Uid && item.Code == expectedItem.Code && item.Quantity == expectedItem.Quantity; }) != default(ComboItemLineItemDto));
			}
		}


		protected ComboItemDto GetComboItem01()
		{
			ComboItemDto comboItemDto = new ComboItemDto();
			comboItemDto.Code = "C - " + System.Guid.NewGuid().ToString().Substring(0, 10);
			comboItemDto.Description = "Insert Combo Item Test";
			comboItemDto.Items = new List<ComboItemLineItemDto>();

			ComboItemLineItemDto lineItem = new ComboItemLineItemDto();
			lineItem.Uid = this.AsusLaptop.Uid;
			lineItem.Code = this.AsusLaptop.Code;
			lineItem.Quantity = 1;
			comboItemDto.Items.Add(lineItem);

			lineItem = new ComboItemLineItemDto();
			lineItem.Uid = this.Cat5Cable.Uid;
			lineItem.Code = this.Cat5Cable.Code;
			lineItem.Quantity = 1;
			comboItemDto.Items.Add(lineItem);

			comboItemDto.IsInventoried = true;
			comboItemDto.AssetAccountUid = this.AssetInventory.Uid;

			comboItemDto.IsSold = true;
			comboItemDto.SaleIncomeAccountUid = this.IncomeHardwareSales.Uid;
			comboItemDto.SaleCoSAccountUid = this.CoSHardware.Uid;
			comboItemDto.SaleTaxCode = "G1";
			comboItemDto.SellingPrice = 350.00M;
			comboItemDto.RrpInclTax = 350.00M;

			comboItemDto.IsBought = true;
			comboItemDto.PurchaseTaxCode = "G10";
			comboItemDto.PrimarySupplierContactUid = this.MrSmith.Uid;
			comboItemDto.BuyingPrice = 175.00M;
			comboItemDto.PrimarySupplierItemCode = "ITOP090";
			comboItemDto.DefaultReOrderQuantity = 10;

			comboItemDto.IsVoucher = true;
			comboItemDto.ValidFrom = DateTime.Parse("28-May-2010");
			comboItemDto.ValidTo = DateTime.Parse("28-Dec-2010");

			comboItemDto.IsVirtual = true;
			comboItemDto.VType = "Virtual Voucher";
			comboItemDto.Notes = "Combo item notes.";
			comboItemDto.IsVisible = true;

			return comboItemDto;
		}


		protected ComboItemDto GetComboItem02()
		{
			ComboItemDto comboItemDto = new ComboItemDto();
			comboItemDto.Code = "C - " + System.Guid.NewGuid().ToString().Substring(0, 10);
			comboItemDto.Description = "Insert Combo Item Test";
			comboItemDto.Items = new List<ComboItemLineItemDto>();

			ComboItemLineItemDto lineItem = new ComboItemLineItemDto();
			lineItem.Uid = this.Shipping1.Uid;
			lineItem.Code = this.Shipping1.Code;
			lineItem.Quantity = 1;
			comboItemDto.Items.Add(lineItem); 

			lineItem = new ComboItemLineItemDto();
			lineItem.Uid = this.AsusLaptop.Uid;
			lineItem.Code = this.AsusLaptop.Code;
			lineItem.Quantity = 1;
			comboItemDto.Items.Add(lineItem);

			comboItemDto.IsInventoried = true;
			comboItemDto.AssetAccountUid = this.AssetInventory.Uid;

			comboItemDto.IsSold = true;
			comboItemDto.SaleIncomeAccountUid = this.IncomeHardwareSales.Uid;
			comboItemDto.SaleCoSAccountUid = this.CoSHardware.Uid;
			comboItemDto.SaleTaxCode = "G1";
			comboItemDto.SellingPrice = 350.00M;
			comboItemDto.RrpInclTax = 350.00M;

			return comboItemDto;
		}
		#endregion
	}
}
