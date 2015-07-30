using System;
using System.Collections;
using System.Xml.Serialization;
using Ola.RestClient.Dto.Enums;


namespace Ola.RestClient.Dto
{
	[XmlRoot(ElementName = "tradingTerms")]
	public class TradingTermsDto : IDto
	{
	    [XmlElement(ElementName = "type")]
        public int Type
	    {
            get { return (int)_typeEnum; }
            set
            {
                if (!Enum.IsDefined(typeof(TradingTermsType), value))
                {
                    throw new ArgumentException(string.Format("The value '{0}' is outside the range of values allowed for this type. See the property TypeEnum for a list of valid values.", value));
                }
                _typeEnum = (TradingTermsType)value;
            }
	    }

        [XmlElement(ElementName = "interval")]
        public int Interval { get; set; }

	    [XmlElement(ElementName = "intervalType")]
	    public int IntervalType
	    {
            get { return (int)_intervalTypeEnum; }
            set
            {
                if (!Enum.IsDefined(typeof(TimeIntervalType), value))
                {
                    throw new ArgumentException(string.Format("The value '{0}' is outside the range of values allowed for this type. See the property IntervalTypeEnum for a list of valid values." , value));
                }
                _intervalTypeEnum = (TimeIntervalType)value;
            }
	    }

	    private TradingTermsType _typeEnum;
	    [XmlElement(ElementName = "typeEnum")]
	    public TradingTermsType TypeEnum
	    {
	        get { return _typeEnum; }
	        set { _typeEnum = value; }
	    }

	    private TimeIntervalType _intervalTypeEnum;
	    [XmlElement(ElementName = "intervalTypeEnum")]
        public TimeIntervalType IntervalTypeEnum
	    {
	        get { return _intervalTypeEnum; }
	        set { _intervalTypeEnum = value; }
	    }
	}
}
