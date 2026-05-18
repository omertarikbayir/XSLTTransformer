using System.Globalization;

namespace XSLTTransformer.Core
{
    /// <summary>
    /// Lightweight bilingual helper. Switches between TR/EN at runtime.
    /// </summary>
    public static class Localization
    {
        private static bool _turkish = true;

        public static void SetTurkish() => _turkish = true;
        public static void SetEnglish() => _turkish = false;

        public static string Get(string keyTr, string keyEn) => _turkish ? keyTr : keyEn;
    }
}