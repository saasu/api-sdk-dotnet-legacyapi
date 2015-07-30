using System.Collections.Generic;


namespace Ola.RestClient.Proxies
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Tasks;
	using Ola.RestClient.Utils;
	
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Xml;

	
	public abstract class CrudProxy : RestProxy, IFinderProxy
	{
		protected abstract Type ResponseDtoType { get; }

		
		protected abstract EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto);


		protected abstract IInsertTask GetNewInsertTaskInstance();


		protected abstract IUpdateTask GetNewUpdateTaskInstance();


		protected virtual IInsertTask CreateInsertTask(EntityDto entity)
		{
			IInsertTask task = this.GetNewInsertTaskInstance();
			task.EntityToInsert = entity;
			return task;
		}

		
		protected virtual IUpdateTask CreateUpdateTask(EntityDto entity)
		{
			IUpdateTask task = this.GetNewUpdateTaskInstance();
			task.EntityToUpdate = entity;
			return task;
		}


		public virtual EntityDto GetByUid(int uid)
		{
			string url = this.MakeUrl() + "&uid=" + uid;
			string result = HttpUtils.Get(url);
			ResponseDto response = (ResponseDto) XmlSerializationUtils.Deserialize(this.ResponseDtoType, result);
			this.CheckErrors(response.Errors);
			return this.ExtractContentFromGetByUidResponse(response);
		}


		public virtual void Insert(EntityDto entity)
		{
			TasksRunner tasksRunner = new TasksRunner(this.WSAccessKey, this.FileUid);
			tasksRunner.Tasks.Add(this.CreateInsertTask(entity));
			this.HandleInsertResult(tasksRunner.Execute(), entity);
		}


		public virtual void Update(EntityDto entity)
		{
			TasksRunner tasksRunner = new TasksRunner(this.WSAccessKey, this.FileUid);
			tasksRunner.Tasks.Add(this.CreateUpdateTask(entity));
			this.HandleUpdateResult(tasksRunner.Execute(), entity);
		}

		
		protected virtual void HandleInsertResult(TasksResponse tasksResponse, EntityDto entity)
		{
			InsertResult result = (InsertResult) tasksResponse.Results[0];
			this.CheckErrors(result.Errors);	
			entity.Uid = result.InsertedEntityUid;
			entity.LastUpdatedUid = result.LastUpdatedUid;
			entity.UtcLastModified = result.UtcLastModified; 
		}


		protected virtual void HandleUpdateResult(TasksResponse tasksResponse, EntityDto entity)
		{
			UpdateResult updateResult = (UpdateResult) tasksResponse.Results[0];
			this.CheckErrors(updateResult.Errors);
			entity.LastUpdatedUid = updateResult.LastUpdatedUid;
			entity.UtcLastModified = updateResult.UtcLastModified; 
		}


		public virtual void DeleteByUid(int uid)
		{
			string url = this.MakeUrl() + "&uid=" + uid;
			string result = HttpUtils.Delete(url);
			CrudResponseDto response = (CrudResponseDto) XmlSerializationUtils.Deserialize(
				this.ResponseDtoType, result);
			this.CheckErrors(response.Errors);
		}


		#region IFinderProxy Members

		public XmlDocument Find()
		{
			return this.Find(null);
		}


		public XmlDocument Find(NameValueCollection queries)
		{
			string url = MakeUrl
				(
					this.BaseUri,
					this.ResourceListName,
					this.WSAccessKey,
					this.FileUid
				);

			if (queries != null)
			{
				url += "&" + Util.ToQueryString(queries);
			}

			string result = HttpUtils.Get(url);
			XmlDocument document = new XmlDocument();
			document.LoadXml(result);
			this.CheckErrors(document);
			return document;
		}

		public List<T> FindList<T>(string nodeXPath, params string[] parametersAndValues)
			where T : EntityDto, new()
		{
			NameValueCollection parameters = null;

			if (parametersAndValues != null)
			{
				for (int i = 0; i < parametersAndValues.Length; i += 2)
				{
					if (parameters == null)
						parameters = new NameValueCollection();
					parameters.Add(parametersAndValues[i], parametersAndValues[i + 1]);
				}
			}

			XmlDocument document = Find(parameters);

			XmlElement root = document.DocumentElement;
			XmlNodeList itemsNode = root.SelectNodes(nodeXPath);

			List<T> list = new List<T>();

			foreach (XmlNode itemNode in itemsNode)
			{
				try
				{
					T dto = (T)XmlSerializationUtils.Deserialize(typeof(T), itemNode.OuterXml);
					list.Add(dto);
				}
				catch (Exception ex)
				{
					throw new ApplicationException("Could not deserialize:" + itemNode.OuterXml, ex);
				}
			}

			return list;
		}
		#endregion
	}
}
