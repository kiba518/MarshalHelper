using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarshalHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            IntPtr unsafePI = new IntPtr(); 
            int testint = 518;
            unsafePI = MarshalHelper.Int32ToIntPtr(testint);
            int UnsaferetNoFree = Unsafe_Int32ToIntPtr_NoFree(unsafePI);
            Console.WriteLine("Unsafe_Int32ToIntPtr_Free-取IntPtr的值" + UnsaferetNoFree); 

            int retNoFree = Int32ToIntPtr_NoFree();
            IntPtr retNoFreeIP = new IntPtr(retNoFree);
            int retFree = Int32ToIntPtr_Free();
            IntPtr retFreeIP = new IntPtr(retFree); 

            new Task(()=> {
                try
                {
                    int unsafeafterNoFree = MarshalHelper.IntPtrToInt32(unsafePI);
                    Console.WriteLine("Int32ToIntPtr_NoFree-未释放Intptr的线程取值" + unsafeafterNoFree);
                    int afterNoFree = MarshalHelper.IntPtrToInt32(retNoFreeIP);
                    Console.WriteLine("Int32ToIntPtr_NoFree-未释放Intptr的线程取值" + afterNoFree);
                    int afterFree = MarshalHelper.IntPtrToInt32(retFreeIP);
                    Console.WriteLine("Int32ToIntPtr_Free-已释放Intptr的线程取值" + afterNoFree);

                }
                catch (Exception ex)
                {

                }
                }).Start();

            Console.ReadKey();

            string str = "I am Kiba518!";
            int strlen = str.Length;
            IntPtr sptr = MarshalHelper.StringToIntPtr(str);
            unsafe
            {
                char* src = (char*)sptr.ToPointer();
                //Console.WriteLine("地址" + (&src)); //报错
                for (int i = 0; i <= strlen; i++)
                {
                    Console.Write(src[i]);
                    src[i] = '0';
                }
                Console.WriteLine();
                Console.WriteLine("========不安全代码改值========="); 
                for (int i = 0; i <= strlen; i++)
                {
                    Console.Write(src[i]); 
                }
            }
            Console.ReadKey(); 
        }

        #region SafeCode 安全代码
        static int Int32ToIntPtr_Free()
        {
            IntPtr pointerInt = new IntPtr();
            int testint = 518; 
            pointerInt = MarshalHelper.Int32ToIntPtr(testint);
            int testintT = MarshalHelper.IntPtrToInt32(pointerInt); 
            Console.WriteLine("Int32ToIntPtr_Free-取IntPtr的值" + testintT);
            MarshalHelper.Free(pointerInt);
            int testintT2 = (int)pointerInt;
            return testintT2;
        }
        static int Int32ToIntPtr_NoFree()
        {
            IntPtr pointerInt = new IntPtr();
            int testint = 518;
            pointerInt = MarshalHelper.Int32ToIntPtr(testint);
            int testintT = MarshalHelper.IntPtrToInt32(pointerInt);
            Console.WriteLine("Int32ToIntPtr_NoFree-取IntPtr的值" + testintT);
            int testintT2 = (int)pointerInt;
            return testintT2;

        }
        #endregion

        #region UnsafeCode 不安全代码
        static int Unsafe_Int32ToIntPtr_NoFree(IntPtr pointerInt)
        { 
            unsafe
            {
                int *pi = (int*)pointerInt.ToPointer(); 
                return *pi;
            }
        }
        #endregion
    }
}
