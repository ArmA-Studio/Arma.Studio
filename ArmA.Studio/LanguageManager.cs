using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using Localization = ArmA.Studio.Properties.Localization;

namespace ArmA.Studio
{
    public static class LanguageManager
    {
        public static int Language
        {
            get { return ConfigHost.App.Language; }
            set
            {
                if ( value != ConfigHost.App.Language )
                {
                    ConfigHost.App.Language = value;
                    LanguageChanged();
                }
            }
        }

        public static IEnumerable<KeyValuePair<string, int>> SupportedLanguages()
        {
            // TODO : Implement function to scan app resources files, for automatic language support detection
            yield return new KeyValuePair<string, int>( CultureInfo.GetCultureInfo( 1033 ).NativeName, 1033 );
            yield return new KeyValuePair<string, int>( CultureInfo.GetCultureInfo( 1031 ).NativeName, 1031 );
        }

        public static void SetDisplayLanguage()
        {
            var ci = CultureInfo.GetCultureInfo( Language );
            Localization.Culture = ci;
        }

        private static void LanguageChanged()
        {
            var msgbox = MessageBox.Show( Localization.MessageDialog_PropertyChange, string.Empty, MessageBoxButton.YesNo );

            if (msgbox == MessageBoxResult.Yes)
            {
                Application.Current.Exit += ( sender, args ) =>
                {
                    var exe = Application.ResourceAssembly.Location;
                    if (!string.IsNullOrEmpty( exe ))
                    {
                        Process.Start( exe );
                    }
                };
                App.Shutdown( 0 );
            }
        }
    }
}