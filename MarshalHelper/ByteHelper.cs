using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Utility.Tool
{
    public class ByteHelper
    {
        /// <summary>
        /// 将文件转换成byte[]数组
        /// </summary>
        /// <param name="fileUrl">文件路径文件名称</param>
        /// <returns>byte[]数组</returns>
        public static byte[] FileToByte(string fileUrl)
        {
            try
            {
                using (FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read))
                {
                    byte[] byteArray = new byte[fs.Length];
                    fs.Read(byteArray, 0, byteArray.Length);
                    return byteArray;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将byte[]数组保存成文件
        /// </summary>
        /// <param name="byteArray">byte[]数组</param>
        /// <param name="fileName">保存至硬盘的文件路径</param>
        /// <returns></returns>
        public static bool ByteToFile(byte[] byteArray, string fileName)
        {
            bool result = false;
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static byte[] IntToByte(int source)
        {
            byte[] intBuff = BitConverter.GetBytes(source);
            return intBuff;
        }

        public static byte[] StringToBytebyEncoding(string source)
        {
            byte[] Stringbyte = Encoding.Default.GetBytes(source);
            return Stringbyte;
        }
        public static string ByteToStringbyEncoding(byte[] source)
        {
            string str = System.Text.Encoding.Default.GetString(source);
            return str;
        }
        /// <summary>
        /// 使用方法 ByteHelper.ByteToInt(pByte.Skip(0).Take(4).ToArray());
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ByteToInt32(byte[] source)
        {
            int intBuff = BitConverter.ToInt32(source, 0);
            return intBuff;
        }

        public static short ByteToInt16(byte[] source)
        {
            short intBuff = BitConverter.ToInt16(source, 0);
            return intBuff;
        }
        public static bool ByteToBool(byte[] source)
        {
            bool booBuff = BitConverter.ToBoolean(source, 0);
            return booBuff;
        }
        public static char ByteToChar(byte[] source)
        {
            char charBuff = BitConverter.ToChar(source, 0);
            return charBuff;
        }
        public static String ByteToString(byte[] source)
        {
            String strBuff = BitConverter.ToString(source, 0);
            return strBuff;
        }

        public static byte[] StructToBytes<T>(T obj)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr bufferPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(obj, bufferPtr, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(bufferPtr, bytes, 0, size);
                return bytes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in StructToBytes ! " + ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPtr);
            }
        }


        //将一个结构序列化为字节数组
        private IFormatter formatter = new BinaryFormatter();
        private ValueType DeserializeByteArrayToInfoObj(byte[] bytes)
        {
            ValueType vt;
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            try
            {
                MemoryStream stream = new MemoryStream(bytes);
                stream.Position = 0;
                stream.Seek(0, SeekOrigin.Begin);
                vt = (ValueType)formatter.Deserialize(stream);
                stream.Close();
                return vt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //将一个结构序列化为字节数组
        private byte[] SerializeInfoObjToByteArray(ValueType infoStruct)
        {
            if (infoStruct == null)
            {
                return null;
            }

            try
            {
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, infoStruct);

                byte[] bytes = new byte[(int)stream.Length];
                stream.Position = 0;
                int count = stream.Read(bytes, 0, (int)stream.Length);
                stream.Close();
                return bytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
