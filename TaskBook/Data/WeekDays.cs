using System;
using System.Runtime.Serialization;

namespace TaskBook.Data
{
    [DataContract]
    [Flags]
    public enum WeekDays
    {
        [EnumMember]
        Пн = 0x01,
        [EnumMember]
        Вт = 0x02,
        [EnumMember]
        Ср = 0x04,
        [EnumMember]
        Чт = 0x08,
        [EnumMember]
        Пт = 0x10,
        [EnumMember]
        Сб = 0x20,
        [EnumMember]
        Вс = 0x40,
    }
}
