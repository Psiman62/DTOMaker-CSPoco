﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DTOMaker.Generator
{
    [Generator(LanguageNames.CSharp)]
    public class SourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        private void EmitDiagnostics(GeneratorExecutionContext context, TargetBase target)
        {
            // todo fix msg ids
            foreach (var message in target.SyntaxErrors)
            {
                // report diagnostic
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "MFNSSG001", "DiagnosticTitle",
                            message.Message,
                            "DiagnosticCategory",
                            message.Severity,
                            true),
                    message.Location));
            }
            foreach (var message in target.ValidationErrors())
            {
                // report diagnostic
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "MFNSSG001", "DiagnosticTitle",
                            message.Message,
                            "DiagnosticCategory",
                            message.Severity,
                            true),
                    message.Location));
            }
        }
        private void CheckReferencedAssemblyNamesInclude(GeneratorExecutionContext context, Assembly assembly)
        {
            string packageName = assembly.GetName().Name;
            Version packageVersion = assembly.GetName().Version;
            if (!context.Compilation.ReferencedAssemblyNames.Any(ai => ai.Name.Equals(packageName, StringComparison.OrdinalIgnoreCase)))
            {
                // todo major version error/minor version warning
                context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "MFNSSG001", "DiagnosticTitle",
                            $"The generated code requires a reference to {packageName} (v{packageVersion} or later).",
                            "DiagnosticCategory",
                            DiagnosticSeverity.Warning,
                            true),
                            Location.None));
            }
        }
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is not SyntaxReceiver syntaxReceiver) return;

            // check that the users compilation references the expected libraries
            CheckReferencedAssemblyNamesInclude(context, typeof(DTOMaker.Models.DomainAttribute).Assembly);

            foreach (var domain in syntaxReceiver.Domains.Values)
            {
                EmitDiagnostics(context, domain);
                foreach (var entity in domain.Entities.Values.OrderBy(e => e.Name))
                {
                    EmitDiagnostics(context, entity);
                    if (entity.CanEmit())
                    {
                        Version fv = new Version(ThisAssembly.AssemblyFileVersion);
                        string shortVersion = $"{fv.Major}.{fv.Minor}";
                        string hintName = $"{domain.Name}.{entity.Name}.g.cs";
                        var builder = new StringBuilder();
                        string entityHead =
                            $$"""
                            // <auto-generated>
                            // This file was generated by {{typeof(SourceGenerator).Namespace}} V{{shortVersion}}
                            // Warning: Changes made to this file will be lost if re-generated.
                            // </auto-generated>
                            #pragma warning disable CS0414
                            #nullable enable
                            using System;
                            namespace {{domain.Name}}
                            {
                                public partial class {{entity.Name}}
                                {
                                    private const int BlockSize = {{entity.BlockSize}};
                                    private readonly Memory<byte> _block;
                                    public ReadOnlyMemory<byte> Block => _block;
                                    public {{entity.Name}}() => _block = new byte[BlockSize];
                                    public {{entity.Name}}(ReadOnlySpan<byte> source) => _block = source.Slice(0, BlockSize).ToArray();
                            """;
                        string entityTail =
                            """
                                }
                            }
                            """;
                        builder.AppendLine(entityHead);
                        // begin member map
                        string memberMapHead =
                            """
                                    // <field-map>
                                    //  Pos.  Len.  Type        Endian  Name
                                    //  ----  ----  --------    ------  --------
                            """;
                        builder.AppendLine(memberMapHead);
                        foreach (var member in entity.Members.Values.OrderBy(m => m.FieldOffset))
                        {
                            string memberMapBody =
                                $$"""
                                        //  {{member.FieldOffset,4:N0}}  {{member.FieldLength,4:N0}}  {{member.MemberType,-8}}    {{(member.IsBigEndian ? "Big   " : "Little")}}  {{member.Name}}
                                """;
                            builder.AppendLine(memberMapBody);
                        }
                        string memberMapTail =
                            """
                                    // </field-map>
                            """;
                        builder.AppendLine(memberMapTail);
                        // end member map
                        foreach (var member in entity.Members.Values.OrderBy(m => m.FieldOffset))
                        {
                            EmitDiagnostics(context, member);
                            if (member.CanEmit())
                            {
                                string memberSource =
                                    $$"""
                                            public {{member.MemberType}} {{member.Name}}
                                            {
                                                get => {{member.CodecTypeName}}.Instance.ReadFrom(_block.Slice({{member.FieldOffset}}, {{member.FieldLength}}).Span);
                                                set => {{member.CodecTypeName}}.Instance.WriteTo(_block.Slice({{member.FieldOffset}}, {{member.FieldLength}}).Span, value);
                                            }
                                    """;
                                builder.AppendLine(memberSource);
                            }
                        }
                        builder.AppendLine(entityTail);
                        context.AddSource(hintName, builder.ToString());
                    }
                }
            }
        }
    }
}