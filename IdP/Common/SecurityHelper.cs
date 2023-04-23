using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security;
using System.Text;

namespace IdP.Common;

public static class SecurityHelper
{
    // Binary to Base 64
    public static string ToBase64(this byte[] bytes, bool LineBreaks = true) =>
        Convert.ToBase64String(bytes, LineBreaks ? Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None);

    // Back to binary
    public static byte[] FromBase64(this string s) => Convert.FromBase64String(s);

    public static byte[] ToBytes(this string s) => Encoding.UTF8.GetBytes(s);

    public static string ToStringFromBytes(this byte[] arr) => Encoding.UTF8.GetString(arr);
    public static string ToInsecureString(this SecureString secureStr) => new System.Net.NetworkCredential(string.Empty, secureStr).Password;

    public static SecureString ToSecureString(this string plainStr)
    {
        var ss = new SecureString();
        ss.Clear();
        foreach (char c in plainStr.ToCharArray())
            ss.AppendChar(c);
        ss.MakeReadOnly();
        plainStr.ZeroString();
        return ss;
    }

    public static bool IsEqualTo(this SecureString ss1, SecureString ss2)
    {
        IntPtr bstr1 = IntPtr.Zero;
        IntPtr bstr2 = IntPtr.Zero;
        try
        {
            bstr1 = Marshal.SecureStringToBSTR(ss1);
            bstr2 = Marshal.SecureStringToBSTR(ss2);
            int length1 = Marshal.ReadInt32(bstr1, -4);
            int length2 = Marshal.ReadInt32(bstr2, -4);
            if (length1 == length2)
            {
                for (int x = 0; x < length1; ++x)
                {
                    byte b1 = Marshal.ReadByte(bstr1, x);
                    byte b2 = Marshal.ReadByte(bstr2, x);
                    if (b1 != b2)
                        return false;
                }
            }
            else
                return false;
            return true;
        }
        finally
        {
            if (bstr2 != IntPtr.Zero)
                Marshal.ZeroFreeBSTR(bstr2);
            if (bstr1 != IntPtr.Zero)
                Marshal.ZeroFreeBSTR(bstr1);
        }
    }

    // PBKDF2 - "random" bytes from password (Password-Based Key Derivation Function), salt=null uses default salt, 
    public static byte[] GetKeyFromPassword(SecureString password, byte[] salt, int keySizeInBytes = 32, int noOfIterations = 1000000)
    {
        if (salt == null)
            throw new ArgumentException("Salt not provided");

        IntPtr ptr = Marshal.SecureStringToBSTR(password);

        int length = Marshal.ReadInt32(ptr, -4);
        byte[] pwdByteArray = new byte[length];
        try
        {
            GCHandle handle = GCHandle.Alloc(pwdByteArray, GCHandleType.Pinned);
            try
            {
                for (int i = 0; i < length; i++)
                    pwdByteArray[i] = Marshal.ReadByte(ptr, i);

                using Rfc2898DeriveBytes key_PBKDF2 = new Rfc2898DeriveBytes(pwdByteArray, salt, noOfIterations, HashAlgorithmName.SHA256);
                return key_PBKDF2.GetBytes(keySizeInBytes);
            }
            finally
            {
                Array.Clear(pwdByteArray, 0, pwdByteArray.Length);
                handle.Free();
            }
        }
        finally
        {
            Marshal.ZeroFreeBSTR(ptr);
        }
    }

    //  Call this function to remove the key from memory after use for security
    [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
    private static extern bool ZeroMemory(IntPtr Destination, int Length);

    public static bool ZeroString(this string s)
    {
        GCHandle gch = GCHandle.Alloc(s, GCHandleType.Pinned);
        bool res = ZeroMemory(gch.AddrOfPinnedObject(), s.Length * 2);
        gch.Free();
        return res;
    }

    public static void Clear(this byte[] arr)
    {
        Array.Clear(arr, 0, arr.Length);
        arr = null;
    }
}
