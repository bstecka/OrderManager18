using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManager.DAL.InternalSysDAO;
using OrderManager.DAO;

namespace OrderManager.DAL.InternalSysDAO
{
    class Tranche : ReaderDAO, ITrancheDAO
    {
        public Tranche() : base("Transza") { }

        public void Add(DataTable entity)
        {
            throw new NotImplementedException();
        }

        public DataTable GetCounterpartysStock(DataRow tranche)
        {
            return DBOperations.Query(@"SELECT * FROM TowarKontrahenta 
                WHERE TowarID IN (" + tranche["TowarKontrahentaID"] + ")");
        }

        public DataTable GetPercentageDiscounts(DataRow tranche)
        {
            return DBOperations.Query(@"SELECT * FROM Transza_RabatProcentowy JOIN RabatProcentowy ON 
                Transza_RabatProcentowy.RabatProcentowyID = RabatProcentowy.ID
                WHERE TranszaID IN (" + tranche["ID"] + ")");
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(DataTable entity)
        {
            throw new NotImplementedException();
        }
    }
}
