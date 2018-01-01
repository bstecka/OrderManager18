using OrderManager.Domain.Service;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    class Stock
    {
        int id;
        int maxInStockroom;
        int minInStockRoom;
        int numberOfImtems;
        int weightOfItem;
        int numOfItems;
        int vat;
        string code;
        string name;
        private List<CounterpartysStock> counterpartysStock;

        public Stock(int id, int maxInStockroom, int minInStockRoom, int numberOfImtems, int weightOfItem, int numOfItems, int VAT, string code, string name)
        {
            this.id = id;
            this.maxInStockroom = maxInStockroom;
            this.minInStockRoom = minInStockRoom;
            this.numberOfImtems = numberOfImtems;
            this.weightOfItem = weightOfItem;
            this.numOfItems = numOfItems;
            this.vat = VAT;
            this.code = code;
            this.name = name;
        }

        public int Id { get => id; set => id = value; }
        public int MaxInStockroom { get => maxInStockroom; set => maxInStockroom = value; }
        public int MinInStockRoom { get => minInStockRoom; set => minInStockRoom = value; }
        public int NumberOfImtems { get => numberOfImtems; set => numberOfImtems = value; }
        public int WeightOfItem { get => weightOfItem; set => weightOfItem = value; }
        public int NumOfItems { get => numOfItems; set => numOfItems = value; }
        public int VAT { get => vat; set => vat = value; }
        public string Code { get => code; set => code = value; }
        public string Name { get => name; set => name = value; }
        public List<CounterpartysStock> CounterpartysStock
        {
            get
            {
                if (counterpartysStock == null)
                    counterpartysStock = (new StockService(new DAL.InternalSysDAO.Stock(), new StockMapper())).
                        GetStocksCounterpartysStock(this, new CounterpartysStockMapper(new DAL.InternalSysDAO.CounterpartysStock()));
                return counterpartysStock;
            }
            set => counterpartysStock = value;
        }
        public override bool Equals(object obj)
        {
            return obj is Stock && ((Stock)obj).Id == id;
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}
