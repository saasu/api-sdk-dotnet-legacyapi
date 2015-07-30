using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Ola.RestClient.Dto;
using Ola.RestClient.Exceptions;
using Ola.RestClient.Proxies;

namespace Ola.RestClient.NUnitTests
{
	[TestFixture]
	public class ActivityTests : RestClientTests
	{
		private InvoiceDto _sale1;
		private InvoiceDto _purchase1;

		public override void TestFixtureSetUp()
		{
			base.TestFixtureSetUp();
			SetupTransactions();
		}

		private void SetupTransactions()
		{
			CrudProxy proxy = new InvoiceProxy();
			_sale1 = GetServiceSale();
			proxy.Insert(_sale1);
			Assert.IsTrue(_sale1.Uid > 0, "Sale tran uid must be > 0 after insert.");

			_purchase1 = GetServicePurchase();
			proxy.Insert(_purchase1);
			Assert.IsTrue(_purchase1.Uid > 0, "Purchase tran uid must be > 0 after insert.");
		}

		public InvoiceDto GetServiceSale()
		{
			var dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Service);

			dto.Date = DateTime.Parse("30-Sep-05");
			dto.ContactUid = MrSmith.Uid;
			dto.Summary = "Test POST sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("1-Dec-05");
			dto.Status = "I";
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO222";
			dto.IsSent = false;

			var item = new ServiceInvoiceItemDto();
			item.Description = "Design & Development of REST WS";
			item.AccountUid = IncomeService.Uid;
			item.TaxCode = TaxCode.SaleInclGst;
			item.TotalAmountInclTax = 2132.51M;
			dto.Items.Add(item);

			return dto;
		}

		private InvoiceDto GetServicePurchase()
		{
			var dto = new InvoiceDto(TransactionType.Purchase, InvoiceLayout.Service);

			dto.Date = DateTime.Today.Date;
			dto.ContactUid = MrSmith.Uid;
			dto.Summary = "Test POST Purchase";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = dto.Date.AddMonths(1);
			dto.Status = InvoiceStatus.Order;
			dto.PurchaseOrderNumber = "<Auto Number>";

			var item = new ServiceInvoiceItemDto();
			item.Description = "Purchase - Line Item 1";
			item.AccountUid = ExpenseOffice.Uid;
			item.TaxCode = TaxCode.ExpInclGst;
			item.TotalAmountInclTax = 123.45M;
			dto.Items.Add(item);

			item = new ServiceInvoiceItemDto();
			item.Description = "Purchase - Line Item 2";
			item.AccountUid = ExpenseMisc.Uid;
			// item.TaxCode				= TaxCode.ExpInclGstPrivateNonDeductable;
			item.TotalAmountInclTax = 678.90M;
			dto.Items.Add(item);

			return dto;
		}

		private void AssertInList(List<ActivityDto> subList, List<ActivityDto> all)
		{
			foreach (ActivityDto activity in subList)
			{
				bool found = false;
				foreach (ActivityDto toTest in all)
				{
					if (toTest.Uid == activity.Uid)
					{
						ReflectionTester.AssertAreEqual(toTest, activity, "LastModified");
						all.Remove(toTest);
						found = true;
						break;
					}
				}
				if (!found)
					Assert.Fail("Activity {0} for {1} was not found in the complete list of activities", activity.Uid, activity.Owner);
			}
		}

		public ActivityDto GetActivityDto()
		{
			var dto = new ActivityDto();
			dto.Type = "Meeting";
			dto.Due = DateTime.Parse("01-Jul-2020");
			dto.Title = "Meeting with John Doe.";
			dto.Details =
				@"To Discuss: 
								- Strategy for implementing new CRM, the stages.
								- Integrating the accounting software with shopping cart.
								- Upgrading the assembly line for increased productivity.";
			dto.Done = false;
			return dto;
		}

		[Test]
		public void Find()
		{
			var proxy = new ActivityProxy();
			ActivityDto first = GetActivityDto();
			first.Owner = ApiUserEmailAddress;
			proxy.Insert(first);
			Assert.AreNotEqual(0, first.Uid);

			var firstGet = (ActivityDto) proxy.GetByUid(first.Uid);
			ReflectionTester.AssertAreEqual(first, firstGet, "LastModified");

			ActivityDto second = GetActivityDto();
			second.Owner = ApiUserEmailAddress;
			second.AttachedToType = "Contact";
			second.AttachedToUid = MrSmith.Uid;
			proxy.Insert(second);

			var secondGet = (ActivityDto) proxy.GetByUid(second.Uid);
			ReflectionTester.AssertAreEqual(second, secondGet, "LastModified");

			List<ActivityDto> activities = proxy.FindList<ActivityDto>(ActivityProxy.ResponseXPath, "Type", "Meeting");
			Assert.AreNotEqual(0, activities.Count);

			List<ActivityDto> kazActivities = proxy.FindList<ActivityDto>(ActivityProxy.ResponseXPath, "Type", "Meeting", "Owner",
			                                                              ApiUserEmailAddress);
			Assert.AreNotEqual(0, kazActivities.Count);

			// all api/kaz should be part of activities
			AssertInList(kazActivities, activities);

			// delete
			proxy.DeleteByUid(first.Uid);
			try
			{
				firstGet = (ActivityDto) proxy.GetByUid(first.Uid);
			}
			catch (RestException exception)
			{
				Assert.AreEqual("RecordNotFoundException", exception.Type);
			}

			proxy.DeleteByUid(second.Uid);
			try
			{
				secondGet = (ActivityDto) proxy.GetByUid(second.Uid);
			}
			catch (RestException exception)
			{
				Assert.AreEqual("RecordNotFoundException", exception.Type);
			}
		}

		[Test]
		public void FindAttachedToContact()
		{
			var proxy = new ActivityProxy();
			ActivityDto entity = GetActivityDto();
			entity.Owner = ApiUserEmailAddress;
			entity.AttachedToType = "Contact";
			entity.AttachedToUid = MrSmith.Uid;
			proxy.Insert(entity);

			List<ActivityDto> apiActivities = proxy.FindList<ActivityDto>(ActivityProxy.ResponseXPath, "Type", "Meeting", "Owner",
			                                                              ApiUserEmailAddress, "AttachedToType", "Contact",
			                                                              "AttachedToUid", MrSmith.Uid.ToString());
			Assert.AreNotEqual(0, apiActivities.Count);

			bool found = false;
			foreach (ActivityDto activity in apiActivities)
			{
				Assert.AreEqual(entity.AttachedToUid, activity.AttachedToUid);
				Assert.AreEqual(entity.AttachedToType, activity.AttachedToType);

				if (activity.Uid == entity.Uid)
				{
					found = true;
				}
			}
			if (!found)
				Assert.Fail("Could not find entity {0} attached to contact.", entity.Uid);

			proxy.DeleteByUid(entity.Uid);
		}

		[Test]
		public void InsertAttachedTo()
		{
			var proxy = new ActivityProxy();
			ActivityDto dto = GetActivityDto();
			dto.Owner = ApiUserEmailAddress;
			dto.AttachedToType = "Sale";
			dto.AttachedToUid = _sale1.Uid;

			proxy.Insert(dto);
			var result = (ActivityDto) proxy.GetByUid(dto.Uid);
			ReflectionTester.AssertAreEqual(dto, result, "LastModified", "UtcLastModified");

			dto = GetActivityDto();
			dto.Owner = ApiUserEmailAddress;
			dto.AttachedToType = "Purchase";
			dto.AttachedToUid = _purchase1.Uid;

			proxy.Insert(dto);
			result = (ActivityDto) proxy.GetByUid(dto.Uid);
			ReflectionTester.AssertAreEqual(dto, result, "LastModified", "UtcLastModified");

			dto = GetActivityDto();
			dto.Owner = ApiUserEmailAddress;
			dto.AttachedToType = "Contact";
			dto.AttachedToUid = MrSmith.Uid;

			proxy.Insert(dto);
			result = (ActivityDto) proxy.GetByUid(dto.Uid);
			ReflectionTester.AssertAreEqual(dto, result, "LastModified", "UtcLastModified");
		}

		[Test]
		public void TestInsertAndGetByUid()
		{
			var proxy = new ActivityProxy();
			ActivityDto insert = GetActivityDto();
			proxy.Insert(insert);

			Assert.IsTrue(insert.Uid > 0, "Uid must be > 0 after save.");

			var read = (ActivityDto) proxy.GetByUid(insert.Uid);

			ReflectionTester.AssertAreEqual("Activity", insert, read, new[] {"LastModified", "UtcLastModified", "Tags"});

		}

		[Test]
		public void TestIsDoneFlagUpdate()
		{
			var proxy = new ActivityProxy();
			ActivityDto insert = GetActivityDto();
			proxy.Insert(insert);

			var read = (ActivityDto) proxy.GetByUid(insert.Uid);
			read.Done = true;
			proxy.Update(read);

			var doneActivity = (ActivityDto) proxy.GetByUid(insert.Uid);
			Assert.IsTrue(doneActivity.Done);
		}

		[Test]
		public void TryGetByLastModified()
		{
			var proxy = new ActivityProxy();

			ActivityDto first = GetActivityDto();
			first.Owner = ApiUserEmailAddress;
			Thread.Sleep(2*1000);
			proxy.Insert(first);

			var retrived = (ActivityDto) proxy.GetByUid(first.Uid);
            DateTime firstInsert = retrived.LastModified.AddSeconds(-1); // go back once


			Thread.Sleep(5*1000);

			ActivityDto second = GetActivityDto();
			second.Owner = ApiUserEmailAddress;
			Thread.Sleep(2*1000);
			proxy.Insert(second);

			retrived = (ActivityDto) proxy.GetByUid(second.Uid);
			DateTime secondInsert = retrived.LastModified.AddSeconds(-1); // go back once


		    var dateFrom = secondInsert;
		    var dateTo = DateTime.UtcNow.AddMinutes(5);
			List<ActivityDto> secondOnly = proxy.FindList<ActivityDto>(ActivityProxy.ResponseXPath, "datetype", "modified",
			                                                           "Owner", ApiUserEmailAddress, "DateFrom",
			                                                           dateFrom.ToString("s"), "DateTo",
			                                                           dateTo.ToString("s"));
			Assert.IsTrue(secondOnly.Count >= 1);
		    foreach (var activityDto in secondOnly)
		    {
		        Assert.IsTrue(activityDto.LastModified >= dateFrom && activityDto.LastModified <= dateTo, "Activity where last mod outside date range included. ActivityId: {0}.", activityDto.Uid);
		    }

			ReflectionTester.AssertAreEqual(second, secondOnly[0], new[] {"LastModified", "UtcLastModified"});

			List<ActivityDto> firstAndSecond = proxy.FindList<ActivityDto>(ActivityProxy.ResponseXPath, "datetype", "modified",
			                                                               "Owner", ApiUserEmailAddress, "DateFrom",
			                                                               firstInsert.ToString("s"), "DateTo",
			                                                               DateTime.UtcNow.AddMinutes(5).ToString("s"));
			Assert.AreEqual(2, firstAndSecond.Count);
			ReflectionTester.AssertAreEqual(first, firstAndSecond[1], new[] {"LastModified", "UtcLastModified"});
			ReflectionTester.AssertAreEqual(second, firstAndSecond[0], new[] {"LastModified", "UtcLastModified"});
		}

		[Test]
		[ExpectedException(typeof (RestException))]
		public void TryInsertAttachedToDifferentFileUid()
		{
			var proxy = new ActivityProxy();
			ActivityDto dto = GetActivityDto();
			dto.Owner = ApiUserEmailAddress;
			dto.AttachedToType = "Contact";
			dto.AttachedToUid = 186600; // this contact is not part of this file!!!
			proxy.Insert(dto);
		}

		[Test]
		public void TryInsertAttachedToTransactionFromDifferentFileUid()
		{
			var proxy = new ActivityProxy();
			ActivityDto dto = GetActivityDto();
			dto.Owner = ApiUserEmailAddress;
			dto.AttachedToType = "Sale";
			dto.AttachedToUid = 72599; // this sale is not part of this file!!!

			try
			{
				proxy.Insert(dto);
			}
			catch (RestException ex)
			{
				StringAssert.Contains("Invalid attachedToUid", ex.Message);
			}
		}


		[Test]
		public void Update()
		{
			var proxy = new ActivityProxy();
			ActivityDto dto = GetActivityDto();
			proxy.Insert(dto);

			Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after save.");

			dto.Title = "A nee Title.";
			dto.Type = "Note";

			proxy.Update(dto);

			var read = (ActivityDto) proxy.GetByUid(dto.Uid);

			ReflectionTester.AssertAreEqual("Activity", dto, read, "LastModified", "UtcLastModified");
		}
	}
}