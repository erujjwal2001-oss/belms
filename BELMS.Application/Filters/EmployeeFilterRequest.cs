using BELMS.Application.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace BELMS.Application.Filters
{
    public class EmployeeFilterRequest : PaginationRequest
    {
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
    }
}
