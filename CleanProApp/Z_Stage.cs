using System.Windows.Forms;

namespace CleanProApp
{
    public partial class Z_Stage : Form
    {
        public Z_Stage()
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[2];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
        }
    }
}
