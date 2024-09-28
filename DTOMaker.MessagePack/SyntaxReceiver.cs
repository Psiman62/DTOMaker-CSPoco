using DTOMaker.Gentime;
using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DTOMaker.MessagePack
{
    internal sealed class MessagePackDomain : TargetDomain
    {
        public MessagePackDomain(string name, Location location) : base(name, location) { }

        // todo remove this
        protected override IEnumerable<SyntaxDiagnostic> OnGetValidationDiagnostics() { yield break; }
    }
    internal sealed class MessagePackEntity : TargetEntity
    {
        public MessagePackEntity(string name, Location location) : base(name, location) { }
    }
    internal sealed class MessagePackMember : TargetMember
    {
        public MessagePackMember(string name, Location location) : base(name, location) { }
    }
    internal class SyntaxReceiver : ISyntaxContextReceiver
    {
        public ConcurrentDictionary<string, TargetDomain> Domains { get; } = new ConcurrentDictionary<string, TargetDomain>();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            SyntaxReceiverHelper.ProcessNode(context, Domains,
                (n, l) => new MessagePackDomain(n, l),
                (n, l) => new MessagePackEntity(n, l),
                (n, l) => new MessagePackMember(n, l));
        }
    }
}
