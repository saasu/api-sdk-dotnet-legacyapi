namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;


	public interface IUpdateTask : ITask
	{
		EntityDto EntityToUpdate { get; set; }
	}
}
