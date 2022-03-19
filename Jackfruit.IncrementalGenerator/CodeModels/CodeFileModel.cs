﻿namespace Jackfruit.IncrementalGenerator.CodeModels
{
    public class CodeFileModel
    {
        public CodeFileModel(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public NamespaceModel? Namespace { get; set; }
        public List<UsingModel> Usings { get; set; } = new List<UsingModel>();
        public List<string> HeaderComments { get; set; } = new List<string>() { "This file is created by a generator." };

    }
}
/* Hmmm. Will we need these?

type ICompareExpression = 
inherit IExpression


type ReturnType =
| ReturnTypeVoid
| ReturnTypeUnknown
| ReturnType of t: NamedItem
//interface IStatement
static member Create typeName =
    match typeName with 
        | "void" -> ReturnTypeVoid
        | _ -> ReturnType(NamedItem.Create typeName)
static member op_Implicit(typeName: string) : ReturnType = 
    ReturnType.Create typeName

type InheritedFrom =
| SomeBase of BaseClass: NamedItem
| NoBase
//interface IMember

//type ImplementedInterface =
//    | ImplementedInterface of Name: NamedItem
//    //interface IMember
}
*/