using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using RACI.Client;
using RACI.Data;

namespace xAscom.App.ControlPanel
{
    public partial class frmDomeControl : Form
    {
        public DriverUIState State { get; set; }
        public frmDomeControl()
        {
            InitializeComponent();
            DriverUIState State= new DriverUIState();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetUIState();
        }
        protected void AppInit()
        {
            if (Endpoints == null)
                Endpoints = new ObservableCollection<RaciEndpoint>();
            else
                Endpoints.Clear();
            if (Endpoints == null)
                Endpoints = new ObservableCollection<RaciEndpoint>();

        }

        private void SetUIState()
        {
            grpControl.Enabled = Driver.Connected;
            grpState.Enabled = Driver.Connected;
            tabDeviceView.Enabled = Driver.Connected;

            buttonConnect.Enabled = !String.IsNullOrWhiteSpace(Driver.EndpointUrl);
            buttonConnect.Text = Driver.Connected ? "Disconnect" : "Connect";

        }
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            Driver.Connected = false;
            Properties.Settings.Default.Save();
        }
        private void OnConnectClick(object sender, EventArgs e)
        {
            if (Driver.Connected)
                Driver.Connected = false;
            else
                Driver.Connected = true;
            SetUIState();
        }
        private void OnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void ddlEndpoint_SelectedValueChanged(object sender, EventArgs e)
        {
            string url = ddlEndpoint.SelectedValue.ToString();

        }

        //public void RefreshEndpoints()
        //{
        //    string currEpUrl = Endpoint?.ServiceRoot;
        //    string currDriver = Driver?.Name;

        //    Drivers.Clear();
        //    Endpoints.Clear();
        //    foreach (var item in RaciServiceManager.GetEndpoints())
        //        Endpoints.Add(item);
        //    Connect(currEpUrl);
        //    SetDriver(currDriver);
        //}
        //public bool Connect(string rootUrl)
        //{
        //    bool result = false;
        //    rootUrl = rootUrl?.Trim() ?? "";
        //    Uri uri = new Uri(rootUrl);
        //    rootUrl = uri.AbsoluteUri;
        //    if (uri?.IsAbsoluteUri ?? false && Endpoint?.ServiceRoot != rootUrl)
        //    {
        //        string currDriver = Driver?.Name;
        //        Drivers.Clear();
        //        Driver = null;
        //        Endpoint = Endpoints.FirstOrDefault(t => t.ServiceRoot == rootUrl);
        //        if (Endpoint == null)
        //        {
        //            Endpoint = RaciServiceManager.GetEndpointByUrl(rootUrl);
        //            if (Endpoint == null)
        //                Endpoint = RaciServiceManager.AddEndpoint(rootUrl);
        //        }
        //        if (Endpoint != null)
        //        {
        //            foreach (var item in RaciServiceManager.GetDrivers(Endpoint.Name, DriverType))
        //                Drivers.Add(item);
        //            SetDriver(currDriver);
        //        }
        //    }
        //    return result;
        //}
        //public void SetDriver(string driverName)
        //{
        //    Driver = Drivers?.OfDriverType(DriverType).SelectName(driverName?.Trim() ?? "");
        //    Client = new RaciClient(Endpoint, Driver?.Name);
        //}

    }
}
