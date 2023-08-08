using System;
using System.Windows.Forms;

namespace DS_Pokemon_Stat_Editor
{
    public partial class NumericNoArrows : NumericUpDown
    {
        public NumericNoArrows()
        {
            InitializeComponent();
            Controls[0].Hide();
            this.InterceptArrowKeys = false;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        protected override void OnTextBoxResize(object source, EventArgs e)
        {
            Controls[1].Width = Width - 4;
        }
        
    }
}
