namespace TournamentTrackerWPFUI.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Indicates whether the specified array of strings contains strings that are
        /// null, ("") or consists only of whitespace characters.
        /// </summary>
        /// <param name="strings"> An array of strings to check. </param>
        /// <returns></returns>
        public static bool IsNullEmptyOrWhitespace(this string[] strings) 
        {
            foreach (var str in strings)
            {
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
