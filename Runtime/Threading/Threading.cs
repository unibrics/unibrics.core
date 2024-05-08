namespace Unibrics.Core.Threads
{
    public static class Threading
    {
        private static int mainThreadId;

        internal static void CaptureMainThreadId()
        {
            mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }
        
        public static bool IsMainThread() => System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadId;
    }
}