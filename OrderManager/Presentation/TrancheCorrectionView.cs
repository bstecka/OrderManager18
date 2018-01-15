using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
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
        private ITrancheService trancheService;
        private double priceNetto;
        private double priceBrutto;
        private int numberOfItems;
        private double quotaDiscount;
        private Boolean saved;

        public TrancheCorrectionView(Tranche tranche, ITrancheService trancheService)
        {
            InitializeComponent();
            this.trancheService = trancheService;
            this.tranche = tranche;
            this.priceNetto = tranche.PriceNetto;
            this.priceBrutto = tranche.PriceBrutto;
            this.numberOfItems = tranche.NumberOfItems;
            this.quotaDiscount = tranche.QuotaDiscount;
            this.saved = false;
            this.Text += tranche.Stock.Stock.Name;
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
            labelStockPrice.Text = "" + Math.Round(tranche.Stock.PriceNetto, 2);
            labelVAT.Text = "" + tranche.Stock.Stock.VAT;
            textBoxNumberOfItems.Text = "" + tranche.NumberOfItems;
            textBoxQuota.Text = "" + tranche.QuotaDiscount;
            labelNetto.Text = "" + Math.Round(tranche.PriceNetto, 2);
            labelBrutto.Text = "" + Math.Round(tranche.PriceBrutto, 2);
            AddCheckBoxColumn();
            FillDiscounts();
        }

        private void AddCheckBoxColumn()
        {
            var list = dataGridDiscounts;
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.Width = 30;
            checkboxColumn.Name = "Zaznacz";
            checkboxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            list.Columns.Insert(0, checkboxColumn);
            dataGridDiscounts.ReadOnly = false;

            foreach (DataGridViewColumn column in dataGridDiscounts.Columns)
            {
                if (column.DisplayIndex != 0)
                    column.ReadOnly = true;
            }
            foreach (DataGridViewRow row in dataGridDiscounts.Rows)
            {
                row.Cells["Zaznacz"].Value = true;
            }
        }

        private void FillDiscounts()
        {
            DataTable dataGridSource = new DataTable();
            dataGridSource.Columns.Add("Id");
            dataGridSource.Columns.Add("Wysokość");
            dataGridSource.Columns.Add("Data rozpoczęcia");
            dataGridSource.Columns.Add("Data zakończenia");

            List<PercentageDiscount> previouslyAssignedDiscounts = trancheService.GetPercentageDiscounts(tranche);
            List<PercentageDiscount> viableDiscounts = trancheService.GetViableDiscounts(tranche);
            int lp = 1;
            foreach (var discount in viableDiscounts)
            {
                DataRow dataRow = dataGridSource.NewRow();
                dataRow["Id"] = discount.Id;
                dataRow["Wysokość"] = Math.Round(discount.Amount, 2);
                dataRow["Data rozpoczęcia"] = discount.Since.ToString("dd/MM/yyyy");
                dataRow["Data zakończenia"] = discount.Until.ToString("dd/MM/yyyy");
                dataGridSource.Rows.Add(dataRow);
                lp++;
            }
            dataGridDiscounts.DataSource = dataGridSource;
            dataGridDiscounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
            Regex regexObj = new Regex(@"-?\d+(?:\,\d+)?");
            Match matchResult = regexObj.Match(textBoxQuota.Text);
            if (textBoxQuota.Text.Length > 0 && textBoxQuota.Text.Length < 10 && matchResult.Length > 0)
                this.quotaDiscount = Double.Parse(textBoxQuota.Text);
            priceNetto = (tranche.Stock.PriceNetto - this.quotaDiscount) * this.numberOfItems;
            labelNetto.Text = "" + priceNetto;
            priceBrutto = priceNetto / 100 * (100 + tranche.Stock.Stock.VAT);
            labelBrutto.Text = "" + priceBrutto;
        }

        private void TextBoxQuota_TextChanged(object sender, EventArgs e)
        {
            if (textBoxNumberOfItems.Text.Length > 0 && textBoxNumberOfItems.Text.Length < 10)
                this.numberOfItems = Int32.Parse(textBoxNumberOfItems.Text);
            Regex regexObj = new Regex(@"-?\d+(?:\,\d+)?");
            Match matchResult = regexObj.Match(textBoxQuota.Text);
            if (textBoxQuota.Text.Length > 0 && textBoxQuota.Text.Length < 10 && matchResult.Length > 0)
                this.quotaDiscount = Double.Parse(textBoxQuota.Text);
            priceNetto = (tranche.Stock.PriceNetto - this.quotaDiscount) * this.numberOfItems;
            labelNetto.Text = "" + priceNetto;
            priceBrutto = priceNetto / 100 * (100 + tranche.Stock.Stock.VAT);
            labelBrutto.Text = "" + priceBrutto;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                List<PercentageDiscount> chosenDiscounts = new List<PercentageDiscount>();
                this.tranche.NumberOfItems = this.numberOfItems;
                this.tranche.QuotaDiscount = this.quotaDiscount;
                var checkedRows = this.dataGridDiscounts.Rows.Cast<DataGridViewRow>().Where(row => (bool?)row.Cells[0].Value == true).ToList();
                List<PercentageDiscount> viableDiscounts = trancheService.GetViableDiscounts(tranche);
                foreach (PercentageDiscount discount in viableDiscounts)
                {
                    Boolean shouldBeAdded = false;
                    foreach (DataGridViewRow row in checkedRows)
                    {
                        var id1 = discount.Id.ToString();
                        var id2 = row.Cells["Id"].Value.ToString();
                        if (id1.Equals(id2))
                            shouldBeAdded = true;
                    }
                    if (shouldBeAdded)
                        chosenDiscounts.Add(discount);
                }
                this.tranche.Discounts = chosenDiscounts;
                this.saved = true;
                this.Close();
            } catch (ArgumentException exception)
            {
                MessageBox.Show("Naliczenie rabatów spowoduje obniżenie ceny transzy poniżej 0.01 zł.");
            }
        }
    }
}
