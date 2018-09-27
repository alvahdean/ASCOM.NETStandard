using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RACI.Client;
using RACI.Data;


namespace ASCOM.RACI
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            Dome.epUrl = (string)comboBoxEndpoints.SelectedItem;
            Dome.tl.Enabled = chkTrace.Checked;
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            chkTrace.Checked = Dome.tl.Enabled;
            // set the list of com ports to those that are currently available
            comboBoxEndpoints.Items.Clear();
            comboBoxEndpoints.Items.AddRange(Dome.knownEndpoints.ToArray());      // use System.IO because it's static
            // select the current port if possible
            if (comboBoxEndpoints.Items.Contains(Dome.epUrl))
            {
                comboBoxEndpoints.SelectedItem = Dome.epUrl;
            }
        }

        private void OnEndpointSelected(object sender, EventArgs e)
        {
            string url= comboBoxEndpoints.SelectedItem.ToString();
            string dType = Dome.driverType;
            Dome.epUrl = url;
            List<RaciEndpointDriver> drivers=RaciClient.QueryDrivers(url).ToList();
            Dome.epDrivers = drivers.Where(t => t.DriverType == dType).Select(t => t.Name).ToArray();
            comboBoxDrivers.Items.Clear();
            comboBoxDrivers.Items.AddRange(Dome.epDrivers);
        }
        private void OnDriverSelected(object sender, EventArgs e)
        {
            Dome.epDriver = comboBoxDrivers.SelectedItem.ToString();
        }
        private void OnTraceChanged(object sender, EventArgs e)
        {
            Dome.traceState = chkTrace.CheckState == CheckState.Checked;
        }

    }
}