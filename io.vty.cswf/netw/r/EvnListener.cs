using System;

namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// Providers event listener on runner.
    /// </summary>
    public interface EvnListener
    {
        /// <summary>
        /// calling begin connect.
        /// </summary>
        /// <param name="nr">runner.</param>
        void begCon(NetwRunnable nr);
        /// <summary>
        /// calling on connected.
        /// </summary>
        /// <param name="nr">runner</param>
        /// <param name="w">stream</param>
        void onCon(NetwRunnable nr, Netw w);

        /// <summary>
        /// calling on error occur.
        /// </summary>
        /// <param name="nr">runner</param>
        /// <param name="e">error</param>
        void onErr(NetwRunnable nr, Exception e);

        /// <summary>
        /// calling on runner stop.
        /// </summary>
        /// <param name="nr"></param>
        void endCon(NetwRunnable nr);
    }
}
