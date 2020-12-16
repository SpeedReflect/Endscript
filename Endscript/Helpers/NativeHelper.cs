using System.Text;



namespace Endscript.Helpers
{
    public static class NativeHelper
    {
        public static unsafe string GetString<T>(T obj, int offset) where T : unmanaged
        {
            var sb = new StringBuilder(0x30);

            byte b;
            var ptr = (byte*)&obj + offset;
            while ((b = *ptr++) != 0) sb.Append((char)b);
            return sb.ToString();
        }

        public static unsafe string GetString<T>(T obj, int offset, int maxLength) where T : unmanaged
        {
            var sb = new StringBuilder(maxLength);

            byte b;
            var ptr = (byte*)&obj + offset;
            for (int i = 0; i < maxLength && (b = *ptr++) != 0; ++i) sb.Append((char)b);
            return sb.ToString();
        }
    }
}
