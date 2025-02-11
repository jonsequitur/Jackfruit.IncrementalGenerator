﻿namespace Jackfruit.IncrementalGenerator.CodeModels
{
    public class ConstructorModel : IMember, IHasScope, IHasParameters,IHasStatements
    {
        public string ClassName { get; }

        public ConstructorModel(string className)
        {
            ClassName = className;
        }

        public BaseOrThis BaseOrThis { get; set; }
        public List<ExpressionBase> BaseOrThisArguments { get; set; } = new List<ExpressionBase> { };
        public Scope Scope { get; set; }
        public bool IsStatic { get; set; }
        public List<ParameterModel> Parameters { get; set; } = new List<ParameterModel> { };
        public List<IStatement> Statements { get; set; } = new List<IStatement> { };
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