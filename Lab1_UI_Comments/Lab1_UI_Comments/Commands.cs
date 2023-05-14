using System.Windows.Input;

namespace Lab1_UI_Comments
{
    internal class Commands
    {
        public static RoutedCommand SplineData =
                new RoutedCommand("RawData from Controls", typeof(Commands));
        public static RoutedCommand RawDataFileCommand =
                new RoutedCommand("RawData from File", typeof(Commands));
        public static RoutedCommand RawDataControls =
         new RoutedCommand("RawData from File", typeof(Commands));
    }
}