using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManager
{
    public partial class ThePrettiestGUIEver : Form
    {
        int filtersHeight;

        public ThePrettiestGUIEver()
        {
            InitializeComponent();
            filtersHeight = 30;
        }


        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            //e.Graphics.DrawLine(Pens.Black, e.CellBounds.Location, new Point(e.CellBounds.Right, e.CellBounds.Top));

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void prepareFiltersPanel()
        {
            (tableLayoutPanelFilter.RowStyles)[1].SizeType = SizeType.Absolute;
            (tableLayoutPanelFilter.RowStyles)[1].Height = 0;
            (tableLayoutPanelContent.RowStyles)[1].Height = filtersHeight * 2;
        }

        private void label4_MouseDown(object sender, MouseEventArgs e)
        {
            

        }

        private void flowLayoutPanelFilter_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void pictureBoxFilter_MouseClick(object sender, MouseEventArgs e)
        {
            (tableLayoutPanelFilter.RowStyles)[1].SizeType = SizeType.Absolute;
            if ((tableLayoutPanelFilter.RowStyles)[1].Height == 0)
            {
                (tableLayoutPanelFilter.RowStyles)[1].Height = filtersHeight;
                (tableLayoutPanelContent.RowStyles)[1].Height = filtersHeight * 3;
            }
            else
            {
                (tableLayoutPanelFilter.RowStyles)[1].Height = 0;
                (tableLayoutPanelContent.RowStyles)[1].Height = filtersHeight * 2;
            }
        }

        public Bitmap RotateImage(Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }
    }
}
