
namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Utils;
	using Ola.RestClient.Proxies;
	using System.Diagnostics;

	using System;
	using System.Collections;
	using System.Xml.Serialization;


	public class TasksRunner : RestProxy
	{
		private static BooleanSwitch _boolSwitch = new BooleanSwitch("DataMessagesSwitch", "Bool Switch in config file");

		public TasksRunner() : base()
		{
		}


		public TasksRunner(string wsAccessKey, int fileUid) : base()
		{
			this.WSAccessKey = wsAccessKey;
			this.FileUid = fileUid;
		}


		public override string ResourceName
		{
			get
			{
				return "Tasks";
			}
		}


		public override string ResourceListName
		{
			get { throw new NotImplementedException(); }
		}


		public TaskWrapper _Wrapper = new TaskWrapper();
		public virtual ArrayList Tasks
		{
			get
			{
				return this._Wrapper.Tasks;
			}
			set
			{
				this._Wrapper.Tasks = value;
			}
		}
		
		
		public TasksResponse Execute()
		{
			var xml = XmlSerializationUtils.Serialize(this._Wrapper);
			var url = this.MakeUrl();

			if (_boolSwitch.Enabled)
			{
				Trace.WriteLine("Post URL:");
				Trace.WriteLine(url);
				Trace.WriteLine("Request XML:");
				Trace.WriteLine(xml);
			}
			
			var result = HttpUtils.Post(this.MakeUrl(), xml);

			if (_boolSwitch.Enabled)
			{
				Trace.WriteLine("Response XML:");
				Trace.WriteLine(result);
			}
			
			var tasksResponse = (TasksResponse) XmlSerializationUtils.Deserialize(typeof(TasksResponse), result);

			CheckErrors(tasksResponse.Errors);

			return tasksResponse;
		}
	}
}
