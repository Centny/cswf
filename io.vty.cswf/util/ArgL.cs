using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.vty.cswf.util
{
    public class ArgL_<AType, BType, CType, DType, EType, FType>
    {
        public AType A;
        public BType B;
        public CType C;
        public DType D;
        public EType E;
        public FType F;
        public ArgL_(AType a, BType b, CType c, DType d, EType e, FType f)
        {
            this.A = a;
            this.B = b;
            this.C = c;
            this.D = d;
            this.E = e;
            this.F = f;
        }

    }

    public class Arg2<AType, BType> : ArgL_<AType, BType, object, object, object, object>
    {
        public Arg2(AType a, BType b) : base(a, b, null, null, null, null)
        {

        }
    }
    public class Arg3<AType, BType, CType> : ArgL_<AType, BType, CType, object, object, object>
    {
        public Arg3(AType a, BType b, CType c) : base(a, b, c, null, null, null)
        {

        }
    }

    public class Arg4<AType, BType, CType, DType> : ArgL_<AType, BType, CType, DType, object, object>
    {
        public Arg4(AType a, BType b, CType c, DType d) : base(a, b, c, d, null, null)
        {

        }
    }

    public class Arg5<AType, BType, CType, DType, EType> : ArgL_<AType, BType, CType, DType, EType, object>
    {
        public Arg5(AType a, BType b, CType c, DType d, EType e) : base(a, b, c, d, e, null)
        {

        }
    }

    public class Arg6<AType, BType, CType, DType, EType, FType> : ArgL_<AType, BType, CType, DType, EType, FType>
    {
        public Arg6(AType a, BType b, CType c, DType d, EType e, FType f) : base(a, b, c, d, e, f)
        {

        }
    }
}
