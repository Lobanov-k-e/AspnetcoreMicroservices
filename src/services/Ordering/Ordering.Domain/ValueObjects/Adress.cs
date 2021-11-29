using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.ValueObjects
{
    public class Adress : ValueObject
    {
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AddressLine;
            yield return Country;
            yield return State;
            yield return ZipCode;
        }
    }
}
