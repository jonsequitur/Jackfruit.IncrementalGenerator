﻿using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Jackfruit.Models;
using Jackfruit.IncrementalGenerator;
using Newtonsoft.Json;
using Jackfruit.IncrementalGenerator.Output;
using System.Management;

namespace Jackfruit.Tests
{
    [Generator]
    public class CommandDefGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            initContext.RegisterPostInitializationOutput(ctx => ctx.AddSource(
                "ConsoleApplication.g.cs",
                SourceText.From(Helpers.ConsoleClass, Encoding.UTF8)));

            IncrementalValuesProvider<CommandDef> commandDefs = initContext.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => Helpers.IsSyntaxInteresting(s),
                    transform: static (ctx, _) => Helpers.GetCommandDef(ctx))
                .Where(static m => m is not null)!;

            initContext.RegisterSourceOutput(commandDefs,
                static (context, commandDef) => Generate(commandDef, context));
        }

        private static void Generate(CommandDef commandDef, SourceProductionContext context)
        {
            var joinedPath = string.Join("", commandDef.Path);

            var writer = new StringBuilderWriter(3);
            writer.AddLine("/*");
            OutputCommand(writer, commandDef);
            writer.AddLine("*/");

            context.AddSource($"{joinedPath}.g.cs", writer.Output());

            static void OutputCommand(IWriter writer, CommandDef commandDef)
            {
            var joinedPath = string.Join("", commandDef.Path);
                writer.AddLine($"Key:         {joinedPath}");
                writer.AddLine($"Id:          {commandDef.Id}");
                var path = string.Join(".", commandDef.Path);
                writer.AddLine($"Path:        {path}");
                writer.AddLine($"Description: {commandDef.Description}");
                writer.AddLine($"Namespace:   {commandDef.Namespace}");
                writer.AddLine($"Members:     ");
                writer.IncreaseIndent();
                foreach (var member in commandDef.Members)
                {
                    switch (member)
                    {
                        case OptionDef option: OutputOption(writer, option); break;
                        case ArgumentDef arg: OutputArgument(writer, arg); break;
                        case ServiceDef service: OutputService(writer, service); break;
                    }
                }
                foreach (var subCommandDef in commandDef.SubCommands)
                {
                    if (subCommandDef is CommandDef subCommand)
                    {
                        writer.IncreaseIndent();
                        OutputCommand(writer, subCommand);
                        writer.DecreaseIndent();
                    }
                }
                writer.DecreaseIndent();
            }
            static void OutputOption(IWriter writer, OptionDef option)
            {
                writer.AddLine("Option");
                writer.IncreaseIndent();
                writer.AddLine($"Option Id:      {option.Id}");
                writer.AddLine($"Name:           {option.Name}");
                writer.AddLine($"TypeName:       {option.TypeName}");
                writer.AddLine($"Description:    {option.Description}");
                writer.AddLine($"Aliases:        {string.Join(", ", option.Aliases)}");
                writer.AddLine($"ArgDisplayName: {option.ArgDisplayName}");
                writer.AddLine($"Required:       {option.Required}");
                writer.DecreaseIndent();
            }

            static void OutputArgument(IWriter writer, ArgumentDef option)
            {
                writer.AddLine("Option");
                writer.IncreaseIndent();
                writer.AddLine($"Argumet Id:     {option.Id}");
                writer.AddLine($"Name:           {option.Name}");
                writer.AddLine($"TypeName:       {option.TypeName}");
                writer.AddLine($"Description:    {option.Description}");
                writer.AddLine($"Required:       {option.Required}");
                writer.DecreaseIndent();
            }

            static void OutputService(IWriter writer, ServiceDef option)
            {
                writer.AddLine("Option");
                writer.IncreaseIndent();
                writer.AddLine($"Service Id:     {option.Id}");
                writer.AddLine($"Name:           {option.Name}");
                writer.AddLine($"TypeName:       {option.TypeName}");
                writer.AddLine($"Description:    {option.Description}");
                writer.DecreaseIndent();
            }
        }

    }
}
