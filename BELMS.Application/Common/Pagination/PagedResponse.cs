using System;
using System.Collections.Generic;
using System.Text;

namespace BELMS.Application.Common.Pagination
{
    public class PagedResponse<T>
    {
        public List<T> Items { get; set; } = new();

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
