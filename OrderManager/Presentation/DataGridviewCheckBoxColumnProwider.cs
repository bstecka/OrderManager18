using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManager.Presentation
{
    class DataGridviewCheckBoxColumnProwider
    {
        private DataGridView list;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridviewCheckBoxColumnProwider"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public DataGridviewCheckBoxColumnProwider(DataGridView list)
        {
            this.list = list;
        }

        /// <summary>
        /// Adds the CheckBox column to the dataGridView.
        /// </summary>
        public void addCheckBoxColumn()
        {
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.Width = 10;
            checkboxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            list.Columns.Insert(0, checkboxColumn);
            
            Rectangle rect = list.GetCellDisplayRectangle(0, -1, true);
            rect.X = rect.Location.X + (rect.Width / 4);

            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(18, 18);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);

            list.Controls.Add(checkboxHeader);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkboxHeader control. Updates the state of checkboxes
        /// corresponding to the state of the checkbox in the header.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < list.RowCount; i++)
            {
                list[0, i].Value = ((CheckBox)list.Controls.Find("checkboxHeader", true)[0]).Checked;
            }
            list.EndEdit();
        }
    }
}
