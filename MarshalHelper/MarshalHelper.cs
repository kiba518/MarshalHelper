using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Utility.Tool;

namespace MarshalHelper
{
    public class MarshalHelper
    {
        #region unsafe 不安全代码 IntPtr to byte[]
        public static byte[] Unsafe_IntPtrToByte(IntPtr source, Int32 byteLength)
        {
            unsafe
            {
                byte[] data = new byte[byteLength];
                void* tempData = source.ToPointer();
                using (System.IO.UnmanagedMemoryStream tempUMS = new System.IO.UnmanagedMemoryStream((byte*)tempData, byteLength))
                {
                    tempUMS.Read(data, 0, byteLength);
                }
                return data;
            }
        }
        #endregion

        #region IntPtr to byte[]
        public static byte[] IntPtrToByte(IntPtr pointerInt, Int32 byteLength)
        {
            try
            {
                byte[] destinationData = new byte[byteLength];
                Marshal.Copy(pointerInt, destinationData, 0, byteLength);//将数据从非托管内存指针复制到托管 8 位无符号整数数组。
                return destinationData;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 将bytelist复制到非托管内存指针IntPtr里  
        /// </summary> 
        /// <param name="sourceByteArray"></param> 
        public static IntPtr ByteToIntPtr(byte[] source)
        {
            try
            {
                IntPtr destpointer = Marshal.AllocHGlobal(source.Length);
                Marshal.Copy(source, 0, destpointer, source.Length);
                return destpointer;
            }
            catch
            {
                return IntPtr.Zero;
            }
        }
       
        #endregion

        #region string to IntPtr / IntPtr to string
        /// <summary>
        /// 使用Marshal提取[句柄/C++指针]指向的值，并转换成String
        /// </summary>
        public static string IntPtrToString(IntPtr pointer, int byteLength = byte.MaxValue)
        {
            byte[] destinationData = new byte[byteLength];
            Marshal.Copy(pointer, destinationData, 0, byteLength);//将数据从非托管内存指针复制到托管 8 位无符号整数数组。
            string strData = Encoding.ASCII.GetString(destinationData);
            return strData;
        }
        /// <summary>
        /// 使用Marshal将String封装成[句柄/C++指针]，形成IntPtr
        /// </summary>
        public static IntPtr StringToIntPtr(string source, Encoding type = null)
        {
            IntPtr destpointer = IntPtr.Zero;
            if (type == null)
            {
                type = Encoding.Default;
            }
            switch (type.ToString())
            {
                case "Default":
                    destpointer = Marshal.StringToHGlobalAnsi(source);
                    break;
                default:
                    destpointer = Marshal.StringToHGlobalUni(source);
                    break;

            }
            return destpointer;
        } 
        #endregion

        #region  Int32 to IntPtr / IntPtr to Int32
        public static IntPtr Int32ToIntPtr(Int32 source)
        {
            IntPtr destpointer = Marshal.AllocHGlobal(4);
            byte[] sourceData = ByteHelper.IntToByte(source);
            Marshal.Copy(sourceData, 0, destpointer, sourceData.Length);// 将数据从一维托管 8 位无符号整数数组复制到非托管内存指针。  
            return destpointer;
        }
        public static int IntPtrToInt32(IntPtr pointerInt, int byteLength = 4)
        {
            byte[] destinationData = new byte[byteLength];
            Marshal.Copy(pointerInt, destinationData, 0, byteLength);//将数据从非托管内存指针复制到托管 8 位无符号整数数组。
            int ret = ByteHelper.ByteToInt32(destinationData);
            return ret;
        }
        #endregion

        #region Int16 to IntPtr / IntPtr to Int16
        public static IntPtr Int16ToIntPtr(Int16 source)
        {
            IntPtr destpointer = Marshal.AllocHGlobal(4);
            byte[] sourceData = ByteHelper.IntToByte(source);
            Marshal.Copy(sourceData, 0, destpointer, sourceData.Length);// 将数据从一维托管 8 位无符号整数数组复制到非托管内存指针。  
            return destpointer;
        }
        public static int IntPtrToInt16(IntPtr pointerInt, int byteLength = 2)
        {
            byte[] destinationData = new byte[byteLength];
            Marshal.Copy(pointerInt, destinationData, 0, byteLength);//将数据从非托管内存指针复制到托管 8 位无符号整数数组。
            int ret = ByteHelper.ByteToInt32(destinationData);
            return ret;
        }
        #endregion

        #region struct to IntPtr / IntPtr to struct
        public static IntPtr StructToIntPtr(object source)
        {
            Type type = source.GetType();
            int size = Marshal.SizeOf(type);
            IntPtr destpointer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(source, destpointer, false);
                return destpointer;
            }
            catch (Exception ex)
            {
                FreeStruct(destpointer, type);
                return IntPtr.Zero;
            }
        }
        public static T IntPtrToStruct<T>(IntPtr source)
        { 
            return (T)Marshal.PtrToStructure(source, typeof(T)); 
        }
        public static object IntPtrToStruct(IntPtr source, Type type)
        {
            return Marshal.PtrToStructure(source, type); 
        }
        #endregion

        #region 释放IntPtr
        public static void Free(IntPtr pointer)
        {
            Marshal.FreeHGlobal(pointer);
        }
        public static void FreeStruct(IntPtr pointer, Type type)
        {
            Marshal.DestroyStructure(pointer, type);
        }
        #endregion

        #region 委托 to IntPtr
        public static IntPtr GetFunctionPointer(Delegate pointer)
        {
            IntPtr ret = Marshal.GetFunctionPointerForDelegate(pointer);
            return ret;

        }
        #endregion

        #region byte to struct
        public static StructType BytesToStruct<StructType>(byte[] bytesBuffer)
        {
            // 检查长度。
            if (bytesBuffer.Length != Marshal.SizeOf(typeof(StructType)))
            {
                throw new ArgumentException("bytesBuffer参数和structObject参数字节长度不一致。");
            }

            IntPtr bufferHandler = Marshal.AllocHGlobal(bytesBuffer.Length);
            for (int index = 0; index < bytesBuffer.Length; index++)
            {
                Marshal.WriteByte(bufferHandler, index, bytesBuffer[index]);
            }
            StructType structObject = (StructType)Marshal.PtrToStructure(bufferHandler, typeof(StructType));
            Marshal.FreeHGlobal(bufferHandler);
            return structObject;
        }
        #endregion

       
        
    }
}
