#nullable enable
using MessagePack;

namespace T_Namespace_.MessagePack
{
    //##if false
    using T_MemberValType_ = System.Int64;
    using T_MemberRefType_ = System.String;
    //##endif

    [MessagePackObject]
    public partial class T_EntityName_
    {
        //##if false
        private static readonly T_MemberRefType_ T_DefaultRefMemberValue_ = string.Empty;
        private static readonly T_MemberValType_ T_DefaultValMemberValue_ = default;
        private const int T_OptionalRefMemberSequence_ = 1;
        private const int T_RequiredRefMemberSequence_ = 2;
        private const int T_OptionalValMemberSequence_ = 3;
        private const int T_RequiredValMemberSequence_ = 4;
        //##endif

        //##foreach Members
        //##if IsNullable
        //##if IsRefType
        [Key(T_OptionalRefMemberSequence_)] public T_MemberRefType_? T_OptionalRefMemberName_ { get; set; }
        //##else
        [Key(T_OptionalValMemberSequence_)] public T_MemberValType_? T_OptionalValMemberName_ { get; set; }
        //##endif
        //##else
        //##if IsRefType
        [Key(T_RequiredRefMemberSequence_)] public T_MemberRefType_ T_RequiredRefMemberName_ { get; set; } = T_DefaultRefMemberValue_;
        //##else
        [Key(T_RequiredValMemberSequence_)] public T_MemberValType_ T_RequiredValMemberName_ { get; set; } = T_DefaultValMemberValue_;
        //##endif
        //##endif
        //##endfor
    }
}
