using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serialization3
{
    internal interface ICSVserializationDeserialization<T>
    {
        void SerilizeToCSV(List<T> list, string filepath) { }
        List<T> DeserializeCSV(string filepath);
    }
}
