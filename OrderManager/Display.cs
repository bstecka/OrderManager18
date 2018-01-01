using OrderManager.DAL.InternalSysDAO;
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
            Show();
        }
    }
}
