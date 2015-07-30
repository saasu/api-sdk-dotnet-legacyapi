namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Collections;
	using System.Xml.Serialization;


	public class TaskResult
	{
		[XmlArray(ElementName = "errors")]
		[XmlArrayItem(ElementName = "error", Type = typeof(ErrorInfo))]
		public ArrayList Errors = new ArrayList();
	}
}
