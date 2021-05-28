using System;
using System.ComponentModel;

namespace ApiRequestManager.Services
{
    public class HelperService
    {
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes
                                             (typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}