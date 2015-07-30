namespace Ola.RestClient.Utils
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Web;
	

	public class Util
	{
		private Util()
		{
		}


		public static string DefaultValueToEmpty(string s)
		{
			return (s == null) ? "" : s.Trim();
		}


		public static int ToInt32(string value)
		{
			string s = DefaultValueToEmpty(value);
			return (s.Length == 0) ? 0 : Int32.Parse(s);
		}


		public static string ToQueryString(NameValueCollection queries)
		{
			if (queries.Count == 0)
			{
				throw new ArgumentException("No query string parameter found.", "queries");
			}
			
			string s = "";
			int count = queries.Count;
			for (int i = 0; i < count; i++)
			{
				s += queries.GetKey(i) + "=" + HttpUtility.UrlEncode(queries[i]) + "&";
			}
			return s.TrimEnd('&');
		}
	}
}
