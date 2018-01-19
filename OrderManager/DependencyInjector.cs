using OrderManager.DAL.ExternalSysDAO;
using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain;
using OrderManager.Domain.Entity;
using OrderManager.Domain.OrderGenerator;
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

        /// <summary>
        /// Injector for the order generator service.
        /// </summary>
        /// <value>The order generator service.</value>
        public static IOrdersGenerator IOrdersGenerator
        {
            get
            {
                return new OrdersGenerator(DependencyInjector.ICounterpartyService,
                    DependencyInjector.IPriorityService, DependencyInjector.ICounterpartysStockService,
                    DependencyInjector.IStockService, DependencyInjector.IEligibleOrdersNamesService);
            }
        }

        /// <summary>
        /// Injector for the counterparty service.
        /// </summary>
        /// <value>
        /// The counterparty service.
        /// </value>
        public static ICounterpartyService ICounterpartyService
        {
            get
            {
                return new CounterpartyService(ICounterpartyDAO, IMapperBaseCounterparty, IMapperBaseCounterpartysStock, IMapperBaseOrder);
            }
        }

        /// <summary>
        /// Injector for the priority service.
        /// </summary>
        /// <value>
        /// The priority service.
        /// </value>
        public static IPriorityService IPriorityService
        {
            get
            {
                return new PriorityService(IPriorityDAO, IMapperBasePriority, IMapperBaseStock);
            }
        }

        /// <summary>
        /// Injector for the counterpartys stock service.
        /// </summary>
        /// <value>
        /// The counterpartys stock service.
        /// </value>
        public static ICounterpartysStockService ICounterpartysStockService
        {
            get
            {
                return new CounterpartysStockService(ICounterpartysStockDAO, IMapperBaseCounterpartysStock, IMapperBasePercentageDiscount);
            }
        }

        /// <summary>
        /// Injector for the stock service.
        /// </summary>
        /// <value>
        /// The stock service.
        /// </value>
        public static IStockService IStockService
        {
            get
            {
                return new StockService(IStockDAO, IMapperBaseStock, IMapperBaseOrder, IMapperBaseCounterpartysStock);
            }
        }

        /// <summary>
        /// Injector for the order service.
        /// </summary>
        /// <value>
        /// The order service.
        /// </value>
        public static IOrderService IOrderService
        {
            get
            {
                return new OrderService(IOrderDAO, IMapperBaseOrder);
            }
        }

        /// <summary>
        /// Injector for the orders names service.
        /// </summary>
        /// <value>
        /// The eligible orders names service.
        /// </value>
        public static IEligibleOrdersNamesService IEligibleOrdersNamesService
        {
            get
            {
                return new EligibleORersNamesService(IEligibleOrdersNamesDAO, IMapperBaseOrdersNames);
            }
        }

        /// <summary>
        /// Injector for the counterparty DAO.
        /// </summary>
        /// <value>
        /// The counterparty DAO.
        /// </value>
        public static ICounterpartyDAO ICounterpartyDAO
        {
            get
            {
                return new ExternalSysDAO.CounterpartyDAO();
            }
        }

        /// <summary>
        /// Injector for the tranche DAO.
        /// </summary>
        /// <value>
        /// The tranche DAO.
        /// </value>
        public static ITrancheDAO ITrancheDAO
        {
            get
            {
                return new TrancheDAO();
            }
        }

        /// <summary>
        /// Injector for the tranche service.
        /// </summary>
        /// <value>
        /// The tranche service.
        /// </value>
        public static ITrancheService ITrancheService
        {
            get
            {
                return new TrancheService(ITrancheDAO, IMapperBaseTranche, IMapperBasePercentageDiscount);
            }
        }

        /// <summary>
        /// Injector for the counterpartys stock DAO.
        /// </summary>
        /// <value>
        /// The counterpartys stock DAO.
        /// </value>
        public static ICounterpartysStockDAO ICounterpartysStockDAO
        {
            get
            {
                return new DAL.ExternalSysDAO.CounterpartysStockDAO();
            }
        }

        /// <summary>
        /// Injector for the priority DAO.
        /// </summary>
        /// <value>
        /// The priority DAO.
        /// </value>
        public static IPriorityDAO IPriorityDAO
        {
            get
            {
                return new PriorityDAO();
            }
        }

        /// <summary>
        /// Injector for the percentage discount DAO.
        /// </summary>
        /// <value>
        /// The percentage discount DAO.
        /// </value>
        public static IPercentageDiscountDAO IPercentageDiscountDAO
        {
            get
            {
                return new DAL.InternalSysDAO.PercentageDiscountDAO();
            }
        }

        /// <summary>
        /// Injector for the order DAO.
        /// </summary>
        /// <value>
        /// The order DAO.
        /// </value>
        public static IOrderDAO IOrderDAO
        {
            get
            {
                return new DAL.InternalSysDAO.OrderDAO();
            }
        }

        /// <summary>
        /// Injector for the stock DAO.
        /// </summary>
        /// <value>
        /// The stock DAO.
        /// </value>
        public static IStockDAO IStockDAO
        {
            get
            {
                return new DAL.InternalSysDAO.StockDAO();
            }
        }

        /// <summary>
        /// Injector for the eligible orders names DAO.
        /// </summary>
        /// <value>
        /// The eligible orders names DAO.
        /// </value>
        public static IEligibleOrdersNamesDAO IEligibleOrdersNamesDAO
        {
            get
            {
                return new EligibleOrdersNames();
            }
        }

        /// <summary>
        /// Injector for the counterparty mapper.
        /// </summary>
        /// <value>
        /// Counterparty mapper.
        /// </value>
        public static IMapperBase<Counterparty> IMapperBaseCounterparty
        {
            get
            {
                return new CounterpartyMapper();
            }
        }

        /// <summary>
        /// Injector for the tranche mapper.
        /// </summary>
        /// <value>
        /// The tranche mapper.
        /// </value>
        public static IMapperBase<Tranche> IMapperBaseTranche
        {
            get
            {
                return new TrancheMapper(ITrancheDAO);
            }
        }

        /// <summary>
        ///  Injector for the counterpartysStock mapper.
        /// </summary>
        /// <value>
        /// The counterpartysStock mapper.
        /// </value>
        public static IMapperBase<Domain.Entity.CounterpartysStock> IMapperBaseCounterpartysStock
        {
            get
            {
                return new CounterpartysStockMapper(ICounterpartysStockDAO);
            }
        }

        /// <summary>
        /// Injector for the priority mapper.
        /// </summary>
        /// <value>
        /// The priority mapper.
        /// </value>
        public static IMapperBase<List<Priority>> IMapperBasePriority
        {
            get
            {
                return new PriorityMapper();
            }
        }

        /// <summary>
        /// Injector for the stock mapper.
        /// </summary>
        /// <value>
        /// The stock mapper.
        /// </value>
        public static IMapperBase<Domain.Entity.Stock> IMapperBaseStock
        {
            get
            {
                return new StockMapper();
            }
        }

        /// <summary>
        /// Injector for the percentage discount mapper.
        /// </summary>
        /// <value>
        /// The percentage discount mapper.
        /// </value>
        public static IMapperBase<Domain.Entity.PercentageDiscount> IMapperBasePercentageDiscount
        {
            get
            {
                return new PercentageDiscountMapper(IPercentageDiscountDAO, ICounterpartysStockDAO);
            }
        }

        /// <summary>
        /// Injector for the order mapper.
        /// </summary>
        /// <value>
        /// The order mapper.
        /// </value>
        public static IMapperBase<Domain.Entity.Order> IMapperBaseOrder
        {
            get
            {
                return new OrderMapper(IOrderDAO);
            }
        }

        /// <summary>
        /// Injector for the orders names mapper.
        /// </summary>
        /// <value>
        /// The orders names mapper.
        /// </value>
        public static IMapperBase<string> IMapperBaseOrdersNames
        {
            get
            {
                return new EligibleOrdersNamesMapper();
            }
        }
    }
}