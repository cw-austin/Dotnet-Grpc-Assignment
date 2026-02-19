using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApi.Helpers
{
    public static class Pagination
    {
        public static string? GenerateNextPageUrl<T>(this PagedResponse<T> response, HttpRequest request)
        {
            const int pageSize = 20;
            if (response.Page * pageSize >= response.TotalCount) return null;

            var queryParams = QueryHelpers.ParseQuery(request.QueryString.Value);
            var nextParams = new List<string>();

            foreach (var key in queryParams.Keys)
            {
                // Skip the existing page key so we can add the new one
                if (key.Equals("page", StringComparison.OrdinalIgnoreCase)) continue;

                // Replace spaces with '+' for each value
                var value = queryParams[key].ToString().Replace(" ", "+");
                nextParams.Add($"{key}={value}");
            }

            // Add the incremented page
            nextParams.Add($"page={response.Page + 1}");

            // Combine everything
            return $"{request.Path}?{string.Join("&", nextParams)}";
        }
    }
}