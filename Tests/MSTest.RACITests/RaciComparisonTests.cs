using Microsoft.VisualStudio.TestTools.UnitTesting;
using RACI.Data;
using static RACI.Utils.RaciUtil;
using System;
using System.Threading;
using System.Globalization;

namespace MSTest.RACITests
{
    [TestClass]
    public class RaciComparisonTests
    {
        string defCulture = "en-US";
        string sNull = null;
        string sEmpty = string.Empty;
        string sWhite = "  \t\n\r";

        string sInvariant = "Encyclopaedia";
        string sVariant = "Encyclopædia";

        public RaciComparisonTests()
        {
            defCulture = CurrentCulture;
        }

        private string CurrentCulture
        {
            get => Thread.CurrentThread.CurrentCulture.Name;
            set
            {
                if (CurrentCulture != value)
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(value);
            }
        }

        [TestMethod]
        public void EnumTextTest()
        {
            string enumString;
            RaciStringComparison mode = RaciStringComparison.KeyMode;
            StringComparison smode = StringComparison.InvariantCulture;

            enumString = smode.EnumText();
            Console.WriteLine($"StringComparison: {enumString}");

            enumString = mode.EnumText();
            Console.WriteLine($"RaciStringComparison: {enumString}");

            RaciComparer comp = new RaciComparer(mode);
            string compString = comp.ToString();
            Console.WriteLine($"Comparer String: {compString}");
        }

        [TestMethod]
        public void ConvertModeTest()
        {
            RaciComparer comp = new RaciComparer();
            RaciStringComparison mode = 
                RaciStringComparison.Invariant
                | RaciStringComparison.IgnoreCase;
            StringComparison smode = comp.ConvertMode(mode);
            string smodeString = smode.EnumText();
            Console.WriteLine($"StringMode: '{smodeString}'");
        }

        [TestMethod]
        public void TrimTest()
        {
            string a = sInvariant;
            string b = $"{a}{sWhite}";
            string msg;
            RaciComparer comp;

            comp = new RaciComparer(RaciStringComparison.Trim);
            msg = $"[{CurrentCulture} {comp}]: '{a}' == '{b}'";
            Console.WriteLine($"[TEST] {msg}");
            Assert.IsTrue(comp.Equals(a, b), msg);
        }

        [TestMethod]
        public void EmptyIsNullTest()
        {
            string a = null;
            string b = "";
            RaciComparer comp = new RaciComparer();
            Assert.IsTrue(!comp.Equals(a, b), $"{a} != {b}");
            comp.Mode = RaciStringComparison.EmptyIsNull;
            Assert.IsTrue(comp.Equals(a, b), $"{a} == {b}");
        }

        [TestMethod]
        public void IgnoreCaseTest()
        {
            string a = "case";
            string b = "CaSe";
            RaciComparer comp = new RaciComparer();
            Assert.IsTrue(!comp.Equals(a, b), $"{a} != {b}");
            comp.Mode = RaciStringComparison.IgnoreCase;
            Assert.IsTrue(comp.Equals(a, b), $"{a} == {b}");
        }

        [TestMethod]
        public void WhiteIsEmptyTest()
        {
            string a = " \t\r\n";
            string b = "";
            RaciComparer comp = new RaciComparer();
            Assert.IsTrue(!comp.Equals(a, b), $"{a} != {b}");
            comp.Mode = RaciStringComparison.WhiteIsEmpty;
            Assert.IsTrue(comp.Equals(a, b), $"{a} == {b}");
        }

        [TestMethod]
        public void InvariantTest()
        {
            string a = sInvariant;
            string b = sVariant;
            string msg;

            RaciComparer comp = new RaciComparer();

            try
            {
                // Culture: en-US
                CurrentCulture = "en-US";
                msg = $"[{CurrentCulture} System]: '{a}' == '{b}'";
                Console.WriteLine($"[TEST] {msg}");
                Assert.IsTrue(String.Equals(a, b, StringComparison.InvariantCulture), msg);

                msg = $"[{CurrentCulture} {comp}]: '{a}' == '{b}'";
                Console.WriteLine($"[TEST] {msg}");
                Assert.IsTrue(String.Equals(a, b, StringComparison.CurrentCulture), msg);

                comp.Invariant = false;
                msg = $"[{CurrentCulture} {comp}]: '{a}' == '{b}'";
                Console.WriteLine($"[TEST] {msg}");
                Assert.IsTrue(comp.Equals(a, b), msg);

                comp.Invariant=true;
                msg = $"[{CurrentCulture} {comp}]: '{a}' == '{b}'";
                Console.WriteLine($"[TEST] {msg}");
                Assert.IsTrue(comp.Equals(a, b), msg);

                // Culture: se-SE
                CurrentCulture = "se-SE";
                msg = $"[{CurrentCulture} {comp}]: '{a}' == '{b}'";
                Console.WriteLine($"[TEST] {msg}");
                Assert.IsTrue(String.Equals(a, b, StringComparison.InvariantCulture), msg);

                msg = $"[{CurrentCulture} {comp}]: '{a}' != '{b}'";
                Console.WriteLine($"[TEST] {msg}");
                Assert.IsTrue(!String.Equals(a, b, StringComparison.CurrentCulture), msg);

                comp.Invariant = false;
                msg = $"[{CurrentCulture} {comp}]: '{a}' != '{b}'";
                Console.WriteLine($"[TEST] {msg}");
                Assert.IsTrue(!comp.Equals(a, b), msg);

                comp.Invariant = true;
                msg = $"[{CurrentCulture} {comp}]: '{a}' == '{b}'";
                Console.WriteLine($"[TEST] {msg}");
                Assert.IsTrue(comp.Equals(a, b), msg);

                // Culture: default
                CurrentCulture = defCulture;
            }
            catch
            {
                throw;
            }
            finally
            {
                CurrentCulture = defCulture;
            }
        }
    }
}

