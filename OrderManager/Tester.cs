using OrderManager.DAL.ExternalSysDAO;
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
            

            //objToStr.Text = list.First().ToString();
            runGenerator();
            //objToStr.Text = runDiscountCounter();
            objToStr.Size = new Size(400, 100);

        }

        public void runGenerator()
        {

            IStockService stockService = DependencyInjector.IStockService;
            var stockList = stockService.GetAll();

            Dictionary<Domain.Entity.Stock, int> toOrder = new Dictionary<Domain.Entity.Stock, int>();
            foreach (var stock in stockList)
                toOrder.Add(stock, 10);

            var tranches = (new OrdersGenerator(toOrder,DependencyInjector.ICounterpartyService, 
                DependencyInjector.IPriorityService, DependencyInjector.ICounterpartysStockService,
                DependencyInjector.IStockService))
                .Generate();
        }

    }
}
