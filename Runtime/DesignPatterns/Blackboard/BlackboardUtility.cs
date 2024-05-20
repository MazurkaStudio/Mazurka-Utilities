namespace TheMazurkaStudio.Utilities
{
    public static class BlackboardUtility
    {
        public static int ComputeFNV1aHash(this string str)
        {
            uint hash = 2166136261;

            foreach (var VARIABLE in str)
            {
                hash = (hash ^ VARIABLE) * 16777619;
            }

            return unchecked((int)hash);
        }
    }
}