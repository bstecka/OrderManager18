using OrderManager.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManager.Presentation
{
    public partial class TrancheCorrectionView : Form
    {
        private Tranche tranche;
        double priceNetto;
        double priceBrutto;

        public TrancheCorrectionView(Tranche tranche)
        {
            InitializeComponent();
            this.tranche = tranche;
            this.priceNetto = tranche.PriceNetto;
            this.priceBrutto = tranche.PriceBrutto;
            FillForm();
            tableLayoutPanel4.CellPaint += TableLayoutPanel_CellPaint;
        }

        private void FillForm()
        {
            labelTitle.Text += tranche.Stock.Stock.Name;
            labelTrancheName.Text = tranche.Stock.Stock.Name;
            labelCode.Text = tranche.Stock.Stock.Code;
            labelStockPrice.Text = "" + tranche.Stock.PriceNetto;
            labelVAT.Text = "" + tranche.Stock.Stock.VAT;
            textBoxNumberOfItems.Text = "" + tranche.NumberOfItems;
            textBoxQuota.Text = "" + tranche.QuotaDiscount;
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

        private void TextBoxNumberOfItems_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
                e.Handled = true;
        }

        private void TextBoxQuota_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == 44))
                e.Handled = true;
        }

        private void TextBoxNumberOfItems_TextChanged(object sender, EventArgs e)
        {
            int nr = 0;
            if (textBoxNumberOfItems.Text.Length > 0 && textBoxNumberOfItems.Text.Length < 10)
                nr = Int32.Parse(textBoxNumberOfItems.Text);
            priceNetto = tranche.Stock.PriceNetto * nr;
            labelNetto.Text = "" + priceNetto;
            priceBrutto = priceNetto / 100 * (100 + tranche.Stock.Stock.VAT);
            labelBrutto.Text = "" + priceBrutto;
        }

        private void TextBoxQuota_TextChanged(object sender, EventArgs e)
        {
            Regex regexObj = new Regex(@"-?\d+(?:\,\d+)?");
            Match matchResult = regexObj.Match(textBoxQuota.Text);
            double quota = 0.0;
            if (textBoxQuota.Text.Length > 0 && textBoxQuota.Text.Length < 10 && matchResult.Length > 0)
                quota = Double.Parse(textBoxQuota.Text);
            double amount = priceBrutto - quota;
            labelBrutto.Text = "" + amount;
        }
    }
}
