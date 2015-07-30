namespace Ola.RestClient.NUnitTests
{
	using Ola.RestClient.Utils;

	using System;
	using System.Net;
	using System.Configuration;
	using System.Collections;
	using System.Collections.Specialized;


	public class BaseTests
	{
		protected string TestUid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15) + " - ";
		protected string BaseUri;
		protected string WSAccessKey;
		protected int FileUid;


		public BaseTests()
		{
			NameValueCollection proxySettings = (NameValueCollection) 
				ConfigurationSettings.GetConfig("ola.restclient/proxySettings");
			
			if (proxySettings != null)
			{
				this.BaseUri		= Util.DefaultValueToEmpty(proxySettings["baseUri"]);
				this.WSAccessKey	= Util.DefaultValueToEmpty(proxySettings["wsAccessKey"]);
				this.FileUid		= Util.ToInt32(proxySettings["fileUid"]);
			}

			System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
		}
	}
}
