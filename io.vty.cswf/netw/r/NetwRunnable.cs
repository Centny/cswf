namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// Providers runnable to run read on stream.
    /// </summary>
    public interface NetwRunnable
    {
        /// <summary>
        /// property for get the Netw.
        /// </summary>
        Netw netw { get; }

        /// <summary>
        /// run read on stream.
        /// </summary>
        void runc(Netw nw);
    }
}
