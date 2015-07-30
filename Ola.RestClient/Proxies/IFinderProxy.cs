namespace Ola.RestClient.Proxies
{
	using System;
	using System.Collections.Specialized;
	using System.Xml;


	public interface IFinderProxy
	{
		XmlDocument Find();
		

		XmlDocument Find(NameValueCollection queries);
	}
}
