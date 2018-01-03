using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain;
using OrderManager.Domain.Service;
using OrderManager.DTO;
using OrderManager.ExternalSysDAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManager
{
    public partial class Tester : Form
    {
        public Tester()
        {
            InitializeComponent();
            Label objToStr = new Label();


            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(String));/*
            dt.Columns.Add("Odleglosc", typeof(String));
            dt.Columns.Add("Nazwa", typeof(String));
            dt.Columns.Add("NIP", typeof(String));*/

            DataRow dr = dt.NewRow();
            dr["ID"] = "2";/*
            dr["Odleglosc"] = "2";
            dr["Nazwa"] = "Apart";
            dr["NIP"] = "2221243";*/
            dt.Rows.Add(dr);

            objToStr.Text = "";

            this.Controls.Add(objToStr);/*
            CounterpartyMapper map = new CounterpartyMapper(new ExternalSysDAO.Counterparty());
            DataTable all = new ExternalSysDAO.Counterparty().GetAll();
            List<Domain.Entity.Counterparty> list = map.MapAllFrom(all);
            foreach(Domain.Entity.Counterparty elem in list)
                objToStr.Text +=elem.ToString();*/
            CounterpartysStockService service = new CounterpartysStockService(
                new DAL.InternalSysDAO.CounterpartysStock(), 
                new CounterpartysStockMapper(new DAL.InternalSysDAO.CounterpartysStock()));
            var list = service.GetAll().First().ValidDiscounts;



            //objToStr.Text = list.First().ToString();

            objToStr.Text = runDiscountCounter();
            objToStr.Size = new Size(400, 100);

        }

        public string runDiscountCounter()
        {
            /* StockService stockService = new StockService(
                 new Stock(), new StockMapper());
             var stockList = stockService.GetAll();

             Dictionary<Domain.Entity.Stock, int> toOrder = new Dictionary<Domain.Entity.Stock, int>();
             foreach (var stock in stockList)
                 toOrder.Add(stock, 10);
             DiscountCounter discountCounter = new DiscountCounter(toOrder);

             var parcels = discountCounter.BestChosenDiscounts();

             string result = "";
             foreach (var parcel in parcels)
                 result = result + parcel.ToString();
             return result;*/
            return "";
        }
    }
}
