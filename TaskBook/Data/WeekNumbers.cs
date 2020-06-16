using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.Data
{
    [DataContract]
    [Flags]
    public enum WeekNumbers
    {
        [EnumMember]
        First = 0x01,
        [EnumMember]
        Second = 0x02,
        [EnumMember]
        Third = 0x04,
        [EnumMember]
        Fourth = 0x08,
        [EnumMember]
        Fifth = 0x10,
    }

    public static class WeekNumbersExtensions
    {
        public static string GetString(this WeekNumbers weekNumbers)
        {
            return weekNumbers.ToString().Replace("First" , "1")
                                         .Replace("Second", "2")
                                         .Replace("Third" , "3")
                                         .Replace("Fourth", "4")
                                         .Replace("Fifth" , "5");
        }
    }

}
