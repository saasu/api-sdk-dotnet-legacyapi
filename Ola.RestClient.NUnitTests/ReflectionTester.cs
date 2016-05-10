using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace Ola.RestClient.NUnitTests
{
	public static class ReflectionTester
	{
		public static void AssertAreEqual(object expected, object actual, params string[] ignoreProperties)
		{
			AssertAreEqual(String.Empty, expected, actual, ignoreProperties);
		}
		public static void AssertAreEqual(string name, object expected, object actual, params string[] ignoreProperties)
		{
			//OR: 0 | 0 = 0, 0 | 1 = 1, 1 | 0 = 1
			//AND: 0 & 0 = 0, 0 & 1 = 0, 1 & 0 = 0, 1 & 1 = 1
			//XOR: 0 & 0 = 0, 0 & 1 = 1, 1 & 0 = 1, 1 & 1 = 0
			Assert.IsFalse(expected == null ^ actual == null, "One of the items is null.");
			if (expected == null)	// they are both null
				return;

			Assert.IsTrue(expected.GetType() == actual.GetType(), "Expected type {0} Actual Type {1}", expected.GetType(), actual.GetType());

			foreach(PropertyInfo pi in expected.GetType().GetProperties())
			{
				if (ignoreProperties != null)
				{
					if (Array.IndexOf(ignoreProperties, pi.Name) != -1)
						continue; // ignore!
				}
				if (pi.CanRead && pi.CanWrite)
				{
					object expectedValue = pi.GetValue(expected, null);
					object actualValue = pi.GetValue(actual, null);
					AssertAreEqualValues(name + "." + pi.Name, expectedValue, actualValue, ignoreProperties);
				}
			}

			foreach (FieldInfo fi in expected.GetType().GetFields())
			{
				if (ignoreProperties != null)
				{
					if (Array.IndexOf(ignoreProperties, fi.Name) != -1)
						continue; // ignore!
				}
				object expectedValue = fi.GetValue(expected);
				object actualValue = fi.GetValue(actual);
				AssertAreEqualValues(name + "." + fi.Name, expectedValue, actualValue, ignoreProperties);
			}
		}

		public static void AssertAreEqualValues(string name, object expectedValue, object actualValue, params string[] ignoreProperties)
		{
			if (expectedValue is IEnumerable && expectedValue.GetType() != typeof(string))
			{	// we don't yet know to compare internal lists
				IEnumerable actualEnumearable = (IEnumerable) actualValue;
				IEnumerator actualEnumearator = actualEnumearable.GetEnumerator();

				actualEnumearator.MoveNext();

				int index = 0;
				foreach (object expected in (IEnumerable) expectedValue)
				{
					object actual = actualEnumearator.Current;

					AssertAreEqualValues(name + "[" + index + "]", expected, actual, ignoreProperties);

					index++;

					actualEnumearator.MoveNext();
				}
			}

			if (expectedValue != null)
			{
				if (
					expectedValue.GetType().IsPrimitive ||
					expectedValue.GetType() == typeof(string) ||
					expectedValue.GetType() == typeof(Decimal) ||
					expectedValue.GetType() == typeof(DateTime) ||
					expectedValue.GetType().IsEnum)
				{
					Console.WriteLine(name + ": " + expectedValue + " to " + actualValue);

                    if (expectedValue.GetType() == typeof(DateTime))
                    {
                        AssertDatetimesEqualWithVariance((DateTime)expectedValue, (DateTime)actualValue);
                    }
                    Assert.AreEqual(expectedValue, actualValue, "Property {0}", name);
					return;
				}
				else
				{
					AssertAreEqual(name, expectedValue, actualValue, ignoreProperties);
				}
			}
		}

        public static bool AssertDatetimesEqualWithVariance(DateTime dateToVary, DateTime dateToNotTouch)
        {
            var variance = System.Configuration.ConfigurationManager.AppSettings["TestingDateTimeVariance"];

            Int16 varianceInt = 0;

            Int16.TryParse(variance, out varianceInt);

            if (varianceInt == 0)
            {
                return dateToVary == dateToNotTouch;
            }

            var minDate = dateToVary.AddSeconds(-varianceInt);
            var maxDate = dateToVary.AddSeconds(varianceInt);

            return dateToNotTouch > minDate && dateToNotTouch < maxDate;
        }
    }
}
