﻿using Microsoft.CodeAnalysis;

namespace DTOMaker.Generator
{
    internal sealed class SyntaxDiagnostic
    {
        public readonly Location Location;
        public readonly DiagnosticSeverity Severity;
        public readonly string Message;
        public SyntaxDiagnostic(Location location, DiagnosticSeverity severity, string message)
        {
            Location = location;
            Message = message;
            Severity = severity;
        }
    }
}