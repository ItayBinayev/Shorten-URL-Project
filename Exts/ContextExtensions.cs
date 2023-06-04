public static class ContextExtensions {
    public static string GetFullDomain(this HttpContext context) {
        var req = context.Request;
        return $"{req.Scheme}://{req.Host}";
    }
}