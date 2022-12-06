using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ICALibrary.Models
{
    public class XMLProcessing
    {
        public static T Deserializer<T>(string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public static string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> Ilist, int size)
        {
            for (int i = 0; i < Ilist.Count; i += size)
            {
                yield return Ilist.GetRange(i, Math.Min(size, Ilist.Count - i));
            }
        }

    }
}
