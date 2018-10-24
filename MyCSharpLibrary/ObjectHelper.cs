using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace MyCSharpLibrary
{
    public static class ObjectHelper
    {
        /// <summary>
        /// 深複製物件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static void SerializeToXmlFile<T>(this T mObject, string FileName)
        {
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Create))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(T));
                    StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                    formatter.Serialize(writer, mObject);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
            }
        }

        public static T DeserializeFromXmlFile<T>(string FileName)
        {
            T mObject = default(T);
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(T));
                    mObject = (T)formatter.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return mObject;
        }
    }
}