﻿using System;
using System.Security.Cryptography;

namespace WhatsAppApi.Helper
{
    public class KeyStream
    {
        public const string AuthMethod = "WAUTH-2";
        private const int Drop = 768;
        private RC4 rc4;
        private HMACSHA1 mac;
        private uint seq;

        public KeyStream(byte[] key, byte[] macKey)
        {
            this.rc4 = new RC4(key, 768);
            this.mac = new HMACSHA1(macKey);
        }

        public static byte[][] GenerateKeys(byte[] password, byte[] nonce)
        {
            byte[][] aArray = new byte[4][];
            byte[][] bArray = aArray;
            byte[] cArray = new byte[]
            {
                1,
                2,
                3,
                4
            };
            byte[] dArray = new byte[nonce.Length + 1];
            for (int i = 0; i < nonce.Length; i++)
            {
                dArray[i] = nonce[i];
            }
            nonce = dArray;
            for (int j = 0; j < bArray.Length; j++)
            {
                nonce[nonce.Length - 1] = cArray[j];
                Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, nonce, 2);
                bArray[j] = rfc2898DeriveBytes.GetBytes(20);
            }
            return bArray;
        }

        public void DecodeMessage(byte[] buffer, int macOffset, int offset, int length)
        {
            byte[] array = this.ComputeMac(buffer, offset, length);
            for (int i = 0; i < 4; i++)
            {
                if (buffer[macOffset + i] != array[i])
                {
                    throw new Exception(string.Format("MAC mismatch on index {0}! {1} != {2}", i, buffer[macOffset + i], array[i]));
                }
            }
            this.rc4.Cipher(buffer, offset, length);
        }

        public void EncodeMessage(byte[] buffer, int macOffset, int offset, int length)
        {
            this.rc4.Cipher(buffer, offset, length);
            byte[] array = this.ComputeMac(buffer, offset, length);
            Array.Copy(array, 0, buffer, macOffset, 4);
        }

        private byte[] ComputeMac(byte[] buffer, int offset, int length)
        {
            this.mac.Initialize();
            this.mac.TransformBlock(buffer, offset, length, buffer, offset);
            byte[] array = new byte[]
            {
                (byte)(this.seq >> 24),
                (byte)(this.seq >> 16),
                (byte)(this.seq >> 8),
                (byte)this.seq
            };
            this.mac.TransformFinalBlock(array, 0, array.Length);
            this.seq += 1u;
            return this.mac.Hash;
        }
    }
}