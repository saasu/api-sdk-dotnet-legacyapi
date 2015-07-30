namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "updateContactCategory")]
	public class UpdateContactCategoryTask : IUpdateTask
	{
		#region IUpdateTask Members
		
		private ContactCategoryDto _EntityToUpdate = new ContactCategoryDto();
		[XmlElement(ElementName = "contactCategory", Type = typeof(ContactCategoryDto))]
		public EntityDto EntityToUpdate
		{
			get
			{
				return this._EntityToUpdate;
			}
			set
			{
				this._EntityToUpdate = (ContactCategoryDto) value;
			}
		}

		#endregion
	}
}
