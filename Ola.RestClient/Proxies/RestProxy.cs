namespace Ola.RestClient.Proxies
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Utils;

	using System;
	using System.Configuration;
	using System.Collections;
	using System.Collections.Specialized;
	using System.IO;
	using System.Net;
	using System.Text;
	using System.Xml;


	public abstract class RestProxy
	{
		public string	BaseUri			= "";
		public string	WSAccessKey		= "";
		public int		FileUid			= 0;


		public abstract string ResourceName { get; }


		public abstract string ResourceListName { get; }


		public RestProxy()
		{
			NameValueCollection proxySettings = (NameValueCollection) 
				ConfigurationManager.GetSection("ola.restclient/proxySettings");
			
			if (proxySettings != null)
			{
				this.BaseUri		= Util.DefaultValueToEmpty(proxySettings["baseUri"]);
				this.WSAccessKey	= Util.DefaultValueToEmpty(proxySettings["wsAccessKey"]);
				this.FileUid		= Util.ToInt32(proxySettings["fileUid"]);
			}
		}


		public static string MakeUrl(string resourceName)
		{
			NameValueCollection proxySettings = (NameValueCollection) 
				ConfigurationManager.GetSection("ola.restclient/proxySettings");
			
			if (proxySettings == null)
			{
				throw new ApplicationException("Unable to construct url for the given resource - proxySettings are not specified in config.");
			}
			
			return MakeUrl
				(
					Util.DefaultValueToEmpty(proxySettings["baseUri"]),
					resourceName,
					Util.DefaultValueToEmpty(proxySettings["wsAccessKey"]),
					Util.ToInt32(proxySettings["fileUid"])
				);
		}

		
		public static string MakeUrl(string baseUri, string resourceName, string wsAccessKey, int fileUid)
		{
			return baseUri + resourceName +
				"?wsaccesskey=" + wsAccessKey + "&fileuid=" + fileUid.ToString();
		}


		protected string MakeUrl()
		{
			return MakeUrl(this.BaseUri, this.ResourceName, this.WSAccessKey, this.FileUid);
		}

		
		protected virtual void CheckErrors(ArrayList errors)
		{
			if (errors.Count > 0)
			{
				throw new RestException(errors);
			}
		}	


		protected virtual void CheckErrors(XmlDocument document)
		{
			XmlElement root = document.DocumentElement;
			XmlNodeList nodeList = root.SelectNodes("errors/error");
			
			int errorCount = nodeList.Count;

			if (errorCount == 0)
			{
				return;
			}
			
			throw new RestException(this.ExtracErrors(nodeList));
		}


		private ArrayList ExtracErrors(XmlNodeList errorNodeList)
		{
			ArrayList errors = new ArrayList();
			foreach (XmlNode errorNode in errorNodeList)
			{
				errors.Add(this.ExtractError(errorNode));
			}
			return errors;
		}


		private ErrorInfo ExtractError(XmlNode errorNode)
		{
			ErrorInfo errorInfo = new ErrorInfo();
			errorInfo.Type = errorNode.SelectSingleNode("type").InnerText;
			errorInfo.Message = errorNode.SelectSingleNode("message").InnerText;
			return errorInfo;
		}
	}
}
