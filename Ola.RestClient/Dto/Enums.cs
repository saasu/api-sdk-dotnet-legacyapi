using System.Xml.Serialization;

namespace Ola.RestClient.Dto.Enums
{
    public enum TimeIntervalType
    {
        [XmlEnum(Name = "Unspecified")]
        Unspecified = 0,
        [XmlEnum(Name = "Day")]
        Day = 1,
        [XmlEnum(Name = "Week")]
        Week = 2,
        [XmlEnum(Name = "Month")]
        Month = 3,
        [XmlEnum(Name = "CashOnDelivery")]
        CashOnDelivery = 4,
        [XmlEnum(Name = "Year")]
        Year = 5
    }

    public enum TradingTermsType
    {
        [XmlEnum(Name = "Unspecified")]
        Unspecified = 0,
        [XmlEnum(Name = "DueIn")]
        DueIn = 1,
        [XmlEnum(Name = "DueInEomPlusXDays")]
        DueInEomPlusXDays = 2,
        [XmlEnum(Name = "CashOnDelivery")]
        CashOnDelivery = 3
    }
}
