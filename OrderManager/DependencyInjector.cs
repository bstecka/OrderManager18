using OrderManager.DAL.ExternalSysDAO;
using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OrderManager
{
    class DependencyInjector
    {
        public static ICounterpartyService ICounterpartyService
        {
            get
            {
                return new CounterpartyService(ICounterpartyDAO, IMapperBaseCounterparty, IMapperBaseCounterpartysStock);
            }
        }

        public static IPriorityService IPriorityService
        {
            get
            {
                return new PriorityService(IPriorityDAO, IMapperBasePriority, IMapperBaseStock);
            }
        }

        public static ICounterpartyDAO ICounterpartyDAO
        {
            get
            {
                return new ExternalSysDAO.Counterparty();
            }
        }

        public static ICounterpartysStockDAO ICounterpartysStockDAO
        {
            get
            {
                return new DAL.ExternalSysDAO.CounterpartysStock();
            }
        }

        public static IPriorityDAO IPriorityDAO
        {
            get
            {
                return new Priority();
            }
        }


        public static IMapperBase<Counterparty> IMapperBaseCounterparty
        {
            get
            {
                return new CounterpartyMapper();
            }
        }

        public static IMapperBase<Domain.Entity.CounterpartysStock> IMapperBaseCounterpartysStock
        {
            get
            {
                return new CounterpartysStockMapper(ICounterpartysStockDAO);
            }
        }
         public static IMapperBase<List<Domain.Priority>> IMapperBasePriority
        {
            get
            {
                return new PriorityMapper();
            }
        }
        public static IMapperBase<Domain.Entity.Stock> IMapperBaseStock
        {
            get
            {
                return new StockMapper();
            }
        }
    }
}