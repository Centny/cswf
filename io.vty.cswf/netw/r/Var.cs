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
        public static readonly byte[] H_MOD = Encoding.Default.GetBytes(new char[3] { '^', '~', '^' });

        /// <summary>
        /// valid the mode head.
        /// </summary>
        /// <param name="bys">byte[]</param>
        /// <param name="off">data offset</param>
        /// <returns></returns>
        public static bool valid_h(byte[] bys, int off)
        {
            for (int i = 0; i < H_MOD.Length; i++)
            {
                if (H_MOD[i] == bys[off + i])
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
