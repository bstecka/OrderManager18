using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    class EligibleOrdersNames : ReaderAndWriterDAO, IEligibleOrdersNamesDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EligibleOrdersNames"/> class.
        /// </summary>
        public EligibleOrdersNames() : base("WolneNazwyZamowien") { }

        /// <summary>
        /// Fetches the eligible names for orders from the database, the amount is equal to specified numberOfNames.
        /// </summary>
        /// <param name="numberOfNames">The number of names.</param>
        /// <returns>Returns the DataTable of eligible names of orders.</returns>
        public DataTable FetchNames(int numberOfNames)
        {
            return DBOperations.Query(@"
            SELECT Nazwa FROM(
            SELECT Nazwa, DENSE_RANK() OVER (ORDER BY Nazwa) AS miejsce FROM WolneNazwyZamowien) 
            AS UszeregowaneNazwy
            WHERE UszeregowaneNazwy.miejsce <= " + numberOfNames);
        }
    }
}
