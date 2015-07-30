using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ola.RestClient.Dto;
using Ola.RestClient.Tasks;

namespace Ola.RestClient.Proxies
{
	public class TagProxy : CrudProxy
	{
		public override string ResourceName
		{
			get { throw new NotImplementedException(); }
		}

		public override string ResourceListName
		{
			get { return RestClient.ResourceName.TagList; }
		}

		protected override Type ResponseDtoType
		{
			get { throw new NotImplementedException(); }
		}

		protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
		{
			throw new NotImplementedException();
		}

		protected override IInsertTask GetNewInsertTaskInstance()
		{
			throw new NotImplementedException();
		}

		protected override IUpdateTask GetNewUpdateTaskInstance()
		{
			throw new NotImplementedException();
		}
	}
}
