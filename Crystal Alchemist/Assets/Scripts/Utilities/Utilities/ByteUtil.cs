using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text;

namespace CrystalAlchemist
{
    public static class ByteUtil
    {
        public static byte[] JoinBytes(params byte[][] arrays)
        {
            byte[] result = new byte[arrays.Sum(a => a.Length)];

            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, result, offset, array.Length);
                offset += array.Length;
            }

            return result;
        }

        public static byte[] ConvertToByte(bool value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] ConvertToByte(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] ConvertToByte(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] ConvertToByte(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static int ConvertToInt(byte[] bytes)
        {
            byte[] newBytes = new byte[4];
            Array.Copy(bytes, 0, newBytes, 0, newBytes.Length);
            if (BitConverter.IsLittleEndian) Array.Reverse(newBytes);
            return BitConverter.ToInt32(newBytes, 0);
        }

        public static string ConvertToString(byte[] bytes)
        {
            byte[] newBytes = new byte[4];
            if (newBytes.Length > 0)
            {
                Array.Copy(bytes, 0, newBytes, 0, newBytes.Length);
                if (BitConverter.IsLittleEndian) Array.Reverse(newBytes);
                return Encoding.UTF8.GetString(newBytes);
            }
            else return string.Empty;
        }

        public static bool ConvertToBool(byte[] bytes)
        {
            byte[] newBytes = new byte[4];
            Array.Copy(bytes, 0, newBytes, 0, newBytes.Length);
            if (BitConverter.IsLittleEndian) Array.Reverse(newBytes);
            return BitConverter.ToBoolean(newBytes, 0);
        }

        public static float ConvertToFloat(byte[] bytes)
        {
            byte[] newBytes = new byte[4];
            Array.Copy(bytes, 0, newBytes, 0, newBytes.Length);
            if (BitConverter.IsLittleEndian) Array.Reverse(newBytes);
            return (float)BitConverter.ToDouble(newBytes, 0);
        }
    }
}
