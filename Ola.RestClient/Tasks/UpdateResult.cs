namespace Ola.RestClient.Tasks
{
	using System;
	using System.Xml.Serialization;


	public class UpdateResult : TaskResult
	{
		[XmlAttribute("updatedEntityUid")]
		public int UpdatedEntityUid;
		
		[XmlAttribute("lastUpdatedUid")]
		public string LastUpdatedUid;

		[XmlAttribute("utcLastModified")] public DateTime UtcLastModified;
	}
}
