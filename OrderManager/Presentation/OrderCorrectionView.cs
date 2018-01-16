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
    public partial class OrderCorrectionView : Form
    {
        private Order order;
        private Order originalOrder;
        private List<Tranche> tranchesToUpdate;
        private List<Tranche> tranchesToDelete;
        private IOrderService orderService;
        private ITrancheService trancheService;
        private Boolean saved;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderCorrectionView"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="orderService">The order service.</param>
        /// <param name="trancheService">The tranche service.</param>
        public OrderCorrectionView(Order order, IOrderService orderService, ITrancheService trancheService)
        {
            InitializeComponent();
            this.order = CloneObj.DeepClone<Order>(order);
            originalOrder = order;
            this.orderService = orderService;
            this.trancheService = trancheService;
            this.tranchesToUpdate = new List<Tranche>();
            this.tranchesToDelete = new List<Tranche>();
            this.saved = false;
            this.Text += order.Name;
            FillForm();
        }

        /// <summary>
        /// Fills the form with the data of the single order currently being corrected.
        /// </summary>
        private void FillForm()
        {
            labelTitle.Text += order.Name;
            labelOrdersName.Text = order.Name;
            labelDate.Text = order.DateOfCreation.ToString("dd/MM/yyyy");
            labelCounterpartysName.Text = order.Counterparty.Name;
            labelCounterpartysCode.Text = order.Counterparty.Nip.ToString();
            labelNetto.Text = Math.Round(order.PriceNetto, 2).ToString();
            labelBrutto.Text = Math.Round(order.PriceBrutto, 2).ToString();
            labelAuthor.Text = order.Creator.Name + " " + order.Creator.Surname;
            FillTranches();
            AddActionColumns();
        }

        /// <summary>
        /// Fills the dataGridView with the data of the currently edited order's tranches.
        /// </summary>
        private void FillTranches()
        {
            DataTable dataGridSource = new DataTable();
            dataGridSource.Columns.Add("Lp");
            dataGridSource.Columns.Add("Id");
            dataGridSource.Columns.Add("Nazwa");
            dataGridSource.Columns.Add("Ilość");
            dataGridSource.Columns.Add("Cena netto");
            dataGridSource.Columns.Add("VAT");
            dataGridSource.Columns.Add("Wartość netto");
            dataGridSource.Columns.Add("Wartość brutto");

            int lp = 1;
            foreach (var tranche in order.Tranches)
            {
                DataRow dataRow = dataGridSource.NewRow();
                dataRow["Lp"] = lp;
                dataRow["Id"] = tranche.Id;
                dataRow["Nazwa"] = tranche.Stock.Stock.Name;
                dataRow["Ilość"] = tranche.NumberOfItems;
                dataRow["VAT"] = tranche.Stock.Stock.VAT + "%";
                dataRow["Cena netto"] = Math.Round(tranche.Stock.PriceNetto, 2);
                dataRow["Wartość netto"] = Math.Round(tranche.PriceNetto, 2);
                dataRow["Wartość brutto"] = Math.Round(tranche.PriceBrutto, 2);
                dataGridSource.Rows.Add(dataRow);
                lp++;
            }
            dataGridViewTranches.DataSource = dataGridSource;
            dataGridViewTranches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Adds the columns enabling to edit or delete tranche to the dataGridView containing tranche data.
        /// </summary>
        private void AddActionColumns()
        {
            var list = dataGridViewTranches;
            DataGridViewLinkColumn editColumn = new DataGridViewLinkColumn();
            editColumn.Width = 10;
            editColumn.DefaultCellStyle.NullValue = "Edytuj";
            editColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            list.Columns.Insert(list.ColumnCount, editColumn);
            DataGridViewLinkColumn deleteColumn = new DataGridViewLinkColumn();
            deleteColumn.Width = 10;
            deleteColumn.DefaultCellStyle.NullValue = "Usuń";
            deleteColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            list.Columns.Insert(list.ColumnCount, deleteColumn);
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control. Closes the form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the FormClosing event of the OrderCorrectionView control. Asks for confirmation to close the form,
        /// if the changes were not saved, and informs about changes being saved if they were.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void OrderCorrectionView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.saved)
            {
                if (MessageBox.Show("Czy chcesz zamknąć to okno? Wprowadzone zmiany nie zostały zapisane.", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
                else
                    e.Cancel = false;
            }
            else
            {
                MessageBox.Show("Wprowadzone zmiany zostały zapisane.", "");
            }
        }

        /// <summary>
        /// Handles the Load event of the OrderCorrectionView control. Adds the closing handler to form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OrderCorrectionView_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(OrderCorrectionView_FormClosing);
        }

        /// <summary>
        /// Handles the CellContentClick event of the DataGridViewTranches control. Adds the events for tranche edition
        /// (opening a new form) and tranche deletion to the DataGridView containing tranche data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void DataGridViewTranches_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex.Equals(0) || e.ColumnIndex.Equals(1))
            {
                DataGridViewRow row = dataGridViewTranches.Rows[e.RowIndex];
                Tranche tranche = null;
                foreach (Tranche tr in order.Tranches)
                {
                    if (tr.Id.ToString().Equals(row.Cells["Id"].Value.ToString()))
                        tranche = tr;
                }
                if (tranche != null && e.ColumnIndex.Equals(0))
                {
                    var form = new TrancheCorrectionView(tranche, trancheService);
                    form.FormClosed += new FormClosedEventHandler(TrancheCorrectionView_Closed);
                    form.Show();
                }
                else if (tranche != null && e.ColumnIndex.Equals(1))
                {
                    order.Tranches.RemoveAt(e.RowIndex);
                    dataGridViewTranches.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        /// <summary>
        /// Handles the Closed event of the TrancheCorrectionView control. Edits the tranche DataGridView to 
        /// properly display changed tranche data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        void TrancheCorrectionView_Closed(object sender, FormClosedEventArgs e)
        {
            TrancheCorrectionView form = (TrancheCorrectionView)sender;
            if (form.Saved)
            {
                Tranche tr = form.Tranche;
                this.tranchesToUpdate.Add(form.Tranche);
                foreach (DataGridViewRow row in dataGridViewTranches.Rows)
                {
                    if (form.Tranche.Id.ToString().Equals(row.Cells["Id"].Value.ToString()))
                    {
                        row.Cells["Ilość"].Value = form.Tranche.NumberOfItems;
                        row.Cells["Cena netto"].Value = Math.Round(form.Tranche.Stock.PriceNetto, 2);
                        row.Cells["Wartość netto"].Value = Math.Round(form.Tranche.PriceNetto, 2);
                        row.Cells["Wartość brutto"].Value = Math.Round(form.Tranche.PriceBrutto, 2);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control. Performs database operations for the new order, its
        /// assigned tranches and percentage discounts. Sets the state of edited order to 'cancelled'.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            order.State = ORDERSTATE.duringRealization;
            originalOrder.State = ORDERSTATE.cancelled; 
            int newOrderId = orderService.InsertOrder(order);
            order.Id = newOrderId;
            foreach (var tranche in order.Tranches)
            {
                tranche.OrderId = newOrderId;
                int newTrancheId = trancheService.InsertTranche(tranche);
                tranche.Id = newTrancheId;
                foreach (PercentageDiscount discount in tranche.Discounts)
                    trancheService.AssignDiscountToTranche(tranche, discount);
            }
            originalOrder.ParentOrder = orderService.GetById(newOrderId.ToString());
            orderService.UpdateOrder(originalOrder);
            this.saved = true;
            this.Close();
        }
    }
}
