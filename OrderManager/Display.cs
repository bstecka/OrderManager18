using OrderManager.DAL.InternalSysDAO;
using OrderManager.DAO;
using OrderManager.ExternalSysDAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManager
{
    public partial class Display : Form
    {

        public Display()
        {
        }
        /// <summary>
        /// Metooda zwraca referencje do siatki znajdującej się w oknie.
        /// </summary>
        /// <returns>siatka</returns>
        public DataGridView getDataGridView() { return dataGridView1; }

        
        public void DisplayTables()
        {
            dataGridView1 = new DataGridView();
            this.Controls.Add(dataGridView1);
            DataTable dt = new DataTable();
            dt.Columns.Add("Odleglosc", typeof(String));
            dt.Columns.Add("Nazwa", typeof(String));
            dt.Columns.Add("NIP", typeof(String));

            DataRow dr = dt.NewRow();
            dr["Odleglosc"] = "2";
            dr["Nazwa"] = "Apart";
            dr["NIP"] = "2221243";
            dt.Rows.Add(dr);
            PercentageDiscount disc = new PercentageDiscount();
            //disc.Add(dt);
            Stock stock = new Stock();
            PercentageDiscount percentageDiscount = new PercentageDiscount();
            dataGridView1.DataSource = percentageDiscount.GetCounterpartysStockValidDicounts(
                (new CounterpartysStock()).GetById("1"));
            //Show();

            DataTable trancheTable = new Tranche().GetById("1");
            DataTable counterPartysStockTable = new CounterpartysStock().GetById("1");
            ITrancheDAO trancheDAO = new Tranche();
            ICounterpartysStockDAO counterpartysStockDAO = new CounterpartysStock();
            var trancheStock = trancheDAO.GetCounterpartysStock(trancheTable.Rows[0]);
            DTO.CounterpartysStockMapper mapper = new DTO.CounterpartysStockMapper(counterpartysStockDAO);
            string res = string.Join(Environment.NewLine, trancheTable.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
            //Console.WriteLine(new DTO.TrancheMapper(trancheDAO).MapFrom(table));
            //Console.WriteLine(mapper.MapFrom(trancheStock));
            
            IOrderDAO orderDAO = new Order();
            DataTable orderTable = orderDAO.GetById("22");
            DTO.OrderMapper orderMapper = new DTO.OrderMapper(orderDAO);
            Console.WriteLine(orderMapper.MapFrom(orderTable));
        }
    }
}
