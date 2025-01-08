namespace PersonalAccounting
{
    public static class GlobalConfigs
    {
#if DEBUG
        public const string ProdSuffix = "";
#else
        public const string ProdSuffix = "prod";
#endif
    }
}
