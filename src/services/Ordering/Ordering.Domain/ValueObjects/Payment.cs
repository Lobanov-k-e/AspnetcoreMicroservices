using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.ValueObjects
{
    public class Payment : ValueObject
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CardName;
            yield return CardNumber;
            yield return Expiration;
            yield return CVV;
            yield return PaymentMethod;
        }
    }
}
