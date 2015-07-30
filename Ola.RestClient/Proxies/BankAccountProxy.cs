namespace Ola.RestClient.Proxies
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Tasks;
	using Ola.RestClient.Utils;

	using System;
	

	public class BankAccountProxy : CrudProxy
	{
		public override string ResourceName
		{
			get
			{
				return Ola.RestClient.ResourceName.BankAccount;
			}
		}

		public override string ResourceListName
		{
			get { return RestClient.ResourceName.BankAccountList; }
		}


		protected override Type ResponseDtoType
		{
			get
			{
				return typeof(BankAccountResponseDto);
			}
		}


		protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
		{
			return ((BankAccountResponseDto) responseDto).BankAccount;
		}


		protected override IInsertTask GetNewInsertTaskInstance()
		{
			return new InsertBankAccountTask();
		}

		
		protected override IUpdateTask GetNewUpdateTaskInstance()
		{
			return new UpdateBankAccountTask();
		}
	}
}
