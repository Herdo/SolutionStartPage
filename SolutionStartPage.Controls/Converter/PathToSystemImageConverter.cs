namespace SolutionStartPage.Controls.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class PathToSystemImageConverter : IValueConverter
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly Dictionary<string, ImageSource> _sourceCache; 

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public PathToSystemImageConverter()
        {
            _sourceCache = new Dictionary<string, ImageSource>();
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods

        private ImageSource GetFromCache(string file)
        {
            var extension = Path.GetExtension(file);
            if (extension == null)
                return null;
            if (!_sourceCache.ContainsKey(extension))
                _sourceCache[extension] = GetFileIcon(file);
            return _sourceCache[extension];
        }

        private static ImageSource GetFileIcon(string fileName)
        {
            // if file does not exist, create a temp file with the same file extension
            var isTemp = false;
            if (!File.Exists(fileName) && !Directory.Exists(fileName))
            {
                isTemp = true;
                fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Path.GetExtension(fileName));
                File.WriteAllText(fileName, string.Empty);
            }
            var shinfo = new Natives.SHFILEINFO();
            var flags = Natives.SHGFI_SYSICONINDEX;
            if (fileName.IndexOf(":", StringComparison.Ordinal) == -1)
                flags = flags | Natives.SHGFI_USEFILEATTRIBUTES;
            flags = flags | Natives.SHGFI_ICON | Natives.SHGFI_SMALLICON;
            Natives.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);
            var icon = Icon.FromHandle(shinfo.hIcon);
            var bitmap = icon.ToBitmap();
            var hBitmap = bitmap.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                Natives.DeleteObject(hBitmap);
                if (isTemp)
                {
                    File.Delete(fileName);
                }
            }
        }


        #endregion

        /////////////////////////////////////////////////////////
        #region IValueConverter Member

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var file = value as String;
            if (file == null) return null;

            return GetFromCache(file);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion

        // ReSharper disable InconsistentNaming
        private static class Natives
        {
            internal const uint SHGFI_ICON = 0x100;
            internal const uint SHGFI_SMALLICON = 0x1;
            internal const uint SHGFI_SYSICONINDEX = 16384;
            internal const uint SHGFI_USEFILEATTRIBUTES = 16;

            [DllImport("shell32.dll")]
            internal static extern IntPtr SHGetFileInfo(string pszPath,
                                                       uint dwFileAttributes,
                                                       ref SHFILEINFO psfi,
                                                       uint cbSizeFileInfo,
                                                       uint uFlags);

            [DllImport("gdi32.dll")]
            internal static extern bool DeleteObject(IntPtr hObject);

            [StructLayout(LayoutKind.Sequential)]
            internal struct SHFILEINFO
            {
                internal readonly IntPtr hIcon;
                private readonly IntPtr iIcon;
                private readonly uint dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] private readonly string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)] private readonly string szTypeName;
            };
        }
        // ReSharper restore InconsistentNaming
    }
}