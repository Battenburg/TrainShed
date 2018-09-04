namespace TrainShed.Core
{
    public static class RegexFactory
    {
        public static string ContainsSubStringCaseInvariant(string subString)
        {
            return string.Format("(?i:({0}))", subString);
        }

        public static string ContainsPick => @"(?i:(Pick))";

        public static string ContainsDock => @"(?i:(Dock))";
    }
}
