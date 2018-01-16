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
        /// <summary>
        /// Initializes a new instance of the <see cref="TrancheDAO"/> class.
        /// </summary>
        public TrancheDAO() : base("Transza") { }

        /// <summary>
        /// Assigns the discount to tranche in the database.
        /// </summary>
        /// <param name="tranche">The tranche.</param>
        /// <param name="discount">The discount.</param>
        public void AssignDiscountToTranche(DataRow tranche, DataRow discount)
        {
            DBOperations.Query(@"INSERT INTO Transza_RabatProcentowy (TranszaID, RabatProcentowyID) VALUES (" + tranche["ID"] +", " + discount["ID"] + ")");
        }

        /// <summary>
        /// Gets the counterpartys stock for the given tranche.
        /// </summary>
        /// <param name="tranche">The tranche.</param>
        /// <returns>Returns the DataTable of counterpartys stock for the given tranche.</returns>
        public DataTable GetCounterpartysStock(DataRow tranche)
        {
            return DBOperations.Query(@"SELECT * FROM TowarKontrahenta WHERE ID = " + tranche["TowarKontrahentaID"] + " AND KontrahentID IN (SELECT KontrahentID FROM Transza FULL JOIN Zamowienie ON Transza.ZamowienieID = Zamowienie.ID WHERE TRANSZA.ID = " + tranche["ID"] + " )");
        }

        /// <summary>
        /// Gets the percentage discounts assigned to the given tranche.
        /// </summary>
        /// <param name="tranche">The tranche.</param>
        /// <returns>Returns the DataTable of percentage discounts assigned to the given tranche.</returns>
        public DataTable GetPercentageDiscounts(DataRow tranche)
        {
            return DBOperations.Query(@"SELECT * FROM Transza_RabatProcentowy JOIN RabatProcentowy ON 
                Transza_RabatProcentowy.RabatProcentowyID = RabatProcentowy.ID
                WHERE TranszaID IN (" + tranche["ID"] + ")");
        }

        /// <summary>
        /// Gets the viable discounts for the given tranche.
        /// </summary>
        /// <param name="tranche">The tranche.</param>
        /// <returns>Returns the DataTable of viable discounts for the given tranche.</returns>
        public DataTable GetViableDiscounts(DataRow tranche)
        {
            return DBOperations.Query(@"SELECT * FROM RabatProcentowy JOIN RabatProcentowy_TowarKontrahenta
                ON RabatProcentowy.ID = RabatProcentowy_TowarKontrahenta.RabatProcentowyID
                WHERE TowarKontrahentaID IN (" + tranche["TowarKontrahentaID"] + ")");
        }
    }
}
