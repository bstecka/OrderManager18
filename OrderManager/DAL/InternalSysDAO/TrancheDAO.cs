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
    class TrancheDAO : ReaderAndWriterDAO, ITrancheDAO
    {
        public TrancheDAO() : base("Transza") { }

        public void AssignDiscountToTranche(DataRow tranche, DataRow discount)
        {
            DBOperations.Query(@"INSERT INTO Transza_RabatProcentowy (TranszaID, RabatProcentowyID) VALUES (" + tranche["ID"] +", " + discount["ID"] + ")");
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

        public DataTable GetViableDiscounts(DataRow tranche)
        {
            return DBOperations.Query(@"SELECT * FROM RabatProcentowy JOIN RabatProcentowy_TowarKontrahenta
                ON RabatProcentowy.ID = RabatProcentowy_TowarKontrahenta.RabatProcentowyID
                WHERE TowarKontrahentaID IN (" + tranche["TowarKontrahentaID"] + ")");
        }
    }
}
