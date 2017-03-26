using System;
using System.Runtime.InteropServices;

namespace Wren.NET
{
	public static class Raw
	{
		//all vm is of type WrenVM* actually
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void WrenFinalizerFn(IntPtr data);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr WrenReallocateFn(IntPtr memory, IntPtr newSize);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void WrenForeignMethodFn(IntPtr vm);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr WrenLoadModuleFn(IntPtr vm, [In()] [MarshalAs(UnmanagedType.LPStr)] string name);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate WrenForeignMethodFn WrenBindForeignMethodFn(IntPtr vm,
			[In()] [MarshalAs(UnmanagedType.LPStr)] string module,
			[In()] [MarshalAs(UnmanagedType.LPStr)] string className,
			[MarshalAs(UnmanagedType.I1)] bool isStatic,
			[In()] [MarshalAs(UnmanagedType.LPStr)] string signature);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void WrenWriteFn(IntPtr vm, [In()] [MarshalAs(UnmanagedType.LPStr)] string text);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void WrenErrorFn(IntPtr vm, WrenErrorType type,
			[In()] [MarshalAs(UnmanagedType.LPStr)] string module, int line,
			[In()] [MarshalAs(UnmanagedType.LPStr)] string message);

		[StructLayout(LayoutKind.Sequential)]
		public struct WrenForeignClassMethods
		{
			public WrenForeignMethodFn allocate;
			public WrenFinalizerFn finalize;
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate WrenForeignClassMethods WrenBindForeignClassFn(IntPtr vm,
			[In()] [MarshalAs(UnmanagedType.LPStr)] string module,
			[In()] [MarshalAs(UnmanagedType.LPStr)] string className);

		[StructLayout(LayoutKind.Sequential)]
		public struct WrenConfiguration
		{
			public WrenReallocateFn reallocateFn;
			public WrenLoadModuleFn loadModuleFn;
			public WrenBindForeignMethodFn bindForeignMethodFn;
			public WrenBindForeignClassFn bindForeignClassFn;
			public WrenWriteFn writeFn;
			public WrenErrorFn errorFn;
			public uint initialHeapSize;
			public uint minHeapSize;
			public int heapGrowthPercent;
			/// void*
			public IntPtr userData;
		}

		public partial class NativeMethods
		{
			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenInitConfiguration")]
			public static extern void wrenInitConfiguration(ref WrenConfiguration configuration);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenNewVM")]
			public static extern IntPtr wrenNewVM(ref WrenConfiguration configuration);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenFreeVM")]
			public static extern void wrenFreeVM(IntPtr vm);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenCollectGarbage")]
			public static extern void wrenCollectGarbage(IntPtr vm);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenInterpret")]
			public static extern WrenInterpretResult wrenInterpret(IntPtr vm,
				[In()] [MarshalAs(UnmanagedType.LPStr)] string source);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenMakeCallHandle")]
			public static extern IntPtr wrenMakeCallHandle(IntPtr vm,
				[In()] [MarshalAs(UnmanagedType.LPStr)] string signature);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenCall")]
			public static extern WrenInterpretResult wrenCall(IntPtr vm, IntPtr method);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenReleaseHandle")]
			public static extern void wrenReleaseHandle(IntPtr vm, IntPtr handle);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetSlotCount")]
			public static extern int wrenGetSlotCount(IntPtr vm);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenEnsureSlots")]
			public static extern void wrenEnsureSlots(IntPtr vm, int numSlots);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetSlotType")]
			public static extern WrenType wrenGetSlotType(IntPtr vm, int slot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetSlotBool")]
			[return: MarshalAs(UnmanagedType.I1)]
			public static extern bool wrenGetSlotBool(IntPtr vm, int slot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetSlotBytes")]
			public static extern IntPtr wrenGetSlotBytes(IntPtr vm, int slot, ref int length);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetSlotDouble")]
			public static extern double wrenGetSlotDouble(IntPtr vm, int slot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetSlotForeign")]
			public static extern IntPtr wrenGetSlotForeign(IntPtr vm, int slot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetSlotString")]
			public static extern IntPtr wrenGetSlotString(IntPtr vm, int slot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetSlotHandle")]
			public static extern IntPtr wrenGetSlotHandle(IntPtr vm, int slot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenSetSlotBool")]
			public static extern void wrenSetSlotBool(IntPtr vm, int slot,
				[MarshalAs(UnmanagedType.I1)] bool value);

			///length: size_t->unsigned int
			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenSetSlotBytes")]
			public static extern void wrenSetSlotBytes(IntPtr vm, int slot,
				[In()] [MarshalAs(UnmanagedType.LPStr)] string bytes,
				[MarshalAs(UnmanagedType.SysUInt)] uint length);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenSetSlotDouble")]
			public static extern void wrenSetSlotDouble(IntPtr vm, int slot, double value);

			/// Return Type: void*
			///size: size_t->unsigned int
			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenSetSlotNewForeign")]
			public static extern IntPtr wrenSetSlotNewForeign(IntPtr vm, int slot, int classSlot,
				[MarshalAs(UnmanagedType.SysUInt)] uint size);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenSetSlotNewList")]
			public static extern void wrenSetSlotNewList(IntPtr vm, int slot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenSetSlotNull")]
			public static extern void wrenSetSlotNull(IntPtr vm, int slot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenSetSlotString")]
			public static extern void wrenSetSlotString(IntPtr vm, int slot,
				[In()] [MarshalAs(UnmanagedType.LPStr)] string text);

			///handle: WrenHandle*
			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenSetSlotHandle")]
			public static extern void wrenSetSlotHandle(IntPtr vm, int slot, IntPtr handle);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetListCount")]
			public static extern int wrenGetListCount(IntPtr vm, int slot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetListElement")]
			public static extern void wrenGetListElement(IntPtr vm, int listSlot, int index, int elementSlot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenInsertInList")]
			public static extern void wrenInsertInList(IntPtr vm, int listSlot, int index, int elementSlot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetVariable")]
			public static extern void wrenGetVariable(IntPtr vm,
				[In()] [MarshalAs(UnmanagedType.LPStr)] string module,
				[In()] [MarshalAs(UnmanagedType.LPStr)] string name, int slot);

			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenAbortFiber")]
			public static extern void wrenAbortFiber(IntPtr vm, int slot);

			/// Return Type: void*
			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenGetUserData")]
			public static extern IntPtr wrenGetUserData(IntPtr vm);

			///userData: void*
			[DllImport("Wren.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wrenSetUserData")]
			public static extern void wrenSetUserData(IntPtr vm, IntPtr userData);
		}
		public static void WrenWrite(IntPtr vm, [In()] [MarshalAs(UnmanagedType.LPStr)] string text)
		{
			Console.Write(text);
		}
		public static void WrenError(IntPtr vm, WrenErrorType type, [In()] [MarshalAs(UnmanagedType.LPStr)] string module, int line,
			[In()] [MarshalAs(UnmanagedType.LPStr)] string message)
		{
			Console.WriteLine(string.Format("error: {0} at {1}:{2}", message, module, line));
		}
	}
}
