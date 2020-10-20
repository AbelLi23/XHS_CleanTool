using System.Windows.Forms;

namespace CleanProApp
{
    public partial class Pump_M : Form
    {
        public Pump_M()
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[3];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
        }
    }
}
