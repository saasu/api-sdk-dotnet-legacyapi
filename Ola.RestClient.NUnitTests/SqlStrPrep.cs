using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ola.RestClient.NUnitTests
{
	public class SqlStrPrep
	{
		private SqlStrPrep()
		{
		}


		public static string Escape(string val)
		{
			if (val == null)
			{
				return null;
			}

			string s = val.Replace("'", "''");
			return s;
		}


		/// <summary>
		/// Prepares a string used for "LIKE" comparison is SQL - escapes chars [, % and _
		/// </summary>
		public static string LikeEscape(string pStr)
		{
			pStr.Replace("[", "[[]");
			pStr.Replace("%", "[%]");
			pStr.Replace("_", "[_]");

			return pStr;
		}




		/// <overloads>
		/// Returns a string representation of the supplied integer with item delimiters suitable for inserting 
		/// into an SQL statement.
		/// </overloads>
		/// <returns>
		/// String representation of integer or (null).  No delimiters appended.
		/// </returns>
		/// <param name="pInt">
		/// Integer to convert.
		/// </param>
		public static string Int(int pInt)
		{
			return pInt.ToString();
		}




		/// <param name="pInt">
		/// String integer to convert.
		/// </param>
		public static string Int(string pInt)
		{
			if (pInt.Trim() == "")
			{
				return "(null)";
			}

			return pInt;
		}




		/// <summary>
		/// Returns the supplied string with item delimiters suitable for inserting into an SQL statement.
		/// </summary>
		/// <returns>
		/// String 'string' or (null).
		/// </returns>
		/// <param name="pStr">
		/// String to convert.
		/// </param>
		public static string String(string pStr)
		{
			if (pStr.Trim() == "")
			{
				return "(null)";
			}

			pStr = pStr.Replace("'", "''");
			return "'" + pStr + "'";
		}




		/// <summary>
		/// Returns the supplied DateTime in "safe" format with item delimiters suitable for inserting into an SQL statement.
		/// </summary>
		/// <returns>
		/// String 'dd-MMM-yyyy' or (null).
		/// </returns>
		/// <param name="pDtm">
		/// DateTime to convert.
		/// </param>
		public static object ToDateOnly(DateTime dateTime)
		{
			if (dateTime == DateTime.MinValue)
			{
				return DBNull.Value;
			}

			//  Used to use yyyy/MM/dd in .NET 1.1.
			return dateTime.ToString("dd-MMM-yy");
		}




		/// <summary>
		/// Returns the supplied DateTime in "safe" format with item delimiters suitable for inserting into an SQL statement.
		/// </summary>
		/// <returns>
		/// String 'yyyy'-'MM'-'dd'T'HH':'mm':'ss' or (null).
		/// </returns>
		/// <param name="pDtm">
		/// DateTime to convert.
		/// </param>
		public static object ToDateTime(DateTime dateTime)
		{
			if (dateTime == DateTime.MinValue)
			{
				return DBNull.Value;
			}

			//  Used to be this in .NET 1.1: return "'" + dateTime.ToString("yyyy/MM/dd HH:mm:ss") + "'";
			//	return dateTime.ToString("s");
			return dateTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
		}
	}
}
