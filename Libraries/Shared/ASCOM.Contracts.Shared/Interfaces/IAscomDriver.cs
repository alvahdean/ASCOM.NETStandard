using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.DeviceInterface
{ 
    public interface IAscomDriver: IDisposable
    {
        bool Connected { get; set; }
        string Description { get; }
        string DriverInfo { get; }
        string DriverVersion { get; }
        short InterfaceVersion { get; }
        string Name { get; }
        ArrayList SupportedActions { get; }

        string Action(string ActionName, string ActionParameters);
        void CommandBlind(string Command, bool Raw);
        bool CommandBool(string Command, bool Raw);
        string CommandString(string Command, bool Raw);
        void SetupDialog();
    }

    public interface IAscomDriverV2 : IAscomDriver
    {

        new ICollection<string> SupportedActions { get; }
    }
}