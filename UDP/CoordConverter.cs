using System;

namespace UDPMode
{
    internal static class CoordConverter
    {
        private const double A = 6378137.0;
        private const double F = 1.0 / 298.257223563;
        private const double B = A * (1.0 - F);
        private const double E2 = 2.0 * F - F * F;

        public static void ToECEF(double latDeg, double lonDeg, double altM,
            out double x, out double y, out double z)
        {
            double lat = latDeg * Math.PI / 180.0;
            double lon = lonDeg * Math.PI / 180.0;
            double sinLat = Math.Sin(lat);
            double cosLat = Math.Cos(lat);
            double sinLon = Math.Sin(lon);
            double cosLon = Math.Cos(lon);
            double N = A / Math.Sqrt(1.0 - E2 * sinLat * sinLat);
            x = (N + altM) * cosLat * cosLon;
            y = (N + altM) * cosLat * sinLon;
            z = (N * (1.0 - E2) + altM) * sinLat;
        }

    
    }
}
