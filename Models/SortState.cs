using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Money_exchange.Models
{
    public enum SortState
    {       
        FromCurrencyAsc,
        ToCurrencyAsc,
        FromAmountAsc,
        ToAmountAsc,
        DateAsc,
        FromCurrencyDesc,
        ToCurrencyDesc,
        FromAmountDesc,
        ToAmountDesc,
        DateDesc
    }
}
