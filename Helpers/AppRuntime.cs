namespace CNPMFastFood.Helpers
{
    public static class AppRuntime
    {
        public static string AppStartId { get; } =
            Guid.NewGuid().ToString();
    }
}