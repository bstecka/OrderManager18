using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Service;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    class Counterparty
    {
        private int id;
        private int distance;
        private Int64 nip;
        private string name;

        public Counterparty(int id, long nip, string name, int distance)
        {
            this.id = id;
            this.distance = distance;
            this.nip = nip;
            this.name = name;
        }

        public int Id { get => id; set => id = value; }
        public int Distance { get => distance; set => distance = value; }
        public long Nip { get => nip; set => nip = value; }
        public string Name { get => name; set => name = value; }

        public override string ToString() { return id + " " + name; }
    }
}
