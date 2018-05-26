using System;
using System.Reflection;

namespace ASCOM.Utilities
{
    public sealed class Versioned
    {
        public static object CallByName(object Instance, string MethodName, CallType UseCallType, params object[] Arguments)
        {

            throw new NotImplementedException();
            //return Microsoft.VisualBasic.CompilerServices.Versioned.CallByName(Instance, MethodName, (Microsoft.VisualBasic.CallType)UseCallType, Arguments);
        }
        public static bool IsNumeric(object Expression)
        {
            
            return Double.TryParse(Expression?.ToString(), out double x);
            //return Microsoft.VisualBasic.CompilerServices.Versioned.IsNumeric(Expression);
        }
        public static string SystemTypeName(string VbName)
        {
            return Type.GetType(VbName)?.FullName ?? "";
            //return Microsoft.VisualBasic.CompilerServices.Versioned.SystemTypeName(VbName);
        }
        public static string TypeName(object Expression)
        {
            return Expression?.GetType()?.Name ?? "";
            //return Microsoft.VisualBasic.CompilerServices.Versioned.TypeName(Expression);
        }
        public static string VbTypeName(string SystemName)
        {
            return Type.GetType(SystemName)?.Name ?? "";
            //return Microsoft.VisualBasic.CompilerServices.Versioned.VbTypeName(SystemName);
        }
    }
}