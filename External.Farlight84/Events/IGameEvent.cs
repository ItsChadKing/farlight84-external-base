namespace External.Farlight84.Events
{
    internal interface IGameEvent
    {
        /// <summary>
        /// Name of the application
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Determines if the application is opened
        /// </summary>
        event EventHandler<int> OnGameActive;

        /// <summary>
        /// Determines if the application is closed
        /// </summary>
        event EventHandler<int> OnGameInactive;

        /// <summary>
        /// Determines if the application is running
        /// </summary>
        event EventHandler<int> OnGameIsRunning;

        /// <summary>
        /// Subscribes the program to all applicable events
        /// </summary>
        /// <returns></returns>
        Task Subscribe();
    }
}
