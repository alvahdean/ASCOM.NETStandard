using System;
//using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RACI.Data
{
    [Flags]
    public enum RaciStringComparison
    {
        Default = 0,
        Invariant = 1,
        IgnoreCase = 2,
        EmptyIsNull = 4,
        WhiteIsEmpty = 8,
        Trim = 16,
        Ordinal = 32,
        KeyMode = Invariant | IgnoreCase | EmptyIsNull | WhiteIsEmpty | Trim
    }

    public class RaciComparer : IComparer<String>, IEqualityComparer<String>
    {
        #region internal members
        private RaciStringComparison _mode;
        public StringComparison ConvertMode(RaciStringComparison mode)
        {
            return mode == RaciStringComparison.Default
                ? StringComparison.CurrentCulture
                : mode.HasFlag(RaciStringComparison.Ordinal)
                    ? mode.HasFlag(RaciStringComparison.IgnoreCase)
                        ? StringComparison.OrdinalIgnoreCase
                        : StringComparison.Ordinal
                : mode.HasFlag(RaciStringComparison.Invariant)
                    ? mode.HasFlag(RaciStringComparison.IgnoreCase)
                        ? StringComparison.InvariantCultureIgnoreCase
                        : StringComparison.InvariantCulture
                : mode.HasFlag(RaciStringComparison.IgnoreCase)
                    ? StringComparison.CurrentCultureIgnoreCase
                    : StringComparison.CurrentCulture;
        }
        public RaciStringComparison ConvertMode(StringComparison mode) => ConvertMode(mode, EmptyIsNull, WhiteIsEmpty);
        public RaciStringComparison ConvertMode(StringComparison mode, bool emptyIsNull, bool whiteIsEmpty)
        {
            RaciStringComparison result = 0;
            if (EmptyIsNull) result |= RaciStringComparison.EmptyIsNull;
            if (WhiteIsEmpty) result |= RaciStringComparison.WhiteIsEmpty;
            switch (StringMode)
            {
                case StringComparison.CurrentCultureIgnoreCase:
                    result |= RaciStringComparison.IgnoreCase;
                    break;
                case StringComparison.InvariantCulture:
                    result |= RaciStringComparison.Invariant;
                    break;
                case StringComparison.InvariantCultureIgnoreCase:
                    result |= RaciStringComparison.IgnoreCase;
                    result |= RaciStringComparison.Invariant;
                    break;
                case StringComparison.Ordinal:
                    result |= RaciStringComparison.Ordinal;
                    break;
                case StringComparison.OrdinalIgnoreCase:
                    result |= RaciStringComparison.Ordinal;
                    result |= RaciStringComparison.IgnoreCase;
                    break;
            }
            return result;
        }

        private string Transform(string s) => Transform(s, Mode);
        private string Transform(string s, RaciStringComparison mode)
        {
            if (mode.HasFlag(RaciStringComparison.Trim))
                s = s?.Trim();
            if (mode.HasFlag(RaciStringComparison.WhiteIsEmpty) && string.IsNullOrEmpty(s?.Trim()))
                s = s?.Trim();
            if (mode.HasFlag(RaciStringComparison.EmptyIsNull) && string.IsNullOrEmpty(s))
                s = null;
            return s;
        }
        private string Transform(string s, bool emptyIsNull, bool whiteIsEmpty)
        {
            if(whiteIsEmpty) s=s?.Trim();
            if (emptyIsNull && string.IsNullOrEmpty(s)) s = null;
            return s;
        }
        #endregion

        #region Instance management
        public RaciComparer() : this(RaciStringComparison.Default) { }
        public RaciComparer(StringComparison mode) : this(mode, false, false) { }
        public RaciComparer(StringComparison mode, bool emptyIsNull, bool whiteIsEmpty)
        {
            Mode = ConvertMode(mode,emptyIsNull,whiteIsEmpty);
        }
        public RaciComparer(RaciStringComparison mode) { Mode = mode; }

        #endregion

        #region Public members
        public RaciStringComparison Mode
        {
            get => _mode;
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    EmptyIsNull = Mode.HasFlag(RaciStringComparison.EmptyIsNull);
                    WhiteIsEmpty = Mode.HasFlag(RaciStringComparison.WhiteIsEmpty);
                }
                _mode = _mode.Normalize();
            }
        }
        public StringComparison StringMode => ConvertMode(_mode);
        public bool EmptyIsNull
        {
            get => Mode.HasFlag(RaciStringComparison.EmptyIsNull);
            set
            {
                if (EmptyIsNull != value)
                    Mode ^= RaciStringComparison.EmptyIsNull;
            }
        }
        public bool WhiteIsEmpty
        {
            get => Mode.HasFlag(RaciStringComparison.WhiteIsEmpty);
            set
            {
                if (WhiteIsEmpty != value)
                    Mode ^= RaciStringComparison.WhiteIsEmpty;
            }
        }
        public bool IgnoreCase
        {
            get => Mode.HasFlag(RaciStringComparison.IgnoreCase);
            set
            {
                if (IgnoreCase != value)
                    Mode ^= RaciStringComparison.IgnoreCase;
            }
        }
        public bool Invariant
        {
            get => !Ordinal && Mode.HasFlag(RaciStringComparison.Invariant);
            set
            {
                if (Invariant != value)
                    Mode ^= RaciStringComparison.Invariant;
            }
        }
        public bool Trim
        {
            get => Mode.HasFlag(RaciStringComparison.Trim);
            set
            {
                if (Trim != value)
                    Mode ^= RaciStringComparison.Trim;
            }
        }
        public bool Ordinal
        {
            get => Mode.HasFlag(RaciStringComparison.Ordinal);
            set
            {
                if (Ordinal != value)
                    Mode ^= RaciStringComparison.Ordinal;
            }
        }

        public int Compare(string x, string y) => string.Compare(Transform(x), Transform(y), ConvertMode(Mode));
        public bool Equals(string x, string y) => Compare(Transform(x), Transform(y)) == 0;

        public int GetHashCode(string s)
        {
            return Transform(s)?.GetHashCode()??0;
        }
        public override string ToString() => Mode.EnumText();

        #endregion
    }
    public class CIKeyComparer : RaciComparer
    {
        public CIKeyComparer() : this(false, false) { }
        public CIKeyComparer(bool emptyIsNull, bool whiteIsEmpty) : base(StringComparison.InvariantCultureIgnoreCase, emptyIsNull, whiteIsEmpty) { }
    }
    public class CSKeyComparer : RaciComparer
    {
        public CSKeyComparer() : this(false, false) { }
        public CSKeyComparer(bool emptyIsNull, bool whiteIsEmpty) : base(StringComparison.InvariantCulture, emptyIsNull, whiteIsEmpty) { }
    }
    public class ProfileKeyComparer : RaciComparer
    {
        public ProfileKeyComparer() : base(RaciStringComparison.KeyMode) { }
    }

}