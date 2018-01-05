using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    interface IEligibleOrdersNamesService
    {
        List<string> FetchNames(int numberOfNames);
    }
}
