using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    public class PercentageDiscountDAO : ReaderAndWriterDAO, IPercentageDiscountDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PercentageDiscountDAO"/> class.
        /// </summary>
        public PercentageDiscountDAO() : base("RabatProcentowy") { }

        /// <summary>
        /// Gets the counterparties stock with given discount.
        /// </summary>
        /// <param name="discount">The discount.</param>
        /// <returns>Returns the DataTable of all counterparty stock with given discount.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the table is empty or invalid.</exception>
        public DataTable GetCounterpartiesStockWithDiscount(DataTable discount)
        {
            if (discount.Rows.Count != 1 && !discount.Columns.Contains("ID"))
                throw new ArgumentOutOfRangeException();
            return DBOperations.Query(
                @"SELECT * FROM TowarKontrahenta WHERE ID IN
                (SELECT TowarKontrahentaID FROM RabatProcentowy_TowarKontrahenta
                WHERE RabatProcentowyID = " + discount.Rows[0]["ID"].ToString() + ")");
        }

        /// <summary>
        /// Gets the counterparties stock with given discount..
        /// </summary>
        /// <param name="discount">The discount.</param>
        /// <returns>Returns the DataTable of all counterparty stock with given discount.</returns>
        public DataTable GetCounterpartiesStockWithDiscount(DataRow discount)
        {
            return DBOperations.Query(
                @"SELECT * FROM TowarKontrahenta WHERE ID IN
                (SELECT TowarKontrahentaID FROM RabatProcentowy_TowarKontrahenta
                WHERE RabatProcentowyID = " + discount["ID"].ToString() + ")");
        }
    }
}
