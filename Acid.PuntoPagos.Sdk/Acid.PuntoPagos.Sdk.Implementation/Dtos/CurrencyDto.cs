using System.Globalization;

namespace Acid.PuntoPagos.Sdk.Dtos
{
    public class CurrencyDto
    {
        public decimal Mount { get; set; }

        public override string ToString()
        {
            return Mount.ToString("0.00", GetNumberformat());
        }
        private static NumberFormatInfo GetNumberformat()
        {
            return new NumberFormatInfo()
                       {
                           NumberDecimalSeparator = ".",
                           NumberDecimalDigits = 2
                       };
        }

        public CurrencyDto(decimal mount)
        {
            Mount = mount;
        }

        public CurrencyDto(string mount)
        {
            Mount = decimal.Parse(mount, GetNumberformat());
        }
    }
}