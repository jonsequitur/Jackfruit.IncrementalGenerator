﻿using Jackfruit.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using static Jackfruit.IncrementalGenerator.RoslynHelpers;

namespace Jackfruit.IncrementalGenerator
{
    public class Helpers
    {
        private static readonly string[] names = { "AddSubCommand", "CreateWithRootCommand" };

        public const string ConsoleClass = @"
using System;

namespace Jackfruit
{
    public class ConsoleApplication
    {
        public static ConsoleApplication CreateWithRootCommand(Delegate rootCommandHandler) { }
    }

    public class CliCommand
    {
        public static CliCommand AddCommand(Delegate CommandHandler)
    }
}
";

        public static bool IsSyntaxInteresting(SyntaxNode node)
        {
            // Select1:
            //      * Extract all method invocations and filter by:
            //      * Name: comparing with expected list
            //      * Parameter count of 1
            if (node is InvocationExpressionSyntax invocation)
            {
                if (invocation.ArgumentList.Arguments.Count != 1)
                { return false; }
                var (_, name) = GetNameAndTarget(invocation.Expression);
                return name is not null && names.Contains(name);
            }
            else
            { return false; }

        }

        private static (string path, string? name) GetNameAndTarget(SyntaxNode expression)
        {
            return expression switch
            {
                MemberAccessExpressionSyntax memberAccess when expression.IsKind(SyntaxKind.SimpleMemberAccessExpression)
                    => (memberAccess.Expression.ToString(), memberAccess.Name.ToString()),
                IdentifierNameSyntax identifier
                     => ("", identifier.ToString()),
                _ => ("", null)
            };
        }

        public static CommandDef? GetCommandDef(GeneratorSyntaxContext context)
        {
            // Transform1: (using the mode)
            //      * Check the path and namespace if available
            //      * Lookup the delegate method 
            //      * Extract XML comments (as an XML blob)
            //      * Extract known attributes from method declaration and parameters
            //      * Create command and member defs
            if (context.Node is not InvocationExpressionSyntax invocation)
            {
                // Weird, but we do not want to throw
                return null;
            }
            var (nspace, path) = GetNamespaceAndPath(context, invocation.Expression);

            var delegateArg = invocation.ArgumentList.Arguments[0].Expression;
            var methodSymbol = MethodOrCandidateSymbol(context.SemanticModel, delegateArg);
            if (methodSymbol is null) { return null; }

            var details = methodSymbol.BasicDetails();
            details = DescFromXmlDocComment(methodSymbol.GetDocumentationCommentXml(), details);
            details = DescFromAttributes(methodSymbol, details);



            //var arg = invocation.ArgumentList.Arguments[0];
            if (!details.TryGetValue(CommandKey, out var commandDetail))
            {
                return null;
            }
            else
            {
                var members = new List<MemberDef>();
                foreach (var memberPair in details)
                {
                    if (memberPair.Key == CommandKey) { continue; }
                    var memberDetail = memberPair.Value;
                    members.Add(
                            memberDetail.MemberKind switch
                            {
                                MemberKind.Option => new OptionDef(
                                    memberDetail.Id,
                                    memberDetail.Description,
                                    memberDetail.TypeName,
                                    memberDetail.Aliases,
                                    memberDetail.ArgDisplayName,
                                    memberDetail.Required),
                                MemberKind.Argument => new ArgumentDef(
                                    memberDetail.Id,
                                    memberDetail.Description,
                                    memberDetail.TypeName,
                                    memberDetail.Required),
                                MemberKind.Service => new ServiceDef(
                                    memberDetail.Id,
                                    memberDetail.Description,
                                    memberDetail.TypeName),
                                _ => throw new InvalidOperationException("Unexpected member kind")
                            });

                }
                return new CommandDef(
                    delegateArg.ToString(),
                    string.Join("_", path),
                    nspace,
                    path,
                    commandDetail.Description,
                    commandDetail.Aliases,
                    members,
                    delegateArg.ToString(),
                    new CommandDef[] { },
                    commandDetail.ReturnType ?? "Unknown");
            }

            static (string nspace, IEnumerable<string> path) GetNamespaceAndPath(GeneratorSyntaxContext context, ExpressionSyntax callingExpression)
            {
                var symbol = context.SemanticModel.GetSymbolInfo(callingExpression).Symbol;
                return symbol switch
                {
                    IMethodSymbol callingMethodSymbol
                            => (callingMethodSymbol.ContainingNamespace.ToString(),
                                callingMethodSymbol.ContainingType
                                        .ToDisplayParts()
                                        .Select(x => x.ToString())),
                    _
                            => ("", GetNameAndTarget(callingExpression).path.ToString().Split('.'))
                };
            }
        }
    }
}
