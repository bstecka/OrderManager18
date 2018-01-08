﻿using OrderManager.DAL.ExternalSysDAO;
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
    public partial class MainOrdersView : Form
    {
        private IOrderService orderService;
        private List<Order> listOrder;

        internal MainOrdersView(IOrderService orderService)
        {
            InitializeComponent();
            this.orderService = orderService;
            listOrder = orderService.GetAllDuringRealization();
            (new DataGridviewCheckBoxColumnProwider(dataGridViewStock)).addCheckBoxColumn();

            fillGridview(listOrder);
            addDataSourceForFilters();
            this.FormClosing += MainOrdersView_FormClosing;
        }

        private void MainOrdersView_Load(object sender, EventArgs e)
        {

        }

        private void addDataSourceForFilters()
        {
            string[] comboBoxStateDataSource = {"W trakcie realizacji", "Zrealizowane", "Anulowane", "W trakcie reklamacji"};
            comboBoxState.DataSource = comboBoxStateDataSource;
            comboBoxState.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        
        private void fillGridview(IEnumerable<Order> listOrder)
        {
            DataTable dataGridSource = new DataTable();
            dataGridSource.Columns.Add("Nazwa");
            dataGridSource.Columns.Add("Kontrahent");
            dataGridSource.Columns.Add("Status");
            dataGridSource.Columns.Add("Data złożenia");
            dataGridSource.Columns.Add("Suma wart. poz. Netto");
            dataGridSource.Columns.Add("Suma wart. poz. Brutto");

            foreach (var order in listOrder)
            {
                DataRow dataRow = dataGridSource.NewRow();

                dataRow["Nazwa"] = order.Name;
                dataRow["Kontrahent"] = order.Counterparty;
                dataRow["Status"] = order.State;
                dataRow["Data złożenia"] = order.DateOfCreation;
                dataRow["Suma wart. poz. Netto"] = order.PriceNetto;
                dataRow["Suma wart. poz. Brutto"] = order.PriceBrutto;
                dataGridSource.Rows.Add(dataRow);
            }

            dataGridViewStock.DataSource = dataGridSource;
            dataGridViewStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in dataGridViewStock.Columns)
                if (column.Index.Equals(0))
                    column.ReadOnly = false;
                else
                    column.ReadOnly = true;
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

        /*private void buttonGenerateOrders_Click(object sender, EventArgs e)
        {
            Dictionary<Stock, int> stockToOrder = new Dictionary<Stock, int>();
            foreach(DataGridViewRow row in dataGridViewStock.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    Stock currentStock =
                        listStock.FirstOrDefault(stock => stock.Code.Equals(row.Cells[1].Value));
                    int numberOfItemsToOrder = stockService.GetNumOfItemsToOrder(currentStock) 
                        + getNumberOfItemsInIndividualOrdersColumn(row);
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
                informAboutUnorderedStock(unorderedStock);
            if (orders.Count == 0)
                MessageBox.Show("Brak wygenerowanych zamówień.");
            else
                (new GeneratedOrders(orders)).Show();
        }*/

        private void informAboutUnorderedStock(IEnumerable<Stock> stock)
        {
            StringBuilder message = new StringBuilder("Nie udało się wygenerować zamówień dla części wybranych towarów."); 
            message.AppendLine("Nie wygenerowano zamówień dla towarów: "); 
            foreach (Stock unorderedStock in stock)
                message.AppendLine(unorderedStock.Name);
            MessageBox.Show(message.ToString());
        }

        private int getNumberOfItemsInIndividualOrdersColumn(DataGridViewRow row)
        {
            int number;
            return int.TryParse(row.Cells[7].Value.ToString(), out number) ? number : 0;
        }

        private void tableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, e.CellBounds.Location, new Point(e.CellBounds.Right, e.CellBounds.Top));
        }

        private void MainOrdersView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz zamknąć to okno?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void setButtonsForOrdersDuringRealization(bool value)
        {
            button3.Enabled = value;
            button4.Enabled = value;
        }

        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Order> orders = listOrder;
            setButtonsForOrdersDuringRealization(false);
            switch (comboBoxState.SelectedValue)
            {
                case "W trakcie realizacji": orders = orderService.GetAllDuringRealization(); setButtonsForOrdersDuringRealization(true); break;
                case "Zrealizowane": orders = orderService.GetAllByState(4); break;
                case "Anulowane": orders = orderService.GetAllByState(3); break;
                case "W trakcie reklamacji": orders = orderService.GetAllByState(2); break;
            }
            fillGridview(orders);
        }
    }
    
}