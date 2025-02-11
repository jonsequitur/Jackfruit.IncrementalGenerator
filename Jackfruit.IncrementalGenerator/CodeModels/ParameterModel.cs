﻿namespace Jackfruit.IncrementalGenerator.CodeModels
{
    public class ParameterModel
    {
        public ParameterModel(string name, NamedItemModel type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public NamedItemModel Type { get; }
        public ParameterStyle Style { get; set; }
        public bool IsParamArray { get; set; }
        public ExpressionBase? DefaultValue { get; set; }
        public string XmlDescription { get; set; }
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