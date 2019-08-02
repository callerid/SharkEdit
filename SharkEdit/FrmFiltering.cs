using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharkEdit
{
    public partial class FrmFiltering : Form
    {
        public FrmFiltering(string list)
        {
            InitializeComponent();
            tbList.Text = list;
            tbList.SelectionStart = tbList.Text.Length;
        }
    }
}
