
namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// Providers command listner on runner.
    /// </summary>
    public interface CmdListener
    {
        /// <summary>
        /// calling when received on command.
        /// </summary>
        /// <param name="nr">runner</param>
        /// <param name="m">message</param>
        void onCmd(NetwRunnable nr, Bys m);
    }
}
