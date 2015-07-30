namespace Ola.RestClient.NUnitTests
{
	using NUnit.Framework;

	using Ola.RestClient.Dto;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Proxies;
	using Ola.RestClient.Tasks;
	
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Xml;


	[TestFixture]
	public class TasksTests : RestClientTests
	{
		[Test]
		public void TestTasks1()
		{
			TasksRunner tasksRunner = new TasksRunner(this.WSAccessKey, this.FileUid);
			IInsertTask insertTask = null;
			IUpdateTask updateTask = null;

			this.MrsSmith.PostalAddress.Street = "11/111 ABC Av";
			this.MrsSmith.PostalAddress.City = "Sydney";
			this.MrsSmith.PostalAddress.State = "NSW";
			this.MrsSmith.MobilePhone = "0666 666 666";
			
			updateTask = new UpdateContactTask();
			updateTask.EntityToUpdate = this.MrsSmith;
			tasksRunner.Tasks.Add(updateTask);

			SaleTests saleTests = new SaleTests();
			saleTests.TestFixtureSetUp();

			insertTask = new InsertInvoiceTask();
			insertTask.EntityToInsert = saleTests.GetServiceSale();
			tasksRunner.Tasks.Add(insertTask);

			insertTask = new InsertInvoiceTask();
			insertTask.EntityToInsert = saleTests.GetUnpaidItemSale();
			tasksRunner.Tasks.Add(insertTask);
			
			TasksResponse response = tasksRunner.Execute();
		}


		[Test]
		public void TestTasks2()
		{
			TasksRunner tasksRunner = new TasksRunner(this.WSAccessKey, this.FileUid);
			IInsertTask insertTask = null;
			IUpdateTask updateTask = null;

			this.MrsSmith.PostalAddress.Street = "11/111 ABC Av";
			this.MrsSmith.PostalAddress.City = "Sydney";
			this.MrsSmith.PostalAddress.State = "NSW";
			this.MrsSmith.MobilePhone = "0666 666 666";
			
			updateTask = new UpdateContactTask();
			updateTask.EntityToUpdate = this.MrsSmith;
			tasksRunner.Tasks.Add(updateTask);

			SaleTests saleTests = new SaleTests();
			saleTests.TestFixtureSetUp();

			insertTask = new InsertInvoiceTask();
			InvoiceDto invoiceDto = saleTests.GetServiceSale();
			invoiceDto.ContactUid = 99999;
			insertTask.EntityToInsert = invoiceDto;
			tasksRunner.Tasks.Add(insertTask);

			insertTask = new InsertInvoiceTask();
			insertTask.EntityToInsert = saleTests.GetUnpaidItemSale();
			tasksRunner.Tasks.Add(insertTask);
			
			TasksResponse response = tasksRunner.Execute();
		}
	}
}
