namespace BELMS.Frontend.Models
{
    public class ApiResponse
    {
        private string? _message;
        public bool IsSuccess { get; set; }

        public string? Message
        {
            get
            {
                if (!string.IsNullOrEmpty(_message)) return _message;

                // Format validation errors if present
                if (Errors is System.Text.Json.JsonElement element && element.ValueKind == System.Text.Json.JsonValueKind.Object)
                {
                    var list = new System.Collections.Generic.List<string>();
                    foreach (var prop in element.EnumerateObject())
                    {
                        if (prop.Value.ValueKind == System.Text.Json.JsonValueKind.Array)
                        {
                            foreach (var item in prop.Value.EnumerateArray())
                            {
                                list.Add($"{prop.Name}: {item.GetString()}");
                            }
                        }
                        else if (prop.Value.ValueKind == System.Text.Json.JsonValueKind.String)
                        {
                            list.Add($"{prop.Name}: {prop.Value.GetString()}");
                        }
                    }
                    if (list.Count > 0)
                    {
                        return string.Join("; ", list);
                    }
                }

                if (!string.IsNullOrEmpty(Detail)) return Detail;
                return Title;
            }
            set => _message = value;
        }

        public string? Detail { get; set; }
        public string? Title { get; set; }
        public string? ErrorCode { get; set; }
        public object? Errors { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }
    }

    public class AuthResponse
    {
       
            public string AccessToken { get; set; } = "";

            public string RefreshToken { get; set; } = "";

            public DateTime AccessTokenExpiry { get; set; }

            public DateTime RefreshTokenExpiry { get; set; }
        
    }
}
