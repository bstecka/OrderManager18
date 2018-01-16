using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
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
    public partial class GeneratedOrdersView : Form
    {
        private List<Order> orders;
        private IStockService stockService;
        private List<GeneratedOrderView> displayedOrders;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedOrdersView"/> class.
        /// </summary>
        /// <param name="orders">The orders to be displayed.</param>
        internal GeneratedOrdersView(List<Order> orders)
        {
            InitializeComponent();
            labelTitle.Text = "Wygenerowane zamówienia z dnia " + DateTime.Now.Date.ToString("dd/MM/yyyy");
            this.orders = orders;
            displayedOrders = new List<GeneratedOrderView>();
            (new DataGridviewCheckBoxColumnProwider(dataGridViewOrders)).addCheckBoxColumn();
            FillDataGridView();
            foreach (DataGridViewColumn column in dataGridViewOrders.Columns)
                if (column.Index.Equals(0))
                    column.ReadOnly = false;
                else
                    column.ReadOnly = true;
            stockService = DependencyInjector.IStockService;
            this.FormClosing += GeneratedOrders_FormClosing;
        }

        /// <summary>
        /// Fills the data grid view with the data of orders.
        /// </summary>
        private void FillDataGridView()
        {
            DataTable dataGridSource = new DataTable();

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

        /// <summary>
        /// Handles the click event of the buttonEdit_Click control. Opens the GeneratedOrderView displaying the
        /// details of a single generated order.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonEdit_Click_1(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewOrders.Rows)
            {
                var tmp1 = Convert.ToBoolean(row.Cells[0].Value);
                var tmp = orders.FirstOrDefault(
                        order => order.Name.Equals(row.Cells["Nazwa"].Value.ToString()));
                if (Convert.ToBoolean(row.Cells[0].Value) && tmp != null)
                {
                    var ordersForm = (new GeneratedOrderView(orders.FirstOrDefault(
                        order => order.Name.Equals(row.Cells["Nazwa"].Value.ToString()))));
                    displayedOrders.Add(ordersForm);
                    ordersForm.Show();
                }
                   }
        }

        /// <summary>
        /// Handles the FormClosing event of the GeneratedOrders control. Asks for confirmation to close the form,
        /// if confirmed closes the form and all children forms.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void GeneratedOrders_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((Form)sender).Visible)
            {
                if (MessageBox.Show("Czy chcesz zamknąć to okno?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
                else
                {
                    foreach (Stock stock in orders.Select
                        (o => o.Tranches.Select(t => t.Stock.Stock)).SelectMany(i => i))
                        stockService.SetPossibilityToGenerateOrder(stock.Id, 0);
                    foreach(var child in displayedOrders)
                        child.Close();
                    e.Cancel = false;
                }
            }
        }
    }
}
