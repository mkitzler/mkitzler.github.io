using System.Globalization;

namespace BouncingDVDLogo
{
    public static class Extensions
    {
        public static string ToCss(this double d)
        {
            return d.ToString("G", CultureInfo.InvariantCulture);
        }
    }
}
