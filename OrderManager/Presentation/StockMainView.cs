using OrderManager.Domain;
using OrderManager.Domain.Entity;
using OrderManager.Domain.OrderGenerator;
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
    public partial class StockMainView : Form
    {
        private int filtersHeight;
        private IStockService stockService;
        private IOrdersGenerator ordersGenerator;
        private List<Domain.Entity.Stock> listStock;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockMainView"/> class.
        /// </summary>
        internal StockMainView()
        {
            InitializeComponent();

            filtersHeight = 30;

            stockService = DependencyInjector.IStockService;
            ordersGenerator = DependencyInjector.IOrdersGenerator;

            try
            {
                listStock = stockService.GetAll();

                (new DataGridviewCheckBoxColumnProwider(dataGridViewStock)).addCheckBoxColumn();
                FillGridview(listStock);
                AddDataSourceForFilters();
                comboBoxState.SelectedIndexChanged += comboBoxState_SelectedIndexChanged;
                this.FormClosing += MainStockView_FormClosing;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Nie udało się pobrać danych towarów. Sprawdź połączenie z bazą danych.");
            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpił nieprzewidziany błąd. Skontaktuj się z twórcami oprogramowania.");
            }
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns></returns>
        private string[] getCategories()
        {
            HashSet<string> categories = new HashSet<string>(listStock.Select(s => s.Category.Name));
            categories.Add("Dowolna");
            return categories.ToArray();
        }

        /// <summary>
        /// Adds the data source for filters for stock category, current supply in stockroom and last time ordered.
        /// </summary>
        private void AddDataSourceForFilters()
        {
            string[] comboBoxStateDataSource = { "Dowolny", "Ponizej minimum" };
            string[] comboBoxCategoryDataSource = getCategories();
            string[] comboBoxOrderedDataSource = { "Dowolnie", "W poprzedzim cyklu" };

            comboBoxState.DataSource = comboBoxStateDataSource;
            comboBoxCategory.DataSource = comboBoxCategoryDataSource;
            comboBoxOrdered.DataSource = comboBoxOrderedDataSource;
            comboBoxState.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxOrdered.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCategory.SelectedIndexChanged += comboBoxCategory_SelectedIndexChanged;
        }

        /// <summary>
        /// Fills the gridview with the data of stock.
        /// </summary>
        /// <param name="listStock">The list stock.</param>
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
                dataRow["Minimum magazynowe"] = stock.MinInStockRoom;
                int numOfItemsInOrders = stockService.GetNumOfItemsInOrders(stock);
                dataRow["Stan zamówień"] = numOfItemsInOrders;
                dataRow["Stan razem"] = numOfItemsInOrders + stock.NumberOfItemsInStockRoom;
                dataRow["Kategoria"] = stock.Category.Name;
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

        /// <summary>
        /// Informs about unordered stock in case of inability to create orders.
        /// </summary>
        /// <param name="stock">The collection of unordered stock.</param>
        /// <param name="extraMessage">The extra message.</param>
        private void InformAboutUnorderedStock(IEnumerable<Stock> stock, String extraMessage)
        {
            StringBuilder message = new StringBuilder("Nie udało się wygenerować zamówień dla części wybranych towarów.");
            message.AppendLine("Nie wygenerowano zamówień dla towarów: ");
            foreach (Stock unorderedStock in stock)
                message.AppendLine(unorderedStock.Name);
            if (extraMessage != null)
                message.AppendLine(extraMessage);
            foreach (Stock s in stock)
                stockService.SetPossibilityToGenerateOrder(s.Id, 0);
            MessageBox.Show(message.ToString());
        }

        /// <summary>
        /// Gets the number of items in individual orders column.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        private int GetNumberOfItemsInIndividualOrdersColumn(DataGridViewRow row)
        {
            int number;
            if (!int.TryParse(row.Cells[7].Value.ToString(), out number) && number < 0)
            {
                number = 0;
                row.Cells[7].Value = number;
            }
            row.Cells[7].Tag = number.ToString();
            return number;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxState control. Fills the dataGridView with the stock
        /// corresponding to the number of items in stockroom corresponding to the variant chosen from the filter.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxState.SelectedValue)
            {
                case "Dowolny": FillGridview(listStock); break;
                case "Ponizej minimum": FillGridview(listStock.Where
                    (stock => stock.NumberOfItemsInStockRoom + stockService.GetNumOfItemsInOrders(stock)
                    < stock.MinInStockRoom)); break;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxCategory control. Fills the dataGridView with the stock
        /// corresponding to the category chosen from the filter.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCategory.SelectedValue.Equals("Dowolna"))
                FillGridview(listStock);
            else
                FillGridview(listStock.Where(s => s.Category.Name.Equals(comboBoxCategory.SelectedValue)));
        }

        private void MainStockView_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        /// <summary>
        /// Sets visual properties of the filters panel.
        /// </summary>
        private void prepareFiltersPanel()
        {
            (tableLayoutPanelFilter.RowStyles)[1].SizeType = SizeType.Absolute;
            (tableLayoutPanelFilter.RowStyles)[1].Height = 0;
            (tableLayoutPanelContent.RowStyles)[1].Height = filtersHeight * 2;
        }

        /// <summary>
        /// Handles the MouseClick event of the pictureBoxFilter control. Opens up the filters.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Gets the stock to order.
        /// </summary>
        /// <returns>Returns a tuple of disctionary and list of stock.</returns>
        private Tuple<Dictionary<Stock, int>, List<Stock>> getStockToOrder()
        {
            Dictionary<Stock, int> stockToOrder = new Dictionary<Stock, int>();
            List<Stock> blockedStock = new List<Stock>();
            foreach (DataGridViewRow row in dataGridViewStock.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    Stock currentStock =
                        listStock.FirstOrDefault(stock => stock.Code.Equals(row.Cells[1].Value));
                    int numberOfItemsToOrder = stockService.GetNumOfItemsToOrder(currentStock)
                        + GetNumberOfItemsInIndividualOrdersColumn(row);
                    if (numberOfItemsToOrder > 0)
                        if (currentStock.InGeneratedOrders ||
                        !stockService.SetPossibilityToGenerateOrder(currentStock.Id, 1))
                            blockedStock.Add(currentStock);
                        else
                            stockToOrder.Add(currentStock, numberOfItemsToOrder);
                }
            }

            return new Tuple<Dictionary<Stock, int>, List<Stock>>(stockToOrder, blockedStock);
        }


        /// <summary>
        /// Handles the MouseClick event of the buttonGenerateOrders control. Generates orders for selected stock, 
        /// with counterparties chosen according to the set main priority, or specified priority for a single stock.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void ButtonGenerateOrders_MouseClick(object sender, MouseEventArgs e)
        {
            listStock = stockService.GetAll();
            try
            {
                var selectedStock = getStockToOrder();
                Dictionary<Stock, int> stockToOrder = selectedStock.Item1;
                var stockToOrderCopy = stockToOrder.Select(s => s.Key).ToList();
                List<Stock> blockedStock = selectedStock.Item2;
                List<Order> orders = ordersGenerator.Generate(stockToOrder);
                var orderedStock = new HashSet<Stock>(orders.Select(order => order.Tranches).SelectMany(i => i).Select(tranche => tranche.Stock.Stock));
                var unorderedStock = (stockToOrderCopy).Except(orderedStock);
                if (blockedStock.Count() != 0)
                    InformAboutUnorderedStock(blockedStock, "Inny pracownik jest w trakcie generowania zamówienia na te towary.");
                if (unorderedStock.Count() != 0)
                    InformAboutUnorderedStock(unorderedStock, "Żaden z kontrahentów nie ma w ofercie tych towarów.");
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

        /// <summary>
        /// Handles the LinkClicked event of the LinkLabel2 control. Closes the form and switches to the main view containing orders.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var ordersForm = new OrdersMainView();
            ordersForm.Closed += (s, args) => this.Close();
            ordersForm.Show();
        }

        /// <summary>
        /// Handles the MouseClick event of the StockMainView control. After confirmation from a message box, quits the Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void StockMainView_MouseClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz zamknąć to okno?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Close();
        }

        /// <summary>
        /// Handles the FormClosing event of the StockMainView control. Asks user to confirm if they want to quit.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void StockMainView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(((Form) sender).Visible)
            {
                if (MessageBox.Show("Czy chcesz zamknąć to okno?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
                else
                    e.Cancel = false;
            }
        }

        private void dataGridViewStock_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }
    }
}
