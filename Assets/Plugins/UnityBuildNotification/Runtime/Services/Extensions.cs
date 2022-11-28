using System;

namespace BuildNotification.Runtime.Services
{
    public static class Extensions
    {
        private const float ConvertRation = 1024f;

        public enum SizeUnits
        {
            Byte = 0,
            Kb = 1,
            Mb = 2,
            Gb = 3,
            Tb = 4,
            Pb = 5,
            Eb = 6,
            Zb = 7,
            Yb = 8
        }

        public static double ToSize(this ulong value, SizeUnits unit)
        {
            return (value / Math.Pow(1024, (long)unit));
        }

        public static double ToMegabytes(this ulong bytes)
        {
            return bytes.ToSize(SizeUnits.Mb);
        }
    }
}