using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ASCOM.Controls.Properties
{
    [CompilerGenerated]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        public Settings()
        { }
        public static Settings Default { get; }
        [ApplicationScopedSetting]
        [SettingsDescription("The URL of the ASCOM Standards web site.")]
        [DebuggerNonUserCode]
        [DefaultSettingValue("http://www.ascom-standards.org")]
        public string ASCOMStandardsURL { get; }
        [DefaultSettingValue("http://www.tigranetworks.co.uk/Astronomy")]
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        public string TiGraAstronomyURL { get; }
    }
}