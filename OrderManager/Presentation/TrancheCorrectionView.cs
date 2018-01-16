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
        private List<PercentageDiscount> discounts;
        private ITrancheService trancheService;
        private double priceNetto;
        private double priceBrutto;
        private int numberOfItems;
        private double quotaDiscount;
        private Boolean saved;
        private List<PercentageDiscount> viableDiscounts;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrancheCorrectionView"/> class.
        /// </summary>
        /// <param name="tranche">The tranche.</param>
        /// <param name="trancheService">The tranche service.</param>
        public TrancheCorrectionView(Tranche tranche, ITrancheService trancheService)
        {
            InitializeComponent();
            discounts = trancheService.GetViableDiscounts(tranche);
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

        /// <summary>
        /// Fills the form with the data of the tranche currently being edited.
        /// </summary>
        private void FillForm()
        {
            labelTitle.Text += tranche.Stock.Stock.Name;
            labelTrancheName.Text = tranche.Stock.Stock.Name;
            labelCode.Text = tranche.Stock.Stock.Code;
            labelStockPrice.Text = "" + Math.Round(tranche.Stock.PriceNetto, 2).ToString();
            labelVAT.Text = "" + tranche.Stock.Stock.VAT;
            textBoxNumberOfItems.Text = "" + tranche.NumberOfItems;
            textBoxQuota.Text = "" + tranche.QuotaDiscount;
            labelNetto.Text = "" + Math.Round(tranche.PriceNetto, 2).ToString();
            labelBrutto.Text = "" + Math.Round(tranche.PriceBrutto, 2).ToString();

            (new DataGridviewCheckBoxColumnProwider(dataGridDiscounts)).addCheckBoxColumn();
            FillDiscounts();
            dataGridDiscounts.CellValueChanged += checkedDiscountsChanged;
        }

        /// <summary>
        /// Updates the discounts of the tranche to match currently selected discounts. Updates the view accordingly.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void checkedDiscountsChanged(object sender, DataGridViewCellEventArgs e)
        {
            List<PercentageDiscount> discounts = new List<PercentageDiscount>();
            foreach (DataGridViewRow row in dataGridDiscounts.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (Convert.ToBoolean(row.Cells[0].Value))
                    discounts.Add(this.viableDiscounts.FirstOrDefault
                        (d => d.Id == Convert.ToInt32(row.Cells["Id"].Value)));
            }
            tranche.Discounts = discounts;
            labelNetto.Text = Math.Round(tranche.PriceNetto, 2).ToString();
            labelBrutto.Text = Math.Round(tranche.PriceBrutto, 2).ToString();
        }

        /// <summary>
        /// Adds the CheckBox column to the dataGridView containing discount data. 
        /// </summary>
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

        /// <summary>
        /// Fills the discounts dataGridView with the data of discounts which can be assigned to the currently edited tranche.
        /// </summary>
        private void FillDiscounts()
        {
            DataTable dataGridSource = new DataTable();
            dataGridSource.Columns.Add("Id");
            dataGridSource.Columns.Add("Wysokość");
            dataGridSource.Columns.Add("Data rozpoczęcia");
            dataGridSource.Columns.Add("Data zakończenia");

            List<PercentageDiscount> previouslyAssignedDiscounts = trancheService.GetPercentageDiscounts(tranche);
            viableDiscounts = trancheService.GetViableDiscounts(tranche);
            int lp = 1;
            foreach (var discount in viableDiscounts)
            {
                DataRow dataRow = dataGridSource.NewRow();
                dataRow["Id"] = discount.Id;
                dataRow["Wysokość"] = Math.Round(discount.Amount,2);
                dataRow["Data rozpoczęcia"] = discount.Since.ToShortDateString();
                dataRow["Data zakończenia"] = discount.Until.ToShortDateString();
                dataGridSource.Rows.Add(dataRow);
                lp++;
            }
            dataGridDiscounts.DataSource = dataGridSource;
            dataGridDiscounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in dataGridDiscounts.Columns)
                if (column.Index.Equals(0))
                    column.ReadOnly = false;
                else
                    column.ReadOnly = true;

            var list = dataGridDiscounts;

            foreach (DataGridViewRow row in dataGridDiscounts.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = chk.FalseValue;
            }
        }

        /// <summary>
        /// Handles the CellPaint event of the TableLayoutPanel control. Changes the look of the layout.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TableLayoutCellPaintEventArgs"/> instance containing the event data.</param>
        private void TableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, e.CellBounds.Location, new Point(e.CellBounds.Right, e.CellBounds.Top));
        }

        /// <summary>
        /// Handles the FormClosing event of the TrancheCorrectionView control. Asks for confirmation to close the form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Load event of the TrancheCorrectionView control. Adds the closing handler to form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TrancheCorrectionView_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(TrancheCorrectionView_FormClosing);
        }

        /// <summary>
        /// Handles the Click event of the Button1 control. Closes the form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the KeyPress event of the TextBoxNumberOfItems control. Disallows user to input any key other
        /// than a digit or backspace in the textbox for assigning the number of items to the tranche.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private void TextBoxNumberOfItems_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
                e.Handled = true;
        }

        /// <summary>
        /// Handles the KeyPress event of the TextBoxQuota control. Disallows user to input any key other
        /// than a digit, comma or backspace in the textbox for assigning the quota discount to the tranche.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private void TextBoxQuota_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == 44))
                e.Handled = true;
        }

        /// <summary>
        /// Handles the TextChanged event of the TextBoxNumberOfItems control. Updates the view so that the prices
        /// of the tranche match the changed value of numberOfItems.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TextBoxNumberOfItems_TextChanged(object sender, EventArgs e)
        {
            if (textBoxNumberOfItems.Text.Length > 0 && textBoxNumberOfItems.Text.Length < 10)
                try { tranche.NumberOfItems = Int32.Parse(textBoxNumberOfItems.Text); }
                catch(System.FormatException) { MessageBox.Show("Nieprawidłowa wartość."); textBoxNumberOfItems.Text = "1"; }
            labelNetto.Text = Math.Round(tranche.PriceNetto, 2).ToString();
            labelBrutto.Text = Math.Round(tranche.PriceBrutto, 2).ToString();
        }

        /// <summary>
        /// Handles the TextChanged event of the TextBoxQuota control. Updates the view so that the prices
        /// of the tranche match the changed value of the quotaDiscount.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TextBoxQuota_TextChanged(object sender, EventArgs e)
        {
            Regex regexObj = new Regex(@"-?\d+(?:\,\d+)?");
            Match matchResult = regexObj.Match(textBoxQuota.Text);
            if (textBoxQuota.Text.Length > 0 && textBoxQuota.Text.Length < 10 && matchResult.Length > 0)
                try { tranche.QuotaDiscount = Double.Parse(textBoxQuota.Text); }
                catch (System.FormatException) { MessageBox.Show("Nieprawidłowa wartość."); textBoxQuota.Text = "0"; }
            labelNetto.Text = Math.Round(tranche.PriceNetto, 2).ToString();
            labelBrutto.Text = Math.Round(tranche.PriceBrutto, 2).ToString();
        }

        /// <summary>
        /// Handles the Click event of the Button4 control. Sets the state of the form to saved and closes the form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Button4_Click(object sender, EventArgs e)
        {
            this.saved = true;
            this.Close();
        }
    }
}
