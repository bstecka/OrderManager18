using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain
{
    interface IOrdersGenerator
    {
        List<Order> Generate(IPriorityService priorityService);
    }
}
