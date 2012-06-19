using System.Globalization;

namespace Acid.PuntoPagos.Sdk.Dtos
{
    public class CurrencyDto
    {
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return Amount.ToString("0.00", GetNumberformat());
        }
        private static NumberFormatInfo GetNumberformat()
        {
            return new NumberFormatInfo()
                       {
                           NumberDecimalSeparator = ".",
                           NumberDecimalDigits = 2
                       };
        }

        public CurrencyDto(decimal amount)
        {
            Amount = amount;
        }

        public CurrencyDto(string amount)
        {
            Amount = decimal.Parse(amount, GetNumberformat());
        }
    }
}