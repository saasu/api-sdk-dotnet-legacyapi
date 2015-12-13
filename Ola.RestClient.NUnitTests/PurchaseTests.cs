namespace Ola.RestClient.NUnitTests
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Proxies;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Utils;
	
	using NUnit.Framework;

	using System;
	using System.Net;
	using System.IO;


	[TestFixture]
	public class PurchaseTests : InvoiceTests
	{
		[Test]
		public void TestInsertAndGetUnpaidServicePurchase()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetUnpaidServicePurchase();
			proxy.Insert(dto1);
			
			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");
			
			InvoiceDto dto2 = (InvoiceDto) proxy.GetByUid(dto1.Uid);
			
			AssertEqual(dto1, dto2);
		}


		[Test]
		public void TestInsertAndGetServicePurchaseWithShipToContact()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetUnpaidServicePurchase();
			dto1.ShipToContactUid = MrSmith.Uid; 
			
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2);
		}

		
		[Test]
		public void TestInsertAndGetItemPurchase()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetUnpaidItemPurchase();
			proxy.Insert(dto1);
			
			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");
			
			InvoiceDto dto2 = (InvoiceDto) proxy.GetByUid(dto1.Uid);
			
			AssertEqual(dto1, dto2);
		}

        [Test]
        public void TestInsertAndGetItemPurchase_Invoice_Totals_Test()
        {
            //Arange
            var proxy = new InvoiceProxy();
            var input = GetUnpaidItemPurchase();
            input.Items.Clear();

            var item = new ItemInvoiceItemDto
                           {
                               Quantity = 100,
                               InventoryItemUid = this.HardDisk.Uid,
                               Description = this.HardDisk.Description,
                               TaxCode = TaxCode.SaleInclGst,
                               UnitPriceInclTax = 99.95M
                           };

            input.Items.Add(item);

            item = new ItemInvoiceItemDto
                       {
                           Quantity = 5000,
                           InventoryItemUid = this.Cat5Cable.Uid,
                           Description = this.Cat5Cable.Description,
                           TaxCode = TaxCode.SaleInclGst,
                           UnitPriceInclTax = 0.95M
                       };

            input.Items.Add(item);

            //act
            var item1 = ((ItemInvoiceItemDto)input.Items[0]);
            var item2 = ((ItemInvoiceItemDto)input.Items[1]);

            var totalItem1 = (item1.UnitPriceInclTax*item1.Quantity);
            var totalItem2 = (item2.UnitPriceInclTax*item2.Quantity);

            var totalTax1 = totalItem1 - (Math.Round(totalItem1 / (1 + 0.100000m), 2, MidpointRounding.AwayFromZero));
            var totalTax2 = totalItem2 - (Math.Round(totalItem2 / (1 + 0.100000m), 2, MidpointRounding.AwayFromZero));

            proxy.Insert(input);

            var output = (InvoiceDto)proxy.GetByUid(input.Uid);
            var outputLineItem1 = ((ItemInvoiceItemDto)output.Items[0]);
            var outputLineItem2 = ((ItemInvoiceItemDto)output.Items[1]);

            //assert

            //Test Line Item Totals.
            Assert.AreEqual(outputLineItem1.TotalAmountInclTax, totalItem1);
            Assert.AreEqual(outputLineItem1.TotalTaxAmount, totalTax1);
            Assert.AreEqual(outputLineItem1.TotalAmountExclTax, outputLineItem1.TotalAmountInclTax - totalTax1);

            Assert.AreEqual(outputLineItem2.TotalAmountInclTax, totalItem2);
            Assert.AreEqual(outputLineItem2.TotalTaxAmount, totalTax2);
            Assert.AreEqual(outputLineItem2.TotalAmountExclTax, outputLineItem2.TotalAmountInclTax - totalTax2);

            //Test Invoice Totals.
            Assert.AreEqual(output.TotalAmountInclTax, outputLineItem1.TotalAmountInclTax + outputLineItem2.TotalAmountInclTax);
            Assert.AreEqual(output.TotalTaxAmount, outputLineItem1.TotalTaxAmount + outputLineItem2.TotalTaxAmount);
            Assert.AreEqual(output.TotalAmountExclTax, outputLineItem1.TotalAmountExclTax + outputLineItem2.TotalAmountExclTax);
        }

		[Test]
		public void TestInsertAndGetInvoicePayment()
		{
			CrudProxy proxy = new InvoiceProxy();
			
			InvoiceDto purchase1 = this.GetUnpaidItemPurchase();
			InvoiceDto purchase2 = this.GetUnpaidServicePurchase();
			
			proxy.Insert(purchase1);
			proxy.Insert(purchase2);
			
			InvoicePaymentDto paymentInfo1 = new InvoicePaymentDto(TransactionType.PurchasePayment);
			paymentInfo1.Date = DateTime.Today.Date;
			paymentInfo1.PaymentAccountUid = this.Westpac.Uid;
			paymentInfo1.Reference = Guid.NewGuid().ToString();
			paymentInfo1.Summary = "Payment for 2 Outstanding Invoices";
		    paymentInfo1.Fee = 9.99m;

			InvoicePaymentItemDto item = null;
			
			item = new InvoicePaymentItemDto();
			item.InvoiceUid = purchase2.Uid;
			item.Amount = 20.05M;
			paymentInfo1.Items.Add(item);

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = purchase1.Uid;
			item.Amount = 23.75M;
			paymentInfo1.Items.Add(item);

			proxy = new InvoicePaymentProxy();
			proxy.Insert(paymentInfo1);
			
			Assert.IsTrue(paymentInfo1.Uid > 0, "Uid must be > 0 after save.");

			InvoicePaymentDto paymentInfo2 = (InvoicePaymentDto) proxy.GetByUid(paymentInfo1.Uid);
			AssertEqual(paymentInfo1, paymentInfo2);	
		}


		[Test]
		public void TestDeletePurchasePayment()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto purchase1 = this.GetUnpaidItemPurchase();
			InvoiceDto purchase2 = this.GetUnpaidServicePurchase();

			proxy.Insert(purchase1);
			proxy.Insert(purchase2);

			InvoicePaymentDto paymentInfo1 = new InvoicePaymentDto(TransactionType.PurchasePayment);
			paymentInfo1.Date = DateTime.Today.Date;
			paymentInfo1.PaymentAccountUid = this.Westpac.Uid;
			paymentInfo1.Reference = Guid.NewGuid().ToString();
			paymentInfo1.Summary = "Payment for 2 Outstanding Invoices";
            paymentInfo1.Fee = 3.99m;

			InvoicePaymentItemDto item = null;

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = purchase2.Uid;
			item.Amount = 20.05M;
			paymentInfo1.Items.Add(item);

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = purchase1.Uid;
			item.Amount = 23.75M;
			paymentInfo1.Items.Add(item);

			proxy = new InvoicePaymentProxy();
			proxy.Insert(paymentInfo1);

			Assert.IsTrue(paymentInfo1.Uid > 0, "Uid must be > 0 after save.");

			InvoicePaymentDto paymentInfo2 = (InvoicePaymentDto)proxy.GetByUid(paymentInfo1.Uid);
			AssertEqual(paymentInfo1, paymentInfo2);

			proxy = new InvoicePaymentProxy();
			proxy.DeleteByUid(paymentInfo1.Uid);

			try
			{
				proxy.GetByUid(paymentInfo1.Uid);
			}
			catch (RestException ex)
			{
				Assert.AreEqual("RecordNotFoundException", ex.Type);
			}
		}

		#region testing auto numbering of invoices

		[Test]
		public void TestAutoNumberingOfPurchaseNextAvailableNumberIsAvailable()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetUnpaidServicePurchase();
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, savedDto);

			Assert.IsNotNullOrEmpty(dto1.PurchaseOrderNumber);
		}

		[Test]
		public void TestAutoNumberingOfPurchaseIncrementNextAvailableNumber()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetUnpaidServicePurchase();
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			Assert.IsNotNullOrEmpty(dto1.PurchaseOrderNumber);

			string alphaComponent1;
			var numericalComponentDto1 = GetNumericalComponentOfPurchaseNumber(dto1.PurchaseOrderNumber, out alphaComponent1);

			var dto2 = this.GetUnpaidServicePurchase();
			proxy.Insert(dto2);

			Assert.IsTrue(dto2.Uid > 0, "Uid must be > 0 after save.");

			Assert.IsNotNullOrEmpty(dto2.PurchaseOrderNumber);

			string alphaComponent2;
			var numericalComponentDto2 = GetNumericalComponentOfPurchaseNumber(dto2.PurchaseOrderNumber, out alphaComponent2);

			Assert.AreEqual(alphaComponent1, alphaComponent2);
			Assert.AreEqual(numericalComponentDto2, numericalComponentDto1 + 1);
		}

		[Test]
		//Should select next available number after taken one.
		public void TestAutoNumberingOfPurchaseNextAvailableNumberNotAvailable()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetUnpaidServicePurchase();
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			string alphaComponent;
			var numericalComponentDto1 = GetNumericalComponentOfPurchaseNumber(dto1.PurchaseOrderNumber, out alphaComponent);

			Assert.IsNotNull(numericalComponentDto1, "PurchaseOrderNumber returned did not contain a numerical component");

			//manually insert an invoice.
			var nextAvailablePONumber = alphaComponent + (numericalComponentDto1 + 1).ToString();

			var dto2 = this.GetUnpaidServicePurchase(nextAvailablePONumber);
			proxy.Insert(dto2);

			Assert.IsTrue(dto2.Uid > 0, "Uid must be > 0 after save.");

			//create next auto numbering invoice.
			var dto3 = this.GetUnpaidServicePurchase();
			proxy.Insert(dto3);

			Assert.IsTrue(dto3.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto3.Uid);

			Assert.IsNotNull(savedDto.PurchaseOrderNumber);

			Assert.AreEqual(savedDto.PurchaseOrderNumber, alphaComponent + (numericalComponentDto1 + 3).ToString());
		}

		private int? GetNumericalComponentOfPurchaseNumber(string PurchaseOrderNumber, out string alphaComponent)
		{
			int indexOffirstAplha = -1;

			for (int i = PurchaseOrderNumber.Length; i > 0; i--)
			{
				if (!Char.IsNumber(PurchaseOrderNumber[i - 1]))
				{
					indexOffirstAplha = i;
					break;
				}
			}

			if (indexOffirstAplha > -1)
			{
				alphaComponent = PurchaseOrderNumber.Substring(0, indexOffirstAplha);
				return (Convert.ToInt32(PurchaseOrderNumber.Substring(indexOffirstAplha)));
			}
			else
			{
				alphaComponent = string.Empty;
				return Convert.ToInt32(PurchaseOrderNumber);
			}
		}

		#endregion

		
		private InvoiceDto GetUnpaidServicePurchase(string PONumber = null)
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Purchase, InvoiceLayout.Service);
			
			dto.Date				= DateTime.Today.Date;
			dto.ContactUid			= this.MrSmith.Uid;
			dto.Summary				= "Test POST Purchase";
			dto.Notes				= "From REST";
			dto.DueOrExpiryDate		= dto.Date.AddMonths(1);
			dto.Status				= InvoiceStatus.Order;
			dto.PurchaseOrderNumber = "<Auto Number>";

			if (!string.IsNullOrWhiteSpace(PONumber))
			{
				dto.PurchaseOrderNumber = PONumber.Trim();
			}
			
			ServiceInvoiceItemDto item	= new ServiceInvoiceItemDto();
			item.Description			= "Purchase - Line Item 1";
			item.AccountUid				= this.ExpenseOffice.Uid;
			item.TaxCode				= TaxCode.ExpInclGst;
			item.TotalAmountInclTax		= 123.45M;
			dto.Items.Add(item);
			
			item						= new ServiceInvoiceItemDto();
			item.Description			= "Purchase - Line Item 2";
			item.AccountUid				= this.ExpenseMisc.Uid;
			// item.TaxCode				= TaxCode.ExpInclGstPrivateNonDeductable;
			item.TotalAmountInclTax		= 678.90M;
			dto.Items.Add(item);
			
			return dto;
		}


		private InvoiceDto GetUnpaidItemPurchase()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Purchase, InvoiceLayout.Item);
			
			dto.Date				= DateTime.Today.Date;
			dto.ContactUid			= this.MrSmith.Uid;
			dto.Summary				= "Test Insert Item Purchase";
			dto.Notes				= "From REST";
			dto.DueOrExpiryDate		= dto.Date.AddMonths(1);
			dto.Status				= InvoiceStatus.Invoice;
			dto.InvoiceNumber		= "I123";
			dto.PurchaseOrderNumber = "<Auto Number>";
			
			ItemInvoiceItemDto item = null;
			
			item = new ItemInvoiceItemDto();
			item.Quantity			= 100;
			item.InventoryItemUid	= this.HardDisk.Uid;
			item.Description		= this.HardDisk.Description;
			item.TaxCode			= TaxCode.ExpInclGst;
			item.UnitPriceInclTax	= 99.95M;
			dto.Items.Add(item);
			
			item = new ItemInvoiceItemDto();
			item.Quantity			= 5000;
			item.InventoryItemUid	= this.Cat5Cable.Uid;
			item.Description		= this.Cat5Cable.Description;
			item.TaxCode			= TaxCode.ExpInclGst;
			item.UnitPriceInclTax	= 0.95M;
			dto.Items.Add(item);

			item = new ItemInvoiceItemDto();
			item.Quantity			= 1;
			item.InventoryItemUid	= this.Shipping1.Uid;
			item.Description		= this.Shipping1.Description;
			item.TaxCode			= TaxCode.ExpInclGst;
			item.UnitPriceInclTax	= 111.11M;
			dto.Items.Add(item);
			
			return dto;
		}
	}
}
