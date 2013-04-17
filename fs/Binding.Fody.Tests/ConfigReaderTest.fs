namespace Binding.Fody.Tests
open System
open System.Collections.Generic
open System.Linq
open System.Xml.Linq
open System.Reflection
open NUnit.Framework
open FsUnit 
open ModuleWeaver.Internal

[<TestFixture>]
type ConfigReaderTest () =
    [<Test>]
    member x.
        ``GetBindToAttributeNames() Should Include AttributeName Specified In BindToAttributeNames On Config`` () =
            let bindToAttributeNames = 
                """<Binding BindToAttributeNames="MyBindToAttribute" />"""
                |> XElement.Parse
                |> ConfigReader.GetBindToAttributeNames
            bindToAttributeNames |> should contain "MyBindToAttribute"

    [<Test>]
    member x.
        ``GetReactToAttributeNames() Should Include AttributeName Specified In ReactToAttributeNames On Config`` () =
            let bindToAttributeNames = 
                """<Binding ReactToAttributeNames="MyReactToAttribute" />"""
                |> XElement.Parse
                |> ConfigReader.GetReactToAttributeNames
            bindToAttributeNames |> should contain "MyReactToAttribute"
