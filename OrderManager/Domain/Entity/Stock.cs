using OrderManager.Domain.Service;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    public class Stock
    {
        int id;
        int maxInStockRoom;
        int minInStockRoom;
        int numberOfImtems;
        int weightOfItem;
        int numOfItems;
        int vat;
        string code;
        string name;
        Category category;

        public Stock(int id, int maxInStockRoom, int minInStockRoom, int numberOfImtems, int weightOfItem, int numOfItems, int VAT, string code, string name, Category category)
        {
            this.id = id;
            this.maxInStockRoom = maxInStockRoom;
            this.minInStockRoom = minInStockRoom;
            this.numberOfImtems = numberOfImtems;
            this.weightOfItem = weightOfItem;
            this.numOfItems = numOfItems;
            this.vat = VAT;
            this.code = code;
            this.name = name;
            this.category = category;
        }

        public int Id { get => id; set => id = value; }
        public int MaxInStockRoom { get => maxInStockRoom; set => maxInStockRoom = value; }
        public int MinInStockRoom { get => minInStockRoom; set => minInStockRoom = value; }
        public int NumberOfItemsInStockRoom { get => numberOfImtems; set => numberOfImtems = value; }
        public int WeightOfItem { get => weightOfItem; set => weightOfItem = value; }
        public int NumOfItems { get => numOfItems; set => numOfItems = value; }
        public int VAT { get => vat; set => vat = value; }
        public string Code { get => code; set => code = value; }
        public string Name { get => name; set => name = value; }
        public Category Category { get => category; set => category = value; }
        
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
