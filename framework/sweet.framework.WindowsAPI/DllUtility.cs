using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace sweet.framework.WindowsAPI
{
    public static class DllUtility
    {
        /// <summary>
        /// 动态注册COM
        /// 同 regsvr32 命令
        /// </summary>
        /// <returns></returns>
        public static int DllRegisterServer(string dllPath)
        {
            using (DllCaller getdc = new DllCaller(dllPath, "DllRegisterServer", 0))
            {
                return (int)getdc.Call();
            }
        }

        /// <summary>
        /// 解除注册
        /// </summary>
        /// <returns></returns>
        public static int DllUnregisterServer(string dllPath)
        {
            using (DllCaller getdc = new DllCaller(dllPath, "DllUnregisterServer", 0))
            {
                return (int)getdc.Call();
            }
        }

        public class DllCaller : IDisposable
        {
            [DllImport("Kernel32.dll")]
            private static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport("Kernel32.dll")]
            private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

            [DllImport("Kernel32.dll")]
            private static extern bool FreeLibrary(IntPtr hModule);

            private IntPtr _libPtr;
            private MethodInfo _method;

            /// <param name="dllFile">DLL文件的位置</param>
            /// <param name="functionName">函数名，注意带字符串的函数分W版跟A版</param>
            /// <param name="result">返回的数类型，如果返回void，使用typeof(void)</param>
            /// <param name="args">参数列表</param>
            /// <remarks>
            /// 注意:为了方便使用，返回类型跟参数类型都用一个实例表示，实例值没影响。
            /// 例如:用true,false表示bool; 0表示int; (byte)0表示byte, IntPtr.Zero表示指针等
            /// </remarks>
            /// <example>
            /// int MessageBox(
            /// HWND hWnd,
            /// LPCTSTR lpText,
            /// LPCTSTR lpCaption,
            /// UINT uType
            /// );
            /// DllCaller MessageBox = new DllCaller(
            /// "user32.dll", "MessageBoxW", IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (uint)0
            /// );
            /// </example>
            public DllCaller(string dllFile, string functionName, object result, params object[] args)
            {
                if (dllFile == null) throw new ArgumentNullException();
                if (functionName == null) throw new ArgumentNullException();

                this._libPtr = LoadLibrary(dllFile);
                if (this._libPtr == IntPtr.Zero) throw new DllNotFoundException(dllFile);

                IntPtr procPtr = GetProcAddress(this._libPtr, functionName);
                if (procPtr == IntPtr.Zero) throw new EntryPointNotFoundException(functionName);

                AssemblyName asmName = new AssemblyName();
                asmName.Name = "DynamicAssembly";

                AssemblyBuilder asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
                ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule("DynamicModule");

                Type resultType = (result == typeof(void) ? typeof(void) : result.GetType());
                Type[] argTypes = new Type[args.Length];
                for (int i = 0; i < args.Length; i++)
                    argTypes[i] = args[i].GetType();

                MethodBuilder funBuilder = modBuilder.DefineGlobalMethod(
                    functionName,
                    MethodAttributes.Public | MethodAttributes.Static,
                    resultType,
                    argTypes
                    );

                ILGenerator ilGen = funBuilder.GetILGenerator();
                for (int i = 0; i < args.Length; i++)
                    ilGen.Emit(OpCodes.Ldarg, i);

                if (IntPtr.Size == 4)
                    ilGen.Emit(OpCodes.Ldc_I4, (int)procPtr);
                else if (IntPtr.Size == 8)
                    ilGen.Emit(OpCodes.Ldc_I8, (long)procPtr);

                ilGen.EmitCalli(OpCodes.Calli, CallingConvention.StdCall, resultType, argTypes);
                ilGen.Emit(OpCodes.Ret);
                modBuilder.CreateGlobalFunctions();
                this._method = modBuilder.GetMethod(functionName);
            }

            public object Call(params object[] args)
            {
                return this._method.Invoke(null, args);
            }

            public void Dispose()
            {
                if (this._method != null)
                {
                    if (this._libPtr != IntPtr.Zero)
                        FreeLibrary(this._libPtr);
                    this._libPtr = IntPtr.Zero;
                    this._method = null;
                    GC.SuppressFinalize(this);
                }
            }

            ~DllCaller()
            {
                this.Dispose();
            }
        }

        public class MarshalBuffer : IDisposable
        {
            public static MarshalBuffer FromString(string text)
            {
                MarshalBuffer buffer = new MarshalBuffer();
                buffer.Ptr = Marshal.StringToCoTaskMemUni(text);
                return buffer;
            }

            public IntPtr Ptr;

            private MarshalBuffer()
            {
            }

            public MarshalBuffer(int cb)
            {
                this.Ptr = Marshal.AllocHGlobal(cb);
            }

            public void Dispose()
            {
                if (Ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(Ptr);
                    this.Ptr = IntPtr.Zero;
                }
            }

            ~MarshalBuffer()
            {
                this.Dispose();
                GC.SuppressFinalize(this);
            }

            public override string ToString()
            {
                return Marshal.PtrToStringUni(this.Ptr);
            }

            public int this[int index]
            {
                get { return Marshal.ReadInt32(this.Ptr, index * 4); }
                set { Marshal.WriteInt32(this.Ptr, index * 4, value); }
            }
        }
    }
}

//class Test
//{
//    public static void DrawScreenTest()
//    {
//        DllCaller getdc = new DllCaller("user32.dll", "GetDC", IntPtr.Zero, IntPtr.Zero);
//        DllCaller textout = new DllCaller("gdi32.dll", "TextOutW", false, IntPtr.Zero, 0, 0, IntPtr.Zero, 0);
//        IntPtr dc = (IntPtr)getdc.Call(IntPtr.Zero);
//        MarshalBuffer text = MarshalBuffer.FromString("Hello World");
//        Random rand = new Random();
//        for (int i = 0; i < 100; i++)
//            textout.Call(dc, rand.Next(1024), rand.Next(768), text.ptr, "Hello World".Length);
//    }

//    public static void GetComputerNameTest()
//    {
//        MarshalBuffer text = new MarshalBuffer(100);
//        MarshalBuffer len = new MarshalBuffer(4);
//        len[0] = 50;
//        DllCaller getComName = new DllCaller("kernel32.dll", "GetComputerNameW", false, IntPtr.Zero, IntPtr.Zero);
//        getComName.Call(text.ptr, len.ptr);
//        Console.WriteLine(text.ToString());
//        Console.WriteLine(len[0]);
//    }

//    static void Main()
//    {
//        DrawScreenTest();
//        GetComputerNameTest();
//    }
//}