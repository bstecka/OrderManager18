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
    public partial class GeneratedOrders : Form
    {
        private List<Order> orders;

        internal GeneratedOrders(List<Order> orders)
        {
            InitializeComponent();
            labelTitle.Text = "Wygenerowane zamówienia z dnia " + DateTime.Now.Date.ToString("dd/MM/yyyy");
            this.orders = orders;
            fillDataGridView();
            //tableLayoutPanel3.CellPaint += tableLayoutPanel_CellPaint;
        }
        
        private void fillDataGridView()
        {
            DataTable dataGridSource = new DataTable();
            //(new DataGridviewCheckBoxColumnProwider(dataGridViewOrders)).addCheckBoxColumn();
            addCheckBoxColumn();
            dataGridSource.Columns.Add("Nazwa");
            dataGridSource.Columns.Add("Kontrahent");
            dataGridSource.Columns.Add("Wartość netto");
            dataGridSource.Columns.Add("Wartość brutto");
            foreach (var order in orders)
            {
                DataRow dataRow = dataGridSource.NewRow();
                dataRow["Nazwa"] = order.Name;
                dataRow["Kontrahent"] = order.Counterparty.Name;
                dataRow["Wartość netto"] = Math.Round(order.PriceNetto, 2);
                dataRow["Wartość brutto"] = Math.Round(order.PriceBrutto, 2);
                dataGridSource.Rows.Add(dataRow);
            }
            dataGridViewOrders.DataSource = dataGridSource;
            dataGridViewOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewOrders.Rows)
            {
                var tmp1 = Convert.ToBoolean(row.Cells[0].Value);
                var tmp = orders.FirstOrDefault(
                        order => order.Name.Equals(row.Cells["Nazwa"].Value.ToString()));
                if (Convert.ToBoolean(row.Cells[0].Value) && tmp != null)
                    (new Form1(orders.FirstOrDefault(
                        order => order.Name.Equals(row.Cells["Nazwa"].Value.ToString())))).Show();
            }
        }

        private void labelTitle_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, e.CellBounds.Location, new Point(e.CellBounds.Right, e.CellBounds.Bottom));
        }

















        public void addCheckBoxColumn()
        {
            var list = dataGridViewOrders;
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.Width = 30;
            checkboxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            list.Columns.Insert(0, checkboxColumn);
            
            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(18, 18);
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);

            list.Controls.Add(checkboxHeader);
        }

        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            var list = dataGridViewOrders;

            for (int i = 0; i < list.RowCount; i++)
            {
                list[0, i].Value = ((CheckBox)list.Controls.Find("checkboxHeader", true)[0]).Checked;
            }
            list.EndEdit();
        }
    }
}
