namespace Ola.RestClient.Proxies
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Tasks;
	using Ola.RestClient.Utils;
	
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Xml;

	
	public class InstallerProxy : RestProxy
	{
		public override string ResourceName
		{
			get
			{
				return "Installer";
			}
		}


		public override string ResourceListName
		{
			get { throw new NotImplementedException(); }
		}


		public virtual void  Run()
		{
			string result = HttpUtils.Post(this.MakeUrl());
			if (result.Length != 0)
			{
				XmlDocument document = new XmlDocument();
				document.LoadXml(result);
				this.CheckErrors(document);
			}
		}
	}
}
