using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyCSharpLibrary
{
    public static class DbHelper
    {
        //效率較差
        public static IEnumerable<T> GetEntityCollectionWithAutoMapping<T>(this IDataReader reader)
            where T : class, new()
        {
            var result = new List<T>();

            while (reader.Read())
            {
                var newRow = new T();
                foreach (var propertyInfo in typeof(T).GetProperties())
                {
                    var type = propertyInfo.PropertyType;

                    object value;
                    try
                    {
                        value = Convert.ChangeType(reader[propertyInfo.Name], type);
                        typeof(T).GetProperty(propertyInfo.Name)?.SetValue(newRow, value, null);
                    }
                    catch (Exception e)
                    {
                        //可省略下行，參考型別可初始化才不爲null public string JOB { get; set; } = "123";
                        //value = type.IsValueType ? Activator.CreateInstance(type) : null;
                    }
                }

                result.Add(newRow);
            }

            return result;
        }

        public static IEnumerable<T> GetEntityCollection<T>(this IDataReader reader, Func<IDataReader, T> rowMapper)
            where T : class, new()
        {
            while (reader.Read())
            {
                yield return rowMapper(reader);
            }
        }

        public static object DbNullToNull(this object original)
        {
            return original == DBNull.Value ? null : original;
        }
    }
}
