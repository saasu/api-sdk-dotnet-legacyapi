using System;
using System.Collections.Generic;
using System.Text;
using Ola.RestClient.Dto;
using Ola.RestClient.Dto.Journals;
using Ola.RestClient.Proxies;
using Ola.RestClient.Tasks;
using Ola.RestClient.Tasks.Journals;


namespace Ola.RestClient.Proxies
{
	public class JournalProxy : CrudProxy
	{
		public const string ResponseXPath = "/journalListResponse/journalList/journal";

		public override string ResourceName
		{
			get
			{
				return Ola.RestClient.ResourceName.Journal;
			}
		}


		public override string ResourceListName
		{
			get { return RestClient.ResourceName.JournalList; }
		}


		protected override Type ResponseDtoType
		{
			get
			{
				return typeof (JournalResponseDto);
			}
		}


		protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
		{
			return ((JournalResponseDto)responseDto).Journal;
		}


		protected override IInsertTask GetNewInsertTaskInstance()
		{
			return new InsertJournalTask();
		}


		protected override IUpdateTask GetNewUpdateTaskInstance()
		{
			return new UpdateJournalTask();
		}
	}
}
