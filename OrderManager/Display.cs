using OrderManager.DAL.ExternalSysDAO;
using OrderManager.DAL.InternalSysDAO;
using OrderManager.DAO;
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
        /// Metoda zwraca referencje do siatki znajdującej się w oknie.
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
            PercentageDiscountDAO disc = new PercentageDiscountDAO();
            //disc.Add(dt);
            StockDAO stock = new StockDAO();
            PercentageDiscountDAO percentageDiscount = new PercentageDiscountDAO();
            //dataGridView1.DataSource = percentageDiscount.GetCounterpartysStockValidDicounts(
                //(new CounterpartysStock()).GetById("1"));
            //Show();

            DataTable trancheTable = new TrancheDAO().GetById("3");
            DataTable counterPartysStockTable = new CounterpartysStockDAO().GetById("1");
            ITrancheDAO trancheDAO = new TrancheDAO();
            ICounterpartysStockDAO counterpartysStockDAO = new CounterpartysStockDAO();
            var trancheStock = trancheDAO.GetCounterpartysStock(trancheTable.Rows[0]);
            DTO.CounterpartysStockMapper mapper = new DTO.CounterpartysStockMapper(counterpartysStockDAO);
            string res = string.Join(Environment.NewLine, trancheTable.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
            //Console.WriteLine(new DTO.TrancheMapper(trancheDAO).MapFrom(table));
            //Console.WriteLine(mapper.MapFrom(trancheStock).PriceNetto);
            
            IOrderDAO orderDAO = new OrderDAO();
            DataTable orderTable = orderDAO.GetById("1");
            DTO.OrderMapper orderMapper = new DTO.OrderMapper(orderDAO);
            Domain.Entity.Order orderEntity = orderMapper.MapFrom(orderTable);
            //Console.WriteLine(orderEntity);
            //Console.WriteLine(orderEntity.PriceNetto);
            //Console.WriteLine(orderEntity.PriceBrutto);
            var back = orderMapper.MapFrom(orderMapper.MapTo(orderEntity));
            //Console.WriteLine(orderEntity.ParentOrder);

            DataTable stockTable = new StockDAO().GetById("1");
            DTO.StockMapper stockMapper = new DTO.StockMapper();
            Console.WriteLine(stockMapper.MapFrom(stockTable).Category.Id);
        }
    }
}
