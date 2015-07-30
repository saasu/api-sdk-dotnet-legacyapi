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
    public class InventoryAdjustmentTests : RestClientTests
    {
        [Test]
        public void TestInsertAndGetByUid()
        {
            CrudProxy proxy = new InventoryAdjustmentProxy();
            InventoryAdjustmentDto dto1 = this.GetInventoryAdjustment1();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            InventoryAdjustmentDto dto2 = (InventoryAdjustmentDto)proxy.GetByUid(dto1.Uid);

            AssertEqual(dto1, dto2); 
        }

        [Test]
        public void TestInsertAndGetByUidWithDecimalQuantity()
        {
            CrudProxy proxy = new InventoryAdjustmentProxy();
            InventoryAdjustmentDto dto1 = this.GetInventoryAdjustment2();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            InventoryAdjustmentDto dto2 = (InventoryAdjustmentDto)proxy.GetByUid(dto1.Uid);

            AssertEqual(dto1, dto2);
        }

        [Test]
        public void TestUpdate()
        {
            CrudProxy proxy = new InventoryAdjustmentProxy();
            InventoryAdjustmentDto dto1 = this.GetInventoryAdjustment1();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");
            dto1.Summary = "Updated inventory adjustment"; 
            proxy.Update(dto1);

            InventoryAdjustmentDto dto2 = (InventoryAdjustmentDto)proxy.GetByUid(dto1.Uid);

            AssertEqual(dto1, dto2); 
        }

        [Test]
        public void TestUpdateWithDecimalQuantity()
        {
            CrudProxy proxy = new InventoryAdjustmentProxy();
            InventoryAdjustmentDto dto1 = this.GetInventoryAdjustment2();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");
            dto1.Summary = "Updated inventory adjustment";
            var adjustmentItem = (InventoryAdjustmentItemDto) dto1.Items[0]; 
            adjustmentItem.Quantity = 1.5M;
            adjustmentItem.UnitPriceExclTax = 100.00M;
            adjustmentItem.TotalPriceExclTax = 150.00M;
 
            proxy.Update(dto1);

            InventoryAdjustmentDto dto2 = (InventoryAdjustmentDto)proxy.GetByUid(dto1.Uid);

            AssertEqual(dto1, dto2);
        }

        [Test]
        public void TestUpdatingStockOnHand()
        {
            CrudProxy proxy = new InventoryAdjustmentProxy();
            InventoryAdjustmentDto dto = new InventoryAdjustmentDto();
            dto.Date = DateTime.Now;
            dto.Summary = "Update stock on hand only";

            InventoryAdjustmentItemDto dtoItem = new InventoryAdjustmentItemDto();
            dtoItem.InventoryItemUid = this.AsusLaptop.Uid;
            dtoItem.Quantity = 5;
            dto.Items.Add(dtoItem);

            proxy.Insert(dto);

            Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after save.");

            InventoryAdjustmentDto dtoFromDB = (InventoryAdjustmentDto)proxy.GetByUid(dto.Uid);

            AssertEqual(dto, dtoFromDB);
        }


        [Test]
        public void TestUpdatingTotal()
        {
            CrudProxy proxy = new InventoryAdjustmentProxy();
            InventoryAdjustmentDto dto = new InventoryAdjustmentDto();
            dto.Date = DateTime.Now;

            InventoryAdjustmentItemDto dtoItem = new InventoryAdjustmentItemDto();
            dtoItem.InventoryItemUid = this.AsusLaptop.Uid;
            dtoItem.Quantity = 10;
            dtoItem.AccountUid = this.AssetInventory.Uid;
            dtoItem.UnitPriceExclTax = 150.00M; 
            dtoItem.TotalPriceExclTax = 1500.00M;
            dto.Items.Add(dtoItem);

            proxy.Insert(dto);

            Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after save.");

            dto = new InventoryAdjustmentDto();
            dto.Date = DateTime.Now;
            dto.Summary = "Update Total only";

            dtoItem = new InventoryAdjustmentItemDto();
            dtoItem.InventoryItemUid = this.AsusLaptop.Uid;
            dtoItem.AccountUid = this.AssetInventory.Uid;
            dtoItem.TotalPriceExclTax = 1800.00M;
            dto.Items.Add(dtoItem);

            proxy.Insert(dto);

            Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after save.");

            InventoryAdjustmentDto dtoFromDB = (InventoryAdjustmentDto)proxy.GetByUid(dto.Uid);

            AssertEqual(dto, dtoFromDB);
        }
        
            
        [Test]
        public void TestDelete()
        {
            CrudProxy proxy = new InventoryAdjustmentProxy();
            InventoryAdjustmentDto dto1 = this.GetInventoryAdjustment1();
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


        private InventoryAdjustmentDto GetInventoryAdjustment1()
        {
            InventoryAdjustmentDto dto = new InventoryAdjustmentDto();
            dto.Date = DateTime.Now;
            dto.Summary = "Test Inventory Adjustment CRUD";
            dto.Notes = "Inventory Adjustmemt from API";
            dto.Tags = "IA, Test"; 

            InventoryAdjustmentItemDto item = new InventoryAdjustmentItemDto();
            item.Quantity = 1;
            item.InventoryItemUid = this.Cat5Cable.Uid;
            item.AccountUid = this.AssetInventory.Uid;
            item.UnitPriceExclTax = 120.50M;
            item.TotalPriceExclTax = 120.50M;
            dto.Items.Add(item);

            item = new InventoryAdjustmentItemDto();
            item.Quantity = 2;
            item.InventoryItemUid = this.AsusLaptop.Uid;
            item.AccountUid = this.ExpenseOffice.Uid;
            item.UnitPriceExclTax = 100.25M;
            item.TotalPriceExclTax = 200.50M;
            dto.Items.Add(item);

            return dto;
        }


        private InventoryAdjustmentDto GetInventoryAdjustment2()
        {
            InventoryAdjustmentDto dto = new InventoryAdjustmentDto();
            dto.Date = DateTime.Now;
            dto.Summary = "Test Inventory Adjustment CRUD 2";
            dto.Notes = "Inventory Adjustmemt from API 2";
            dto.Tags = "IA, Test";

            InventoryAdjustmentItemDto item = new InventoryAdjustmentItemDto();
            item.Quantity = 2.5M;
            item.InventoryItemUid = this.HardDisk.Uid;
            item.AccountUid = this.ExpenseOffice.Uid; 
            item.UnitPriceExclTax = 100.00M;
            item.TotalPriceExclTax = 250.00M;
            dto.Items.Add(item);

            return dto;
        }


        private void AssertEqual(InventoryAdjustmentDto expected, InventoryAdjustmentDto actual)
        {
            foreach(InventoryAdjustmentItemDto expectedItem in expected.Items)
            {
                this.AssertExistsAndRemove(expectedItem, actual.Items);
            }
        }


        private void AssertExistsAndRemove
            (
                InventoryAdjustmentItemDto expectedItem,
                ArrayList actualItems
            )
        {
            int count = actualItems.Count;
            for (int i = 0; i < count; i++)
            {
                InventoryAdjustmentItemDto actualItem = (InventoryAdjustmentItemDto)actualItems[i];

                if (
                        actualItem.Quantity == expectedItem.Quantity
                        && actualItem.InventoryItemUid == expectedItem.InventoryItemUid
                        && actualItem.AccountUid == expectedItem.AccountUid
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
    }
}
