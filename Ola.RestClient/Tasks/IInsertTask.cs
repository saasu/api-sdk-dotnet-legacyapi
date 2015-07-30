namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;


	public interface IInsertTask : ITask
	{
		EntityDto EntityToInsert { get; set; }	
	}
}
