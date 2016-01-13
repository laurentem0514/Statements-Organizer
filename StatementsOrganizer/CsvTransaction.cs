using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatementsOrganizer
{
    [DelimitedRecord(",")]
    [IgnoreFirst(1)]
    public class CsvTransaction
    {
        public string PostedDate;
        public string ReferenceNumber;
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public string Payee;
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public string Address;
        public string Amount;
    }
}
