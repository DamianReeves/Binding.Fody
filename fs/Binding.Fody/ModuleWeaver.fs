namespace Binding.Fody
open System
open System
open System.Linq
open System.Xml.Linq
open Mono.Cecil
open Mono.Cecil.Rocks
open Mono.Cecil.Cil
open ModuleWeaver.Internal

type ModuleWeaver() = 
    let mutable typeSystem : TypeSystem = null
    let mutable moduleDefinition : ModuleDefinition = null 
    let mutable logInfo : Action<string> = null
    let mutable config : XElement = null

    do
        logInfo <- Action<string>(fun _ -> ())

    member this.Config with get() = config and set(value:XElement) = config <- value

    member this.LogInfo
        with get() = logInfo 
        and set(value:Action<string>) = logInfo <- value

    member this.ModuleDefinition with get() = moduleDefinition and set(value) = moduleDefinition <- value

    member this.Execute =
        let bindToAttributeNames = ConfigReader.GetBindToAttributeNames
        let reactToAttributeNames = ConfigReader.GetReactToAttributeNames

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