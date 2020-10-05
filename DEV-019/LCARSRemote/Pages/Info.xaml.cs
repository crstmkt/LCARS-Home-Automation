using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace LCARSRemote.Pages
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class Info : Page
    {
        public Info()
        {
            this.InitializeComponent();

            this.txb_Info.Text = String.Concat(string.Format("{0}", Package.Current.DisplayName), string.Format(" Version: {0}.{1}.{2}.{3}",
                                                        Package.Current.Id.Version.Major,
                                                        Package.Current.Id.Version.Minor,
                                                        Package.Current.Id.Version.Build,
                                                        Package.Current.Id.Version.Revision), " Codename: Ariana");
        }
    }
}
