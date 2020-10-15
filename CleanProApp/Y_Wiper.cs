using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CleanProApp
{
    public partial class Y_Wiper : Form
    {
        public Y_Wiper()
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[1];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
        }
    }
}
