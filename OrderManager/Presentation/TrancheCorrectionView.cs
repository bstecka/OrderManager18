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
    public partial class TrancheCorrectionView : Form
    {
        private Tranche tranche;
        public TrancheCorrectionView(Tranche tranche)
        {
            InitializeComponent();
            this.tranche = tranche;
            FillForm();
            tableLayoutPanel4.CellPaint += TableLayoutPanel_CellPaint;
        }

        private void FillForm()
        {
            labelTitle.Text += tranche.Stock.Stock.Name;
            labelTrancheName.Text = tranche.Stock.Stock.Name;
            labelCode.Text = tranche.Stock.Stock.Code;
            labelSomething.Text = "" + tranche.Stock.PriceNetto;
            labelVAT.Text = "" + tranche.Stock.Stock.VAT;
            labelNumberOfItems.Text = "" + tranche.NumberOfItems;
            labelQuota.Text = "" + tranche.QuotaDiscount;
            labelNetto.Text = "" + tranche.PriceNetto;
            labelBrutto.Text = "" + tranche.PriceBrutto;
            FillDiscounts();
        }

        private void FillDiscounts()
        {
            DataTable dataGridSource = new DataTable();
            dataGridSource.Columns.Add("Wysokość");
            dataGridSource.Columns.Add("Data rozpoczęcia");
            dataGridSource.Columns.Add("Data zakończenia");

            int lp = 1;
            foreach (var discount in tranche.Discounts)
            {
                DataRow dataRow = dataGridSource.NewRow();
                dataRow["Wysokość"] = discount.Amount;
                dataRow["Data rozpoczęcia"] = discount.Since;
                dataRow["Data zakończenia"] = discount.Until;
                dataGridSource.Rows.Add(dataRow);
                lp++;
            }
            dataGridViewTranches.DataSource = dataGridSource;
            dataGridViewTranches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void TableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, e.CellBounds.Location, new Point(e.CellBounds.Right, e.CellBounds.Top));
        }

        private void TrancheCorrectionView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz zamknąć to okno? Wprowadzone zmiany nie zostały zapisane.", "", MessageBoxButtons.YesNo) == DialogResult.No)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void OrderCorrectionView_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(TrancheCorrectionView_FormClosing);
        }

        private void TableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
