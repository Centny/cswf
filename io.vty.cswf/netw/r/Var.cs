using System.Text;

namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// the common variable.
    /// </summary>
    public class Var
    {
        /// <summary>
        /// the mode head.
        /// </summary>
        public static readonly byte[] H_MOD = Encoding.Default.GetBytes(new char[3] { '^', '-', '^' });
    }
}
