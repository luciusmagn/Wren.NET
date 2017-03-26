using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wren.NET;
using static Wren.NET.Raw;

namespace TestProject
{
	public class Program
	{
		public static void Main()
		{
			//Raw way
			Raw.WrenConfiguration cfg = new WrenConfiguration();
			Raw.NativeMethods.wrenInitConfiguration(ref cfg);
			cfg.writeFn = WrenWrite;
			IntPtr vm = NativeMethods.wrenNewVM(ref cfg);
			WrenInterpretResult result = NativeMethods.wrenInterpret(vm, "System.print(\"Hi!\")");
			Console.WriteLine(result.ToString());
			Console.ReadLine();

			//Wrapped way
			WrenVM VM = new WrenVM();
			VM.Start();
			Console.WriteLine(VM.Interpret(@"System.print(""Howdy worldy"")"));
			Console.ReadLine();
		}
	}
}
