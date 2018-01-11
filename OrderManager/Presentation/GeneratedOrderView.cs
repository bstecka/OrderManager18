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
    public partial class GeneratedOrderView : Form
    {

        private Order order;
        public GeneratedOrderView(Order order)
        {
            InitializeComponent();
            this.order = order;
            FillForm();
            this.Text += order.Name;
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
        }

        private void FillTranches()
        {
            DataTable dataGridSource = new DataTable();
            dataGridSource.Columns.Add("Lp");
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
    }
}
