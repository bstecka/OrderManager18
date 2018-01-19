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
    public partial class OrdersMainView : Form
    {
        private IOrderService orderService;
        private ITrancheService trancheService;
        private List<Order> listOrder;
        private int filtersHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersMainView"/> class.
        /// </summary>
        internal OrdersMainView()
        {
            InitializeComponent();
            filtersHeight = 30;
            orderService = DependencyInjector.IOrderService;
            trancheService = DependencyInjector.ITrancheService;
            try
            {
                listOrder = orderService.GetAllDuringRealization();
                (new DataGridviewCheckBoxColumnProwider(dataGridViewOrders)).addCheckBoxColumn();

                FillGridview(listOrder);
                AddDataSourceForFilters();
                comboBoxState.SelectedIndexChanged += comboBoxState_SelectedIndexChanged;

                this.FormClosing += MainOrdersView_FormClosing;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Nie udało się pobrać danych zamówień. Sprawdź połączenie z bazą danych.");
            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpił nieprzewidziany błąd. Skontaktuj się z twórcami oprogramowania.");
            }
        }

        private void MainOrdersView_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Adds the data source for the filters enabling to filter orders by state.
        /// </summary>
        private void AddDataSourceForFilters()
        {
            string[] comboBoxStateDataSource = { "W trakcie realizacji", "Zrealizowane", "Anulowane", "W trakcie reklamacji" };
            comboBoxState.DataSource = comboBoxStateDataSource;
            comboBoxCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxState.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxOrdered.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Fills the gridview with the data of orders.
        /// </summary>
        /// <param name="listOrder">The list of orders.</param>
        private void FillGridview(IEnumerable<Order> listOrder)
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
                dataRow["Status"] = GetTranslatedOrderStateString(order.State);
                dataRow["Data złożenia"] = order.DateOfCreation.ToShortDateString();
                dataRow["Suma wart. poz. Netto"] = Math.Round(order.PriceNetto, 2).ToString();
                dataRow["Suma wart. poz. Brutto"] = Math.Round(order.PriceBrutto, 2).ToString();
                dataGridSource.Rows.Add(dataRow);
            }

            dataGridViewOrders.DataSource = dataGridSource;
            dataGridViewOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in dataGridViewOrders.Columns)
                if (column.Index.Equals(0))
                    column.ReadOnly = false;
                else
                    column.ReadOnly = true;
        }

        /// <summary>
        /// Adds the CheckBox column to the dataGridView containing order data. 
        /// </summary>
        private void AddCheckBoxColumn()
        {
            var list = dataGridViewOrders;
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.Width = 30;
            checkboxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            list.Columns.Insert(0, checkboxColumn);

            Rectangle rect = list.GetCellDisplayRectangle(0, -1, true);
            rect.X = rect.Location.X + (rect.Width / 4);

            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(18, 18);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(CheckboxHeader_CheckedChanged);

            list.Controls.Add(checkboxHeader);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkboxHeader control. Updates the state of checkboxes
        /// corresponding to the state of the checkbox in the header.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CheckboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            var list = dataGridViewOrders;
            for (int i = 0; i < list.RowCount; i++)
            {
                list[0, i].Value = ((CheckBox)list.Controls.Find("checkboxHeader", true)[0]).Checked;
            }
            list.EndEdit();
        }

        /// <summary>
        /// Handles the FormClosing event of the OrderCorrectionView control. Asks for confirmation to close the form,
        /// if the changes were not saved.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void MainOrdersView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((Form)sender).Visible)
            {
                if (MessageBox.Show("Czy chcesz zamknąć to okno?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
                else
                    e.Cancel = false;
            }
        }

        /// <summary>
        /// Enables or disables the buttons corresponding to actions that can be performed on orders that are currently
        /// under realization.
        /// </summary>
        /// <param name="value">value determining whether buttons should be enabled or disabled</param>
        private void SetButtonsForOrdersDuringRealization(bool value)
        {
            button3.Enabled = value;
            button4.Enabled = value;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxState control. Changes the content of the order
        /// dataGridView to correspond to the state chosen in the state filter.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Order> orders = listOrder;
            SetButtonsForOrdersDuringRealization(false);
            switch (comboBoxState.SelectedValue)
            {
                case "W trakcie realizacji": orders = orderService.GetAllDuringRealization(); SetButtonsForOrdersDuringRealization(true); break;
                case "Zrealizowane": orders = orderService.GetAllByState(4); break;
                case "Anulowane": orders = orderService.GetAllByState(3); break;
                case "W trakcie reklamacji": orders = orderService.GetAllByState(2); break;
            }
            FillGridview(orders);
        }

        /// <summary>
        /// Handles the LinkClicked event of the linkLabel1 control. Closes the form and switches to the main view containing stock.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var stockForm = new StockMainView();
            stockForm.Closed += (s, args) => this.Close();
            stockForm.Show();
        }

        /// <summary>
        /// Handles the click event of the button3 control. Opens a form enabling to correct the details of the order
        /// for all currently selected orders.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button3_Click_1(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewOrders.Rows)
            {
                var tmp1 = Convert.ToBoolean(row.Cells[0].Value);
                var tmp = listOrder.FirstOrDefault(
                        order => order.Name.Equals(row.Cells["Nazwa"].Value.ToString()));
                if (Convert.ToBoolean(row.Cells[0].Value) && tmp != null)
                {
                    var form = new OrderCorrectionView(listOrder.FirstOrDefault(
                        order => order.Name.Equals(row.Cells["Nazwa"].Value.ToString())), orderService, trancheService);
                    form.Show();
                }
            }
        }

        /// <summary>
        /// Translates the enum representing the state of the order to a string that is more readable for the end user.
        /// </summary>
        /// <param name="state">The enum representing the state of the order.</param>
        /// <returns></returns>
        private string GetTranslatedOrderStateString(ORDERSTATE state)
        {
            String value = "";
            switch (state)
            {
                case ORDERSTATE.duringRealization:
                    value = "W trakcie realizacji";
                    break;
                case ORDERSTATE.cancelled:
                    value = "Anulowane";
                    break;
                case ORDERSTATE.duringReview:
                    value = "W trakcie reklamacji";
                    break;
                case ORDERSTATE.realized:
                    value = "Zrealizowane";
                    break;
            }
            return value;
        }

        /// <summary>
        /// Handles the Click event of the pictureBoxFilter control. Shows or hides the filters.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void pictureBoxFilter_Click(object sender, EventArgs e)
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
    }
}
