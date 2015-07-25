namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// Providers interface to convert data to object.
    /// </summary>
    public interface NetwVable
    {
        /// <summary>
        /// covert to object.
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <returns>object</returns>
        T V<T>();
    }
}
