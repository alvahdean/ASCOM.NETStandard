using System;
using System.Runtime.InteropServices;


namespace ASCOM
{
    /// <summary>
    /// Driver interface to inform Conform of valid driver commands and returned error codes.
    /// </summary>
    /// <remarks></remarks>
    //[Guid("903D0C2C-CE3D-4e27-BD50-3B2C76B7EEE1")]
    //[ComVisible(true)]
    public interface IConform
    {
        /// <summary>
        ///     Error numbers returned for "Not Implemented", "Invalid Value" and "Not Set" error states.
        ///     </summary>
        ///     <value>Expected driver error numbers</value>
        ///     <returns>Expected driver error numbers</returns>
        ///     <remarks></remarks>
        //[DispId(1)]
        ConformErrorNumbers ConformErrors { get; }
        /// <summary>
        ///     Commands to be sent with Raw parameter false
        ///     </summary>
        ///     <value>Set of commands to be sent</value>
        ///     <returns>Set of commands to be sent</returns>
        ///     <remarks></remarks>
        //[DispId(2)]
        ConformCommandStrings ConformCommands { get; }
        /// <summary>
        ///     Commands to be sent with Raw parameter true
        ///     </summary>
        ///     <value>Set of commands to be sent</value>
        ///     <returns>Set of commands to be sent</returns>
        ///     <remarks></remarks>
        //[DispId(3)]
        ConformCommandStrings ConformCommandsRaw { get; }
    }

    /// <summary>
    /// Interface for Conform error numbers class
    /// </summary>
    /// <remarks></remarks>
    //[Guid("676E4D8C-2CD1-4e63-B55A-B0A1C338C6CE")]
    //[ComVisible(true)]
    public interface IConformErrorNumbers
    {
        /// <summary>
        ///     Error number the driver will return for a not implemented error
        ///     </summary>
        ///     <value>NotImplemented error number(s)</value>
        ///     <returns>Array of integer error numbers</returns>
        ///     <remarks>If you only use one error number, set it as element 0 of the array.</remarks>
        //[DispId(1)]
        int[] NotImplemented { get; set; }
        /// <summary>
        ///     Error number the driver will return for an invalid value error
        ///     </summary>
        ///     <value>InvalidValue error number(s)</value>
        ///     <returns>Array of integer error numbers</returns>
        ///     <remarks>If you only use one error number, set it as element 0 of the array.</remarks>
        //[DispId(2)]
        int[] InvalidValue { get; set; }
        /// <summary>
        ///     Error number the driver will return for a value not set error
        ///     </summary>
        ///     <value>NotSetError error number(s)</value>
        ///     <returns>Array of integer error number(s)</returns>
        ///     <remarks>If you only use one error number, set it as element 0 of the array.</remarks>
        //[DispId(3)]
        int[] ValueNotSet { get; set; }
    }

    /// <summary>
    /// Interface for Conform CommandStrings class
    /// </summary>
    /// <remarks></remarks>
    //[Guid("4876295B-6FCC-47c8-AEC9-CF39F1244AEB")]
    //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    //[ComVisible(true)]
    public interface IConformCommandStrings
    {
        /// <summary>
        ///     Command to be sent for CommandString test
        ///     </summary>
        ///     <value>String command to be sent</value>
        ///     <returns>String command that will be sent</returns>
        ///     <remarks></remarks>
        //[DispId(1)]
        string CommandString { get; set; }
        /// <summary>
        ///     Expected respons to CommandString command
        ///     </summary>
        ///     <value>String response expected</value>
        ///     <returns>String response expected</returns>
        ///     <remarks></remarks>
        //[DispId(2)]
        string ReturnString { get; set; }
        /// <summary>
        ///     Command to be sent for CommandBlind test
        ///     </summary>
        ///     <value>String command to be sent</value>
        ///     <returns>String command that will be sent</returns>
        ///     <remarks></remarks>
        //[DispId(3)]
        string CommandBlind { get; set; }
        /// <summary>
        ///     Command to be sent for CommandBlind test
        ///     </summary>
        ///     <value>String command to be sent</value>
        ///     <returns>String command that will be sent</returns>
        ///     <remarks></remarks>
        //[DispId(4)]
        string CommandBool { get; set; }
        /// <summary>
        ///     Command to be sent for CommandBool test
        ///     </summary>
        ///     <value>String command to be sent</value>
        ///     <returns>String command that will be sent</returns>
        ///     <remarks></remarks>
        //[DispId(5)]
        bool ReturnBool { get; set; }
    }
    
}