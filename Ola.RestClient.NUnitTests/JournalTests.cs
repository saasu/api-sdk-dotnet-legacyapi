using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using System.Xml;
using NUnit.Framework;
using Ola.RestClient.Exceptions;
using Ola.RestClient.Dto.Journals;
using Ola.RestClient.Exceptions;
using Ola.RestClient.Proxies;
using Ola.RestClient.Dto;


namespace Ola.RestClient.NUnitTests
{
	[TestFixture]
	public class JournalTests : RestClientTests
	{
		[Test]
		public void TestInsertAndGetByUid()
		{
			JournalProxy proxy = new JournalProxy();
			JournalDto insert = CreateJournalDto();

			proxy.Insert(insert);

			Assert.IsTrue(insert.Uid > 0, "Uid must be > 0 after save.");

			JournalDto read = (JournalDto)proxy.GetByUid(insert.Uid);

			ReflectionTester.AssertAreEqual("Journal", insert, read, new[] { "Tags" });
		}


		[Test]
		public void Update()
		{
			JournalProxy proxy = new JournalProxy();
			JournalDto dto = CreateJournalDto();
			proxy.Insert(dto);

			Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after save.");

			dto.Summary = "Add some summary.";
			dto.Notes = "Updated journal.";
			proxy.Update(dto);

			JournalDto read = (JournalDto)proxy.GetByUid(dto.Uid);

			ReflectionTester.AssertAreEqual("Journal", dto, read,new[] {"LastModified", "Tags"});
		}


		[Test]
		public void Delete()
		{
			JournalProxy proxy = new JournalProxy();
			JournalDto dto = CreateJournalDto();
			proxy.Insert(dto);

			proxy.DeleteByUid(dto.Uid);

			try
			{
				JournalDto fromDB = (JournalDto) proxy.GetByUid(dto.Uid);
				Assert.Fail("The Journal Entry was not deleted successfully.");
			}
			catch (RestException ex)
			{
				Assert.IsTrue(ex.Type == "RecordNotFoundException");
			}
		}


		[Test]
		public void TestInsertAndGetByUidUnbalanced()
		{
			JournalProxy proxy = new JournalProxy();
			JournalDto insert = CreateJournalDto();
			insert.Items[0].Amount = 1999;
			try
			{
				proxy.Insert(insert);
				Assert.Fail("No error was thrown.");				
			}
			catch(RestException ex)
			{
				Assert.AreEqual("The debit and credit total for the transaction items in this transaction do not match. Out of balance: -1,875.55.", ex.Message, "Incorrect error message.");
			}
		}


		[Test]
		public void TestInsertAndUpdateByUid()
		{
			JournalProxy proxy = new JournalProxy();
			JournalDto insert = CreateJournalDto();

			proxy.Insert(insert);

			Assert.IsTrue(insert.Uid > 0, "Uid must be > 0 after save.");

			JournalDto read = (JournalDto)proxy.GetByUid(insert.Uid);

			read.Notes = "Updated";

			// change Total and Flip Them Around
			read.Items[0].Amount = 345.78M;
			read.Items[0].Type = DebitCreditType.Debit;

            read.Items[1].Amount = 345.78M;
			read.Items[1].Type = DebitCreditType.Credit;

			proxy.Update(read);

			JournalDto updated = (JournalDto)proxy.GetByUid(insert.Uid);

			ReflectionTester.AssertAreEqual("Journal", read, updated);
		}


		[Test]
		public void TestInsertMultiCcy1()
		{
			JournalProxy proxy = new JournalProxy();
			JournalDto insert = CreateJournalDto2();
			proxy.Insert(insert);
			Assert.IsTrue(insert.Uid > 0, "Uid must be > 0 after save.");

			JournalDto read = (JournalDto)proxy.GetByUid(insert.Uid);

			Assert.IsTrue(read.FCToBCFXRate > 0, "FX Rate should have been auto-populated.");
			
			// For comparison, set the original rate to the one set by Saasu.
			insert.FCToBCFXRate = read.FCToBCFXRate;

			ReflectionTester.AssertAreEqual("Journal", insert, read, new[] { "LastModified", "Tags" });
		}


        [Test]
        public void FindJournalsForAPeriod()
        {
			JournalProxy proxy = new JournalProxy();
			JournalDto dto1 = CreateJournalDto();
        	dto1.Date = DateTime.Parse("01-Jan-2010");
			proxy.Insert(dto1);

        	JournalDto dto2 = CreateJournalDto();
        	dto2.Date = DateTime.Parse("30-Jan-2010");
			proxy.Insert(dto2);

			NameValueCollection queries = new NameValueCollection();
			queries.Add("JournalDateFrom", "2010/01/01");
			queries.Add("JournalDateTo", "2010/01/30");
            
			XmlDocument list = proxy.Find(queries);
        }

		[Test]
		public void FindJournalsByLastModifiedDate()
		{
			JournalProxy proxy = new JournalProxy();
			JournalDto first = CreateJournalDto();
			first.Date = DateTime.Parse("01-Jan-2010");
			Thread.Sleep(5 * 1000);
			proxy.Insert(first);

			DateTime firstInsert = first.UtcLastModified;

			Thread.Sleep(10 * 1000);

			JournalDto second = CreateJournalDto();
			second.Date = DateTime.Parse("01-Jan-2010");
			Thread.Sleep(5 * 1000);
			proxy.Insert(second);

			DateTime secondInsert = second.UtcLastModified.AddSeconds(-1);

			List<JournalDto> secondOnly = proxy.FindList<JournalDto>(JournalProxy.ResponseXPath, "UtcLastModifiedFrom", secondInsert.ToString("s"), "UtcLastModifiedTo", DateTime.UtcNow.AddMinutes(5).ToString("s"));
			Assert.AreEqual(1, secondOnly.Count);
			// ReflectionTester.AssertAreEqual(second, secondOnly[0], "UtcLastModified");

			List<JournalDto> firstAndSecond = proxy.FindList<JournalDto>(JournalProxy.ResponseXPath, "UtcLastModifiedFrom", firstInsert.ToString("s"), "UtcLastModifiedTo", DateTime.UtcNow.AddMinutes(5).ToString("s"));
			Assert.AreEqual(2, firstAndSecond.Count);
			//ReflectionTester.AssertAreEqual(first, firstAndSecond[0], "UtcLastModified");
			//ReflectionTester.AssertAreEqual(second, firstAndSecond[1], "UtcLastModified");
		}

		[Test]
		public void FindJournalsByTags()
		{
			JournalProxy proxy = new JournalProxy();
			JournalDto dto = CreateJournalDto();

			string tag1 = "Journal" + System.Guid.NewGuid().ToString().Substring(0, 12);
			string tag2 = "Journal" + System.Guid.NewGuid().ToString().Substring(0, 12);

			dto.Tags = string.Format("{0},{1}", tag1, tag2);
			proxy.Insert(dto);
			Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after save.");

			dto = CreateJournalDto();
			dto.Tags = string.Format("{0},{1}", tag1, tag2);
			proxy.Insert(dto);
			Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after save.");
		}


		[Test]
		[ExpectedException(ExpectedException = typeof(RestException), MatchType = MessageMatch.Contains, ExpectedMessage = "You have selected a Tax Code for use with a Bank Account")]
		public void InvalidAccountAndTaxCodeCombination()
		{
			JournalProxy proxy = new JournalProxy();
			JournalDto dto = CreateJournalDto();
			dto.Items[0].AccountUid = StGeorge.Uid;
			dto.Items[0].TaxCode = TaxCode.SaleInclGst; 

			proxy.Insert(dto);
		}


		private JournalDto CreateJournalDto()
		{
			JournalDto dto = new JournalDto();
			dto.Date = DateTime.UtcNow.Date;
			dto.Notes = "Notes";
			dto.RequiresFollowUp = false;
	        dto.Reference = "#12345";
			dto.Summary = "Summary";
			dto.Tags = "ABC, DEF";
			dto.FCToBCFXRate = 1M;

			JournalItemDto row1 = new JournalItemDto();
			row1.AccountUid				= this.ExpenseOffice.Uid;
			row1.TaxCode				= TaxCode.ExpInclGst;
			row1.Type					= DebitCreditType.Credit;
            row1.Amount                 = 123.45M;

			JournalItemDto row2 = new JournalItemDto();
			row2.AccountUid				= this.ExpenseTelco.Uid;
			row2.TaxCode				= TaxCode.ExpInclGst;
			row2.Type					= DebitCreditType.Debit;
            row2.Amount                 = 123.45M;

			dto.Items = new JournalItemDto[]
			            	{
								row1, row2
			            	};

			return dto;
		}


	    private JournalDto CreateJournalDto2()
		{
			JournalDto dto = new JournalDto();
			dto.Date = DateTime.UtcNow.Date;
			dto.Summary = "Summary";
			dto.Tags = "Journal, MultiCcy";
	    	dto.AutoPopulateFXRate = true;
	    	dto.Ccy = "USD";
			
			JournalItemDto row1 = new JournalItemDto();
			row1.AccountUid	= this.IncomeSubscription.Uid;
			row1.TaxCode = TaxCode.SaleInclGst;
			row1.Type = DebitCreditType.Credit;
            row1.Amount = 100M;

			JournalItemDto row2 = new JournalItemDto();
			row2.AccountUid = this.IncomeShipping.Uid;
			row2.TaxCode = TaxCode.SaleInclGst;
			row2.Type = DebitCreditType.Credit;
			row2.Amount = 50M;

			JournalItemDto row3 = new JournalItemDto();
			row3.AccountUid = this.Westpac.Uid;
			row3.Type = DebitCreditType.Debit;
			row3.Amount = 150M;

			dto.Items = new JournalItemDto[]{ row1, row2, row3 };

			return dto;
		}
	}
}
