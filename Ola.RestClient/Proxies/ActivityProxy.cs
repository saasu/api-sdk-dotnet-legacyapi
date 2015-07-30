using Ola.RestClient.Dto;
using Ola.RestClient.Dto.Activity;
using Ola.RestClient.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ola.RestClient.Proxies
{
	public class ActivityProxy : CrudProxy
	{
		public const string ResponseXPath = "/activityListResponse/activityList/activity";

		public override string ResourceName
		{
			get { return Ola.RestClient.ResourceName.Activity; }
		}

		public override string ResourceListName
		{
			get { return RestClient.ResourceName.ActivityList; }
		}

		protected override Type ResponseDtoType
		{
			get
			{
				return typeof (ActivityResponseDto);
			}
		}
        
		protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
		{
			return ((ActivityResponseDto)responseDto).Activity;
		}

		protected override IInsertTask GetNewInsertTaskInstance()
		{
			return new InsertActivityTask();
		}

		protected override IUpdateTask GetNewUpdateTaskInstance()
		{
			return new UpdateActivityTask();
		}
	}
}
