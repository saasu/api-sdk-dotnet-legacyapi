using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Ola.RestClient.Dto
{
	[XmlRoot(ElementName="activity")]
	public class ActivityDto : EntityDto
	{
		public ActivityDto()
		{
		}

		[XmlElement(ElementName = "type")]
		public string Type;


		[XmlElement(ElementName = "done")]
		public bool Done;


		[XmlElement(ElementName = "title")]
		public string Title;


		[XmlElement(ElementName = "details")]
		public string Details;


		[XmlElement(ElementName = "due", DataType = "date")]
		public DateTime Due;

		/// <summary>
		/// The lastModified timestamp that was received part of a previous get/insert or update
		/// </summary>
		[XmlElement(ElementName = "modified", DataType = "dateTime")]
		public DateTime LastModified;

		/// <summary>
		/// Email address of the owner
		/// </summary>
		[XmlElement(ElementName = "owner")]
		public string Owner;

		/// <summary>
		/// Contact, Sale, Purchase, Employee
		/// </summary>
		[XmlElement(ElementName = "attachedToType")]
		public string AttachedToType;

		[XmlElement(ElementName = "attachedToUid", IsNullable = true)]
		[DefaultValue(null)]
		public int? AttachedToUid;
	}
}
