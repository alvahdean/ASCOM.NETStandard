using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.Utilities.Interfaces
{
    public interface IAscomException
    {
        //
        // Summary:
        //     Gets a string representation of the immediate frames on the call stack.
        //
        // Returns:
        //     A string that describes the immediate frames of the call stack.
        string StackTrace { get; }
        //
        // Summary:
        //     Gets or sets the name of the application or the object that causes the error.
        //
        // Returns:
        //     The name of the application or the object that causes the error.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The object must be a runtime System.Reflection object
        string Source { get; set; }
        //
        // Summary:
        //     Gets a message that describes the current exception.
        //
        // Returns:
        //     The error message that explains the reason for the exception, or an empty string
        //     ("").
        string Message { get; }
        //
        // Summary:
        //     Gets the System.Exception instance that caused the current exception.
        //
        // Returns:
        //     An object that describes the error that caused the current exception. The System.Exception.InnerException
        //     property returns the same value as was passed into the System.Exception.#ctor(System.String,System.Exception)
        //     constructor, or null if the inner exception value was not supplied to the constructor.
        //     This property is read-only.
        Exception InnerException { get; }
        //
        // Summary:
        //     Gets or sets HRESULT, a coded numerical value that is assigned to a specific
        //     exception.
        //
        // Returns:
        //     The HRESULT value.
        int HResult { get; }

        //
        // Summary:
        //     Gets a collection of key/value pairs that provide additional user-defined information
        //     about the exception.
        //
        // Returns:
        //     An object that implements the System.Collections.IDictionary interface and contains
        //     a collection of user-defined key/value pairs. The default is an empty collection.
        IDictionary Data { get; }
        //
        // Summary:
        //     Gets the method that throws the current exception.
        //
        // Returns:
        //     The System.Reflection.MethodBase that threw the current exception.
        /*MethodBase TargetSite { get; }*/

        //
        // Summary:
        //     Gets the method that throws the current exception.
        //
        // Returns:
        //     The System.Reflection.MethodBase that threw the current exception.
        String TargetSiteName { get; }

        //
        // Summary:
        //     Gets or sets a link to the help file associated with this exception.
        //
        // Returns:
        //     The Uniform Resource Name (URN) or Uniform Resource Locator (URL).
        string HelpLink { get; set; }

        //
        // Summary:
        //     Occurs when an exception is serialized to create an exception state object that
        //     contains serialized data about the exception.
        /*event EventHandler<SafeSerializationEventArgs> SerializeObjectState;*/

        //
        // Summary:
        //     When overridden in a derived class, returns the System.Exception that is the
        //     root cause of one or more subsequent exceptions.
        //
        // Returns:
        //     The first exception thrown in a chain of exceptions. If the System.Exception.InnerException
        //     property of the current exception is a null reference (Nothing in Visual Basic),
        //     this property returns the current exception.
        Exception GetBaseException();

        //
        // Summary:
        //     When overridden in a derived class, sets the System.Runtime.Serialization.SerializationInfo
        //     with information about the exception.
        //
        // Parameters:
        //   info:
        //     The System.Runtime.Serialization.SerializationInfo that holds the serialized
        //     object data about the exception being thrown.
        //
        //   context:
        //     The System.Runtime.Serialization.StreamingContext that contains contextual information
        //     about the source or destination.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The info parameter is a null reference (Nothing in Visual Basic).
        /*void GetObjectData(SerializationInfo info, StreamingContext context);*/

        //
        // Summary:
        //     Gets the runtime type of the current instance.
        //
        // Returns:
        //     A System.Type object that represents the exact runtime type of the current instance.
        Type GetType();
    }
}
