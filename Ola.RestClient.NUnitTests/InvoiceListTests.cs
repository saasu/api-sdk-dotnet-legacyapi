namespace Ola.RestClient.NUnitTests
{
	using NUnit.Framework;

	using Ola.RestClient;
	using Ola.RestClient.Dto;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Proxies;
	
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Xml;


	[TestFixture]
	public class InvoiceListTests : RestClientTests
	{
		[Test]
		public void TestWithDeprecatedExtraQueryStringParameters1()
		{
		    NameValueCollection queries = new NameValueCollection();
		    queries.Add("transactionType", TransactionType.Sale);
		    queries.Add("InvoiceDateFrom", "01-01-08");
			queries.Add("InvoiceDateTo", "01-01-09");
		    queries.Add("fields", "InvoiceUid,InvoiceDate,FirstCreated,ModifiedDate,DueDate,InvoiceNumber,TotalAmountInclTax,AmountOwed");
		    queries.Add("sortoptions", "InvoiceDate ASC");
			
		    InvoiceProxy proxy = new InvoiceProxy();
		    XmlDocument list = proxy.Find(queries);
		}


		[Test]
		public void TestWithDeprecatedExtraQueryStringParameters2()
		{
			NameValueCollection queries = new NameValueCollection();
			queries.Add("transactionType", TransactionType.Sale);
			queries.Add("InvoiceDueDateFrom", "01-11-05");
			queries.Add("InvoiceDueDateTo", "10-11-05");
			queries.Add("fields", "InvoiceUid,InvoiceDate,FirstCreated,ModifiedDate,DueDate,InvoiceNumber,TotalAmountInclTax,AmountOwed");
			queries.Add("sortoptions", "InvoiceDate ASC");

			InvoiceProxy proxy = new InvoiceProxy();
			XmlDocument list = proxy.Find(queries);
		}


		[Test]
		public void TestWithDateTypeExtraQueryStringParameters1()
		{
			NameValueCollection queries = new NameValueCollection();
			queries.Add("transactionType", TransactionType.Sale);
			queries.Add("dateType", "transactiondate");
			queries.Add("DateFrom", "01-01-09");
			queries.Add("DateTo", "31-12-09");
			queries.Add("paidStatus", "unpaid");
			queries.Add("fields", "InvoiceUid,InvoiceDate,FirstCreated,ModifiedDate,DueDate,InvoiceNumber,TotalAmountInclTax,AmountOwed");
			queries.Add("sortoptions", "InvoiceDate ASC");

			InvoiceProxy proxy = new InvoiceProxy();
			XmlDocument list = proxy.Find(queries);
		}


		[Test]
		public void TestWithDateTypeExtraQueryStringParameters2()
		{
			NameValueCollection queries = new NameValueCollection();
			queries.Add("transactionType", TransactionType.Purchase);
			queries.Add("dateType", "DueDate");
			queries.Add("DateFrom", "01-01-09");
			queries.Add("DateTo", "31-12-09");
			queries.Add("paidStatus", "unpaid");
			queries.Add("fields", "InvoiceUid,InvoiceDate,FirstCreated,ModifiedDate,DueDate,InvoiceNumber,TotalAmountInclTax,AmountOwed");
			queries.Add("sortoptions", "InvoiceDate ASC");

			InvoiceProxy proxy = new InvoiceProxy();
			XmlDocument list = proxy.Find(queries);
		}


		[Test]
		public void TestWithDateTypeExtraQueryStringParameters3()
		{
			NameValueCollection queries = new NameValueCollection();
			queries.Add("transactionType", TransactionType.Sale);
			queries.Add("dateType", "FirstCreated");
			queries.Add("DateFrom", "01-01-09");
			queries.Add("DateTo", "31-12-09");
			queries.Add("paidStatus", "unpaid");
			queries.Add("fields", "InvoiceUid,InvoiceDate,FirstCreated,ModifiedDate,DueDate,InvoiceNumber,TotalAmountInclTax,AmountOwed");
			queries.Add("sortoptions", "InvoiceDate ASC");

			InvoiceProxy proxy = new InvoiceProxy();
			XmlDocument list = proxy.Find(queries);
		}


		[Test]
		public void TestWithDateTypeExtraQueryStringParameters4()
		{
			NameValueCollection queries = new NameValueCollection();
			queries.Add("transactionType", TransactionType.Purchase);
			queries.Add("dateType", "ModifiedDate");
			queries.Add("DateFrom", "01-01-09");
			queries.Add("DateTo", "31-12-09");
			queries.Add("paidStatus", "unpaid");
			queries.Add("fields", "InvoiceUid,InvoiceDate,FirstCreated,ModifiedDate,DueDate,InvoiceNumber,TotalAmountInclTax,AmountOwed");
			queries.Add("sortoptions", "InvoiceDate ASC");

			InvoiceProxy proxy = new InvoiceProxy();
			XmlDocument list = proxy.Find(queries);
		}
	}
}
