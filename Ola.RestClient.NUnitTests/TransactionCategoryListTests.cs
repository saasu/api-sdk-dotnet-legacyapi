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
	public class TransactionCategoryListTests : RestClientTests
	{
		[Test]
		public void TestWithoutExtraQueryStringParameters()
		{
			TransactionCategoryProxy proxy = new TransactionCategoryProxy();
			XmlDocument list = proxy.Find();
		}


		[Test]
		public void TestWithExtraQueryStringParameters()
		{
			NameValueCollection queries = new NameValueCollection();
			queries.Add("type", AccountType.Asset);
			queries.Add("isActive", "true");
			queries.Add("fields", "TransactionCategoryUid,Type,Name");
			queries.Add("sortoptions", "Type ASC,Name ASC");
			
			TransactionCategoryProxy proxy = new TransactionCategoryProxy();
			XmlDocument list = proxy.Find(queries);
		}


		[Test]
		public void TestRetrieveInbuiltTransactionCategoryOnly()
		{
			NameValueCollection queries = new NameValueCollection();
			queries.Add("type", AccountType.Asset);
			queries.Add("isActive", "true");
			queries.Add("isInbuilt", "true");
			queries.Add("fields", "TransactionCategoryUid,Type,Name");
			queries.Add("sortoptions", "Type ASC,Name ASC");
			
			TransactionCategoryProxy proxy = new TransactionCategoryProxy();
			XmlDocument list = proxy.Find(queries);
		}
	}
}
