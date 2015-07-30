using Ola.RestClient.Dto;
using Ola.RestClient.Exceptions;
using Ola.RestClient.Proxies;
using Ola.RestClient.Utils;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ola.RestClient.NUnitTests
{
    [TestFixture]
    public class InventoryTransferTests : RestClientTests
    {
        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();
            this.PurchaseItemsForInventory();
        }


        [Test]
        public void TestInsertAndGetByUid()
        {
            InventoryTransferProxy  proxy = new InventoryTransferProxy();
            InventoryTransferDto dto1 = this.GetInventoryTransfer1();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            InventoryTransferDto dto2 = (InventoryTransferDto)proxy.GetByUid(dto1.Uid);

            AssertEqual(dto1, dto2);
        }

        [Test]
        public void TestInsertAndGetByUidWithDecimalQuantity()
        {
            InventoryTransferProxy proxy = new InventoryTransferProxy();
            InventoryTransferDto dto1 = this.GetInventoryTransfer4();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            InventoryTransferDto dto2 = (InventoryTransferDto)proxy.GetByUid(dto1.Uid);

            AssertEqual(dto1, dto2);
        }

        [Test]
        public void TestInsertAndGetByUidDoNotBalance()
        {
            InventoryTransferProxy proxy = new InventoryTransferProxy();
            InventoryTransferDto dto1 = this.GetInventoryTransfer3();

			try
			{
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException rex)
			{
				Assert.AreEqual("The Inventory Transfer is not balanced. The total of all line items must equal to 0.", rex.Message, "Incorrect error message.");
			}
        }


        [Test]
        public void TestUpdate()
        {
            InventoryTransferProxy proxy = new InventoryTransferProxy();
            InventoryTransferDto dto1 = this.GetInventoryTransfer1();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            dto1.Notes = "Updated inventory transfer";
            dto1.Items.AddRange(this.GetInventoryTransfer2().Items);

            proxy.Update(dto1); 
            
            InventoryTransferDto dto2 =  (InventoryTransferDto)proxy.GetByUid(dto1.Uid);

            AssertEqual(dto1, dto2);
        }

        [Test]
        public void TestUpdateWithDecimalQuantity()
        {
            InventoryTransferProxy proxy = new InventoryTransferProxy();
            InventoryTransferDto dto1 = this.GetInventoryTransfer4();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            dto1.Notes = "Updated inventory transfer";
            dto1.Items.AddRange(this.GetInventoryTransfer2().Items);

            proxy.Update(dto1);

            InventoryTransferDto dto2 = (InventoryTransferDto)proxy.GetByUid(dto1.Uid);

            AssertEqual(dto1, dto2);
        }


        [Test]
        public void TestDelete()
        {
            CrudProxy proxy = new InventoryTransferProxy();
            InventoryTransferDto dto1 = this.GetInventoryTransfer1();
            proxy.Insert(dto1);

            proxy.DeleteByUid(dto1.Uid);

            try
            {
                proxy.GetByUid(dto1.Uid);
                throw new Exception("The expected exception was not thrown.");
            }
            catch (RestException ex)
            {
                Assert.AreEqual("RecordNotFoundException", ex.Type, "Incorrect exception type.");
            }
        }


        private InventoryTransferDto GetInventoryTransfer1()
        {
            InventoryTransferDto dto = new InventoryTransferDto();
            dto.Date = DateTime.Now;
            dto.Summary = "Test Inventory Transfer CRUD";
            dto.Notes = "Inventory Transfer from API";
            dto.Tags = "IT, Test";

            InventoryTransferItemDto item = new InventoryTransferItemDto();
            item.Quantity = -2;
            item.InventoryItemUid = this.HardDisk.Uid;
            item.UnitPriceExclTax = 120.50M;
            item.TotalPriceExclTax = -241.00M;
            dto.Items.Add(item);

            item = new InventoryTransferItemDto();
            item.Quantity = 1;
            item.InventoryItemUid = this.AsusLaptop.Uid;
            item.UnitPriceExclTax = 241.00M;
            item.TotalPriceExclTax = 241.00M;
            dto.Items.Add(item);

            return dto;
        }


        private InventoryTransferDto GetInventoryTransfer2()
        {
            InventoryTransferDto dto = new InventoryTransferDto();
            dto.Date = DateTime.Now;
            dto.Summary = "Test Inventory Transfer CRUD";
            dto.Notes = "Inventory Transfer from API";
            dto.Tags = "IT, Test";

            InventoryTransferItemDto item = new InventoryTransferItemDto();
            item.Quantity = -2;
            item.InventoryItemUid = this.Cat5Cable.Uid;
            item.UnitPriceExclTax = 60.50M;
            item.TotalPriceExclTax = -121.00M;
            dto.Items.Add(item);

            item = new InventoryTransferItemDto();
            item.Quantity = 1;
            item.InventoryItemUid = this.HardDisk.Uid;
            item.UnitPriceExclTax = 121.00M;
            item.TotalPriceExclTax = 121.00M;
            dto.Items.Add(item);

            return dto;
        }


        private InventoryTransferDto GetInventoryTransfer3()
        {
            InventoryTransferDto dto = new InventoryTransferDto();
            dto.Date = DateTime.Now;
            dto.Summary = "Test Inventory Transfer CRUD";
            dto.Notes = "Inventory Transfer from API";
            dto.Tags = "IT, Test";

            InventoryTransferItemDto item = new InventoryTransferItemDto();
            item.Quantity = -2;
            item.InventoryItemUid = this.Cat5Cable.Uid;
            item.UnitPriceExclTax = 65.00M;
            item.TotalPriceExclTax = -130.00M;
            dto.Items.Add(item);

            item = new InventoryTransferItemDto();
            item.Quantity = 1;
            item.InventoryItemUid = this.HardDisk.Uid;
            item.UnitPriceExclTax = 121.00M;
            item.TotalPriceExclTax = 121.00M;
            dto.Items.Add(item);

            return dto;
        }

        private InventoryTransferDto GetInventoryTransfer4()
        {
            InventoryTransferDto dto = new InventoryTransferDto();
            dto.Date = DateTime.Now;
            dto.Summary = "Test Inventory Transfer CRUD with decimal quantities";
            dto.Notes = "Inventory Transfer from API";
            dto.Tags = "IT, Test";

            InventoryTransferItemDto item = new InventoryTransferItemDto();
            item.Quantity = -2.5M;
            item.InventoryItemUid = this.Cat5Cable.Uid;
            item.UnitPriceExclTax = 50.00M;
            item.TotalPriceExclTax = -125.00M;
            dto.Items.Add(item);

            item = new InventoryTransferItemDto();
            item.Quantity = 1;
            item.InventoryItemUid = this.HardDisk.Uid;
            item.UnitPriceExclTax = 125.00M;
            item.TotalPriceExclTax = 125.00M;
            dto.Items.Add(item);

            return dto;
        }


        private void AssertEqual(InventoryTransferDto expected, InventoryTransferDto actual)
        {
            foreach (InventoryTransferItemDto expectedItem in expected.Items)
            {
                this.AssertExistsAndRemove(expectedItem, actual.Items);
            }
        }


        private void AssertExistsAndRemove
          (
              InventoryTransferItemDto expectedItem,
              ArrayList actualItems
          )
        {
            int count = actualItems.Count;
            for (int i = 0; i < count; i++)
            {
                InventoryTransferItemDto actualItem = (InventoryTransferItemDto)actualItems[i];

                if (
                        actualItem.Quantity == expectedItem.Quantity
                        && actualItem.InventoryItemUid == expectedItem.InventoryItemUid
                        && actualItem.UnitPriceExclTax == expectedItem.UnitPriceExclTax
                        && actualItem.TotalPriceExclTax == expectedItem.TotalPriceExclTax
                    )
                {
                    actualItems.RemoveAt(i);
                    return;
                }
                Assert.IsTrue(false, "Unable to find the expected inventory adjustment item in the collection.");
            }
        }


        private void PurchaseItemsForInventory()
        {
            InvoiceDto dto = new InvoiceDto(TransactionType.Purchase, InvoiceLayout.Item);

            dto.Date = DateTime.Today.Date;
            dto.ContactUid = this.MrSmith.Uid;
            dto.Summary = "Test Insert Item Purchase";
            dto.Notes = "From REST";
            dto.DueOrExpiryDate = dto.Date.AddMonths(1);
            dto.Status = InvoiceStatus.Invoice;
            dto.InvoiceNumber = "I123";
            dto.PurchaseOrderNumber = "<Auto Number>";

            ItemInvoiceItemDto item = null;

            item = new ItemInvoiceItemDto();
            item.Quantity = 100;
            item.InventoryItemUid = this.HardDisk.Uid;
            item.Description = this.HardDisk.Description;
            item.TaxCode = TaxCode.ExpInclGst;
            item.UnitPriceInclTax = 99.95M;
            dto.Items.Add(item);

            item = new ItemInvoiceItemDto();
            item.Quantity = 5000;
            item.InventoryItemUid = this.Cat5Cable.Uid;
            item.Description = this.Cat5Cable.Description;
            item.TaxCode = TaxCode.ExpInclGst;
            item.UnitPriceInclTax = 0.95M;
            dto.Items.Add(item);

            item = new ItemInvoiceItemDto();
            item.Quantity = 1;
            item.InventoryItemUid = this.Shipping1.Uid;
            item.Description = this.Shipping1.Description;
            item.TaxCode = TaxCode.ExpInclGst;
            item.UnitPriceInclTax = 111.11M;
            dto.Items.Add(item);

            CrudProxy proxy = new InvoiceProxy();
            proxy.Insert(dto);
        }
    }
}
