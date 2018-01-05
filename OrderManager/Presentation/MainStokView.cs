using OrderManager.DAL.ExternalSysDAO;
using OrderManager.Domain;
using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using OrderManager.DTO;
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
    public partial class MainStokView : Form
    {
        private IStockService stockService;
        private List<Domain.Entity.Stock> listStock;

        internal MainStokView(IStockService stockService)
        {
            InitializeComponent();
            //this.stockService = new StockService(new Stock(), new StockMapper());
            this.stockService = stockService;
        }

        private void MainStokView_Load(object sender, EventArgs e)
        {
            DataTable dataGridSource = new DataTable();
            listStock = stockService.GetAll();

            addCheckBoxColumn();

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
                dataRow["Stan zamówień"] = stockService.GetNumOfItemsInOrders(stock);
                dataGridSource.Rows.Add(dataRow);
            }

            dataGridViewStock.DataSource = dataGridSource;

            addDataSourceForFilters();
        }

        private void addDataSourceForFilters()
        {

            string[] comboBoxStateDataSource = { "Dowolny", "Ponizej minimum" };
            string[] comboBoxCategoryDataSource = { "Dowolna" };
            string[] comboBoxOrderedDataSource = { "Dowolnie", "W poprzedzim cyklu" };

            comboBoxState.DataSource = comboBoxStateDataSource;
            comboBoxCategory.DataSource = comboBoxCategoryDataSource;
            comboBoxOrdered.DataSource = comboBoxOrderedDataSource;
        }
        

        private void addCheckBoxColumn()
        {

            var list = dataGridViewStock;
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.Width = 30;
            checkboxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            list.Columns.Insert(0, checkboxColumn);

            // add checkbox header
            Rectangle rect = list.GetCellDisplayRectangle(0, -1, true);
            // set checkbox header to center of header cell. +1 pixel to position correctly.
            rect.X = rect.Location.X + (rect.Width / 4);

            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(18, 18);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);

            list.Controls.Add(checkboxHeader);
        }

        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            var list = dataGridViewStock;
            for (int i = 0; i < list.RowCount; i++)
            {
                list[0, i].Value = ((CheckBox)list.Controls.Find("checkboxHeader", true)[0]).Checked;
            }
            list.EndEdit();
        }

        private void dataGridViewStock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonGenerateOrders_Click(object sender, EventArgs e)
        {
            Dictionary<Stock, int> stockToOrder = new Dictionary<Stock, int>();
            foreach(DataGridViewRow row in dataGridViewStock.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    Stock currentStock =
                        listStock.FirstOrDefault(stock => stock.Code.Equals(row.Cells[1].Value));
                    int numberOfItemsToOrder = (30 + currentStock.MinInStockRoom) - currentStock.NumberOfItemsInStockRoom
                        - stockService.GetNumOfItemsInOrders(currentStock);
                    if (numberOfItemsToOrder > 0)
                        stockToOrder.Add(currentStock, numberOfItemsToOrder);
                }
            }

            (new OrdersGenerator(stockToOrder, DependencyInjector.ICounterpartyService,
                DependencyInjector.IPriorityService, DependencyInjector.ICounterpartysStockService,
                DependencyInjector.IStockService, DependencyInjector.IEligibleOrdersNamesService)).Generate();
        }

        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBoxState.SelectedValue)
            {
                case "Dowolny": label1.Text = "Dowolny"; break;
                case "Ponizej minimum": label1.Text = "";break;
            }
        }
    }
    
}
