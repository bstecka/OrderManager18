using OrderManager.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManager.Presentation
{
    public partial class GeneratedOrders : Form
    {
        private List<Order> orders;

        internal GeneratedOrders(List<Order> orders)
        {
            InitializeComponent();
            labelTitle.Text = "Wygenerowane zamówienia z dnia " + DateTime.Now.Date.ToString("dd/MM/yyyy");
            this.orders = orders;
            fillDataGridView();
        }
        
        private void fillDataGridView()
        {
            DataTable dataGridSource = new DataTable();
            (new DataGridviewCheckBoxColumnProwider(dataGridViewOrders)).addCheckBoxColumn();

            dataGridSource.Columns.Add("Nazwa");
            dataGridSource.Columns.Add("Kontrahent");
            dataGridSource.Columns.Add("Wartość netto");
            dataGridSource.Columns.Add("Wartość brutto");
            foreach (var order in orders)
            {
                DataRow dataRow = dataGridSource.NewRow();
                dataRow["Nazwa"] = order.Name;
                dataRow["Kontrahent"] = order.Counterparty.Name;
                dataRow["Wartość netto"] = Math.Round(order.PriceNetto, 2);
                dataRow["Wartość brutto"] = Math.Round(order.PriceBrutto, 2);
                dataGridSource.Rows.Add(dataRow);
            }
            dataGridViewOrders.DataSource = dataGridSource;
            dataGridViewOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
