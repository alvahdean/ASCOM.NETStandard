using System;
using System.Collections.Generic;

namespace RACI.Data
{
    public class DriverMeta
    {
        public DriverMeta() { }
        public DriverMeta(String devType, String name, String url)
            : this(devType, name, new Uri(url?.Trim() ?? "")) { }
        public DriverMeta(String devType, String name, Uri url)
        {
            DeviceType = (devType ?? GetType().Name.Replace("Meta", "")).Trim();
            Name = name?.Trim();
            DeviceRoot = url;
            Description = $"A {devType} for {Name}";
        }
        public String DeviceType { get; set; } = "";
        public String Name { get; set; } = "";
        public Uri DeviceRoot { get; set; } = null;
        public String Description { get; set; } = "";
        public bool IsValid
        {
            get
            {
                return
                    !String.IsNullOrWhiteSpace(Name)
                    && !String.IsNullOrWhiteSpace(DeviceType)
                    && !String.IsNullOrWhiteSpace(DeviceRoot?.DnsSafeHost)
                    && (DeviceRoot.Scheme=="http" || DeviceRoot.Scheme=="https");
            }
        }
    }

    public class DomeMeta : DriverMeta
    {
        public DomeMeta() : base() { }
        public DomeMeta(String name, String url) : this(name, new Uri(url?.Trim() ?? "")) { }
        public DomeMeta(String name, Uri url) : base("Dome", name, url) { }
    }

    public class FocuserMeta : DriverMeta
    {
        public FocuserMeta() : base() { }
        public FocuserMeta(String name, String url) : this(name, new Uri(url?.Trim() ?? "")) { }
        public FocuserMeta(String name, Uri url) : base("Focuser", name, url) { }
    }
}
