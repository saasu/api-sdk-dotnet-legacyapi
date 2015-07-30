namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "updateContact")]
	public class UpdateContactTask : IUpdateTask
	{
		#region IUpdateTask Members
		
		private ContactDto _EntityToUpdate = new ContactDto();
		[XmlElement(ElementName = "contact", Type = typeof(ContactDto))]
		public EntityDto EntityToUpdate
		{
			get
			{
				return this._EntityToUpdate;
			}
			set
			{
				this._EntityToUpdate = (ContactDto) value;
			}
		}

		#endregion
	}
}
