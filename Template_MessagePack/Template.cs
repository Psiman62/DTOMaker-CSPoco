#nullable enable
using MessagePack;

namespace T_Namespace_.MessagePack
{
    //##if false
    using T_MemberType_ = System.Int64;
    //##endif

    [MessagePackObject]
    public partial class T_EntityName_
    {
        //##if false
        private const int T_MemberSequence_ = 1;
        //##endif

        //##foreach Members
        [Key(T_MemberSequence_)] public T_MemberType_ T_MemberName_ { get; set; }
        //##endfor
    }
}