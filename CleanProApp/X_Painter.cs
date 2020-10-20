using System.Windows.Forms;

namespace CleanProApp
{
    public partial class X_Painter : Form
    {
        public X_Painter()
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[0];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
        }
    }
}
