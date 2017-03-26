using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Wren.NET.Raw;

namespace Wren.NET
{
	public class WrenVM
	{
		public IntPtr vmptr; // don't touch >:(
		public WrenConfiguration cfg; // also don't touch >:(
		public bool Working;

		public uint InitialHeapSize
		{
			get { return cfg.initialHeapSize; }
			set { cfg.initialHeapSize = value; }
		}
		public uint MinHeapSize
		{
			get { return cfg.minHeapSize; }
			set { cfg.minHeapSize = value; }
		}
		public int HeapGrowthPercent
		{
			get { return cfg.heapGrowthPercent; }
			set { cfg.heapGrowthPercent = value; }
		}
		public WrenWriteFn WriteFN
		{
			get { return cfg.writeFn; }
			set { cfg.writeFn = value; }
		}
		public WrenErrorFn ErrorFn
		{
			get { return cfg.errorFn; }
			set { cfg.errorFn = value; }
		}
		public WrenLoadModuleFn LoadModuleFn
		{
			get { return cfg.loadModuleFn; }
			set { cfg.loadModuleFn = value; }
		}
		public WrenBindForeignMethodFn BindForeignMethodFn
		{
			get { return cfg.bindForeignMethodFn; }
			set { cfg.bindForeignMethodFn = value; }
		}
		public WrenBindForeignClassFn BindForeignClassFn
		{
			get { return cfg.bindForeignClassFn; }
			set { cfg.bindForeignClassFn = value; }
		}

		public void Start()
		{
			vmptr = NativeMethods.wrenNewVM(ref cfg);
			Working = true;
		}

		public void Stop()
		{
			NativeMethods.wrenFreeVM(vmptr);
			Working = false;
		}

		public WrenInterpretResult Interpret(string source)
		{
			if (Working)
				return NativeMethods.wrenInterpret(vmptr, source);
			else
				return WrenInterpretResult.WREN_RESULT_COMPILE_ERROR;
		}

		public WrenVM()
		{
			NativeMethods.wrenInitConfiguration(ref cfg);
			cfg.writeFn = WrenWrite;
			cfg.errorFn = WrenError;
			Working = false;
		}

		~WrenVM()
		{
			NativeMethods.wrenFreeVM(vmptr);
		}
	}

	public enum WrenInterpretResult
	{
		WREN_RESULT_SUCCESS,
		WREN_RESULT_COMPILE_ERROR,
		WREN_RESULT_RUNTIME_ERROR,
	}

	public enum WrenType
	{
		WREN_TYPE_BOOL,
		WREN_TYPE_NUM,
		WREN_TYPE_FOREIGN,
		WREN_TYPE_LIST,
		WREN_TYPE_NULL,
		WREN_TYPE_STRING,
		WREN_TYPE_UNKNOWN,
	}

	public enum WrenErrorType
	{
		WREN_ERROR_COMPILE,
		WREN_ERROR_RUNTIME,
		WREN_ERROR_STACK_TRACE,
	}
}
