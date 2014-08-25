using System;
using System.ComponentModel;

namespace Yujt.Common.Tests.Helper
{
    public static class EnumHepler
    {
        public static string GetDescription<TEnum>(this TEnum tEnum, TEnum value) where TEnum : struct
        {
            var description = value.GetType().GetField(value.ToString())
                .GetCustomAttributes(typeof (DescriptionAttribute), false)[0] as DescriptionAttribute;
            if (description == null)
            {
                const string msgFormat = "There is no DescriptionAttribute for {0} in {1}";
                var msg = string.Format(msgFormat, value,typeof (TEnum));
                throw new Exception(msg);
            }
            return description.Description;
        }

        public static TEnum GetEnumByDescription<TEnum>( string description) where TEnum : struct
        {
            foreach (var field in typeof(TEnum).GetFields())
            {
                var desc = field.GetCustomAttributes(typeof (DescriptionAttribute), false)[0] as DescriptionAttribute;
                if (desc.Description.Equals(description))
                {
                    return (TEnum)Enum.Parse(typeof(TEnum), field.Name);
                }
            }
            return default (TEnum);
        }
    }
}
