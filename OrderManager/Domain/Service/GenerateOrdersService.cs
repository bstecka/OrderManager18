using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManager.Domain.Entity;
using OrderManager.DTO;

namespace OrderManager.Domain.Service
{
    class GenerateOrdersService
    {
        private List<Stock> stock;

        public GenerateOrdersService(List<Stock> stock)
        {
            this.stock = stock;
        }

        public List<Object> Generate(List<Stock> stock)
        {/*
            Dictionary<Stock, Counterparty> result = new Dictionary<Stock, Counterparty>();
            List<Stock> list;
            if ((list = getStockByHighestPriority("cena", stock)) != null)
                appendToDictionary(result, (new PriorityPriceService(list)).BestSuppliers());
            if ((list = getStockByHighestPriority("odleglosc", stock)) != null)
                appendToDictionary(result, (new PriorityPriceService(list)).BestSuppliers());
            if ((list = getStockByHighestPriority("czestosc", stock)) != null)
                appendToDictionary(result, (new PriorityPriceService(list)).BestSuppliers());
            */return null;
        }

        private List<Stock> getStockByHighestPriority(string priorityName, List<Stock> stock)
        {
            DAL.InternalSysDAO.Priority priorityDAO = new DAL.InternalSysDAO.Priority();
            StockMapper stockMapper = new StockMapper();
            PriorityMapper priorityMapper = new PriorityMapper();
            List<Stock> result = new List<Stock>();
            foreach (Stock element in stock)
                if (priorityMapper.MapFrom(priorityDAO.GetPriority(stockMapper.MapTo(element))).Criteria[0].Equals(priorityName))
                    result.Add(element);
            return result;
        }
        
        private Dictionary<Stock, Counterparty> appendToDictionary(Dictionary<Stock, Counterparty> baseDictionary, Dictionary<Stock, Counterparty> toAppend)
        {
            foreach (KeyValuePair<Stock, Counterparty> element in toAppend)
                baseDictionary.Add(element.Key, element.Value);
            return baseDictionary;
        }
    }
}
