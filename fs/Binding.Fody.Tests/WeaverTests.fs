namespace Binding.Fody.Tests
open System
open System.Collections.Generic
open System.Linq
open System.Reflection
open NUnit.Framework
open FsUnit 
open ImpromptuInterface.FSharp
 
[<TestFixture>]
type WeaverTests() = 
    let mutable assembly:Assembly = null

    [<TestFixtureSetUp>] //Changed from SetUp to TestFixtureSetUp so file locking doesn't cause false negatives
    member x.Setup () = 
        assembly <- WeaverHelper.WeaveAssembly()

    [<Test>] 
    member x.
        ``Validate Hello World Is Injected`` () =
            let type' = assembly.GetType("Hello")
            let instance = Activator.CreateInstance(type')
            instance?World() |> should equal "Hello World"

#if DEBUG
    [<Test>]
    member x.
        ``Verify assembly validity with PeVerifier`` () =
            let path = assembly.CodeBase.Remove(0, 8)
            Verifier.Verify path
#endif