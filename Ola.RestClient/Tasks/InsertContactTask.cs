namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	public class InsertContactTask : IInsertTask
	{
		#region IInsertTask Members
		
		private ContactDto _EntityToInsert = new ContactDto();
		[XmlElement(ElementName = "contact", Type = typeof(ContactDto))]
		public EntityDto EntityToInsert
		{
			get
			{
				return this._EntityToInsert;
			}
			set
			{
				this._EntityToInsert = (ContactDto) value;
			}
		}

		#endregion
	}
}
