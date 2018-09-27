using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace xAscom.App.ControlPanel
{
    public partial class frmDomeControl : Form
    {
        public frmDomeControl()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetUIState();
        }
        protected void AppInit() { }
        private void SetUIState()
        {

        }
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
