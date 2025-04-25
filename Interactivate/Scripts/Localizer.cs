namespace Nightbyte.Interactivate
{
    public static class Localizer
    {
        // Assign a delegate:  Localizer.OnLocalize = key => YourTable[key];
        public static System.Func<string,string> OnLocalize;

        public static string Localize(string key)
        {
            if (string.IsNullOrEmpty(key)) return key;
            return OnLocalize != null ? OnLocalize(key) ?? key : key;
        }
    }
}