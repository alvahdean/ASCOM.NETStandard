using System;

namespace ASCOM
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DeviceIdAttribute : Attribute
    {
        public DeviceIdAttribute(string deviceId)
        {
            DeviceId = deviceId;
        }
        public string DeviceId { get; }
        public string DeviceName { get; set; }
    }
}
