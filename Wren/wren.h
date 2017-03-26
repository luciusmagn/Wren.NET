#ifndef wren_h
#define wren_h
#include <stdarg.h>
#include <stdlib.h>
#include <stdbool.h>
#define WREN_VERSION_MAJOR 0
#define WREN_VERSION_MINOR 1
#define WREN_VERSION_PATCH 0
#define WREN_VERSION_STRING "0.1.0"
#define WREN_VERSION_NUMBER (WREN_VERSION_MAJOR * 1000000 + \
                             WREN_VERSION_MINOR * 1000 + \
                             WREN_VERSION_PATCH)
typedef struct WrenVM WrenVM;
typedef struct WrenHandle WrenHandle;
typedef void* (*WrenReallocateFn)(void* memory, size_t newSize);
typedef void(*WrenForeignMethodFn)(WrenVM* vm);
typedef void(*WrenFinalizerFn)(void* data);
typedef char* (*WrenLoadModuleFn)(WrenVM* vm, const char* name);
typedef WrenForeignMethodFn(*WrenBindForeignMethodFn)(WrenVM* vm,
	const char* module,
	const char* className,
	bool isStatic,
	const char* signature);
typedef void(*WrenWriteFn)(WrenVM* vm, const char* text);
typedef enum
{
	WREN_ERROR_COMPILE,
	WREN_ERROR_RUNTIME,
	WREN_ERROR_STACK_TRACE
} WrenErrorType;
typedef void(*WrenErrorFn)(
	WrenVM* vm, WrenErrorType type, const char* module, int line,
	const char* message);
typedef struct
{
	WrenForeignMethodFn allocate;
	WrenFinalizerFn finalize;
} WrenForeignClassMethods;
typedef WrenForeignClassMethods(*WrenBindForeignClassFn)(
	WrenVM* vm, const char* module, const char* className);
typedef struct
{
	WrenReallocateFn reallocateFn;
	WrenLoadModuleFn loadModuleFn;
	WrenBindForeignMethodFn bindForeignMethodFn;
	WrenBindForeignClassFn bindForeignClassFn;
	WrenWriteFn writeFn;
	WrenErrorFn errorFn;
	size_t initialHeapSize;
	size_t minHeapSize;
	int heapGrowthPercent;
	void* userData;
} WrenConfiguration;
typedef enum
{
	WREN_RESULT_SUCCESS,
	WREN_RESULT_COMPILE_ERROR,
	WREN_RESULT_RUNTIME_ERROR
} WrenInterpretResult;
typedef enum
{
	WREN_TYPE_BOOL,
	WREN_TYPE_NUM,
	WREN_TYPE_FOREIGN,
	WREN_TYPE_LIST,
	WREN_TYPE_NULL,
	WREN_TYPE_STRING,
	WREN_TYPE_UNKNOWN
} WrenType;
__declspec(dllexport) void wrenInitConfiguration(WrenConfiguration* configuration);
__declspec(dllexport) WrenVM* wrenNewVM(WrenConfiguration* configuration);
__declspec(dllexport) void wrenFreeVM(WrenVM* vm);
__declspec(dllexport) void wrenCollectGarbage(WrenVM* vm);
__declspec(dllexport) WrenInterpretResult wrenInterpret(WrenVM* vm, const char* source);
__declspec(dllexport) WrenHandle* wrenMakeCallHandle(WrenVM* vm, const char* signature);
__declspec(dllexport) WrenInterpretResult wrenCall(WrenVM* vm, WrenHandle* method);
__declspec(dllexport) void wrenReleaseHandle(WrenVM* vm, WrenHandle* handle);

__declspec(dllexport) int wrenGetSlotCount(WrenVM* vm);
__declspec(dllexport) void wrenEnsureSlots(WrenVM* vm, int numSlots);
__declspec(dllexport) WrenType wrenGetSlotType(WrenVM* vm, int slot);
__declspec(dllexport) bool wrenGetSlotBool(WrenVM* vm, int slot);
__declspec(dllexport) const char* wrenGetSlotBytes(WrenVM* vm, int slot, int* length);
__declspec(dllexport) double wrenGetSlotDouble(WrenVM* vm, int slot);
__declspec(dllexport) void* wrenGetSlotForeign(WrenVM* vm, int slot);
__declspec(dllexport) const char* wrenGetSlotString(WrenVM* vm, int slot);
__declspec(dllexport) WrenHandle* wrenGetSlotHandle(WrenVM* vm, int slot);

__declspec(dllexport) void wrenSetSlotBool(WrenVM* vm, int slot, bool value);
__declspec(dllexport) void wrenSetSlotBytes(WrenVM* vm, int slot, const char* bytes, size_t length);
__declspec(dllexport) void wrenSetSlotDouble(WrenVM* vm, int slot, double value);
__declspec(dllexport) void* wrenSetSlotNewForeign(WrenVM* vm, int slot, int classSlot, size_t size);
__declspec(dllexport) void wrenSetSlotNewList(WrenVM* vm, int slot);
__declspec(dllexport) void wrenSetSlotNull(WrenVM* vm, int slot);
__declspec(dllexport) void wrenSetSlotString(WrenVM* vm, int slot, const char* text);
__declspec(dllexport) void wrenSetSlotHandle(WrenVM* vm, int slot, WrenHandle* handle);
__declspec(dllexport) int wrenGetListCount(WrenVM* vm, int slot);
__declspec(dllexport) void wrenGetListElement(WrenVM* vm, int listSlot, int index, int elementSlot);
__declspec(dllexport) void wrenInsertInList(WrenVM* vm, int listSlot, int index, int elementSlot);
__declspec(dllexport) void wrenGetVariable(WrenVM* vm, const char* module, const char* name,
	int slot);
__declspec(dllexport) void wrenAbortFiber(WrenVM* vm, int slot);
__declspec(dllexport) void* wrenGetUserData(WrenVM* vm);
__declspec(dllexport) void wrenSetUserData(WrenVM* vm, void* userData);
#endif
