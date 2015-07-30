namespace Ola.RestClient.Utils
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Net;
	using System.Text;
	using System.Xml;


	public class HttpUtils
	{
		private HttpUtils()
		{
		}


		public static string Post(string url, string xmlFragment)
		{
			WebRequest request = WebRequest.Create(url);
			request.Method = "POST";
			
			System.Console.WriteLine("POST to {0}", url);
			System.Console.WriteLine(xmlFragment);

			using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
			{
				writer.WriteLine(xmlFragment);
				writer.Close();
			}
			
			HttpWebResponse response = (HttpWebResponse) request.GetResponse();
			using (StreamReader reader = new StreamReader(response.GetResponseStream()))
			{
				string s = reader.ReadToEnd();
				System.Console.WriteLine("POST Result:");
				System.Console.WriteLine(s);
				return s;
			}
		}


		public static string Post(string url)
		{
			return Post(url, "");
		}


		public static string Get(string url)
		{
			WebClient webClient = new WebClient();
			System.Console.WriteLine("GET: {0}", url);

			using (StreamReader reader = new StreamReader(webClient.OpenRead(url)))
			{
				string s = reader.ReadToEnd();
				System.Console.WriteLine("GET Result:");
				System.Console.WriteLine(s);
				return s;
			}
		}


		public static string Delete(string url)
		{
			WebRequest request = WebRequest.Create(url);
			request.Method = "DELETE";
			System.Console.WriteLine("DELETE: {0}", url);
			HttpWebResponse response = (HttpWebResponse) request.GetResponse();
			using (StreamReader reader = new StreamReader(response.GetResponseStream()))
			{
				string s = reader.ReadToEnd();
				System.Console.WriteLine("DELETE Result:");
				System.Console.WriteLine(s);
				return s;
			}
		}
	}
}
