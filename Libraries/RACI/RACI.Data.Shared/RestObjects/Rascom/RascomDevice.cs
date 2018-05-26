using System;
using System.Collections.Generic;

namespace RACI.Data
{
    public class RascomDevice
    {
        public String DeviceType { get; protected set; } = "";
        public String Name { get; protected set; } = "";
        public Uri DeviceRoot { get; protected set; } = null;
        public String Description { get; protected set; } = "";
        public bool Connected { get; set; } = false;
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

        public RascomDevice(String devType, String name,String url)
            : this(devType,name, new Uri(url?.Trim() ?? "")) { }

        public RascomDevice(String devType,String name,Uri url)
        {
            DeviceType = devType?.Trim();
            Name = name?.Trim();
            DeviceRoot = url;
            Description = $"A {devType} for {Name}";
            Connected = false;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class RascomFocuser : RascomDevice
    {
        public RascomFocuser(String name, String url)
            : this(name, new Uri(url?.Trim() ?? "")) { }

        public RascomFocuser(String name, Uri url)
            : base("Focuser",name,url)
        {
            Name = name?.Trim();
            DeviceRoot = url;
            Description = $"A {DeviceType} for {Name}";
            Connected = false;
        }
    }
}
