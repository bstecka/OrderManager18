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
                return new CounterpartyService(ICounterpartyDAO, IMapperBaseCounterparty, IMapperBaseCounterpartysStock, IMapperBaseOrder);
            }
        }

        public static IPriorityService IPriorityService
        {
            get
            {
                return new PriorityService(IPriorityDAO, IMapperBasePriority, IMapperBaseStock);
            }
        }

        public static ICounterpartysStockService ICounterpartysStockService
        {
            get
            {
                return new CounterpartysStockService(ICounterpartysStockDAO, IMapperBaseCounterpartysStock, IMapperrBasePercentageDiscount);
            }
        }

        public static IStockService IStockService
        {
            get
            {
                return new StockService(IStockDAO, IMapperBaseStock, IMapperBaseOrder, IMapperBaseCounterpartysStock);
            }
        }

        public static IEligibleOrdersNamesService IEligibleOrdersNamesService
        {
            get
            {
                return new EligibleORersNamesService(IEligibleOrdersNamesDAO, IMapperBaseOrdersNames);
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
                return new DAL.ExternalSysDAO.CounterpartysStockDAO();
            }
        }

        public static IPriorityDAO IPriorityDAO
        {
            get
            {
                return new PriorityDAO();
            }
        }

        public static IPercentageDiscountDAO IPercentageDiscountDAO
        {
            get
            {
                return new DAL.InternalSysDAO.PercentageDiscountDAO();
            }
        }

        public static IOrderDAO IOrderDAO
        {
            get
            {
                return new DAL.InternalSysDAO.OrderDAO();
            }
        }

        public static IStockDAO IStockDAO
        {
            get
            {
                return new DAL.InternalSysDAO.Stock();
            }
        }

        public static IEligibleOrdersNamesDAO IEligibleOrdersNamesDAO
        {
            get
            {
                return new EligibleOrdersNames();
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

        public static IMapperBase<Domain.Entity.PercentageDiscount> IMapperrBasePercentageDiscount
        {
            get
            {
                return new PercentageDiscountMapper(IPercentageDiscountDAO, ICounterpartysStockDAO);
            }
        }

        public static IMapperBase<Domain.Entity.Order> IMapperBaseOrder
        {
            get
            {
                return new OrderMapper(IOrderDAO);
            }
        }

        public static IMapperBase<string> IMapperBaseOrdersNames
        {
            get
            {
                return new EligibleOrdersNamesMapper();
            }
        }
    }
}