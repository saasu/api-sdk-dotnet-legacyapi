using System;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace Ola.RestClient.Utils
{
	public static class XmlSerializationUtils
	{
		public static string Serialize(object instance)
		{
			using (TextWriter writer = new StringWriter())
			{
				XmlSerializer serializer = new XmlSerializer(instance.GetType());
				serializer.Serialize(writer, instance);
				return writer.ToString();
			}
		}


		public static object Deserialize(Type type, string xmlFragment)
		{
			if(xmlFragment == null || xmlFragment.Trim().Length == 0)
			{
				throw new ArgumentException("The xmlFragment was empty.");
			}

			using (XmlTextReader reader = new XmlTextReader(new StringReader(xmlFragment)))
			{	
				// We need to use an XmlTextReader over a StringReader so the \r\n are correctly maintained in the elements
				XmlSerializer serializer = new XmlSerializer(type);
				return serializer.Deserialize(reader);
			}
		}
	}
}
