namespace RecordMyStats.Utils;

internal class GlobalUISettings
{
    private static List<Window> windowList = new List<Window>();
    public static void AddToWindowsList(Window window)
    {
        windowList.Add(window);
    }
    public static void CloseWindows()
    {
        foreach (var window in windowList)
        {
            try
            {
                if (window != null)
                {
                    window.Close();
                }
            }
            catch { }
        }
        windowList.Clear();
    }
}
