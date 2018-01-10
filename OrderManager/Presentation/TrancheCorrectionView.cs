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
        private double priceNetto;
        private double priceBrutto;
        private int numberOfItems;
        private double quotaDiscount;
        private Boolean saved;

        public TrancheCorrectionView(Tranche tranche)
        {
            InitializeComponent();
            this.tranche = tranche;
            this.priceNetto = tranche.PriceNetto;
            this.priceBrutto = tranche.PriceBrutto;
            this.numberOfItems = tranche.NumberOfItems;
            this.quotaDiscount = tranche.QuotaDiscount;
            this.saved = false;
            FillForm();
            tableLayoutPanel4.CellPaint += TableLayoutPanel_CellPaint;
        }

        public int NumberOfItems { get => numberOfItems; }
        public double QuotaDiscount { get => quotaDiscount; }
        public Boolean Saved { get => saved; }
        public Tranche Tranche { get => tranche; }

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
            if (!this.saved)
            {
                if (MessageBox.Show("Czy chcesz zamknąć to okno? Wprowadzone zmiany nie zostały zapisane.", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
                else
                    e.Cancel = false;
            }
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
            if (textBoxNumberOfItems.Text.Length > 0 && textBoxNumberOfItems.Text.Length < 10)
                this.numberOfItems = Int32.Parse(textBoxNumberOfItems.Text);
            priceNetto = tranche.Stock.PriceNetto * this.numberOfItems;
            labelNetto.Text = "" + priceNetto;
            priceBrutto = priceNetto / 100 * (100 + tranche.Stock.Stock.VAT);
            labelBrutto.Text = "" + priceBrutto;
        }

        private void TextBoxQuota_TextChanged(object sender, EventArgs e)
        {
            Regex regexObj = new Regex(@"-?\d+(?:\,\d+)?");
            Match matchResult = regexObj.Match(textBoxQuota.Text);
            if (textBoxQuota.Text.Length > 0 && textBoxQuota.Text.Length < 10 && matchResult.Length > 0)
                this.quotaDiscount = Double.Parse(textBoxQuota.Text);
            double amount = priceBrutto - this.quotaDiscount;
            labelBrutto.Text = "" + amount;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.tranche.NumberOfItems = this.numberOfItems;
            this.tranche.QuotaDiscount = this.quotaDiscount;
            this.saved = true;
            this.Close();
        }
    }
}
