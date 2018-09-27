using System;
using System.Runtime.InteropServices;


namespace ASCOM
{

    /// <summary>
    /// Contains error numbers that the driver will return when expected invalid conditions occur. 
    /// </summary>
    /// <remarks></remarks>
    //[Guid("A8CFEEE1-F27B-4a68-AF05-5F6CCE3F1257")]
    //[ComVisible(true)]
    //[ClassInterface(ClassInterfaceType.None)]
    public class ConformErrorNumbers : IConformErrorNumbers
    {
        private int[] errNotImplemented, errInvalidValue, errValueNotSet;

        /// <summary>
        ///     Creates a new instance of the error numbers class.
        ///     </summary>
        ///     <remarks></remarks>
        public ConformErrorNumbers() // COM visible setup
        {
        }

        /// <summary>
        ///     Set not implemented, invalid value and value not set error codes in one call
        ///     </summary>
        ///     <param name="NotImplemented">Error number for not implemented exceptions</param>
        ///     <param name="InvalidValue1">Error number for first invalid value exception</param>
        ///     <param name="InvalidValue2">Error number for second invalid value exception</param>
        ///     <param name="ValueNotSet">Error number for value not set exception</param>
        ///     <remarks>Use this call if you have one "not implemented", up to two "invalid value" and one
        ///     "value not set" error codes. If you have more than this number of distinct error numbers,
        ///     use the other nethods in the class to set them.</remarks>
        public ConformErrorNumbers(int NotImplemented, int InvalidValue1, int InvalidValue2, int ValueNotSet)
        {
            errNotImplemented = new int[1];
            errInvalidValue = new int[2];
            errValueNotSet = new int[1];
            errNotImplemented[0] = NotImplemented;
            errInvalidValue[0] = InvalidValue1;
            errInvalidValue[1] = InvalidValue2;
            errValueNotSet[0] = ValueNotSet;
        }

        /// <summary>
        ///     Set not implemented, invalid value and value not set error codes in one call
        ///     </summary>
        ///     <param name="NotImplemented">Array of "not implemented" error numbers</param>
        ///     <param name="InvalidValue">Array of "invalid value" error numbers</param>
        ///     <param name="ValueNotSet">Array of "value not set" error numbers</param>
        ///     <remarks>Use this call to set any number of error codes in each category.</remarks>
        public ConformErrorNumbers(int[] NotImplemented, int[] InvalidValue, int[] ValueNotSet)
        {
            this.NotImplemented = NotImplemented;
            this.InvalidValue = InvalidValue;
            this.ValueNotSet = ValueNotSet;
        }

        /// <summary>
        ///     Error code(s) returned for "Not Implemented" errors.
        ///     </summary>
        ///     <value>"Not implemented" error codes</value>
        ///     <returns>Array of integer error numbers</returns>
        ///     <remarks></remarks>
        public int[] NotImplemented
        {
            get
            {
                return errNotImplemented;
            }
            set
            {
                errNotImplemented = value;
            }
        }

        /// <summary>
        ///     Error code(s) returned for "Invalid Value" errors.
        ///     </summary>
        ///     <value>"Invalid value" error codes</value>
        ///     <returns>Array of integer error numbers</returns>
        ///     <remarks></remarks>
        public int[] InvalidValue
        {
            get
            {
                return errInvalidValue;
            }
            set
            {
                errInvalidValue = value;
            }
        }

        /// <summary>
        ///     Error code(s) returned for "Value Not Set" errors.
        ///     </summary>
        ///     <value>"Value not set" error codes</value>
        ///     <returns>Array of integer error numbers</returns>
        ///     <remarks></remarks>
        public int[] ValueNotSet
        {
            get
            {
                return errValueNotSet;
            }
            set
            {
                errValueNotSet = value;
            }
        }
    }

    /// <summary>
    /// The device specific commands and expected responses to be used by Conform when testing the 
    /// CommandXXX commands.
    /// </summary>
    /// <remarks></remarks>
    //[Guid("F6774C71-BA75-4400-B8DB-20960D373170")]
    //[ComVisible(true)]
    //[ClassInterface(ClassInterfaceType.None)]
    public class ConformCommandStrings : IConformCommandStrings
    {
        private string cmdBlind, cmdBool, cmdString;
        private string rtnString;
        private bool rtnBool;

        /// <summary>
        ///     Set all Conform CommandXXX commands and expected responses in one call
        ///     </summary>
        ///     <param name="CommandString">String to be sent through CommandString method</param>
        ///     <param name="ReturnString">Expected return value from CommandString command</param>
        ///     <param name="CommandBlind">String to be sent through CommandBling method</param>
        ///     <param name="CommandBool">Expected return value from CommandBlind command</param>
        ///     <param name="ReturnBool">Expected boolean response from CommandBool command</param>
        ///     <remarks>To suppress a Command XXX test, set the command and return values to Nothing (VB) 
        ///     or null (C#). To accept any response to a command just set the return value to Nothing or null
        ///     as appropriate.</remarks>
        public ConformCommandStrings(string CommandString, string ReturnString, string CommandBlind, string CommandBool, bool ReturnBool)
        {
            this.CommandString = CommandString;
            this.CommandBlind = CommandBlind;
            this.CommandBool = CommandBool;
            this.ReturnString = ReturnString;
            this.ReturnBool = ReturnBool;
        }

        /// <summary>
        ///     Create a new CommandStrings object with all fields set to Nothing (VB) / null (C#), use 
        ///     other properties to set commands and expected return values.
        ///     </summary>
        ///     <remarks></remarks>
        public ConformCommandStrings()
        {
            this.CommandString = null;
            this.CommandBlind = null;
            this.CommandBool = null;
            this.ReturnString = null;
            this.ReturnBool = false;
        }

        /// <summary>
        ///     Set the command to be sent by the Conform CommandString test
        ///     </summary>
        ///     <value>Device command</value>
        ///     <returns>String device command</returns>
        ///     <remarks></remarks>
        public string CommandString
        {
            get
            {
                return cmdString;
            }
            set
            {
                cmdString = value;
            }
        }

        /// <summary>
        ///     Set the expected return value from the CommandString command
        ///     </summary>
        ///     <value>Device response</value>
        ///     <returns>String device response</returns>
        ///     <remarks></remarks>
        public string ReturnString
        {
            get
            {
                return rtnString;
            }
            set
            {
                rtnString = value;
            }
        }

        /// <summary>
        ///     Set the command to be sent by the Conform CommandBlind test
        ///     </summary>
        ///     <value>Device command</value>
        ///     <returns>String device command</returns>
        ///     <remarks></remarks>
        public string CommandBlind
        {
            get
            {
                return cmdBlind;
            }
            set
            {
                cmdBlind = value;
            }
        }

        /// <summary>
        ///     Set the command to be sent by the Conform CommandBool test
        ///     </summary>
        ///     <value>Device command</value>
        ///     <returns>String device command</returns>
        ///     <remarks></remarks>
        public string CommandBool
        {
            get
            {
                return cmdBool;
            }
            set
            {
                cmdBool = value;
            }
        }

        /// <summary>
        ///     Set the expected return value from the CommandBool command
        ///     </summary>
        ///     <value>Device response</value>
        ///     <returns>Boolean device response</returns>
        ///     <remarks></remarks>
        public bool ReturnBool
        {
            get
            {
                return rtnBool;
            }
            set
            {
                rtnBool = value;
            }
        }
    }
}