namespace Ola.RestClient.Proxies
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Tasks;
	using Ola.RestClient.Utils;

	using System;
	

	public class ContactProxy : CrudProxy
	{
		public override string ResourceName
		{
			get
			{
				return Ola.RestClient.ResourceName.Contact;
			}
		}

		public override string ResourceListName
		{
			get { return RestClient.ResourceName.ContactList; }
		}


		protected override Type ResponseDtoType
		{
			get
			{
				return typeof(ContactResponseDto);
			}
		}


		protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
		{
			return ((ContactResponseDto) responseDto).Contact;
		}


		protected override IInsertTask GetNewInsertTaskInstance()
		{
			return new InsertContactTask();
		}


		protected override IUpdateTask GetNewUpdateTaskInstance()
		{
			return new UpdateContactTask();
		}
	}
}
