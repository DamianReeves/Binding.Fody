namespace Binding.Fody
open System
open System.Linq
open Mono.Cecil
open Mono.Cecil.Rocks
open Mono.Cecil.Cil

type ModuleWeaver() = 
    let mutable typeSystem : TypeSystem = null
    let mutable moduleDefinition : ModuleDefinition = null 
    let mutable logInfo : Action<string> = null
    do
        logInfo <- Action<string>(fun _ -> ())

    member this.LogInfo
        with get() = logInfo 
        and set(value:Action<string>) = logInfo <- value

    member this.ModuleDefinition with get() = moduleDefinition and set(value) = moduleDefinition <- value

    member this.Execute =
        typeSystem <- this.ModuleDefinition.TypeSystem
        let newType = TypeDefinition(null, "Hello", TypeAttributes.Public, typeSystem.Object)
        this.AddConstructor newType
        this.AddHelloWorld newType
        this.ModuleDefinition.Types.Add(newType);
        this.LogInfo.Invoke("Added type 'Hello' with method 'World'.")

    member this.AddConstructor (newType:TypeDefinition) =
        let methodAttrib = MethodAttributes.Public ||| MethodAttributes.SpecialName ||| MethodAttributes.RTSpecialName
        let method' = MethodDefinition(".ctor", methodAttrib, typeSystem.Void)
        let objectConstructor = this.ModuleDefinition.Import(typeSystem.Object.Resolve().GetConstructors().First())
        let processor = method'.Body.GetILProcessor()
        processor.Emit(OpCodes.Ldarg_0)
        processor.Emit(OpCodes.Call, objectConstructor)
        processor.Emit(OpCodes.Ret)
        newType.Methods.Add(method')

    member this.AddHelloWorld (newType:TypeDefinition) =
        let method' = MethodDefinition("World", MethodAttributes.Public, typeSystem.String)
        let processor = method'.Body.GetILProcessor()
        processor.Emit(OpCodes.Ldstr, "Hello World")
        processor.Emit(OpCodes.Ret)
        newType.Methods.Add(method')   
