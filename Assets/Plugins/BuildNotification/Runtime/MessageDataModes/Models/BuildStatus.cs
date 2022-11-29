namespace Better.BuildNotification.Runtime.MessageDataModes
{
    public enum BuildStatus : byte
    {
        /// <summary>
        ///     <para>Indicates that the outcome of the build is in an unknown state.</para>
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     <para>Indicates that the build completed successfully.</para>
        /// </summary>
        Succeeded = 1,

        /// <summary>
        ///     <para>Indicates that the build failed.</para>
        /// </summary>
        Failed = 2,

        /// <summary>
        ///     <para>Indicates that the build was cancelled by the user.</para>
        /// </summary>
        Cancelled = 3
    }
}
