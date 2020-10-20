using System.Windows.Forms;

namespace CleanProApp
{
    public partial class Pump_S : Form
    {
        public Pump_S()
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[4];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
        }
    }
}
