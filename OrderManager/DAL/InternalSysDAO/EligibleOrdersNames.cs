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
        public EligibleOrdersNames() : base("WolneNazwyZamowien") { }

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
