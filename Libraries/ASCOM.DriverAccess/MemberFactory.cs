using ASCOM.Utilities;
using ASCOM.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading.Tasks;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.DriverAccess
{
    internal class MemberFactory : IDisposable
    {
        private ITraceLogger TL;

        private readonly string _strProgId;

        internal object GetLateBoundObject { get; private set; }

        internal bool IsComObject { get; private set; }

        internal Type GetObjType { get; private set; }

        internal List<Type> GetInterfaces { get; private set; }

        internal MemberFactory(string progId, TraceLogger ascomDriverTraceLogger)
        {
            this.TL = ascomDriverTraceLogger;
            this.TL.LogMessage("ProgID", progId);
            this._strProgId = progId;
            this.GetInterfaces = new List<Type>();
            this.GetObjType = Type.GetTypeFromProgID(progId);
            if (this.GetObjType == null)
                throw new HelperException("Check Driver: cannot create object type of progID: " + this._strProgId);
            this.IsComObject = this.GetObjType.IsCOMObject;
            this.TL.LogMessage("IsComObject", this.GetObjType.IsCOMObject.ToString());
            this.GetLateBoundObject = Activator.CreateInstance(this.GetObjType);
            try
            {
                foreach (Type type in this.GetObjType.GetInterfaces())
                {
                    this.GetInterfaces.Add(type);
                    this.TL.LogMessage("GetInterfaces", "Found interface: " + type.AssemblyQualifiedName);
                }
            }
            catch (Exception ex)
            {
                this.TL.LogMessageCrLf("GetInterfaces", "Exception: " + ex.ToString());
            }
            if (this.GetLateBoundObject == null)
            {
                this.TL.LogMessage("Exception", "GetLateBoudObject is null, throwing HelperException");
                throw new HelperException("Check Driver: cannot create driver instance of progID: " + this._strProgId);
            }
        }

        public void Dispose()
        {
            if (this.GetLateBoundObject == null)
                return;
            try
            {
                if (this.IsComObject)
                {
                    try
                    {
                        this.TL.LogMessageCrLf("Dispose COM", "This is a COM object, attempting to call its Dispose method");
                        this.GetObjType.InvokeMember("Dispose", BindingFlags.InvokeMethod, (Binder)null, this.GetLateBoundObject, new object[0], CultureInfo.InvariantCulture);
                        this.TL.LogMessageCrLf("Dispose COM", "Successfully called its Dispose method");
                    }
                    catch (COMException ex)
                    {
                        if (ex.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                            this.TL.LogMessageCrLf("Dispose COM", "Driver does not have a Dispose method");
                    }
                    catch (Exception ex)
                    {
                        this.TL.LogMessageCrLf("Dispose COM", "Exception " + ex.ToString());
                    }
                    this.TL.LogMessageCrLf("Dispose COM", "This is a COM object so attempting to release it");
                    int num = Marshal.ReleaseComObject(this.GetLateBoundObject);
                    if (num == 0)
                        this.GetLateBoundObject = (object)null;
                    this.TL.LogMessageCrLf("Dispose COM", "Object Count is now: " + (object)num);
                }
                else
                {
                    this.TL.LogMessageCrLf("Dispose .NET", "This is a .NET object, attempting to call its Dispose method");
                    MethodInfo method = this.GetObjType.GetMethod("Dispose");
                    if (method != null)
                    {
                        this.TL.LogMessage("Dispose .NET", "  Got Dispose Method Info, Calling Dispose");
                        method.Invoke(this.GetLateBoundObject, new object[0]);
                        this.TL.LogMessage("Dispose .NET", "  Successfully called Dispose method");
                    }
                    else
                        this.TL.LogMessage("Dispose .NET", "  No Dispose Method Info so ignoring the call to Dispose");
                }
            }
            catch (Exception ex)
            {
                try
                {
                    this.TL.LogMessageCrLf("Dispose", "Exception " + ex.ToString());
                }
                catch
                {
                }
            }
        }

        internal object CallMember(int memberCode, string memberName, Type[] parameterTypes, params object[] parms)
        {
            this.TL.BlankLine();
            switch (memberCode)
            {
                case 1:
                    PropertyInfo property1 = this.GetObjType.GetProperty(memberName);
                    if (property1 != null)
                    {
                        this.TL.LogMessage(memberName + " Get", "GET " + memberName + " - .NET");
                        try
                        {
                            object obj = property1.GetValue(this.GetLateBoundObject, (object[])null);
                            this.TL.LogMessage(memberName + " Get", "  " + obj.ToString());
                            return obj;
                        }
                        catch (TargetInvocationException ex)
                        {
                            this.GetTargetInvocationExceptionHandler(memberName, (Exception)ex);
                        }
                        catch (Exception ex)
                        {
                            this.TL.LogMessageCrLf("Exception", ex.ToString());
                            throw;
                        }
                    }
                    if (this.IsComObject)
                    {
                        this.TL.LogMessage(memberName + " Get", "GET " + memberName + " - COM");
                        try
                        {
                            object obj = this.GetObjType.InvokeMember(memberName, BindingFlags.GetProperty, (Binder)null, this.GetLateBoundObject, new object[0], CultureInfo.InvariantCulture);
                            this.TL.LogMessage(memberName + " Get", "  " + obj.ToString());
                            return obj;
                        }
                        catch (COMException ex)
                        {
                            this.TL.LogMessageCrLf("COMException", ex.ToString());
                            if (ex.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                            {
                                this.TL.LogMessageCrLf(memberName + " Get", "  Throwing PropertyNotImplementedException: " + this._strProgId + " " + memberName);
                                throw new PropertyNotImplementedException(this._strProgId + " " + memberName, false, (Exception)ex);
                            }
                            this.TL.LogMessageCrLf(memberName + " Get", "Re-throwing exception");
                            throw;
                        }
                        catch (TargetInvocationException ex)
                        {
                            this.TL.LogMessageCrLf("TargetInvocationException", ex.ToString());
                            if (ex.InnerException is COMException)
                            {
                                string message = ex.InnerException.Message;
                                int errorCode = ((ExternalException)ex.InnerException).ErrorCode;
                                if (errorCode == int.Parse("80040400", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                                {
                                    this.TL.LogMessageCrLf(memberName + " Get", "  Translating COM not implemented exception to PropertyNotImplementedException: " + this._strProgId + " " + memberName);
                                    throw new PropertyNotImplementedException(this._strProgId + " " + memberName, false, (Exception)ex);
                                }
                                this.TL.LogMessageCrLf(memberName + " Get", "COM Exception so throwing inner exception: '" + message + "' '0x" + string.Format("{0:x8}", (object)errorCode) + "'");
                                throw new DriverAccessCOMException(message, errorCode, (Exception)ex);
                            }
                            this.GetTargetInvocationExceptionHandler(memberName, (Exception)ex);
                        }
                        catch (Exception ex)
                        {
                            this.TL.LogMessageCrLf("Exception", ex.ToString());
                            throw;
                        }
                    }
                    this.TL.LogMessage(memberName + " Get", "  The object is neither a .NET object nor a COM object!");
                    throw new PropertyNotImplementedException(this._strProgId + " " + memberName, false);
                case 2:
                    PropertyInfo property2 = this.GetObjType.GetProperty(memberName);
                    if (property2 != null)
                    {
                        this.TL.LogMessage(memberName + " Set", "SET " + memberName + " - .NET");
                        try
                        {
                            this.TL.LogMessage(memberName + " Set", "  " + parms[0].ToString());
                            property2.SetValue(this.GetLateBoundObject, parms[0], (object[])null);
                            return (object)null;
                        }
                        catch (TargetInvocationException ex)
                        {
                            this.SetTargetInvocationExceptionHandler(memberName, (Exception)ex);
                        }
                        catch (Exception ex)
                        {
                            this.TL.LogMessageCrLf("Exception", ex.ToString());
                            throw;
                        }
                    }
                    if (this.IsComObject)
                    {
                        this.TL.LogMessage(memberName + " Set", "SET " + memberName + " - COM");
                        try
                        {
                            this.TL.LogMessage(memberName + " Set", "  " + parms[0].ToString());
                            this.GetObjType.InvokeMember(memberName, BindingFlags.SetProperty, (Binder)null, this.GetLateBoundObject, parms, CultureInfo.InvariantCulture);
                            return (object)null;
                        }
                        catch (COMException ex)
                        {
                            this.TL.LogMessageCrLf("COMException", ex.ToString());
                            if (ex.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                            {
                                this.TL.LogMessageCrLf(memberName + " Set", "  Throwing PropertyNotImplementedException: " + this._strProgId + " " + memberName);
                                throw new PropertyNotImplementedException(this._strProgId + " " + memberName, true, (Exception)ex);
                            }
                            this.TL.LogMessageCrLf(memberName + " Set", "  Re-throwing exception");
                            throw;
                        }
                        catch (TargetInvocationException ex)
                        {
                            this.TL.LogMessageCrLf("TargetInvocationException", ex.ToString());
                            if (ex.InnerException is COMException)
                            {
                                string message = ex.InnerException.Message;
                                int errorCode = ((ExternalException)ex.InnerException).ErrorCode;
                                if (errorCode == int.Parse("80040400", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                                {
                                    this.TL.LogMessageCrLf(memberName + " Set", "  Translating COM not implemented exception to PropertyNotImplementedException: " + this._strProgId + " " + memberName);
                                    throw new PropertyNotImplementedException(this._strProgId + " " + memberName, true, (Exception)ex);
                                }
                                this.TL.LogMessageCrLf(memberName + " Set", "COM Exception so throwing inner exception: '" + message + "' '0x" + string.Format("{0:x8}", (object)errorCode) + "'");
                                throw new DriverAccessCOMException(message, errorCode, (Exception)ex);
                            }
                            this.SetTargetInvocationExceptionHandler(memberName, (Exception)ex);
                        }
                        catch (Exception ex)
                        {
                            this.TL.LogMessageCrLf("Exception", ex.ToString());
                            throw;
                        }
                    }
                    this.TL.LogMessage("PropertySet", "  The object is neither a .NET object nor a COM object!");
                    throw new PropertyNotImplementedException(this._strProgId + " " + memberName, true);
                case 3:
                    this.TL.LogMessage(memberName, "Start");
                    MethodInfo method = this.GetObjType.GetMethod(memberName);
                    if (method != null)
                    {
                        try
                        {
                            foreach (object parm in parms)
                                this.TL.LogMessage(memberName, "  Parameter: " + parm.ToString());
                            this.TL.LogMessage(memberName, "  Calling " + memberName);
                            object obj = method.Invoke(this.GetLateBoundObject, parms);
                            if (obj == null)
                                this.TL.LogMessage(memberName, "  Successfully called method, no return value");
                            else
                                this.TL.LogMessage(memberName, "  " + obj.ToString());
                            return obj;
                        }
                        catch (TargetInvocationException ex)
                        {
                            this.MethodTargetInvocationExceptionHandler(memberName, (Exception)ex);
                        }
                        catch (Exception ex)
                        {
                            this.TL.LogMessageCrLf("Exception", ex.ToString());
                            throw;
                        }
                    }
                    if (this.IsComObject)
                    {
                        try
                        {
                            foreach (object parm in parms)
                                this.TL.LogMessage(memberName, "  Parameter: " + parm.ToString());
                            this.TL.LogMessage(memberName, "  Calling " + memberName + " - it is a COM object");
                            object obj = this.GetObjType.InvokeMember(memberName, BindingFlags.InvokeMethod, (Binder)null, this.GetLateBoundObject, parms, CultureInfo.InvariantCulture);
                            if (obj == null)
                                this.TL.LogMessage(memberName, "  Successfully called method, no return value");
                            else
                                this.TL.LogMessage(memberName, "  " + obj.ToString());
                            return obj;
                        }
                        catch (COMException ex)
                        {
                            this.TL.LogMessageCrLf("COMException", ex.ToString());
                            if (ex.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                            {
                                this.TL.LogMessageCrLf(memberName, "  Throwing MethodNotImplementedException: " + this._strProgId + " " + memberName);
                                throw new MethodNotImplementedException(this._strProgId + " " + memberName);
                            }
                            this.TL.LogMessageCrLf(memberName, "Re-throwing exception");
                            throw;
                        }
                        catch (TargetInvocationException ex)
                        {
                            this.TL.LogMessageCrLf("TargetInvocationException", ex.ToString());
                            if (ex.InnerException is COMException)
                            {
                                string message = ex.InnerException.Message;
                                int errorCode = ((ExternalException)ex.InnerException).ErrorCode;
                                if (errorCode == int.Parse("80040400", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                                {
                                    this.TL.LogMessageCrLf(memberName, "  Translating COM not implemented exception to MethodNotImplementedException: " + this._strProgId + " " + memberName);
                                    throw new MethodNotImplementedException(this._strProgId + " " + memberName, (Exception)ex);
                                }
                                this.TL.LogMessageCrLf(memberName, "  COM Exception so throwing inner exception: '" + message + "' '0x" + string.Format("{0:x8}", (object)errorCode) + "'");
                                throw new DriverAccessCOMException(message, errorCode, (Exception)ex);
                            }
                            this.MethodTargetInvocationExceptionHandler(memberName, (Exception)ex);
                        }
                        catch (Exception ex)
                        {
                            this.TL.LogMessageCrLf("Exception", ex.ToString());
                            throw;
                        }
                    }
                    this.TL.LogMessage(memberName, "  is neither a .NET object nor a COM object!");
                    throw new MethodNotImplementedException(this._strProgId + " " + memberName);
                default:
                    return (object)null;
            }
        }

        internal T GetPropertyValue<T>(string propertyName)
        {
            return (T)GetPropertyValue(propertyName);
        }

        internal object GetPropertyValue(string propertyName)
        {
            TraceLogger.Debug($"GetProperty[{GetObjType.FullName}.{propertyName}]");
            this.TL.BlankLine();
            PropertyInfo pi = this.GetObjType.GetProperty(propertyName);
            if (pi != null)
            {
                this.TL.LogMessage(propertyName + " Get", "GET " + propertyName + " - .NET");
                try
                {
                    object obj = pi.GetValue(this.GetLateBoundObject, (object[])null);
                    this.TL.LogMessage(propertyName + " Get", "  " + obj.ToString());
                    return obj;
                }
                catch (TargetInvocationException ex)
                {
                    this.GetTargetInvocationExceptionHandler(propertyName, (Exception)ex);
                }
                catch (Exception ex)
                {
                    this.TL.LogMessageCrLf("Exception", ex.ToString());
                    throw;
                }
            }
            if (this.IsComObject)
            {
                this.TL.LogMessage(propertyName + " Get", "GET " + propertyName + " - COM");
                try
                {
                    object obj = this.GetObjType.InvokeMember(propertyName, BindingFlags.GetProperty, (Binder)null, this.GetLateBoundObject, new object[0], CultureInfo.InvariantCulture);
                    this.TL.LogMessage(propertyName + " Get", "  " + obj.ToString());
                    return obj;
                }
                catch (COMException ex)
                {
                    this.TL.LogMessageCrLf("COMException", ex.ToString());
                    if (ex.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                    {
                        this.TL.LogMessageCrLf(propertyName + " Get", "  Throwing PropertyNotImplementedException: " + this._strProgId + " " + propertyName);
                        throw new PropertyNotImplementedException(this._strProgId + " " + propertyName, false, (Exception)ex);
                    }
                    this.TL.LogMessageCrLf(propertyName + " Get", "Re-throwing exception");
                    throw;
                }
                catch (TargetInvocationException ex)
                {
                    this.TL.LogMessageCrLf("TargetInvocationException", ex.ToString());
                    if (ex.InnerException is COMException)
                    {
                        string message = ex.InnerException.Message;
                        int errorCode = ((ExternalException)ex.InnerException).ErrorCode;
                        if (errorCode == int.Parse("80040400", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                        {
                            this.TL.LogMessageCrLf(propertyName + " Get", "  Translating COM not implemented exception to PropertyNotImplementedException: " + this._strProgId + " " + propertyName);
                            throw new PropertyNotImplementedException(this._strProgId + " " + propertyName, false, (Exception)ex);
                        }
                        this.TL.LogMessageCrLf(propertyName + " Get", "COM Exception so throwing inner exception: '" + message + "' '0x" + string.Format("{0:x8}", (object)errorCode) + "'");
                        throw new DriverAccessCOMException(message, errorCode, (Exception)ex);
                    }
                    this.GetTargetInvocationExceptionHandler(propertyName, (Exception)ex);
                }
                catch (Exception ex)
                {
                    this.TL.LogMessageCrLf("Exception", ex.ToString());
                    throw;
                }
            }
            this.TL.LogMessage(propertyName + " Get", "  The object is neither a .NET object nor a COM object!");
            throw new PropertyNotImplementedException(this._strProgId + " " + propertyName, false);
        }

        internal void SetPropertyValue<T>(string propertyName, T value)
        {
            SetPropertyValue(propertyName, value);
        }

        internal void SetPropertyValue(string propertyName, object value)
        {
            TraceLogger.Debug($"SetProperty[{GetObjType.FullName}.{propertyName}]={value} ({value?.GetType().Name})");
            PropertyInfo pi = this.GetObjType.GetProperty(propertyName);
            if (pi != null)
            {
                this.TL.LogMessage(propertyName + " Set", "SET " + propertyName + " - .NET");
                try
                {
                    this.TL.LogMessage(propertyName + " Set", "  " + value.ToString());
                    pi.SetValue(this.GetLateBoundObject, value, (object[])null);
                    return;
                }
                catch (TargetInvocationException ex)
                {
                    this.SetTargetInvocationExceptionHandler(propertyName, (Exception)ex);
                }
                catch (Exception ex)
                {
                    this.TL.LogMessageCrLf("Exception", ex.ToString());
                    throw;
                }
            }
            if (this.IsComObject)
            {
                this.TL.LogMessage(propertyName + " Set", "SET " + propertyName + " - COM");
                try
                {
                    this.TL.LogMessage(propertyName + " Set", "  " + value.ToString());
                    this.GetObjType.InvokeMember(propertyName, BindingFlags.SetProperty, (Binder)null, this.GetLateBoundObject, new object[] { value }, CultureInfo.InvariantCulture);
                    return;
                }
                catch (COMException ex)
                {
                    this.TL.LogMessageCrLf("COMException", ex.ToString());
                    if (ex.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                    {
                        this.TL.LogMessageCrLf(propertyName + " Set", "  Throwing PropertyNotImplementedException: " + this._strProgId + " " + propertyName);
                        throw new PropertyNotImplementedException(this._strProgId + " " + propertyName, true, (Exception)ex);
                    }
                    this.TL.LogMessageCrLf(propertyName + " Set", "  Re-throwing exception");
                    throw;
                }
                catch (TargetInvocationException ex)
                {
                    this.TL.LogMessageCrLf("TargetInvocationException", ex.ToString());
                    if (ex.InnerException is COMException)
                    {
                        string message = ex.InnerException.Message;
                        int errorCode = ((ExternalException)ex.InnerException).ErrorCode;
                        if (errorCode == int.Parse("80040400", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                        {
                            this.TL.LogMessageCrLf(propertyName + " Set", "  Translating COM not implemented exception to PropertyNotImplementedException: " + this._strProgId + " " + propertyName);
                            throw new PropertyNotImplementedException(this._strProgId + " " + propertyName, true, (Exception)ex);
                        }
                        this.TL.LogMessageCrLf(propertyName + " Set", "COM Exception so throwing inner exception: '" + message + "' '0x" + string.Format("{0:x8}", (object)errorCode) + "'");
                        throw new DriverAccessCOMException(message, errorCode, (Exception)ex);
                    }
                    this.SetTargetInvocationExceptionHandler(propertyName, (Exception)ex);
                }
                catch (Exception ex)
                {
                    this.TL.LogMessageCrLf("Exception", ex.ToString());
                    throw;
                }
            }
            this.TL.LogMessage("PropertySet", "  The object is neither a .NET object nor a COM object!");
            throw new PropertyNotImplementedException(this._strProgId + " " + propertyName, true);
        }
        internal T ExecMethod<T>(string methodName, params object[] parms)
        {
            return (T)ExecMethod(methodName, parms);
        }
        internal object ExecMethod(string methodName, params object[] parms)
        {
            string methSig = "null";
            if (parms != null)
                methSig = String.Join(", ", parms);
            methSig = $"{methodName}({methSig})";
            TraceLogger.Debug($"ExecMethod=> {methSig}");
            this.TL.LogMessage(methodName, "Start");
            MethodInfo method = this.GetObjType.GetMethod(methodName);
            if (method != null)
            {
                try
                {
                    foreach (object parm in parms)
                        this.TL.LogMessage(methodName, "  Parameter: " + parm.ToString());
                    this.TL.LogMessage(methodName, "  Calling " + methodName);
                    object obj = method.Invoke(this.GetLateBoundObject, parms);
                    if (obj == null)
                        this.TL.LogMessage(methodName, "  Successfully called method, no return value");
                    else
                        this.TL.LogMessage(methodName, "  " + obj.ToString());
                    return obj;
                }
                catch (TargetInvocationException ex)
                {
                    this.MethodTargetInvocationExceptionHandler(methodName, (Exception)ex);
                }
                catch (Exception ex)
                {
                    this.TL.LogMessageCrLf("Exception", ex.ToString());
                    throw;
                }
            }
            if (this.IsComObject)
            {
                try
                {
                    foreach (object parm in parms)
                        this.TL.LogMessage(methodName, "  Parameter: " + parm.ToString());
                    this.TL.LogMessage(methodName, "  Calling " + methodName + " - it is a COM object");
                    object obj = this.GetObjType.InvokeMember(methodName, BindingFlags.InvokeMethod, (Binder)null, this.GetLateBoundObject, parms, CultureInfo.InvariantCulture);
                    if (obj == null)
                        this.TL.LogMessage(methodName, "  Successfully called method, no return value");
                    else
                        this.TL.LogMessage(methodName, "  " + obj.ToString());
                    return obj;
                }
                catch (COMException ex)
                {
                    this.TL.LogMessageCrLf("COMException", ex.ToString());
                    if (ex.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                    {
                        this.TL.LogMessageCrLf(methodName, "  Throwing MethodNotImplementedException: " + this._strProgId + " " + methodName);
                        throw new MethodNotImplementedException(this._strProgId + " " + methodName);
                    }
                    this.TL.LogMessageCrLf(methodName, "Re-throwing exception");
                    throw;
                }
                catch (TargetInvocationException ex)
                {
                    this.TL.LogMessageCrLf("TargetInvocationException", ex.ToString());
                    if (ex.InnerException is COMException)
                    {
                        string message = ex.InnerException.Message;
                        int errorCode = ((ExternalException)ex.InnerException).ErrorCode;
                        if (errorCode == int.Parse("80040400", NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture))
                        {
                            this.TL.LogMessageCrLf(methodName, "  Translating COM not implemented exception to MethodNotImplementedException: " + this._strProgId + " " + methodName);
                            throw new MethodNotImplementedException(this._strProgId + " " + methodName, (Exception)ex);
                        }
                        this.TL.LogMessageCrLf(methodName, "  COM Exception so throwing inner exception: '" + message + "' '0x" + string.Format("{0:x8}", (object)errorCode) + "'");
                        throw new DriverAccessCOMException(message, errorCode, (Exception)ex);
                    }
                    this.MethodTargetInvocationExceptionHandler(methodName, (Exception)ex);
                }
                catch (Exception ex)
                {
                    this.TL.LogMessageCrLf("Exception", ex.ToString());
                    throw;
                }
            }
            this.TL.LogMessage(methodName, "  is neither a .NET object nor a COM object!");
            throw new MethodNotImplementedException(this._strProgId + " " + methodName);
        }
        internal async Task<T> ExecMethodAsync<T>(string methodName, params object[] parms)
        {
            return (T) await ExecMethodAsync(methodName, parms);
        }
        internal async Task<object> ExecMethodAsync(string methodName, params object[] parms)
        {
            return await Task.Run(() => ExecMethodAsync(methodName, parms));
            //ExecMethodAsync(methodName, parms);
        }
        private void CheckDotNetExceptions(string memberName, Exception e)
        {
            string fullName = e.InnerException.GetType().FullName;
            if (e.InnerException is DriverAccessCOMException)
            {
                string message = e.InnerException.InnerException.Message;
                this.TL.LogMessageCrLf(memberName, "  *** Found DriverAccessCOMException so stripping this off and reprocessing through CheckDotNetExceptions: '" + message + "'");
                this.TL.LogMessageCrLf(memberName, "  *** Inner exception is: " + e.InnerException.InnerException.GetType().Name);
                try
                {
                    this.TL.LogMessageCrLf(memberName, "  *** InnerException.InnerException is: " + e.InnerException.InnerException.InnerException.GetType().Name);
                }
                catch (Exception ex)
                {
                    this.TL.LogMessageCrLf(memberName, "  *** Exception arose when accessing InnerException.InnerException: " + ex.ToString());
                }
                this.CheckDotNetExceptions(memberName + " inner exception", e.InnerException.InnerException);
            }
            if (e.InnerException is ASCOM.InvalidOperationException)
            {
                string message = e.InnerException.Message;
                this.TL.LogMessageCrLf(memberName, "  Throwing InvalidOperationException: '" + message + "'");
                throw new ASCOM.InvalidOperationException(message, e.InnerException);
            }
            if (e.InnerException is ASCOM.InvalidValueException)
            {
                string propertyOrMethod = (string)e.InnerException.GetType().InvokeMember("PropertyOrMethod", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                string str = (string)e.InnerException.GetType().InvokeMember("Value", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                string range = (string)e.InnerException.GetType().InvokeMember("Range", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                this.TL.LogMessageCrLf(memberName, "  Throwing InvalidValueException: '" + propertyOrMethod + "' '" + str + "' '" + range + "'");
                throw new ASCOM.InvalidValueException(propertyOrMethod, str, range, e.InnerException);
            }
            if (e.InnerException is NotConnectedException)
            {
                string message = e.InnerException.Message;
                this.TL.LogMessageCrLf(memberName, "  Throwing NotConnectedException: '" + message + "'");
                throw new NotConnectedException(message, e.InnerException);
            }
            if (e.InnerException is PropertyNotImplementedException)
            {
                string property = (string)e.InnerException.GetType().InvokeMember("PropertyOrMethod", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                this.TL.LogMessageCrLf(memberName, "  Throwing PropertyNotImplementedException: '" + property + "'");
                throw new PropertyNotImplementedException(property, false, e.InnerException);
            }
            if (e.InnerException is MethodNotImplementedException)
            {
                string method = (string)e.InnerException.GetType().InvokeMember("PropertyOrMethod", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                this.TL.LogMessageCrLf(memberName, "  Throwing MethodNotImplementedException: '" + method + "'");
                throw new MethodNotImplementedException(method, e.InnerException);
            }
            if (e.InnerException is ASCOM.NotImplementedException)
            {
                string propertyOrMethod = (string)e.InnerException.GetType().InvokeMember("PropertyOrMethod", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                this.TL.LogMessageCrLf(memberName, "  Throwing NotImplementedException: '" + propertyOrMethod + "'");
                throw new ASCOM.NotImplementedException(propertyOrMethod, e.InnerException);
            }
            if (e.InnerException is ParkedException)
            {
                string message = e.InnerException.Message;
                this.TL.LogMessageCrLf(memberName, "  Throwing ParkedException: '" + message + "'");
                throw new ParkedException(message, e.InnerException);
            }
            if (e.InnerException is SlavedException)
            {
                string message = e.InnerException.Message;
                this.TL.LogMessageCrLf(memberName, "  Throwing SlavedException: '" + message + "'");
                throw new SlavedException(message, e.InnerException);
            }
            if (e.InnerException is ASCOM.ValueNotSetException)
            {
                string propertyOrMethod = (string)e.InnerException.GetType().InvokeMember("PropertyOrMethod", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                this.TL.LogMessageCrLf(memberName, "  Throwing ValueNotSetException: '" + propertyOrMethod + "'");
                throw new ASCOM.ValueNotSetException(propertyOrMethod, e.InnerException);
            }
            if (e.InnerException is DriverException)
            {
                string message = e.InnerException.Message;
                int number = (int)e.InnerException.GetType().InvokeMember("Number", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                this.TL.LogMessageCrLf(memberName, "  Throwing DriverException: '" + message + "' '" + (object)number + "'");
                throw new DriverException(message, number, e.InnerException);
            }
            if (e.InnerException is COMException)
            {
                string message = e.InnerException.Message;
                int ErrorCode = (int)e.InnerException.GetType().InvokeMember("ErrorCode", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                this.TL.LogMessageCrLf(memberName, "  Throwing DriverAccessCOMException: '" + message + "' '" + (object)ErrorCode + "'");
                throw new DriverAccessCOMException(message, ErrorCode, e.InnerException);
            }
            string message1 = "CheckDotNetExceptions " + this._strProgId + " " + memberName + " " + e.InnerException.ToString() + " (See Inner Exception for details)";
            this.TL.LogMessageCrLf(memberName, "  Throwing Default DriverException: '" + message1 + "'");
            throw new DriverException(message1, e.InnerException);
        }

        private void SetTargetInvocationExceptionHandler(string memberName, Exception e)
        {
            this.TL.LogMessageCrLf("TargetInvocationException", "Set " + memberName + " " + e.ToString());
            if (e.InnerException.GetType().FullName == "ASCOM.PropertyNotImplementedException")
            {
                this.TL.LogMessageCrLf("TargetInvocationException", "Set " + memberName + " Found PropertyNotImplementedException");
                string property = (string)e.InnerException.GetType().InvokeMember("Property", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                bool accessorSet = (bool)e.InnerException.GetType().InvokeMember("AccessorSet", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                this.TL.LogMessageCrLf(memberName + " Set", "  Throwing PropertyNotImplementedException: '" + property + "' '" + (object)accessorSet + "'");
                throw new PropertyNotImplementedException(property, accessorSet, e.InnerException);
            }
            this.CheckDotNetExceptions(memberName + "Set", e);
        }

        private void GetTargetInvocationExceptionHandler(string memberName, Exception e)
        {
            this.TL.LogMessageCrLf("TargetInvocationException", "Get " + e.ToString());
            if (e.InnerException.GetType().FullName == "ASCOM.PropertyNotImplementedException")
            {
                this.TL.LogMessageCrLf("TargetInvocationException", "Get " + memberName + " Found PropertyNotImplementedException");
                string property = (string)e.InnerException.GetType().InvokeMember("Property", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                bool accessorSet = (bool)e.InnerException.GetType().InvokeMember("AccessorSet", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                this.TL.LogMessageCrLf(memberName + " Get", "  Throwing PropertyNotImplementedException: '" + property + "' '" + (object)accessorSet + "'");
                throw new PropertyNotImplementedException(property, accessorSet, e.InnerException);
            }
            this.CheckDotNetExceptions(memberName + " Get", e);
        }

        private void MethodTargetInvocationExceptionHandler(string memberName, Exception e)
        {
            this.TL.LogMessageCrLf("TargetInvocationException", e.ToString());
            if (e.InnerException.GetType().FullName == "ASCOM.MethodNotImplementedException")
            {
                string method = (string)e.InnerException.GetType().InvokeMember("Method", BindingFlags.GetProperty, (Binder)null, (object)e.InnerException, new object[0], CultureInfo.InvariantCulture);
                this.TL.LogMessageCrLf(memberName, "  Throwing MethodNotImplementedException :'" + method + "'");
                throw new MethodNotImplementedException(method, e.InnerException);
            }
            this.CheckDotNetExceptions(memberName, e);
        }
    }
}
