namespace PersonalAccounting.BlazorApp.Components.Receipt_Component.Services
{
    public class UserSymbol
    {
        public static readonly string[] Styles =
        [
            "bg-primary",
            "bg-secondary",
            "bg-success",
            "bg-danger",
            "bg-warning",
            "bg-info",
            "bg-light",
            "bg-dark"
        ];

        public string UserName { get; set; }
        public string Symbol { get; set; }
        public string Style { get; set; }
    }
}
