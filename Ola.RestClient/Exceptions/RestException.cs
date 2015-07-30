namespace Ola.RestClient.Exceptions
{
	using Ola.RestClient.Dto;

	using System;
	using System.Collections;


	public class RestException : ApplicationException
	{
		private ArrayList _Errors;
		public ArrayList Errors
		{
			get
			{
				if (this._Errors == null)
				{
					return new ArrayList();
				}

				return this._Errors;
			}
		}


		private string _Type = "";
		public string Type
		{
			get
			{
				return this._Type;
			}
		}


		public RestException()
		{
		}


		public RestException(string message) : base(message)
		{
		}


		public RestException(ErrorInfo errorInfo) : base(errorInfo.Message)
		{
			this._Type  = errorInfo.Type;
		}


		public RestException(string message, Exception innerException) : base(message, innerException)
		{
		}

 
		public RestException(ArrayList errors) : base
			(
				(errors.Count == 1) ? ((ErrorInfo) errors[0]).Message : "The call returned some errors."
			)
		{
			this._Errors = errors;
			if (errors.Count == 1)
			{
				this._Type = ((ErrorInfo) errors[0]).Type;
			}
		}
	}
}
