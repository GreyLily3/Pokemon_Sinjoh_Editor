using System;
using System.Windows.Forms;

namespace Pokemon_Sinjoh_Editor
{
    public partial class NumericNoArrows : NumericUpDown
    {
        public NumericNoArrows()
        {
            InitializeComponent();
            Controls[0].Hide(); //hides arrows from control
            InterceptArrowKeys = false;
        }

        protected override void OnTextBoxResize(object source, EventArgs e)
        {
            Controls[1].Width = Width - 4; //resizes the textbox to account for the lack of arrows
        }
        
    }
}
