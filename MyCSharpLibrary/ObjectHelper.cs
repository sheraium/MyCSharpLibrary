using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
    }
}
