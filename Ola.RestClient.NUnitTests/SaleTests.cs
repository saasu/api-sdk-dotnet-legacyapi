using Ola.RestClient.Dto.Enums;

namespace Ola.RestClient.NUnitTests
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Proxies;
	using Ola.RestClient.Utils;
	using Ola.RestClient.Tasks;

	using NUnit.Framework;

	using System;
	using System.Configuration;
	using System.Net;
	using System.IO;


	[TestFixture]
	public class SaleTests : InvoiceTests
	{
		[TestFixtureSetUp]
		public override void TestFixtureSetUp()
		{
			base.TestFixtureSetUp();
			this.CreatePurchaseForInventoryItemsUsed();
		}

		[Test]
		public void InsertAndGetServiceSale()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetServiceSale();
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, savedDto);

			var line1 = (ServiceInvoiceItemDto)savedDto.Items[0];
			var line2 = (ServiceInvoiceItemDto)savedDto.Items[1];

			Assert.AreEqual("AUD", savedDto.Ccy, "Incorrect Currency.");
			Assert.AreEqual(1, savedDto.FCToBCFXRate, "Incorrect FXRate.");
			Assert.AreEqual(2132.51M, line1.TotalAmountInclTax, "Incorrect amount on line 1.");
			Assert.AreEqual(11.22M, line2.TotalAmountInclTax, "Incorrect amount on line 2.");

			// For testing load generation on Tag queue
			//const int numRuns = 5000;
			//CrudProxy proxy = new InvoiceProxy();
			//for (var i = 0; i < numRuns; i++)
			//{
			//    Console.WriteLine("Inserting sale #{0}", i);
			//    var dto1 = this.GetServiceSale();
			//    proxy.Insert(dto1);
			//}
		}

		[Test]
		public void Insert_And_Get_Service_Sale_With_Tags_On_Line_Items()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetServiceSale();


			dto1.Tags = "Tag1, Tag2, Tag3";

			var lineItem1 = (ServiceInvoiceItemDto)dto1.Items[0];
			var lineItem2 = (ServiceInvoiceItemDto)dto1.Items[1];

			lineItem1.Tags = "LineItem1Tag1, LineItem1Tag2";
			lineItem2.Tags = "LineItem2Tag1, LineItem2Tag2";

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, savedDto);

			var line1 = (ServiceInvoiceItemDto)savedDto.Items[0];
			var line2 = (ServiceInvoiceItemDto)savedDto.Items[1];

			Assert.AreEqual("AUD", savedDto.Ccy, "Incorrect Currency.");
			Assert.AreEqual(1, savedDto.FCToBCFXRate, "Incorrect FXRate.");
			Assert.AreEqual(2132.51M, line1.TotalAmountInclTax, "Incorrect amount on line 1.");
			Assert.AreEqual(11.22M, line2.TotalAmountInclTax, "Incorrect amount on line 2.");
		}

		[Test]
		public void Update_Service_Sale_Line_Item_Tags()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetServiceSale();


			dto1.Tags = "Tag1, Tag2, Tag3";

			var lineItem1 = (ServiceInvoiceItemDto)dto1.Items[0];
			var lineItem2 = (ServiceInvoiceItemDto)dto1.Items[1];

			lineItem1.Tags = "LineItem1Tag1, LineItem1Tag2";
			lineItem2.Tags = "LineItem2Tag1, LineItem2Tag2";

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, savedDto);

			var line1 = (ServiceInvoiceItemDto)savedDto.Items[0];
			var line2 = (ServiceInvoiceItemDto)savedDto.Items[1];

			line1.Tags = "UpdateLineItem1Tag1, UpdateLineItem1Tag2";
			line2.Tags = "UpdateLineItem2Tag1, UpdateLineItem2Tag2";

			proxy.Update(savedDto);

			var updatedDto = (InvoiceDto)proxy.GetByUid(savedDto.Uid);
			AssertEqual(savedDto, updatedDto);


		}

		[Test]
		public void Insert_And_Get_Item_Sale_With_Tags_On_Line_Items()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetItemSale();


			dto1.Tags = "Tag1, Tag2, Tag3";

			var lineItem1 = (ItemInvoiceItemDto)dto1.Items[0];
			var lineItem2 = (ItemInvoiceItemDto)dto1.Items[1];

			lineItem1.Tags = "LineItem1Tag1, LineItem1Tag2";
			lineItem2.Tags = "LineItem2Tag1, LineItem2Tag2";

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, savedDto);
		}

		[Test]
		public void InsertAndGetServiceSaleWith_TradingTerms_DueIn_6_Months()
		{
			CrudProxy proxy = new InvoiceProxy();
			var dto1 = this.GetServiceSale();

			// Due In Type
			dto1.TradingTerms.TypeEnum = TradingTermsType.DueIn;

			// Months invertal type
			dto1.TradingTerms.IntervalTypeEnum = TimeIntervalType.Month;

			// 6 months
			dto1.TradingTerms.Interval = 6;

			//should be due in 6 months form the end of the transaction date.
			var dueDate = dto1.Date.Date.AddMonths(6);

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			//check trading terms
			Assert.AreEqual(dueDate, savedDto.DueOrExpiryDate);
			Assert.AreEqual(dto1.TradingTerms.Type, savedDto.TradingTerms.Type);
			Assert.AreEqual(dto1.TradingTerms.IntervalType, savedDto.TradingTerms.IntervalType);
			Assert.AreEqual(dto1.TradingTerms.Interval, savedDto.TradingTerms.Interval);

			Assert.AreEqual(dto1.TradingTerms.TypeEnum, savedDto.TradingTerms.TypeEnum);
			Assert.AreEqual(dto1.TradingTerms.IntervalTypeEnum, savedDto.TradingTerms.IntervalTypeEnum);
			Assert.AreEqual((int)dto1.TradingTerms.IntervalTypeEnum, savedDto.TradingTerms.IntervalType);
			Assert.AreEqual((int)dto1.TradingTerms.TypeEnum, savedDto.TradingTerms.Type);
		}

		[Test]
		public void InsertAndGetServiceSaleWith_TradingTerms_DueIn_6_Weeks()
		{
			CrudProxy proxy = new InvoiceProxy();
			var dto1 = this.GetServiceSale();

			// DueIn Type
			dto1.TradingTerms.TypeEnum = TradingTermsType.DueIn;

			// Weeks
			dto1.TradingTerms.IntervalTypeEnum = TimeIntervalType.Week;

			// 6 weeks
			dto1.TradingTerms.Interval = 6;

			//should be due in 6 months form the end of the transaction date.
			var dueDate = dto1.Date.Date.AddDays(7 * 6);

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			//check trading terms
			Assert.AreEqual(dueDate, savedDto.DueOrExpiryDate);
			Assert.AreEqual(dto1.TradingTerms.Type, savedDto.TradingTerms.Type);
			Assert.AreEqual(dto1.TradingTerms.IntervalType, savedDto.TradingTerms.IntervalType);
			Assert.AreEqual(dto1.TradingTerms.Interval, savedDto.TradingTerms.Interval);
			Assert.AreEqual(dto1.TradingTerms.TypeEnum, savedDto.TradingTerms.TypeEnum);
			Assert.AreEqual(dto1.TradingTerms.IntervalTypeEnum, savedDto.TradingTerms.IntervalTypeEnum);
			Assert.AreEqual((int)dto1.TradingTerms.IntervalTypeEnum, savedDto.TradingTerms.IntervalType);
			Assert.AreEqual((int)dto1.TradingTerms.TypeEnum, savedDto.TradingTerms.Type);
		}

		[Test]
		public void InsertAndGetServiceSaleWith_TradingTerms_Due_In_Specific_Date()
		{
			CrudProxy proxy = new InvoiceProxy();
			var dto1 = this.GetServiceSale();

			// DueIn type
			dto1.TradingTerms.TypeEnum = TradingTermsType.DueIn;

			// Unspecified - means actual date
			dto1.TradingTerms.IntervalTypeEnum = TimeIntervalType.Unspecified;

			//Set specific date that invoice is due in.
			var dueDate = new DateTime(2012, 12, 24);

			// Set to specific date
			dto1.DueOrExpiryDate = dueDate;

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			//check trading terms
			Assert.AreEqual(dueDate, savedDto.DueOrExpiryDate);
			Assert.AreEqual(dto1.TradingTerms.Type, savedDto.TradingTerms.Type);
			Assert.AreEqual(dto1.TradingTerms.IntervalType, savedDto.TradingTerms.IntervalType);
			Assert.AreEqual(dto1.TradingTerms.Interval, savedDto.TradingTerms.Interval);
			Assert.AreEqual(dto1.TradingTerms.TypeEnum, savedDto.TradingTerms.TypeEnum);
			Assert.AreEqual(dto1.TradingTerms.IntervalTypeEnum, savedDto.TradingTerms.IntervalTypeEnum);
			Assert.AreEqual((int)dto1.TradingTerms.IntervalTypeEnum, savedDto.TradingTerms.IntervalType);
			Assert.AreEqual((int)dto1.TradingTerms.TypeEnum, savedDto.TradingTerms.Type);
		}

		[Test]
		public void InsertAndGetServiceSaleWith_TradingTerms_Due_In_28_days()
		{
			CrudProxy proxy = new InvoiceProxy();
			var dto1 = this.GetServiceSale();

			// DueIn type
			dto1.TradingTerms.TypeEnum = TradingTermsType.DueIn;

			// Days
			dto1.TradingTerms.IntervalTypeEnum = TimeIntervalType.Day;

			// 14 days.
			dto1.TradingTerms.Interval = 28;

			//should be due in 6 months form the end of the transaction date.
			var dueDate = dto1.Date.AddDays(28);

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			//check trading terms
			Assert.AreEqual(dto1.TradingTerms.Type, savedDto.TradingTerms.Type);
			Assert.AreEqual(dto1.TradingTerms.IntervalType, savedDto.TradingTerms.IntervalType);
			Assert.AreEqual(dto1.TradingTerms.Interval, savedDto.TradingTerms.Interval);
			Assert.AreEqual(dueDate, savedDto.DueOrExpiryDate);
			Assert.AreEqual(dto1.TradingTerms.TypeEnum, savedDto.TradingTerms.TypeEnum);
			Assert.AreEqual(dto1.TradingTerms.IntervalTypeEnum, savedDto.TradingTerms.IntervalTypeEnum);
			Assert.AreEqual((int)dto1.TradingTerms.IntervalTypeEnum, savedDto.TradingTerms.IntervalType);
			Assert.AreEqual((int)dto1.TradingTerms.TypeEnum, savedDto.TradingTerms.Type);
		}

		[Test]
		public void InsertAndGetServiceSaleWith_TradingTerms_EOM_Plus_14_days()
		{
			CrudProxy proxy = new InvoiceProxy();
			var dto1 = this.GetServiceSale();

			// EOM+
			dto1.TradingTerms.TypeEnum = TradingTermsType.DueInEomPlusXDays;

			// Days
			dto1.TradingTerms.IntervalTypeEnum = TimeIntervalType.Day;

			// 14 days.
			dto1.TradingTerms.Interval = 14;

			//should be due in 14 days from the end of the month, using the transaction date as the base DateTime.
			var year = dto1.Date.Year;
			var month = dto1.Date.Month;
			var daysInMonth = DateTime.DaysInMonth(year, month);
			var dueDate = new DateTime(year, month, daysInMonth).AddDays(dto1.TradingTerms.Interval);

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			//check trading terms
			Assert.AreEqual(dueDate, savedDto.DueOrExpiryDate);
			Assert.AreEqual(dto1.TradingTerms.Type, savedDto.TradingTerms.Type);
			Assert.AreEqual(dto1.TradingTerms.IntervalType, savedDto.TradingTerms.IntervalType);
			Assert.AreEqual(dto1.TradingTerms.Interval, savedDto.TradingTerms.Interval);
			Assert.AreEqual(dto1.TradingTerms.TypeEnum, savedDto.TradingTerms.TypeEnum);
			Assert.AreEqual(dto1.TradingTerms.IntervalTypeEnum, savedDto.TradingTerms.IntervalTypeEnum);
			Assert.AreEqual((int)dto1.TradingTerms.IntervalTypeEnum, savedDto.TradingTerms.IntervalType);
			Assert.AreEqual((int)dto1.TradingTerms.TypeEnum, savedDto.TradingTerms.Type);
		}


		[Test]
		public void InsertServiceSale1()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();
			dto1.Date = DateTime.Today;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			AssertEqual(dto1, dto2);

			ServiceInvoiceItemDto line1 = (ServiceInvoiceItemDto)dto2.Items[0];
			ServiceInvoiceItemDto line2 = (ServiceInvoiceItemDto)dto2.Items[1];

			Assert.AreEqual("AUD", dto2.Ccy, "Incorrect Currency");
			Assert.AreEqual(1, dto2.FCToBCFXRate, "Incorrect FXRate.");
			Assert.AreEqual(2132.51M, line1.TotalAmountInclTax, "Incorrect amount on line 1.");
			Assert.AreEqual(11.22M, line2.TotalAmountInclTax, "Incorrect amount on line 2.");
		}


		[Test]
		public void InsertItemSale1()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetItemSale();
			dto1.Date = DateTime.Today;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2);
		}


		[Test]
		public void InsertItemSaleOrder1()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetItemSale();
			dto1.Status = InvoiceStatus.Order;
			dto1.Date = DateTime.Today;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2);
		}


		[Test]
		public void InsertServiceSaleWithQuickPayment()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetUnpaidServiceSale();
			QuickPaymentDto quickPayment = new QuickPaymentDto();
			quickPayment.DatePaid = DateTime.UtcNow;
			quickPayment.BankedToAccountUid = this.Westpac.Uid;
			quickPayment.Amount = 2143.73M;
			quickPayment.Summary = "Quick payment for sale.";

			dto1.QuickPayment = quickPayment;

			proxy.Insert(dto1);
		}


		[Test]
		public void ConvertInvoiceToOrder()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetItemSale();
			dto1.Status = InvoiceStatus.Invoice;
			dto1.Date = DateTime.Today;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			dto1.Status = InvoiceStatus.Order;

			try
			{
				proxy.Update(dto1);
				throw new Exception("Exception should be thrown.");
			}
			catch (RestException rex)
			{
				Assert.AreEqual("Sorry, conversion from invoice to non invoice such as order or quote is not allowed.", rex.Message, "Incorrect exception message.");
			}
		}

		#region testing auto numbering of invoices

		[Test]
		public void TestAutoNumberingOfInvoiceNextAvailableNumberIsAvailable()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetServiceSale();
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, savedDto);

			Assert.IsNotNullOrEmpty(dto1.InvoiceNumber);
		}

		[Test]
		public void TestAutoNumberingOfInvoiceIncrementNextAvailableNumber()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetServiceSale();
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			Assert.IsNotNullOrEmpty(dto1.InvoiceNumber);

			string alphaComponent1;
			var numericalComponentDto1 = GetNumericalComponentOfInvoiceNumber(dto1.InvoiceNumber, out alphaComponent1);

			var dto2 = this.GetServiceSale();
			proxy.Insert(dto2);

			Assert.IsTrue(dto2.Uid > 0, "Uid must be > 0 after save.");

			Assert.IsNotNullOrEmpty(dto2.InvoiceNumber);

			string alphaComponent2;
			var numericalComponentDto2 = GetNumericalComponentOfInvoiceNumber(dto2.InvoiceNumber, out alphaComponent2);

			Assert.AreEqual(alphaComponent1, alphaComponent2);
			Assert.AreEqual(numericalComponentDto2, numericalComponentDto1 + 1);
		}

		[Test]
		//Should select next available number after taken one.
		public void TestAutoNumberingOfInvoiceNextAvailableNumberNotAvailable()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetServiceSale();
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			string alphaComponent;
			var numericalComponentDto1 = GetNumericalComponentOfInvoiceNumber(dto1.InvoiceNumber, out alphaComponent);

			Assert.IsNotNull(numericalComponentDto1, "InvoiceNumber returned did not contain a numerical component");

			//manually insert an invoice.
			var nextAvailableInvoiceNumber = alphaComponent + (numericalComponentDto1 + 1).ToString();

			var dto2 = this.GetServiceSale(nextAvailableInvoiceNumber);
			proxy.Insert(dto2);

			Assert.IsTrue(dto2.Uid > 0, "Uid must be > 0 after save.");

			//create next auto numbering invoice.
			var dto3 = this.GetServiceSale();
			proxy.Insert(dto3);

			Assert.IsTrue(dto3.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto3.Uid);

			Assert.IsNotNull(savedDto.InvoiceNumber);

			Assert.AreEqual(savedDto.InvoiceNumber, alphaComponent + (numericalComponentDto1 + 3).ToString());
		}

		private int? GetNumericalComponentOfInvoiceNumber(string invoiceNumber, out string alphaComponent)
		{
			int indexOffirstAplha = -1;

			for (int i = invoiceNumber.Length; i > 0; i--)
			{
				if (!Char.IsNumber(invoiceNumber[i - 1]))
				{
					indexOffirstAplha = i;
					break;
				}
			}

			if (indexOffirstAplha > -1)
			{
				alphaComponent = invoiceNumber.Substring(0, indexOffirstAplha);
				return (Convert.ToInt32(invoiceNumber.Substring(indexOffirstAplha)));
			}
			else
			{
				alphaComponent = string.Empty;
				return Convert.ToInt32(invoiceNumber);
			}
		}

		#endregion



		/*
	 * Would not work work with new tag backend validation. 
		[Test]
		public void InsertWithFolder()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();
			dto1.FolderUid = 128645;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			dto1.Tags = "000 Folder 0";
			AssertEqual(dto1, dto2);
		}
		*/


		[Test]
		public void InsertWithTags()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetServiceSale();
			dto1.Tags = "TestInsertWithTags1, TestInsertWithTags2, TestInsertWithTags3";
			proxy.Insert(dto1);
			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");
			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			AssertEqual(dto1, dto2);
		}


		[Test]
		public void UpdateServiceSale()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();
			proxy.Insert(dto1);

			InvoiceDto fromDB = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			int invoiceUid = dto1.Uid;
			string lastUpdatedUid = dto1.LastUpdatedUid;
			string invoiceNumber = dto1.InvoiceNumber;

			dto1 = this.GetServiceSale2();
			dto1.Uid = invoiceUid;
			dto1.LastUpdatedUid = lastUpdatedUid;
			dto1.InvoiceNumber = invoiceNumber;

			proxy.Update(dto1);

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			Assert.AreEqual("AUD", dto2.Ccy, "Incorrect Currency.");
			Assert.AreEqual(1, dto2.FCToBCFXRate, "Incorrect FXRate.");
			AssertEqual(dto1, dto2);
		}


		[Test]
		public void UpdateAndEmail()
		{
			InvoiceProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();
			proxy.Insert(dto1);

			int invoiceUid = dto1.Uid;
			string lastUpdatedUid = dto1.LastUpdatedUid;
			string invoiceNumber = dto1.InvoiceNumber;

			dto1 = this.GetServiceSale2();
			dto1.Uid = invoiceUid;
			dto1.LastUpdatedUid = lastUpdatedUid;
			dto1.InvoiceNumber = invoiceNumber;

			EmailMessageDto emailMessage = new EmailMessageDto();
			emailMessage.From = ConfigurationSettings.AppSettings["NUnitTests.Email.From"];
			emailMessage.To = ConfigurationSettings.AppSettings["NUnitTests.Email.To"];
			emailMessage.Subject = "Invoice - Sent using NetAccounts OLA REST API (TestUpdateAndEmail).";
			emailMessage.Body = "Update Invoice then email.";

			proxy.UpdateAndEmail(dto1, this.IncTaxTemplateUid, emailMessage);

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2);
		}


		[Test]
		public void InsertAndGetItemSale()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetItemSale();

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2);
		}

		[Test]
		public void InsertAndUpdateItemSalePassingLineItemId()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetItemSale();

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2);

			var lineItems = dto2.Items;

			foreach (var lineItem in lineItems)
			{
				Assert.IsTrue(((ItemInvoiceItemDto)lineItem).Uid > 0);
			}

			InvoiceDto dto3 = GetItemSale();
			dto3.Uid = dto2.Uid;
			dto3.LastUpdatedUid = dto2.LastUpdatedUid;
			dto3.InvoiceNumber = dto2.InvoiceNumber;
			dto3.Items = lineItems;

			proxy.Update(dto3);

			InvoiceDto dto4 = (InvoiceDto)proxy.GetByUid(dto3.Uid);
			AssertEqual(dto3, dto4);
		}

		[Test]
		public void InsertAndGetItemSale_Invoice_Totals_Test()
		{
			//Arange
			var proxy = new InvoiceProxy();
			var input = GetServiceSaleTotalsTest();

			var item = new ServiceInvoiceItemDto
						   {
							   Description = "Design & Development of REST WS",
							   AccountUid = this.IncomeService.Uid,
							   TaxCode = TaxCode.SaleInclGst,
							   TotalAmountInclTax = 2500.65m
						   };

			input.Items.Add(item);

			item = new ServiceInvoiceItemDto
					   {
						   Description = "Subscription to XYZ",
						   AccountUid = this.IncomeSubscription.Uid,
						   TaxCode = TaxCode.SaleInclGst,
						   TotalAmountInclTax = 11.96m
					   };


			input.Items.Add(item);

			//act
			var totalItem1 = ((ServiceInvoiceItemDto)input.Items[0]).TotalAmountInclTax;
			var totalItem2 = ((ServiceInvoiceItemDto)input.Items[1]).TotalAmountInclTax;
			var totalTax1 = totalItem1 - (Math.Round(totalItem1 / (1 + 0.100000m), 2, MidpointRounding.AwayFromZero));
			var totalTax2 = totalItem2 - (Math.Round(totalItem2 / (1 + 0.100000m), 2, MidpointRounding.AwayFromZero));

			proxy.Insert(input);

			var output = (InvoiceDto)proxy.GetByUid(input.Uid);
			var outputLineItem1 = ((ServiceInvoiceItemDto)output.Items[0]);
			var outputLineItem2 = ((ServiceInvoiceItemDto)output.Items[1]);

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
		public void GetPdfInvoice()
		{
			InvoiceProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetItemSale();
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			proxy.GetPdfInvoiceByUid(dto1.Uid, "ItemSale.pdf");									// Template not specified. Will use default template.
			proxy.GetPdfInvoiceByUid(dto1.Uid, this.IncTaxTemplateUid, "ItemSaleIncTax.pdf");	// Use specific template.
		}


		[Test]
		public void EmailPdfInvoice()
		{
			InvoiceProxy proxy = new InvoiceProxy();
			InvoiceDto saleInfo = this.GetItemSale();
			proxy.Insert(saleInfo);

			EmailMessageDto emailMessage = new EmailMessageDto();
			emailMessage.From = ConfigurationSettings.AppSettings["NUnitTests.Email.From"];
			emailMessage.To = ConfigurationSettings.AppSettings["NUnitTests.Email.To"];
			emailMessage.Subject = "Invoice - Sent using NetAccounts OLA REST API.";
			emailMessage.Body = "Hello World. This email is sent using NetAccounts WSAPI";

			proxy.Email(saleInfo.Uid, emailMessage);
		}


		[Test]
		public void EmailPdfInvoiceUsingSpecificTemplate()
		{
			InvoiceProxy proxy = new InvoiceProxy();
			InvoiceDto saleInfo = this.GetItemSale();
			proxy.Insert(saleInfo);

			EmailMessageDto emailMessage = new EmailMessageDto();
			emailMessage.From = ConfigurationSettings.AppSettings["NUnitTests.Email.From"];
			emailMessage.To = ConfigurationSettings.AppSettings["NUnitTests.Email.To"];
			emailMessage.Subject = "Invoice - Sent using NetAccounts OLA REST API.";
			emailMessage.Body = "Hello World. This email is sent using NetAccounts WSAPI";

			proxy.Email(saleInfo.Uid, this.IncTaxTemplateUid, emailMessage);
		}


		[Test]
		public void InsertInvoicePayment()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto sale1 = this.GetUnpaidItemSale();
			InvoiceDto sale2 = this.GetUnpaidServiceSale();

			proxy.Insert(sale1);
			proxy.Insert(sale2);

			InvoicePaymentDto paymentInfo1 = new InvoicePaymentDto(TransactionType.SalePayment);
			paymentInfo1.Date = DateTime.Today.Date;
			paymentInfo1.PaymentAccountUid = this.Westpac.Uid;
			paymentInfo1.Reference = Guid.NewGuid().ToString();
			paymentInfo1.Summary = "Payment for 2 Outstanding Invoices";
			paymentInfo1.Fee = 9.99m;

			InvoicePaymentItemDto item = null;

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale2.Uid;
			item.Amount = 20.05M;
			paymentInfo1.Items.Add(item);

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale1.Uid;
			item.Amount = 23.75M;
			paymentInfo1.Items.Add(item);

			proxy = new InvoicePaymentProxy();
			proxy.Insert(paymentInfo1);

			Assert.IsTrue(paymentInfo1.Uid > 0, "Uid must be > 0 after save.");

			InvoicePaymentDto paymentInfo2 = (InvoicePaymentDto)proxy.GetByUid(paymentInfo1.Uid);
			Assert.AreEqual("AUD", paymentInfo2.Ccy, "Incorrect Currency.");
			AssertEqual(paymentInfo1, paymentInfo2);
		}


		[Test]
		public void InsertInvoicePaymentIncorrectTranType()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto sale1 = this.GetUnpaidItemSale();
			InvoiceDto sale2 = this.GetUnpaidServiceSale();

			proxy.Insert(sale1);
			proxy.Insert(sale2);

			InvoicePaymentDto paymentInfo1 = new InvoicePaymentDto(TransactionType.SalePayment);
			paymentInfo1.Date = DateTime.Today.Date;
			paymentInfo1.PaymentAccountUid = this.Westpac.Uid;
			paymentInfo1.Reference = Guid.NewGuid().ToString();
			paymentInfo1.Summary = "Payment for 2 Outstanding Invoices";
			paymentInfo1.TransactionType = "PS";
			paymentInfo1.Fee = 3.45m;

			InvoicePaymentItemDto item = null;

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale2.Uid;
			item.Amount = 20.05M;
			paymentInfo1.Items.Add(item);

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale1.Uid;
			item.Amount = 23.75M;
			paymentInfo1.Items.Add(item);

			proxy = new InvoicePaymentProxy();
			try
			{
				proxy.Insert(paymentInfo1);
				throw new Exception("No exception was thrown.");
			}
			catch (RestException rex)
			{
				Assert.AreEqual("Incorrect transaction type. Use 'SP' for Sale Payment and 'PP' for purchase payment.", rex.Message, "Incorrect message.");
			}
		}


		[Test]
		public void InsertInvoicePaymentIncorrectPaymentAccountType()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto sale1 = this.GetUnpaidItemSale();

			proxy.Insert(sale1);

			InvoicePaymentDto paymentInfo1 = new InvoicePaymentDto(TransactionType.SalePayment);
			paymentInfo1.Date = DateTime.Today.Date;
			paymentInfo1.PaymentAccountUid = this.AssetInventory.Uid;
			paymentInfo1.Reference = Guid.NewGuid().ToString();
			paymentInfo1.Summary = "Payment for Outstanding Invoice";
			paymentInfo1.Fee = 19.99m;

			InvoicePaymentItemDto item = null;

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale1.Uid;
			item.Amount = 23.75M;
			paymentInfo1.Items.Add(item);

			proxy = new InvoicePaymentProxy();
			try
			{
				proxy.Insert(paymentInfo1);
				throw new Exception("No exception was thrown.");
			}
			catch (RestException rex)
			{
				Assert.AreEqual("The referenced Bank Account does not exist.", rex.Message, "Incorrect message.");
			}
		}


		[Test]
		public void UpdateInvoicePayment()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto sale1 = this.GetUnpaidItemSale();
			InvoiceDto sale2 = this.GetUnpaidServiceSale();

			proxy.Insert(sale1);
			proxy.Insert(sale2);

			InvoicePaymentDto paymentInfo1 = new InvoicePaymentDto(TransactionType.SalePayment);
			paymentInfo1.Date = DateTime.Today.Date;
			paymentInfo1.PaymentAccountUid = this.Westpac.Uid;
			paymentInfo1.Reference = Guid.NewGuid().ToString();
			paymentInfo1.Summary = "Payment for 2 Outstanding Invoices";
			paymentInfo1.Fee = 59.99m;

			InvoicePaymentItemDto item = null;

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale2.Uid;
			item.Amount = 20.05M;
			paymentInfo1.Items.Add(item);

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale1.Uid;
			item.Amount = 23.75M;
			paymentInfo1.Items.Add(item);

			proxy = new InvoicePaymentProxy();
			proxy.Insert(paymentInfo1);


			//
			//	Update
			paymentInfo1.Summary = "Payment - Updated.";

			paymentInfo1.Items.Clear();

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale1.Uid;
			item.Amount = 30M;
			paymentInfo1.Items.Add(item);

			proxy.Update(paymentInfo1);

			//
			//	TODO: Check result.
			//
		}


		[Test]
		public void DeleteInvoicePayment()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto sale = this.GetUnpaidServiceSale();
			proxy.Insert(sale);

			InvoicePaymentDto paymentInfo1 = new InvoicePaymentDto(TransactionType.SalePayment);
			paymentInfo1.Date = DateTime.Today.Date;
			paymentInfo1.PaymentAccountUid = this.Westpac.Uid;
			paymentInfo1.Reference = Guid.NewGuid().ToString();
			paymentInfo1.Summary = "Payment for Outstanding Sale Invoice";
			paymentInfo1.Fee = 9.99m;

			InvoicePaymentItemDto item = null;

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale.Uid;
			item.Amount = 2132.51M;
			paymentInfo1.Items.Add(item);

			proxy = new InvoicePaymentProxy();
			proxy.Insert(paymentInfo1);

			proxy.DeleteByUid(paymentInfo1.Uid);
		}


		[Test]
		public void BulkInsertInvoice()
		{
			TasksRunner tasksRunner = new TasksRunner(this.WSAccessKey, this.FileUid);

			InsertInvoiceTask insertInvoiceTask = null;

			insertInvoiceTask = new InsertInvoiceTask();
			insertInvoiceTask.EntityToInsert = this.GetServiceSale();
			tasksRunner.Tasks.Add(insertInvoiceTask);

			insertInvoiceTask = new InsertInvoiceTask();
			insertInvoiceTask.EntityToInsert = this.GetItemSale();
			tasksRunner.Tasks.Add(insertInvoiceTask);

			TasksResponse tasksResponse = tasksRunner.Execute();
		}


		/*
		[Test]
		public void BulkInsert1000Invoices()
		{
			TasksRunner tasksRunner = new TasksRunner(this.FileUid);
			InsertInvoiceTask insertInvoiceTask = null;
			for (int i = 0; i < 1000; i++)
			{
				insertInvoiceTask = new InsertInvoiceTask();
				insertInvoiceTask.EntityToInsert = this.GetServiceSale();
				tasksRunner.Tasks.Add(insertInvoiceTask);
			}
			
			TasksResponse tasksResponse = tasksRunner.Execute();
		}
		*/


		[Test]
		public void InsertAndEmail()
		{
			InvoiceProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();

			EmailMessageDto emailMessage = new EmailMessageDto();
			emailMessage.From = ConfigurationSettings.AppSettings["NUnitTests.Email.From"];
			emailMessage.To = ConfigurationSettings.AppSettings["NUnitTests.Email.To"];
			emailMessage.Subject = "Insert And Email: Template Not Specified";
			emailMessage.Body = "Insert Invoice then email.";

			proxy.InsertAndEmail(dto1, emailMessage);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			//
			//	Ensure IsSent is updated.
			//
			dto1 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			Assert.IsTrue(dto1.IsSent, "Invoice should have been sent.");
		}


		[Test]
		public void InsertAndEmailBackwardCompatibilityTest()
		{
			string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<tasks xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <insertInvoice emailToContact=""true"">
    <invoice uid=""0"">
      <transactionType>S</transactionType>
      <date>2005-09-30</date>
      <contactUid>{0}</contactUid>
      <shipToContactUid>0</shipToContactUid>
      <folderUid>0</folderUid>
      <summary>Test POST sale</summary>
      <notes>From REST</notes>
      <requiresFollowUp>false</requiresFollowUp>
      <dueOrExpiryDate>2005-12-01</dueOrExpiryDate>
      <layout>S</layout>
      <status>I</status>
      <invoiceNumber>&lt;Auto Number&gt;</invoiceNumber>
      <purchaseOrderNumber>PO222</purchaseOrderNumber>
      <invoiceItems>
        <serviceInvoiceItem>
          <description>Design &amp; Development of REST WS</description>
          <accountUid>{1}</accountUid>
          <taxCode>G1</taxCode>
          <totalAmountInclTax>2132.51</totalAmountInclTax>
        </serviceInvoiceItem>
        <serviceInvoiceItem>
          <description>Subscription to XYZ</description>
          <accountUid>{2}</accountUid>
          <taxCode>G1</taxCode>
          <totalAmountInclTax>11.22</totalAmountInclTax>
        </serviceInvoiceItem>
      </invoiceItems>
      <quickPayment>
        <datePaid>2005-09-30</datePaid>
        <dateCleared>0001-01-01</dateCleared>
        <bankedToAccountUid>{3}</bankedToAccountUid>
        <amount>100</amount>
        <reference>CASH</reference>
        <summary>Quick payment from NUnitTests.</summary>
      </quickPayment>
      <isSent>false</isSent>
    </invoice>
    <createAsAdjustmentNote>true</createAsAdjustmentNote>
    <emailMessage>
      <from>service@saasu.com</from>
      <to>test@saasu.com</to>
      <subject>Insert And Email: Template Not Specified</subject>
      <body>Insert Invoice then email.</body>
    </emailMessage>
  </insertInvoice>
</tasks>
";
			xml = string.Format(xml, this.MrSmith.Uid, this.IncomeHardwareSales.Uid, this.IncomeMisc.Uid, this.StGeorge.Uid);
			string url = RestProxy.MakeUrl("Tasks");
			string xmlResponse = HttpUtils.Post(url, xml);
			TasksResponse res = (TasksResponse)XmlSerializationUtils.Deserialize(typeof(TasksResponse), xmlResponse);
			InsertInvoiceResult result = (InsertInvoiceResult)res.Results[0];
			Assert.IsTrue(result.InsertedEntityUid > 0, "Inserted entity uid must be > 0.");
			Assert.AreEqual(true, result.SentToContact, "Incorrect sentToContact flag.");
		}


		[Test]
		public void InsertAndEmailUsingIncTaxTemplate()
		{
			InvoiceProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();

			EmailMessageDto emailMessage = new EmailMessageDto();
			emailMessage.From = ConfigurationSettings.AppSettings["NUnitTests.Email.From"];
			emailMessage.To = ConfigurationSettings.AppSettings["NUnitTests.Email.To"];
			emailMessage.Subject = "Insert And Email: Inc Tax Template";
			emailMessage.Body = "Insert Invoice then email.";

			proxy.InsertAndEmail(dto1, this.IncTaxTemplateUid, emailMessage);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			//
			//	Ensure IsSent is updated.
			//
			dto1 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			Assert.IsTrue(dto1.IsSent, "Invoice should have been sent.");
		}


		[Test]
		public void InsertAndEmailUsingShippingSlipTemplate()
		{
			InvoiceProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();

			EmailMessageDto emailMessage = new EmailMessageDto();
			emailMessage.From = ConfigurationSettings.AppSettings["NUnitTests.Email.From"];
			emailMessage.To = ConfigurationSettings.AppSettings["NUnitTests.Email.To"];
			emailMessage.Subject = "Insert And Email: Shipping slip Template";
			emailMessage.Body = "Insert Invoice then email.";

			proxy.InsertAndEmail(dto1, this.ShippingTemplateUid, emailMessage);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			//
			//	Ensure IsSent is updated.
			//
			dto1 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			Assert.IsTrue(dto1.IsSent, "Invoice should have been sent.");
		}


		[Test]
		public void InsertAndEmailUsingInvalidTemplate()
		{
			InvoiceProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();

			EmailMessageDto emailMessage = new EmailMessageDto();
			emailMessage.From = ConfigurationSettings.AppSettings["NUnitTests.Email.From"];
			emailMessage.To = ConfigurationSettings.AppSettings["NUnitTests.Email.To"];
			emailMessage.Subject = "Insert And Email: Template Not Specified";
			emailMessage.Body = "Insert Invoice then email.";

			try
			{
				proxy.InsertAndEmail(dto1, 99999, emailMessage);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException rex)
			{
				Assert.AreEqual("Unable to find the requested  template.", rex.Message, "Incorrect error message.");
			}
		}


		[Test]
		public void DeleteSale()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();
			proxy.Insert(dto1);

			proxy.DeleteByUid(dto1.Uid);

			try
			{
				proxy.GetByUid(dto1.Uid);
			}
			catch (RestException ex)
			{
				Assert.AreEqual("RecordNotFoundException", ex.Type);
			}
		}


		[Test]
		public void DeleteSalePayment()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto sale1 = this.GetUnpaidItemSale();
			InvoiceDto sale2 = this.GetUnpaidServiceSale();

			proxy.Insert(sale1);
			proxy.Insert(sale2);

			InvoicePaymentDto paymentInfo1 = new InvoicePaymentDto(TransactionType.SalePayment);
			paymentInfo1.Date = DateTime.Today.Date;
			paymentInfo1.PaymentAccountUid = this.Westpac.Uid;
			paymentInfo1.Reference = Guid.NewGuid().ToString();
			paymentInfo1.Summary = "Payment for 2 Outstanding Invoices";
			paymentInfo1.Fee = 3.55m;

			InvoicePaymentItemDto item = null;

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale2.Uid;
			item.Amount = 20.05M;
			paymentInfo1.Items.Add(item);

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale1.Uid;
			item.Amount = 23.75M;
			paymentInfo1.Items.Add(item);

			proxy = new InvoicePaymentProxy();
			proxy.Insert(paymentInfo1);


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


		[Test]
		public void InvalidValues()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetServiceSale();

			//
			//	Transaction type is not set.
			//
			dto1.TransactionType = null;
			try
			{
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown 1.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("ArgumentException", ex.Type);
			}

			//
			//	Invalid value set for transaction type.
			//
			dto1.TransactionType = "SP";
			try
			{
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown 2.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("ApplicationException", ex.Type);
			}

			//
			//	Status not set
			//
			dto1.TransactionType = TransactionType.Sale;
			dto1.Status = null;
			proxy.Insert(dto1);	// This will default to invoice.

			//
			//	Invalid status
			//
			dto1.Status = "P";
			try
			{
				dto1.Uid = 0;
				dto1.InvoiceNumber = "<Auto Number>";
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown 4.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("ArgumentException", ex.Type);
			}

			dto1.Status = InvoiceStatus.Order;
			proxy.Insert(dto1);
		}


		[Test]
		public void ApplyingPaymentToQuote()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetServiceSale();
			dto1.Status = InvoiceStatus.Quote;

			try
			{
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("InvalidTransactionException", ex.Type);
			}
		}


		[Test]
		public void DuplicateInvoiceNumber()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetServiceSale();
			string invoiceNumber = this.TestUid + "-INV-1";
			dto1.InvoiceNumber = invoiceNumber;

			proxy.Insert(dto1);

			dto1 = this.GetServiceSale();
			dto1.InvoiceNumber = invoiceNumber;

			try
			{
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("DuplicateInvoiceNumberException", ex.Type);
			}
		}


		[Test]
		public void Overpayment()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetServiceSale();
			dto1.QuickPayment.Amount = 10000;

			try
			{
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("InvalidInvoicePaymentException", ex.Type);
			}
		}


		[Test]
		public void ApplyPaymentOnUpdate()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetUnpaidServiceSale();

			proxy.Insert(dto1);

			dto1.QuickPayment = this.GetServiceSale().QuickPayment;
			proxy.Update(dto1);
		}


		[Test]
		public void ApplyPaymentOnUpdateCausingOverpayment()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetUnpaidServiceSale();

			proxy.Insert(dto1);

			dto1.QuickPayment = this.GetServiceSale().QuickPayment;
			dto1.QuickPayment.Amount = 10000;

			try
			{
				proxy.Update(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("InvalidInvoicePaymentException", ex.Type);
			}
		}


		[Test]
		public void InsertWithInvalidUid()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetUnpaidServiceSale();
			dto1.Uid = 2;

			try
			{
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("ArgumentException", ex.Type);
			}
		}


		[Test]
		public void UpdateWithInvalidUid()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetUnpaidServiceSale();

			proxy.Insert(dto1);

			dto1.Uid = 0;

			try
			{
				proxy.Update(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("ArgumentException", ex.Type);
			}
		}


		[Test]
		public void UpdateShipToContactForServiceSale()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();
			dto1.ShipToContactUid = MrsSmith.Uid;

			proxy.Insert(dto1);

			dto1.ShipToContactUid = MrsSmith.Uid;

			proxy.Update(dto1);

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2);
		}


		[Test]
		public void ApplyingTaxCodeGSTTransactionCategory()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetUnpaidServiceSale();

			ServiceInvoiceItemDto item = (ServiceInvoiceItemDto)dto1.Items[0];
			item.AccountUid = 738;	//	Asset: GST Paid on Purchases
			item.TaxCode = TaxCode.SaleInclGst;

			try
			{
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("InvalidTaxCodeException", ex.Type);
			}
		}

		[Test]
		public void ApplyingBankAccountAsAccountUidShouldThrowException()
		{
			//
			//	Apply TaxCode to GST account.
			//
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetUnpaidServiceSale();

			ServiceInvoiceItemDto item = (ServiceInvoiceItemDto)dto1.Items[0];

			// Banks accounts can't be used as Accounts for line items.
			item.AccountUid = this.Westpac.Uid;
			item.TaxCode = TaxCode.SaleInclGst;

			try
			{
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("InvalidReferenceException", ex.Type);
			}
		}

		[Test]
		public void SellingItemWithInsufficientStockOnHand()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetItemSale();

			ItemInvoiceItemDto item = (ItemInvoiceItemDto)dto1.Items[0];
			item.Quantity = 999999;

			try
			{
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException ex)
			{
				Assert.AreEqual("ApplicationException", ex.Type);
				Assert.IsTrue(ex.Message.Contains("Unable to complete the requested operation as it will cause negative stock-on-hand"), "Error message does not contain the expected value.");
			}
		}


		[Test]
		public void InsertUsingLowerCaseTranType()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetServiceSale();
			dto1.TransactionType = "s";	// Lower case shouldn't really matter
			proxy.Insert(dto1);
			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");
			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
		}


		[Test]
		public void InsertUsingInvalidTranType()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetServiceSale();
			dto1.TransactionType = "PS";
			try
			{
				proxy.Insert(dto1);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException rex)
			{
				Assert.AreEqual("Invalid transaction type or layout. Valid transaction type is either 'S' or 'P'. Valid layout is either 'S' or 'I'.", rex.Message, "Invalid error message.");
			}
		}


		[Test]
		public void InsertWithShipToContact()
		{
			CrudProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetItemSale();
			dto1.ShipToContactUid = MrsSmith.Uid;

			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, dto2);
		}


		[Test]
		public void InsertMultiCcyServiceSaleUsingFXFeed()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();
			dto1.Date = DateTime.Parse("12-Feb-2010");
			dto1.Ccy = "USD";
			dto1.AutoPopulateFXRate = true;
			dto1.FCToBCFXRate = 0M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			Assert.AreEqual(dto1.Ccy, dto2.Ccy, "Incorrect Currency.");
			Assert.AreEqual(dto1.AutoPopulateFXRate, dto2.AutoPopulateFXRate, "Incorrect Auto Populate FX Rate flag.");
			Assert.IsTrue(dto2.FCToBCFXRate > 0, "Incorrect FX rate.");		// Set by the server.

			AssertEqual(dto1, dto2);

			ServiceInvoiceItemDto line1 = (ServiceInvoiceItemDto)dto2.Items[0];
			ServiceInvoiceItemDto line2 = (ServiceInvoiceItemDto)dto2.Items[1];

			Assert.AreEqual(2132.51M, line1.TotalAmountInclTax, "Incorrect amount on line 1.");
			Assert.AreEqual(11.22M, line2.TotalAmountInclTax, "Incorrect amount on line 2.");
		}


		[Test]
		public void InsertMultiCcyServiceSaleNotUsingFXFeed()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();
			dto1.Date = DateTime.Parse("12-Feb-2010");
			dto1.Ccy = "USD";
			dto1.AutoPopulateFXRate = false;
			dto1.FCToBCFXRate = 1.1458902517M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			ServiceInvoiceItemDto line1 = (ServiceInvoiceItemDto)dto2.Items[0];
			ServiceInvoiceItemDto line2 = (ServiceInvoiceItemDto)dto2.Items[1];

			Assert.AreEqual("USD", dto1.Ccy, "Incorrect Currency");
			Assert.AreEqual(dto1.AutoPopulateFXRate, dto2.AutoPopulateFXRate, "Incorrect Auto Populate FX Rate flag.");
			Assert.AreEqual(1.1458902517M, dto2.FCToBCFXRate, "Incorrect FX rate.");

			AssertEqual(dto1, dto2);

			Assert.AreEqual(2132.51M, line1.TotalAmountInclTax, "Incorrect amount on line 1.");
			Assert.AreEqual(11.22M, line2.TotalAmountInclTax, "Incorrect amount on line 2.");
		}


		[Test]
		public void UpdateMultiCcySaleOverrideAutoFXFeedRate()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetUnpaidServiceSale();
			dto1.Date = DateTime.Parse("12-Feb-2010");
			dto1.Ccy = "USD";
			dto1.AutoPopulateFXRate = true;
			dto1.FCToBCFXRate = 0M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			dto2.AutoPopulateFXRate = false;
			dto2.FCToBCFXRate = 1.1458902517M;
			proxy.Update(dto2);

			InvoiceDto dto3 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			Assert.AreEqual(dto1.Ccy, dto2.Ccy, "Incorrect Currency.");
			Assert.AreEqual(dto2.AutoPopulateFXRate, dto3.AutoPopulateFXRate, "Incorrect Auto Populate FX Rate flag.");
			Assert.AreEqual(1.1458902517M, dto3.FCToBCFXRate, "Incorrect FX rate.");

			AssertEqual(dto2, dto3);
		}


		[Test]
		public void UpdateMultiCcySaleOverrideAutoFXFeedRateWithZero()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetUnpaidServiceSale();
			dto1.Date = DateTime.Parse("12-Feb-2010");
			dto1.Ccy = "USD";
			dto1.AutoPopulateFXRate = true;
			dto1.FCToBCFXRate = 0M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			dto2.AutoPopulateFXRate = false;
			dto2.FCToBCFXRate = 0M;

			try
			{
				proxy.Update(dto2);
			}
			catch (RestException rex)
			{
				Assert.AreEqual("Invalid FX Rate. Must be > 0.", rex.Message, "Incorrect message.");
			}
		}


		[Test]
		public void UpdateMultiCcySaleOverrideManualFXRate()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetUnpaidServiceSale();
			dto1.Date = DateTime.Parse("12-Feb-2010");
			dto1.Ccy = "USD";
			dto1.AutoPopulateFXRate = false;
			dto1.FCToBCFXRate = 1.1458902517M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			dto2.AutoPopulateFXRate = true;
			dto2.FCToBCFXRate = 0M;
			proxy.Update(dto2);

			InvoiceDto dto3 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			Assert.AreEqual(dto2.Ccy, dto3.Ccy, "Incorrect Currency.");
			Assert.AreEqual(dto2.AutoPopulateFXRate, dto3.AutoPopulateFXRate, "Incorrect Auto Populate FX Rate flag.");
			Assert.IsTrue(dto3.FCToBCFXRate > 0, "Incorrect FX rate.");

			AssertEqual(dto2, dto3);
		}


		[Test]
		public void UpdateMultiCcySaleOverrideManualFXRateWithZero()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetUnpaidServiceSale();
			dto1.Date = DateTime.Parse("12-Feb-2010");
			dto1.Ccy = "USD";
			dto1.AutoPopulateFXRate = false;
			dto1.FCToBCFXRate = 1.1458902517M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			dto2.AutoPopulateFXRate = true;
			dto2.FCToBCFXRate = 0M;
			proxy.Update(dto2);

			InvoiceDto dto3 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			Assert.AreEqual(dto2.Ccy, dto3.Ccy, "Incorrect Currency.");
			Assert.AreEqual(dto2.AutoPopulateFXRate, dto3.AutoPopulateFXRate, "Incorrect Auto Populate FX Rate flag.");
			Assert.IsTrue(dto3.FCToBCFXRate > 0, "Incorrect FX rate.");

			AssertEqual(dto2, dto3);
		}


		[Test]
		public void UpdateMultiCcySaleWithPaymentOverrideAutoFXFeed()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();
			dto1.Date = DateTime.Parse("12-Feb-2010");
			dto1.Ccy = "USD";
			dto1.AutoPopulateFXRate = true;
			dto1.FCToBCFXRate = 0M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			dto2.AutoPopulateFXRate = false;
			dto2.FCToBCFXRate = 1.039M;
			try
			{
				proxy.Update(dto2);	// Should not be allowed. There's a payment applied already.
				Assert.Fail("No exception thrown.");
			}
			catch (RestException restException)
			{
				var expectedMessage =
					"Sorry, FX rate cannot be changed because there are payments applied to the transaction already. Please change the FX rate on payment instead.";
				Assert.AreEqual(expectedMessage, restException.Message, "Incorrect error message.");
			}
		}


		[Test]
		public void UpdateMultiCcySaleWithPaymentOverrideManualFXFeed()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();
			dto1.Date = DateTime.Parse("12-Feb-2010");
			dto1.Ccy = "USD";
			dto1.AutoPopulateFXRate = false;
			dto1.FCToBCFXRate = 1.1458902517M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			dto2.FCToBCFXRate = 1.1258801678M;

			try
			{
				proxy.Update(dto2);
				throw new Exception("Expected exception not thrown.");
			}
			catch (RestException rex)
			{
				Assert.AreEqual("Sorry, FX rate cannot be changed because there are payments applied to the transaction already. Please change the FX rate on payment instead.", rex.Message, "Incorrect message.");
			}
		}


		[Test]
		public void UpdateMultiCcySaleWithPaymentChangeCurrency()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetServiceSale();
			dto1.Date = DateTime.Parse("12-Feb-2010");
			dto1.Ccy = "USD";
			dto1.AutoPopulateFXRate = true;
			dto1.FCToBCFXRate = 1.1337403549M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			dto2.Ccy = "EUR";

			try
			{
				proxy.Update(dto2);
			}
			catch (RestException rex)
			{
				Assert.AreEqual("Sorry, FX rate cannot be changed because there are payments applied to the transaction already. Please change the FX rate on payment instead.", rex.Message, "Incorrect message.");
			}
		}


		[Test]
		public void UpdateMultiCcySaleChangeCurrency()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto dto1 = this.GetUnpaidServiceSale();
			dto1.Date = DateTime.Parse("12-Feb-2010");
			dto1.Ccy = "USD";
			dto1.AutoPopulateFXRate = true;
			dto1.FCToBCFXRate = 0M;
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			InvoiceDto dto2 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			dto2.Ccy = "EUR";
			dto2.AutoPopulateFXRate = true;
			dto2.FCToBCFXRate = 0M;
			proxy.Update(dto2);

			InvoiceDto dto3 = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			Assert.AreEqual(dto2.Ccy, dto3.Ccy, "Incorrect Currency.");
			Assert.AreEqual(dto2.AutoPopulateFXRate, dto3.AutoPopulateFXRate, "Incorrect Auto Populate FX Rate flag.");
			Assert.IsTrue(dto3.FCToBCFXRate > 0, "Incorrect FX rate.");

			AssertEqual(dto2, dto3);
		}


		[Test]
		public void InsertMultiCcyInvoicePayment()
		{
			CrudProxy proxy = new InvoiceProxy();

			InvoiceDto sale1 = this.GetUnpaidItemSale();
			sale1.Date = DateTime.Parse("12-Feb-2010");
			sale1.Ccy = "EUR";
			sale1.AutoPopulateFXRate = true;
			sale1.FCToBCFXRate = 1.6194842787M;

			proxy.Insert(sale1);


			InvoicePaymentDto paymentInfo1 = new InvoicePaymentDto(TransactionType.SalePayment);
			paymentInfo1.Date = DateTime.Today.Date;
			paymentInfo1.PaymentAccountUid = this.Westpac.Uid;
			paymentInfo1.Reference = Guid.NewGuid().ToString();
			paymentInfo1.Summary = "Payment for 2 Outstanding Invoices";
			paymentInfo1.Ccy = "EUR";
			paymentInfo1.AutoPopulateFXRate = true;
			paymentInfo1.FCToBCFXRate = 1.6194842787M;
			paymentInfo1.Fee = 1.75m;

			InvoicePaymentItemDto item = null;

			item = new InvoicePaymentItemDto();
			item.InvoiceUid = sale1.Uid;
			item.Amount = 23.75M;
			paymentInfo1.Items.Add(item);

			proxy = new InvoicePaymentProxy();
			proxy.Insert(paymentInfo1);

			Assert.IsTrue(paymentInfo1.Uid > 0, "Uid must be > 0 after save.");

			InvoicePaymentDto paymentInfo2 = (InvoicePaymentDto)proxy.GetByUid(paymentInfo1.Uid);

			Assert.AreEqual(paymentInfo1.Ccy, paymentInfo2.Ccy, "Incorrect Currency.");
			Assert.AreEqual(paymentInfo1.AutoPopulateFXRate, paymentInfo2.AutoPopulateFXRate, "Incorrect Auto Populate FX Rate flag.");
			Assert.AreEqual(1.6194842787M, paymentInfo2.FCToBCFXRate, "Incorrect FX rate.");

			AssertEqual(paymentInfo1, paymentInfo2);
		}


		[Test]
		public void InsertServiceSaleUsingDirectXmlWithNewFieldsOmitted()
		{
			// TradingTerms elements are omitted here but should still works. 
			string id = DateTime.Now.Ticks.ToString();
			string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<tasks>
  <insertInvoice>
    <invoice uid=""0"">
      <transactionType>S</transactionType>
      <date>2009-02-27</date>
      <contactUid></contactUid>
      <summary>Test POST sale</summary>
      <notes>From REST</notes>
      <requiresFollowUp>false</requiresFollowUp>
      <dueOrExpiryDate>2005-12-01</dueOrExpiryDate>
      <layout>S</layout>
      <status>I</status>
      <invoiceNumber>&lt;Auto Number&gt;</invoiceNumber>
      <purchaseOrderNumber></purchaseOrderNumber>
      <invoiceItems>
        <serviceInvoiceItem>
          <description>Design &amp; Development of REST WS</description>
          <accountUid>" + IncomeSubscription.Uid + @"</accountUid>
          <taxCode>G1</taxCode>
          <totalAmountInclTax>100</totalAmountInclTax>
        </serviceInvoiceItem>
      </invoiceItems>
      <isSent>false</isSent>
    </invoice>
  </insertInvoice>
</tasks>
";
			string url = RestProxy.MakeUrl("Tasks");
			string result = HttpUtils.Post(url, xml);

			var tasksResponse = (TasksResponse)XmlSerializationUtils.Deserialize(typeof(TasksResponse), result);
			var insertResult = (InsertInvoiceResult)tasksResponse.Results[0];
			Assert.IsTrue(insertResult.InsertedEntityUid > 0, "Uid must be > 0 after save.");
		}

		[Test]
		public void InsertAdjustmentNote()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetAdjustmentNote();
			proxy.Insert(dto1);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);

			AssertEqual(dto1, savedDto);
		}

		[Test]
		public void ChangeInvoiceTypeFromTaxInvoiceToAdjustmentNote()
		{
			CrudProxy proxy = new InvoiceProxy();

			var dto1 = this.GetAdjustmentNote();
			dto1.InvoiceType = InvoiceType.TaxInvoice;
			proxy.Insert(dto1);
			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			var savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			AssertEqual(dto1, savedDto);
			Assert.AreEqual(InvoiceType.TaxInvoice, savedDto.InvoiceType, "Incorrect invoice type.");

			savedDto.InvoiceType = InvoiceType.AdjustmentNote;
			proxy.Update(savedDto);

			savedDto = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			Assert.AreEqual(InvoiceType.AdjustmentNote, savedDto.InvoiceType, "Incorrect invoice type.");
		}

		public InvoiceDto GetServiceSale(string invoiceNumber = null)
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Service);

			dto.Date = DateTime.Parse("30-Sep-05");
			dto.ContactUid = this.MrSmith.Uid;
			dto.Summary = "Test POST sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("1-Dec-05");
			dto.Status = "I";
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO222";
			dto.IsSent = false;

			if (!string.IsNullOrWhiteSpace(invoiceNumber))
			{
				dto.InvoiceNumber = invoiceNumber.Trim();
			}

			ServiceInvoiceItemDto item = new ServiceInvoiceItemDto();
			item.Description = "Design & Development of REST WS";
			item.AccountUid = this.IncomeService.Uid;
			item.TaxCode = TaxCode.SaleInclGst;
			item.TotalAmountInclTax = 2132.51M;
			dto.Items.Add(item);

			item = new ServiceInvoiceItemDto();
			item.Description = "Subscription to XYZ";
			item.AccountUid = this.IncomeSubscription.Uid;
			item.TaxCode = TaxCode.SaleInclGst;
			item.TotalAmountInclTax = 11.22M;
			dto.Items.Add(item);

			QuickPaymentDto payment = new QuickPaymentDto();
			payment.DatePaid = dto.Date;
			payment.BankedToAccountUid = this.Westpac.Uid;
			payment.Reference = "CASH";
			payment.Summary = "Quick payment from NUnitTests.";
			payment.Amount = 100;

			dto.QuickPayment = payment;

			return dto;
		}

		public InvoiceDto GetAdjustmentNote()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Service);

			dto.Date = DateTime.Parse("30-Sep-05");
			dto.ContactUid = this.MrSmith.Uid;
			dto.Summary = "Test Adjustment Note";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("31-Jan-2012");
			// Don't set the status. Use InvoiceType instead. Eventually, InvoiceStatus will be deprecated and replaced with InvoiceType.
			dto.InvoiceType = InvoiceType.AdjustmentNote;
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO222";
			dto.IsSent = false;

			ServiceInvoiceItemDto item = new ServiceInvoiceItemDto();
			item.Description = "Refund";
			item.AccountUid = this.IncomeService.Uid;
			item.TaxCode = TaxCode.SaleInclGst;
			item.TotalAmountInclTax = -1000M;
			dto.Items.Add(item);

			QuickPaymentDto payment = new QuickPaymentDto();
			payment.DatePaid = dto.Date;
			payment.BankedToAccountUid = this.Westpac.Uid;
			payment.Reference = "CASH";
			payment.Summary = "Quick payment from NUnitTests.";
			payment.Amount = -1000;

			dto.QuickPayment = payment;

			return dto;
		}

		public InvoiceDto GetServiceSaleTotalsTest()
		{
			var dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Service);

			dto.Date = DateTime.Parse("30-Sep-05");
			dto.ContactUid = this.MrSmith.Uid;
			dto.Summary = "Test POST sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("1-Dec-05");
			dto.Status = "I";
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO23222";
			dto.IsSent = false;

			return dto;
		}

		public InvoiceDto GetUnpaidServiceSale()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Service);

			dto.Date = DateTime.Parse("30-Sep-05");
			dto.ContactUid = this.MrsSmith.Uid;
			dto.Summary = "Test POST sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("1-Dec-05");
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO222";

			ServiceInvoiceItemDto item = new ServiceInvoiceItemDto();
			item.Description = "Design & Development of REST WS";
			item.AccountUid = this.IncomeHardwareSales.Uid;
			item.TaxCode = "G1";
			item.TotalAmountInclTax = 2132.51M;
			dto.Items.Add(item);

			item = new ServiceInvoiceItemDto();
			item.Description = "Testing";
			item.AccountUid = this.IncomeMisc.Uid;
			item.TaxCode = "G1,G3";
			item.TotalAmountInclTax = 11.22M;
			dto.Items.Add(item);

			return dto;
		}


		public InvoiceDto GetServiceSale2()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Service);

			dto.Date = DateTime.Parse("15-Sep-05");
			dto.ContactUid = this.MrsSmith.Uid;
			dto.Summary = "Service Sale 2";
			dto.DueOrExpiryDate = DateTime.Parse("15-Dec-05");
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO123456789";

			ServiceInvoiceItemDto item = new ServiceInvoiceItemDto();
			item.Description = "LINE 1 LINE 1 LINE 1";
			item.AccountUid = this.IncomeService.Uid;
			item.TaxCode = TaxCode.SaleExports;
			item.TotalAmountInclTax = 12345.12M;
			dto.Items.Add(item);

			item = new ServiceInvoiceItemDto();
			item.Description = "Testing";
			item.AccountUid = this.IncomeHardwareSales.Uid;
			item.TaxCode = TaxCode.SaleAdjustments;
			item.TotalAmountInclTax = -123.9M;
			dto.Items.Add(item);

			item = new ServiceInvoiceItemDto();
			item.Description = "Testing";
			item.AccountUid = this.IncomeShipping.Uid;
			item.TaxCode = TaxCode.SaleInclGst;
			item.TotalAmountInclTax = 569.66M;
			dto.Items.Add(item);

			return dto;
		}



		public InvoiceDto GetItemSale()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Item);

			dto.Date = DateTime.Parse("6-Oct-05");
			dto.ContactUid = this.MrSmith.Uid;
			dto.Summary = "Test Insert Item Sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("6-Nov-05");
			dto.Layout = InvoiceLayout.Item;
			dto.Status = InvoiceStatus.Invoice;
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO333";

			ItemInvoiceItemDto item = null;

			item = new ItemInvoiceItemDto();
			item.Quantity = 2;
			item.InventoryItemUid = this.AsusLaptop.Uid;
			item.Description = "Asus Laptop";
			item.TaxCode = TaxCode.SaleInclGst;
			item.UnitPriceInclTax = 1200.75M;
			item.PercentageDiscount = 12.50M;

			dto.Items.Add(item);

			item = new ItemInvoiceItemDto();
			item.Quantity = 5.125M;
			item.InventoryItemUid = this.Cat5Cable.Uid;
			item.Description = "Cat 5 Cable (in meter)";
			item.TaxCode = TaxCode.SaleGstFree;
			item.UnitPriceInclTax = 2.1234M;
			dto.Items.Add(item);

			item = new ItemInvoiceItemDto();
			item.Quantity = 3;
			item.InventoryItemUid = this.Cat5Cable.Uid;
			item.Description = "Cat 5 Cable (in meter)";
			item.TaxCode = TaxCode.SaleExports;
			item.UnitPriceInclTax = 5.125M;
			dto.Items.Add(item);

			QuickPaymentDto payment = new QuickPaymentDto();
			payment.DatePaid = dto.Date;
			payment.BankedToAccountUid = this.StGeorge.Uid;
			payment.Reference = "C-001-023";
			payment.Amount = 222.22M;

			dto.QuickPayment = payment;

			return dto;
		}


		public InvoiceDto GetUnpaidItemSale()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Item);

			dto.Date = DateTime.Parse("6-Oct-05");
			dto.ContactUid = this.MrsSmith.Uid;
			dto.Summary = "Test Insert Item Sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("6-Nov-05");
			dto.Status = InvoiceStatus.Invoice;
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO333";

			ItemInvoiceItemDto item = null;

			item = new ItemInvoiceItemDto();
			item.Quantity = 2;
			item.InventoryItemUid = this.HardDisk.Uid;
			item.Description = this.HardDisk.Description;
			item.TaxCode = TaxCode.SaleInclGst;
			item.UnitPriceInclTax = 120.75M;
			dto.Items.Add(item);

			item = new ItemInvoiceItemDto();
			item.Quantity = 5.125M;
			item.InventoryItemUid = this.Cat5Cable.Uid;
			item.Description = this.Cat5Cable.Description;
			item.TaxCode = TaxCode.SaleInclGst;
			item.UnitPriceInclTax = 2.555M;
			dto.Items.Add(item);

			return dto;
		}
	}
}
