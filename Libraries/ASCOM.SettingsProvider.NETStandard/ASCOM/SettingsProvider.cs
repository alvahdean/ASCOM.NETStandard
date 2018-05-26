// Decompiled with JetBrains decompiler
// Type: ASCOM.SettingsProvider
// Assembly: ASCOM.SettingsProvider, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 529D6A0A-5BB4-47DC-9909-E8E6ECFC7145
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.SettingsProvider\6.0.0.0__565de7938946fba7\ASCOM.SettingsProvider.dll

using System;
using System.Collections.Specialized;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using ASCOM.Internal;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;
using TiGra;
using System.Collections.Generic;

namespace ASCOM
{
    public class SettingsProvider : ConfigurationProvider
    {
        private string appName = string.Empty;
        private readonly IProfile ascomProfile;

        public string Name
        {
            get
            {
                return "ASCOM Settings Provider";
            }
        }

        public string Description
        {
            get
            {
                return "Stores settings in the ASCOM Device Profile store.";
            }
        }

        public string ApplicationName
        {
            get
            {
                return this.appName;
            }
            set
            {
                Diagnostics.TraceWarning("Unexpected setting of ApplicationName to {0}", (object)value);
                this.appName = value;
            }
        }

        public SettingsProvider()
        {
            this.ascomProfile = (IProfile)new Profile(true);
        }

        public SettingsProvider(IProfile profileProvider)
        {
            this.ascomProfile = profileProvider;
        }

        public void Initialize(string name, NameValueCollection config)
        {
            this.appName = Assembly.GetCallingAssembly().GetName().Name;
            //base.Initialize(name ?? this.appName, config);
            this.ApplicationName = name ?? this.appName;
        }

        public IConfiguration Configuration { get; set; }

        //public NameValueCollection GetPropertyValues(IConfigurationRoot collection)
        //{
        //    Diagnostics.TraceInfo("Retrieving ASCOM Profile Properties for DeviceID={0}, {1} properties", (object)this.ApplicationName, (object)collection.Count);
        //    NameValueCollection propertyValueCollection = new NameValueCollection();
        //    IConfigurationSection section =collection.GetSection("");
        //    foreach (KeyValuePair<string,string> property1 in section.AsEnumerable())
        //    {
        //        string str1 = (string)null;
        //        string str2 = (string)null;
        //        string str3;
        //        try
        //        {
        //            DeviceIdAttribute attribute = property1.Attributes[(object)typeof(DeviceIdAttribute)] as DeviceIdAttribute;
        //            if (attribute == null)
        //            {
        //                Diagnostics.TraceError("Setting {0} is not decorated with a DeviceID attribute.", (object)property2.Name);
        //                continue;
        //            }
        //            str3 = attribute.DeviceId;
        //            int length = str3.LastIndexOf('.');
        //            str1 = str3.Head(length);
        //            str2 = str3.RemoveHead(length + 1);
        //            Diagnostics.TraceVerbose("Parsed DeviceID as {0}.{1}", (object)str1, (object)str2);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (string.IsNullOrEmpty(str1))
        //                str1 = "Unnamed";
        //            if (string.IsNullOrEmpty(str2))
        //                str2 = "Non-Device";
        //            str3 = string.Format("{0}.{1}", (object)str1, (object)str2);
        //            Diagnostics.TraceWarning("Unable to parse DeviceID, using {0}.{1}", (object)str1, (object)str2);
        //        }
        //        this.ascomProfile.DeviceType = str2;
        //        SettingsProvider.EnsureRegistered(this.ascomProfile, str3);
        //        try
        //        {
        //            string str4 = this.ascomProfile.GetValue(str3, property1.Name, (string)null, string.Empty);
        //            if (string.IsNullOrEmpty(str4))
        //            {
        //                property2.SerializedValue = property1.DefaultValue;
        //                Diagnostics.TraceVerbose("Defaulted/empty ASCOM Profile DeviceID={0}, Key={1}, Value={2}", (object)str3, (object)property1.Name, (object)property1.DefaultValue.ToString());
        //            }
        //            else
        //            {
        //                property2.SerializedValue = (object)str4;
        //                Diagnostics.TraceVerbose("Retrieved ASCOM Profile DeviceID={0}, Key={1}, Value={2}", (object)str3, (object)property1.Name, (object)str4);
        //            }
        //        }
        //        catch
        //        {
        //            property2.SerializedValue = property2.Property.DefaultValue;
        //            Diagnostics.TraceVerbose("Defaulted/missing ASCOM Profile DeviceID={0}, Key={1}, Value={2}", (object)str3, (object)property1.Name, property2.PropertyValue);
        //        }
        //        property2.IsDirty = false;
        //        propertyValueCollection.Add(property2);
        //    }
        //    return propertyValueCollection;
        //}

/**/

        //public void SetPropertyValues(SettingsContext context, IConfigurationRoot collection)
        //{
        //    Diagnostics.TraceInfo("Persisting ASCOM Profile Properties for DeviceID={0}, {1} properties", (object)this.ApplicationName, (object)collection.Count);
        //    foreach (SettingsPropertyValue settingsPropertyValue in collection)
        //    {
        //        if (settingsPropertyValue.Property.Attributes.ContainsKey((object)typeof(DeviceIdAttribute)))
        //        {
        //            DeviceIdAttribute attribute = settingsPropertyValue.Property.Attributes[(object)typeof(DeviceIdAttribute)] as DeviceIdAttribute;
        //            if (attribute != null)
        //            {
        //                string deviceId = attribute.DeviceId;
        //                int length = deviceId.LastIndexOf('.');
        //                deviceId.Head(length);
        //                this.ascomProfile.DeviceType = deviceId.RemoveHead(length + 1);
        //                SettingsProvider.EnsureRegistered(this.ascomProfile, deviceId);
        //                try
        //                {
        //                    Diagnostics.TraceVerbose("Writing ASCOM Profile DeviceID={0}, Key={1}, Value={2}", (object)deviceId, (object)settingsPropertyValue.Name, settingsPropertyValue.SerializedValue);
        //                    this.ascomProfile.WriteValue(deviceId, settingsPropertyValue.Name, settingsPropertyValue.SerializedValue.ToString(), string.Empty);
        //                }
        //                catch
        //                {
        //                    Diagnostics.TraceError("Failed to persist property Key={0} - make sure your driver is properly registered", (object)settingsPropertyValue.Name);
        //                }
        //            }
        //            else
        //                Diagnostics.TraceWarning("Property name {0} did not have a DeviceId attribute", (object)settingsPropertyValue.Name);
        //        }
        //    }
        //}

        private static void EnsureRegistered(IProfile ascomProfile, string driverId)
        {
            if (ascomProfile.IsRegistered(driverId))
                return;
            ascomProfile.Register(driverId, driverId + " Auto-registered by SettingsProvider");
            Diagnostics.TraceWarning("Your driver has been auto-registered with ASCOM.Utilities.profile for easy debugging. You must provide a correct registration in your setup before deploying to an end user system.", new object[0]);
        }
    }

  //  public class SettingsProvider : System.Configuration.SettingsProvider
  //{
  //  private string appName = string.Empty;
  //  private readonly IProfile ascomProfile;

  //  public override string Name
  //  {
  //    get
  //    {
  //      return "ASCOM Settings Provider";
  //    }
  //  }

  //  public override string Description
  //  {
  //    get
  //    {
  //      return "Stores settings in the ASCOM Device Profile store.";
  //    }
  //  }

  //  public override string ApplicationName
  //  {
  //    get
  //    {
  //      return this.appName;
  //    }
  //    set
  //    {
  //      Diagnostics.TraceWarning("Unexpected setting of ApplicationName to {0}", (object) value);
  //      this.appName = value;
  //    }
  //  }

  //  public SettingsProvider()
  //  {
  //    this.ascomProfile = (IProfile) new Profile(true);
  //  }

  //  public SettingsProvider(IProfile profileProvider)
  //  {
  //    this.ascomProfile = profileProvider;
  //  }

  //  public override void Initialize(string name, NameValueCollection config)
  //  {
  //    this.appName = Assembly.GetCallingAssembly().GetName().Name;
  //    base.Initialize(name ?? this.appName, config);
  //    this.ApplicationName = name ?? this.appName;
  //  }

  //  public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
  //  {
  //    Diagnostics.TraceInfo("Retrieving ASCOM Profile Properties for DeviceID={0}, {1} properties", (object) this.ApplicationName, (object) collection.Count);
  //    SettingsPropertyValueCollection propertyValueCollection = new SettingsPropertyValueCollection();
  //    foreach (SettingsProperty property1 in collection)
  //    {
  //      SettingsPropertyValue property2 = new SettingsPropertyValue(property1);
  //      string str1 = (string) null;
  //      string str2 = (string) null;
  //      string str3;
  //      try
  //      {
  //        DeviceIdAttribute attribute = property1.Attributes[(object) typeof (DeviceIdAttribute)] as DeviceIdAttribute;
  //        if (attribute == null)
  //        {
  //          Diagnostics.TraceError("Setting {0} is not decorated with a DeviceID attribute.", (object) property2.Name);
  //          continue;
  //        }
  //        str3 = attribute.DeviceId;
  //        int length = str3.LastIndexOf('.');
  //        str1 = str3.Head(length);
  //        str2 = str3.RemoveHead(length + 1);
  //        Diagnostics.TraceVerbose("Parsed DeviceID as {0}.{1}", (object) str1, (object) str2);
  //      }
  //      catch (Exception ex)
  //      {
  //        if (string.IsNullOrEmpty(str1))
  //          str1 = "Unnamed";
  //        if (string.IsNullOrEmpty(str2))
  //          str2 = "Non-Device";
  //        str3 = string.Format("{0}.{1}", (object) str1, (object) str2);
  //        Diagnostics.TraceWarning("Unable to parse DeviceID, using {0}.{1}", (object) str1, (object) str2);
  //      }
  //      this.ascomProfile.DeviceType = str2;
  //      SettingsProvider.EnsureRegistered(this.ascomProfile, str3);
  //      try
  //      {
  //        string str4 = this.ascomProfile.GetValue(str3, property1.Name, (string) null, string.Empty);
  //        if (string.IsNullOrEmpty(str4))
  //        {
  //          property2.SerializedValue = property1.DefaultValue;
  //          Diagnostics.TraceVerbose("Defaulted/empty ASCOM Profile DeviceID={0}, Key={1}, Value={2}", (object) str3, (object) property1.Name, (object) property1.DefaultValue.ToString());
  //        }
  //        else
  //        {
  //          property2.SerializedValue = (object) str4;
  //          Diagnostics.TraceVerbose("Retrieved ASCOM Profile DeviceID={0}, Key={1}, Value={2}", (object) str3, (object) property1.Name, (object) str4);
  //        }
  //      }
  //      catch
  //      {
  //        property2.SerializedValue = property2.Property.DefaultValue;
  //        Diagnostics.TraceVerbose("Defaulted/missing ASCOM Profile DeviceID={0}, Key={1}, Value={2}", (object) str3, (object) property1.Name, property2.PropertyValue);
  //      }
  //      property2.IsDirty = false;
  //      propertyValueCollection.Add(property2);
  //    }
  //    return propertyValueCollection;
  //  }

  //  public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
  //  {
  //    Diagnostics.TraceInfo("Persisting ASCOM Profile Properties for DeviceID={0}, {1} properties", (object) this.ApplicationName, (object) collection.Count);
  //    foreach (SettingsPropertyValue settingsPropertyValue in collection)
  //    {
  //      if (settingsPropertyValue.Property.Attributes.ContainsKey((object) typeof (DeviceIdAttribute)))
  //      {
  //        DeviceIdAttribute attribute = settingsPropertyValue.Property.Attributes[(object) typeof (DeviceIdAttribute)] as DeviceIdAttribute;
  //        if (attribute != null)
  //        {
  //          string deviceId = attribute.DeviceId;
  //          int length = deviceId.LastIndexOf('.');
  //          deviceId.Head(length);
  //          this.ascomProfile.DeviceType = deviceId.RemoveHead(length + 1);
  //          SettingsProvider.EnsureRegistered(this.ascomProfile, deviceId);
  //          try
  //          {
  //            Diagnostics.TraceVerbose("Writing ASCOM Profile DeviceID={0}, Key={1}, Value={2}", (object) deviceId, (object) settingsPropertyValue.Name, settingsPropertyValue.SerializedValue);
  //            this.ascomProfile.WriteValue(deviceId, settingsPropertyValue.Name, settingsPropertyValue.SerializedValue.ToString(), string.Empty);
  //          }
  //          catch
  //          {
  //            Diagnostics.TraceError("Failed to persist property Key={0} - make sure your driver is properly registered", (object) settingsPropertyValue.Name);
  //          }
  //        }
  //        else
  //          Diagnostics.TraceWarning("Property name {0} did not have a DeviceId attribute", (object) settingsPropertyValue.Name);
  //      }
  //    }
  //  }

  //  private static void EnsureRegistered(IProfile ascomProfile, string driverId)
  //  {
  //    if (ascomProfile.IsRegistered(driverId))
  //      return;
  //    ascomProfile.Register(driverId, driverId + " Auto-registered by SettingsProvider");
  //    Diagnostics.TraceWarning("Your driver has been auto-registered with ASCOM.Utilities.profile for easy debugging. You must provide a correct registration in your setup before deploying to an end user system.", new object[0]);
  //  }
  //}

}
