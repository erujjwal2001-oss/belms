using System;
using System.Collections.Generic;
using System.Text;

namespace BELMS.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }

    }
}
