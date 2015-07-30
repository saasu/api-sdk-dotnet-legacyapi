namespace Ola.RestClient.NUnitTests
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Proxies;
	using Ola.RestClient.Utils;
	
	using NUnit.Framework;

	using System;
	using System.Collections;
	using System.Net;
	using System.IO;
	

	public class InvoiceTests : RestClientTests
	{
		protected static void AssertEqual(InvoiceDto expected, InvoiceDto actual)
		{
			Assert.AreEqual(expected.Uid, actual.Uid, "Different uid.");
			Assert.AreEqual(expected.LastUpdatedUid, actual.LastUpdatedUid, "Different last updated uid.");
			Assert.AreEqual(expected.TransactionType, actual.TransactionType, "Different transaction type.");
			Assert.AreEqual(expected.Date, actual.Date, "Different date.");
			Assert.AreEqual(expected.ContactUid, actual.ContactUid, "Different contact uid.");
			Assert.AreEqual(expected.ShipToContactUid, actual.ShipToContactUid, "Different ship to contact uid.");
			Assert.AreEqual(expected.Summary, actual.Summary, "Different summary.");
			Assert.AreEqual(expected.Notes, actual.Notes, "Different notes.");
			Assert.AreEqual(expected.RequiresFollowUp, actual.RequiresFollowUp, "Different requires follow up.");
			Assert.AreEqual(expected.DueOrExpiryDate, actual.DueOrExpiryDate, "Different due date or expiry date.");
			Assert.AreEqual(expected.Layout, actual.Layout, "Different layout.");
			Assert.IsNotNullOrEmpty(actual.Status, "Invoice status must always be set on returned data.");

            var expectedTradingTermsType = expected.TradingTerms.Type;
            if (expected.DueOrExpiryDate != DateTime.MinValue)
            {
                expectedTradingTermsType = 1; // DueIn
            }
            Assert.AreEqual(expectedTradingTermsType, actual.TradingTerms.Type,"Trading terms type not set correctly. DueIn Expected");  
			if (!string.IsNullOrWhiteSpace(expected.InvoiceType))
			{
				Assert.AreEqual(expected.InvoiceType, actual.InvoiceType, "Different invoice type.");	
			}
			Assert.IsNotNullOrEmpty(actual.InvoiceType, "Invoice Type must always be set on returned data.");
			Assert.AreEqual(expected.InvoiceNumber, actual.InvoiceNumber, "Different invoice number.");
			Assert.AreEqual(expected.PurchaseOrderNumber, actual.PurchaseOrderNumber, "Different purchase order number.");
			Assert.AreEqual(expected.IsSent, actual.IsSent, "Different is sent.");

			if (expected.Layout == InvoiceLayout.Service)
			{
				AssertEqualForServiceInvoiceItems(expected.Items, actual.Items);
			}
			else
			{
				AssertEqualForItemInvoiceItems(expected.Items, actual.Items);
			}
		}


		protected static void AssertEqual(InvoicePaymentDto expected, InvoicePaymentDto actual)
		{
			Assert.AreEqual(expected.Uid, actual.Uid, "Different uid.");
			Assert.AreEqual(expected.LastUpdatedUid, actual.LastUpdatedUid, "Different last updated uid.");
			Assert.AreEqual(expected.TransactionType, actual.TransactionType, "Different transaction type.");
			Assert.AreEqual(expected.Date, actual.Date, "Different date.");
			Assert.AreEqual(expected.Reference, actual.Reference, "Different reference.");
			Assert.AreEqual(expected.Summary, actual.Summary, "Different summary.");
			Assert.AreEqual(expected.Notes, actual.Notes, "Different notes.");
			Assert.AreEqual(expected.RequiresFollowUp, actual.RequiresFollowUp, "Different requires follow up.");
			Assert.AreEqual(expected.PaymentAccountUid, actual.PaymentAccountUid, "Different payment account uid.");
			Assert.AreEqual(expected.DateCleared, actual.DateCleared, "Different date cleared.");
            Assert.AreEqual(expected.Fee, actual.Fee, "Fee values not the same.");
			AssertEqualForInvoicePaymentItems(expected.Items, actual.Items);
		}


		protected static void AssertEqualForServiceInvoiceItems(ArrayList expected, ArrayList actual)
		{
			Assert.AreEqual(expected.Count, actual.Count, "Incorrect number of items.");
			for (int i = 0; i < expected.Count; i++)
			{
				AssertEqual((ServiceInvoiceItemDto) expected[i], (ServiceInvoiceItemDto) actual[i], i);
			}
		}


		protected static void AssertEqualForItemInvoiceItems(ArrayList expected, ArrayList actual)
		{
			Assert.AreEqual(expected.Count, actual.Count, "Incorrect number of items.");
			for (int i = 0; i < expected.Count; i++)
			{
				AssertEqual((ItemInvoiceItemDto) expected[i], (ItemInvoiceItemDto) actual[i], i);
			}
		}


		protected static void AssertEqual(ServiceInvoiceItemDto expected, ServiceInvoiceItemDto actual, int index)
		{
			string msg = "Item index: " + index.ToString() + ". Different ";
			Assert.AreEqual(expected.Description, actual.Description, msg + "description.");
			Assert.AreEqual(expected.AccountUid, actual.AccountUid, msg + "account uid.");
			Assert.AreEqual(expected.TaxCode, actual.TaxCode, msg + "tax code.");
			Assert.AreEqual(expected.TotalAmountInclTax, actual.TotalAmountInclTax, msg + "totalAmountInclTax.");
		}


		protected static void AssertEqual(ItemInvoiceItemDto expected, ItemInvoiceItemDto actual, int index)
		{
			string msg = "Item index: " + index.ToString() + ". Different ";
			Assert.AreEqual(expected.Quantity, actual.Quantity, msg + "quantity.");
			Assert.AreEqual(expected.InventoryItemUid, actual.InventoryItemUid, msg + "inventory item uid.");
			Assert.AreEqual(expected.Description, actual.Description, msg + "description.");
			Assert.AreEqual(expected.TaxCode, actual.TaxCode, msg + "tax code.");
			Assert.AreEqual(expected.UnitPriceInclTax, actual.UnitPriceInclTax, msg + "unit price incl tax.");
			Assert.AreEqual(expected.PercentageDiscount, actual.PercentageDiscount, msg + "percentage discount.");
		}


		protected static void AssertEqualForInvoicePaymentItems(ArrayList expected, ArrayList actual)
		{
			Assert.AreEqual(expected.Count, actual.Count, "Incorrect number of items.");
			for (int i = 0; i < expected.Count; i++)
			{
				InvoicePaymentItemDto expectedItem = (InvoicePaymentItemDto) expected[i];
				AssertFound(expectedItem, actual, i);
			}
		}


		protected static void AssertFound(InvoicePaymentItemDto expectedItem, ArrayList actual, int index)
		{
			for (int i = 0; i < actual.Count; i++)
			{
				InvoicePaymentItemDto item = (InvoicePaymentItemDto) actual[i];
				if (expectedItem.InvoiceUid == item.InvoiceUid && expectedItem.Amount == item.Amount)
				{
					return;
				}
			}

			Assert.Fail("Expected invoice payment item at index " + index.ToString() + " is not found.");
		}


		public virtual void CreatePurchaseForInventoryItemsUsed()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Purchase, InvoiceLayout.Item);
			
			dto.Date				= DateTime.Today.Date;
			dto.ContactUid			= this.MrSmith.Uid;
			dto.Summary				= "Invoice Tests - CreatePurchaseForInventoryItemsUsed()";
			dto.Notes				= "From REST";
			dto.DueOrExpiryDate		= dto.Date.AddMonths(1);
			dto.Status				= InvoiceStatus.Invoice;
			dto.PurchaseOrderNumber = "<Auto Number>";
			
			ItemInvoiceItemDto item = null;
			
			item = new ItemInvoiceItemDto();
			item.Quantity			= 10000;
			item.InventoryItemUid	= this.AsusLaptop.Uid;
			item.Description		= this.AsusLaptop.Description;
			item.TaxCode			= TaxCode.ExpInclGst;
			item.UnitPriceInclTax	= 1000M;
			dto.Items.Add(item);

			item = new ItemInvoiceItemDto();
			item.Quantity			= 10000;
			item.InventoryItemUid	= this.HardDisk.Uid;
			item.Description		= this.HardDisk.Description;
			item.TaxCode			= TaxCode.ExpInclGst;
			item.UnitPriceInclTax	= 99.95M;
			dto.Items.Add(item);
			
			item = new ItemInvoiceItemDto();
			item.Quantity			= 500000;
			item.InventoryItemUid	= this.Cat5Cable.Uid;
			item.Description		= this.Cat5Cable.Description;
			item.TaxCode			= TaxCode.ExpInclGst;
			item.UnitPriceInclTax	= 0.95M;
			dto.Items.Add(item);
			
			new InvoiceProxy().Insert(dto);
		}
	}
}
