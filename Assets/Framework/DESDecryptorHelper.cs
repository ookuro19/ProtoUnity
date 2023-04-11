using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace Framework
{

    public class DESDecryptorHelper
    {
        public static byte[] DEFAULT_ENCRYPT_KEY
        {
            get
            {
                return System.Text.Encoding.UTF8.GetBytes("dddddddd");
            }
        }

        public static byte[] DEFAULT_ENCRYPT_IV
        {
            get
            {
                byte[] iv = { 0xAB, 0xAB, 0xAB, 0xAB, 0xAB, 0xAB, 0xAB, 0xAB };
                return iv;
            }
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="content">明文</param>K
        /// <param name="key">加密钥匙</param>
        /// <param name="iv">向量</param>
        /// <returns>返回密文</returns>
        public static byte[] DESEncryptor(byte[] data, byte[] desKey, byte[] desIV)
        {
            byte[] res = null;
            if (desIV.Length != desKey.Length)
            {
                UnityEngine.Debug.LogError("desIV的长度与desKey的长度不一致");
                return res;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(desKey, desIV), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    res = ms.ToArray();
                }
            }

            return res;
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="content">密文</param>K
        /// <param name="key">加密钥匙</param>
        /// <param name="iv">向量</param>
        /// <returns>返回明文</returns>
        public static byte[] DESDecryptor(byte[] data, byte[] desKey, byte[] desIV)
        {
            byte[] res = null;

            if (desIV.Length != desKey.Length)
            {
                UnityEngine.Debug.LogError("desIV的长度与desKey的长度不一致");
                return res;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(desKey, desIV), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    res = ms.ToArray();
                }
            }
            return res;
        }

    }
}
