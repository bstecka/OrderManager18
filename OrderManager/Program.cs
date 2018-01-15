using OrderManager.DAL.ExternalSysDAO;
using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Service;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManager.Presentation
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ThePrettiestGUIEver());
            //Application.Run(new Tester());

            //Application.Run(new MainOrdersView(DependencyInjector.IOrderService, DependencyInjector.ITrancheService));
            //Application.Run(new MainStockView(DependencyInjector.IStockService));
        }
    }
}
