// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Chooser
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;

namespace ASCOM.Utilities
{
#warning Internal enum exposed as public during porting: Interaction
    public class Interaction
    //internal class Interaction
    {
        public static int MsgBox(object obj, MsgBoxStyle style, object caption)
        {
            return MsgBox(obj?.ToString(), style, caption?.ToString());
        }
        public static int MsgBox(String msg, MsgBoxStyle style, string caption)
        {
            int rc = (int)MsgBoxResult.Ok;
            //throw new System.NotImplementedException();
            if (!string.IsNullOrWhiteSpace(caption))
                Console.WriteLine(caption);
            Console.WriteLine(msg??"--");
            char key = 'O';
            switch (style)
            {
                case MsgBoxStyle.OkCancel:
                    Console.Write($"(O)k, *(C)ancel");
                    key = Console.ReadKey().KeyChar.ToString().ToUpperInvariant().ToCharArray()[0];
                    if (key == 'O')
                        rc = (int)MsgBoxResult.Ok;
                    else 
                        rc = (int)MsgBoxResult.Cancel;
                    break;
                case MsgBoxStyle.AbortRetryIgnore:
                    Console.Write($"(A)bort, (R)etry, *(I)gnore");
                    key = Console.ReadKey().KeyChar.ToString().ToUpperInvariant().ToCharArray()[0];
                    if (key == 'R')
                        rc = (int)MsgBoxResult.Retry;
                    if (key == 'A')
                        rc = (int)MsgBoxResult.Abort;
                    else
                        rc = (int)MsgBoxResult.Ignore;
                    break;
                case MsgBoxStyle.YesNoCancel:
                    Console.Write($"(Y)es, (N)o, *(C)ancel");
                    key = Console.ReadKey().KeyChar.ToString().ToUpperInvariant().ToCharArray()[0];
                    if (key == 'Y')
                        rc = (int)MsgBoxResult.Yes;
                    if (key == 'N')
                        rc = (int)MsgBoxResult.No;
                    else
                        rc = (int)MsgBoxResult.Cancel;
                    break;
                case MsgBoxStyle.YesNo:
                    Console.Write($"(Y)es, *(N)o");
                    key = Console.ReadKey().KeyChar.ToString().ToUpperInvariant().ToCharArray()[0];
                    if (key == 'Y')
                        rc = (int)MsgBoxResult.Yes;
                    else
                        rc = (int)MsgBoxResult.No;
                    break;
                default:
                    rc = (int)MsgBoxResult.Ok;
                    break;
            }
            return rc;
        }
    }
}