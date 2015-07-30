namespace Ola.RestClient.Tasks
{
	using System;
	using System.Xml.Serialization;


	public class InsertResult : TaskResult
	{
		[XmlAttribute("insertedEntityUid")]
		public int InsertedEntityUid;
		
		[XmlAttribute("lastUpdatedUid")]
		public string LastUpdatedUid;

		[XmlAttribute("utcLastModified")] public DateTime UtcLastModified; 		
	}
}
