﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrderManager.DAO;
using OrderManager.ExternalSysDAO;

namespace OrderManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //(new Display()).DisplayTables();
            (new Tester()).Show();
        }

        
    }
}