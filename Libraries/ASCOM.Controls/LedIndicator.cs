using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

#if !NETSTANDARD2_0 && !NETCOREAPP2_0
using System.Windows.Forms;
#endif

namespace ASCOM.Controls
{
    public sealed class Annunciator : Label, ICadencedControl
    {
        public new void Dispose();
        protected override void Dispose(bool disposing);
        public void CadenceUpdate(bool newState);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Category("Appearance")]
        [Description("This property is not normally set directly as it will be overwritten at runtime.")]
        [DefaultValue(4286579716L)]
        public override Color ForeColor { get; set; }
        [Description("The anunciator\'s inactive colour. This is usually set to a value close to (but not equal) to the background colour. The default value is recommended for most situations.")]
        [Category("Appearance")]
        [DefaultValue(4282909700L)]
        public Color InactiveColor { get; set; }
        [Description("The anunciators active color. This should be bright and have a high contrast with the control\'s background. The default value is recommended for most situations.")]
        [Category("Appearance")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(4286579716L)]
        public Color ActiveColor { get; set; }
        [Description("This property is not normally set directly as it will be overwritten at runtime.")]
        [DefaultValue(4282384384L)]
        [Category("Appearance")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Color BackColor { get; set; }
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("Enables or disables the anunciator. When muted, the anunciator always displays in its InactiveColor.")]
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool Mute { get; set; }
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(CadencePattern.SteadyOn)]
        [Category("Appearance")]
        [Description("Determines the cadence (blink pattern) for the anunciator. Different cadences imply different levels of severity or urgency.")]
        public CadencePattern Cadence { get; set; }
    }
}

