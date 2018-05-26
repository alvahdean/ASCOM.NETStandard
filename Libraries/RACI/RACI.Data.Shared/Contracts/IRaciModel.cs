using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using RACI.Data;

namespace RACI.Data
{
    public interface IRaciModel 
    {
        DbSet<SimpleValue> SimpleValues { get; set; }
        DbSet<ProfileValue> ProfileValues { get; set; }
        DbSet<ProfileNode> ProfileNodes { get; set; }
        DbSet<AscomDeviceNode> DeviceRoots { get; set; }
        DbSet<AscomDriverNode> DriverTypes { get; set; }
        DbSet<RaciUser> Users { get; set; }
        DbSet<AscomPlatformNode> AscomPlatforms { get; set; }
        DbSet<RaciSettings> SystemSettings { get; set; }
        DbSet<RaciSystem> Systems { get; set; }
    }
}