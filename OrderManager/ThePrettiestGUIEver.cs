using OrderManager.Domain;
using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using OrderManager.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManager
{
    public partial class ThePrettiestGUIEver : Form
    {
        private int filtersHeight;
        private IStockService stockService;
        private List<Domain.Entity.Stock> listStock;

        internal ThePrettiestGUIEver()
        {
            InitializeComponent();

            filtersHeight = 30;

            stockService = DependencyInjector.IStockService;
            listStock = stockService.GetAll();

            (new DataGridviewCheckBoxColumnProwider(dataGridViewStock)).addCheckBoxColumn();
            FillGridview(listStock);
            AddDataSourceForFilters();
            comboBoxState.SelectedIndexChanged += comboBoxState_SelectedIndexChanged;
            this.FormClosing += MainStockView_FormClosing;
        }

        private void AddDataSourceForFilters()
        {
            string[] comboBoxStateDataSource = { "Dowolny", "Ponizej minimum" };
            string[] comboBoxCategoryDataSource = { "Dowolna" };
            string[] comboBoxOrderedDataSource = { "Dowolnie", "W poprzedzim cyklu" };

            comboBoxState.DataSource = comboBoxStateDataSource;
            comboBoxCategory.DataSource = comboBoxCategoryDataSource;
            comboBoxOrdered.DataSource = comboBoxOrderedDataSource;
            comboBoxState.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxOrdered.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FillGridview(IEnumerable<Stock> listStock)
        {
            DataTable dataGridSource = new DataTable();

            dataGridSource.Columns.Add("Kod");
            dataGridSource.Columns.Add("Nazwa");
            dataGridSource.Columns.Add("Kategoria");
            dataGridSource.Columns.Add("Stan razem");
            dataGridSource.Columns.Add("Stan zamówień");
            dataGridSource.Columns.Add("Minimum magazynowe");
            dataGridSource.Columns.Add("Zamówienia indywidualne");

            foreach (var stock in listStock)
            {
                DataRow dataRow = dataGridSource.NewRow();
                dataRow["Kod"] = stock.Code;
                dataRow["Nazwa"] = stock.Name;
                dataRow["Kategoria"] = stock.Category.Name;
                dataRow["Minimum magazynowe"] = stock.MinInStockRoom;
                int numOfItemsInOrders = stockService.GetNumOfItemsInOrders(stock);
                dataRow["Stan zamówień"] = numOfItemsInOrders;
                dataRow["Stan razem"] = numOfItemsInOrders + stock.NumberOfItemsInStockRoom;
                dataGridSource.Rows.Add(dataRow);
            }

            dataGridViewStock.DataSource = dataGridSource;
            dataGridViewStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in dataGridViewStock.Columns)
                if (column.Index.Equals(7) || column.Index.Equals(0))
                    column.ReadOnly = false;
                else
                    column.ReadOnly = true;
        }

        private void InformAboutUnorderedStock(IEnumerable<Stock> stock)
        {
            StringBuilder message = new StringBuilder("Nie udało się wygenerować zamówień dla części wybranych towarów.");
            message.AppendLine("Nie wygenerowano zamówień dla towarów: ");
            foreach (Stock unorderedStock in stock)
                message.AppendLine(unorderedStock.Name);
            MessageBox.Show(message.ToString());
        }
        
        private int GetNumberOfItemsInIndividualOrdersColumn(DataGridViewRow row)
        {
            int number;
            return int.TryParse(row.Cells[7].Value.ToString(), out number) ? number : 0;
        }

        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxState.SelectedValue)
            {
                case "Dowolny": FillGridview(listStock); break;
                case "Ponizej minimum": FillGridview(listStock.Where(stock => stock.NumberOfItemsInStockRoom < stock.MinInStockRoom)); break;
            }
        }

        private void MainStockView_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void prepareFiltersPanel()
        {
            (tableLayoutPanelFilter.RowStyles)[1].SizeType = SizeType.Absolute;
            (tableLayoutPanelFilter.RowStyles)[1].Height = 0;
            (tableLayoutPanelContent.RowStyles)[1].Height = filtersHeight * 2;
        }
        
        private void pictureBoxFilter_MouseClick(object sender, MouseEventArgs e)
        {
            (tableLayoutPanelFilter.RowStyles)[1].SizeType = SizeType.Absolute;
            if ((tableLayoutPanelFilter.RowStyles)[1].Height == 0)
            {
                pictureBoxFilter.Image = Properties.Resources.arrowDown;
                (tableLayoutPanelFilter.RowStyles)[1].Height = filtersHeight;
                (tableLayoutPanelContent.RowStyles)[1].Height = filtersHeight * 3;
            }
            else
            {
                pictureBoxFilter.Image = Properties.Resources.arrow;
                (tableLayoutPanelFilter.RowStyles)[1].Height = 0;
                (tableLayoutPanelContent.RowStyles)[1].Height = filtersHeight * 2;
            }
        }

        private void buttonGenerateOrders_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Dictionary<Stock, int> stockToOrder = new Dictionary<Stock, int>();
                foreach (DataGridViewRow row in dataGridViewStock.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        Stock currentStock =
                            listStock.FirstOrDefault(stock => stock.Code.Equals(row.Cells[1].Value));
                        int numberOfItemsToOrder = stockService.GetNumOfItemsToOrder(currentStock)
                            + GetNumberOfItemsInIndividualOrdersColumn(row);
                        if (numberOfItemsToOrder > 0)
                            stockToOrder.Add(currentStock, numberOfItemsToOrder);
                    }
                }
                List<Order> orders = (new OrdersGenerator(stockToOrder, DependencyInjector.ICounterpartyService,
                    DependencyInjector.IPriorityService, DependencyInjector.ICounterpartysStockService,
                    DependencyInjector.IStockService, DependencyInjector.IEligibleOrdersNamesService)).Generate();
                var orderedStock = new HashSet<Stock>(orders.Select(order => order.Tranches).SelectMany(i => i).Select(tranche => tranche.Stock.Stock));
                var unorderedStock = (stockToOrder.Keys).Except(orderedStock);
                if (unorderedStock.Count() != 0)
                    InformAboutUnorderedStock(unorderedStock);
                if (orders.Count == 0)
                    MessageBox.Show("Brak wygenerowanych zamówień.");
                else
                    (new GeneratedOrdersView(orders)).Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Nie udało się wygenerować zamówień." + Environment.NewLine + exception.ToString());
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var ordersForm = new OrdersMainView();
            ordersForm.Closed += (s, args) => this.Close();
            ordersForm.Show();
        }

        private void ThePrettiestGUIEver_MouseClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz zamknąć to okno?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Close();
        }

        private void ThePrettiestGUIEver_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(((Form) sender).Visible)
            {
                if (MessageBox.Show("Czy chcesz zamknąć to okno?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
                else
                    e.Cancel = false;
            }
        }
    }
}
