using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using NUnit.Framework;
using Ola.RestClient.Dto;
using Ola.RestClient.Proxies;

namespace Ola.RestClient.NUnitTests
{
	[TestFixture]
	public class TagListTests
	{
		[Test]
		public void GetTagList()
		{
			NameValueCollection queries = new NameValueCollection();
			queries.Add("isactive", "true");
			TagProxy proxy = new TagProxy();
			XmlDocument list = proxy.Find(queries);
		}
	}
}
