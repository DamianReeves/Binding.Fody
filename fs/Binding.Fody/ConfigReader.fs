module internal ModuleWeaver.Internal
open System
open System
open System.Linq
open System.Xml.Linq
open Mono.Cecil
open Mono.Cecil.Rocks
open Mono.Cecil.Cil

module ConfigReader =
    let GetBindToAttributeNames(config:XElement) =
        let xn s = XName.Get(s)
        let reactToAttributeNames = ["Binding.BindTo";]
        if config = null then
            reactToAttributeNames
        else
            let reactToAttribute = config.Attributes(xn "BindToAttributeNames").FirstOrDefault()
            if reactToAttribute = null then
                reactToAttributeNames
            else
                reactToAttribute.Value.Split([|','|], StringSplitOptions.RemoveEmptyEntries) 
                |> List.ofArray 
                |> List.append reactToAttributeNames

    let GetReactToAttributeNames(config:XElement) =
        let xn s = XName.Get(s)
        let reactToAttributeNames = ["Binding.ReactTo";]
        if config = null then
            reactToAttributeNames
        else
            let reactToAttribute = config.Attributes(xn "ReactToAttributeNames").FirstOrDefault()
            if reactToAttribute = null then
                reactToAttributeNames
            else
                reactToAttribute.Value.Split([|','|], StringSplitOptions.RemoveEmptyEntries) 
                |> List.ofArray 
                |> List.append reactToAttributeNames

