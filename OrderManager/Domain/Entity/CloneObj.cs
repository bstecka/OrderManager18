using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    class CloneObj
    {
        /// <summary>
        /// Parametrized method for deep cloning of entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object to clone.</param>
        /// <returns>Returns a parametrized clone of the given object.</returns>
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
