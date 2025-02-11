﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jackfruit.IncrementalGenerator.CodeModels
{
    public static class StructureHelpers
    {
        public static NamedItemModel Void()
            => new VoidNamedItemModel();

        public static GenericNamedItemModel Generic(string name, params NamedItemModel[] genericTypes)
            => new(name, genericTypes);

        public static T Public<T>(this T model)
            where T : IHasScope
        {
            model.Scope = Scope.Public;
            return model;
        }
        public static T Internal<T>(this T model)
            where T : IHasScope
        {
            model.Scope = Scope.Internal;
            return model;
        }
        public static T Protected<T>(this T model)
            where T : IHasScope
        {
            model.Scope = Scope.Protected;
            return model;
        }
        public static T Private<T>(this T model)
            where T : IHasScope
        {
            model.Scope = Scope.Private;
            return model;
        }
        public static T ProtectedInternal<T>(this T model)
            where T : IHasScope
        {
            model.Scope = Scope.ProtectedInternal;
            return model;
        }
        public static T PrivateProtected<T>(this T model)
            where T : IHasScope
        {
            model.Scope = Scope.PrivateProtected;
            return model;
        }

        public static T Parameters<T>(this T model, params ParameterModel[] parameters)
            where T : IHasParameters
        {
            model.Parameters = parameters.ToList();
            return model;
        }

        public static ParameterModel Parameter(string name, NamedItemModel type)
            => new(name, type);

        public static ParameterModel XmlDescription(this ParameterModel model, string description)
        {
            model.XmlDescription = description;
            return model;
        }

        public static T Statements<T>(this T model, params IStatement[] statements)
            where T : IHasStatements
        {
            model.Statements.AddRange(statements);
            return model;
        }

        public static T Statements<T>(this T model, IEnumerable<IStatement> statements)
            where T : IHasStatements
        {
            model.Statements.AddRange(statements);
            return model;
        }


        public static ConstructorModel Constructor(string className)
            => new(className);

        public static ConstructorModel Static(this ConstructorModel model)
        {
            model.IsStatic = true;
            return model;
        }

        public static ConstructorModel Base(this ConstructorModel model, params ExpressionBase[] args)
        {
            model.BaseOrThis = BaseOrThis.Base;
            model.BaseOrThisArguments = args.ToList();
            return model;
        }

        public static ConstructorModel This(this ConstructorModel model, params ExpressionBase[] args)
        {
            model.BaseOrThis = BaseOrThis.This;
            model.BaseOrThisArguments = args.ToList();
            return model;
        }

        public static MethodModel Method(string name, NamedItemModel type)
             => new(name, type);

        public static MethodModel Override(this MethodModel model)
        {
            model.IsOverride = true;
            return model;
        }

        public static MethodModel Static(this MethodModel model)
        {
            model.IsStatic = true;
            return model;
        }

        public static MethodModel Async(this MethodModel model)
        {
            model.IsAsync = true;
            return model;
        }

        public static MethodModel Partial(this MethodModel model)
        {
            model.IsPartial = true;
            return model;
        }

        public static PropertyModel Property(string name, NamedItemModel type)
            => new(name, type);

        public static PropertyModel Static(this PropertyModel model)
        {
            model.IsStatic = true;
            return model;
        }

        public static PropertyModel Get(this PropertyModel model, params IStatement[] statements)
        {
            model.Getter = new PropertyAccessorModel
            {
                Statements = statements.ToList()
            };
            return model;
        }

        public static PropertyModel Set(this PropertyModel model, params IStatement[] statements)
        {
            model.Setter = new PropertyAccessorModel
            {
                Statements = statements.ToList()
            };
            return model;
        }

        public static ClassModel Class(NamedItemModel name)
            => new(name);

        public static T XmlDescription<T>(this T model, string description)
            where T : IMember
        {
            model.XmlDescription = description;
            return model;
        }

        public static ClassModel Static(this ClassModel model)
        {
            model.IsStatic = true;
            return model;
        }
        public static ClassModel Async(this ClassModel model)
        {
            model.IsAsync = true;
            return model;
        }

        public static ClassModel Partial(this ClassModel model)
        {
            model.IsPartial = true;
            return model;
        }

        public static ClassModel Abstract(this ClassModel model)
        {
            model.IsAbstract = true;
            return model;
        }

        public static ClassModel Sealed(this ClassModel model)
        {
            model.IsSealed = true;
            return model;
        }

        public static ClassModel InheritedFrom(this ClassModel model, NamedItemModel baseClassName)
        {
            model.InheritedFrom = baseClassName;
            return model;
        }

        public static ClassModel ImplementedInterfaces(this ClassModel model, params NamedItemModel[] interfaces)
        {
            model.ImplementedInterfaces.AddRange(interfaces.ToList());
            return model;
        }

        public static ClassModel Members(this ClassModel model, params IMember[] members)
        {
            model.Members.AddRange(members);
            return model;
        }

        public static ClassModel Members(this ClassModel model, IEnumerable<IMember> members)
        {
            model.Members.AddRange(members);
            return model;
        }

        public static FieldModel Field(string name, NamedItemModel type)
            => new(name, type);

        public static FieldModel Static(this FieldModel field)
        {
            field.IsStatic = true;
            return field;
        }
        public static FieldModel ReadOnly(this FieldModel field)
        {
            field.IsReadonly = true;
            return field;
        }

        public static CodeFileModel Usings(this CodeFileModel model, params UsingModel[] usings)
        {
            model.Usings.AddRange(usings);
            return model;
        }

        public static CodeFileModel Namespace(this CodeFileModel model, string nspace, params ClassModel[] classes)
        {
            model.Namespace = new NamespaceModel(nspace);
            model.Namespace.Classes.AddRange(classes);
            return model;
        }

        public static CodeFileModel CodeFile(string name)
            => new(name);

    }
}
