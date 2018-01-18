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
{/*
    public partial class OrderCorrectionView : Form
    {
        private Order order;
        private List<Tranche> tranchesToUpdate;
        private List<Tranche> tranchesToDelete;
        private IOrderService orderService;
        private ITrancheService trancheService;
        private Boolean saved;

        public OrderCorrectionView(Order order, IOrderService orderService, ITrancheService trancheService)
        {
            InitializeComponent();
            this.order = order;
            this.orderService = orderService;
            this.trancheService = trancheService;
            this.tranchesToUpdate = new List<Tranche>();
            this.tranchesToDelete = new List<Tranche>();
            this.saved = false;
            this.Text += order.Name;
            FillForm();
        }

        private void FillForm()
        {
            labelTitle.Text += order.Name;
            labelOrdersName.Text = order.Name;
            labelDate.Text = order.DateOfCreation.ToString("dd/MM/yyyy");
            labelCounterpartysName.Text = order.Counterparty.Name;
            labelCounterpartysCode.Text = order.Counterparty.Nip.ToString();
            labelNetto.Text = order.PriceNetto.ToString();
            labelBrutto.Text = order.PriceBrutto.ToString();
            labelAuthor.Text = order.Creator.Name + " " + order.Creator.Surname;
            FillTranches();
            AddActionColumns();
        }

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

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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

        private void OrderCorrectionView_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(OrderCorrectionView_FormClosing);
        }

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
                    this.tranchesToDelete.Add(tranche);
                    dataGridViewTranches.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        void TrancheCorrectionView_Closed(object sender, FormClosedEventArgs e)
        {
            TrancheCorrectionView form = (TrancheCorrectionView) sender;
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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Order newOrder = this.order;
            newOrder.State = ORDERSTATE.duringReview;
            int newOrderId = orderService.InsertOrder(newOrder);
            foreach (Tranche tranche in newOrder.Tranches)
            {
                Boolean shouldBeOmitted = false;
                tranche.OrderId = newOrderId;
                foreach (Tranche toDelete in tranchesToDelete)
                {
                    if (toDelete.Id == tranche.Id)
                        shouldBeOmitted = true;
                }
                if (!shouldBeOmitted)
                {
                    int newTrancheId = trancheService.InsertTranche(tranche);
                    tranche.Id = newTrancheId;
                    foreach (PercentageDiscount discount in tranche.Discounts)
                    {
                        trancheService.AssignDiscountToTranche(tranche, discount);
                    }
                }
            }
            Order cancelledOrder = this.order;
            cancelledOrder.State = ORDERSTATE.cancelled;
            cancelledOrder.ParentOrder = orderService.GetById("" + newOrderId);
            orderService.UpdateOrder(cancelledOrder);
            this.saved = true;
            this.Close();
        }
    }*/

    public partial class OrderCorrectionView : Form
    {
        private Order order;
        private Order originalOrder;
        private List<Tranche> tranchesToUpdate;
        private List<Tranche> tranchesToDelete;
        private IOrderService orderService;
        private ITrancheService trancheService;
        private Boolean saved;

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

        private void FillForm()
        {
            labelTitle.Text += order.Name;
            labelOrdersName.Text = order.Name;
            labelDate.Text = order.DateOfCreation.ToString("dd/MM/yyyy");
            labelCounterpartysName.Text = order.Counterparty.Name;
            labelCounterpartysCode.Text = order.Counterparty.Nip.ToString();
            labelNetto.Text = order.PriceNetto.ToString();
            labelBrutto.Text = order.PriceBrutto.ToString();
            labelAuthor.Text = order.Creator.Name + " " + order.Creator.Surname;
            FillTranches();
            AddActionColumns();
        }

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

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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

        private void OrderCorrectionView_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(OrderCorrectionView_FormClosing);
        }

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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            order.State = ORDERSTATE.duringReview;
            originalOrder.State = ORDERSTATE.cancelled; 
            foreach(var t in order.Tranches)
            {
                int newTrancheId = trancheService.InsertTranche(t);
                t.Id = newTrancheId;
                foreach(var disc in t.Discounts)
                {
                    trancheService.AssignDiscountToTranche(t, disc);
                }
            }
            int newOrderId = orderService.InsertOrder(order);
            originalOrder.ParentOrder = orderService.GetById(newOrderId.ToString());
            orderService.UpdateOrder(originalOrder);
            this.saved = true;
            this.Close();
        }
    }
}
