using FluentAssertions;
using MessagePack;
using System;
using System.Linq;

using T_Namespace_.MessagePack;

namespace Template_MessagePack.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(123L, "95-C0-C0-A3-31-32-33-C0-7B")]
        public void Test1(long value, string expectedBytes)
        {
            var orig = new T_EntityName_();
            orig.T_RequiredValMemberName_ = value;
            orig.T_RequiredRefMemberName_ = value.ToString();
            orig.T_OptionalValMemberName_ = null;
            orig.T_OptionalRefMemberName_ = null;
            ReadOnlyMemory<byte> buffer = MessagePackSerializer.Serialize<T_EntityName_>(orig);

            string.Join('-', buffer.ToArray().Select(b => b.ToString("X2"))).Should().Be(expectedBytes);

            var copy = MessagePackSerializer.Deserialize<T_EntityName_>(buffer, out int bytesRead);

            bytesRead.Should().Be(buffer.Length);
            copy.T_RequiredValMemberName_.Should().Be(orig.T_RequiredValMemberName_);
            copy.T_RequiredRefMemberName_.Should().Be(orig.T_RequiredRefMemberName_);
            copy.T_OptionalValMemberName_.Should().Be(orig.T_OptionalValMemberName_);
            copy.T_OptionalRefMemberName_.Should().Be(orig.T_OptionalRefMemberName_);
        }
    }
}