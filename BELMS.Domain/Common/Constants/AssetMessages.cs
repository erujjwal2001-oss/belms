using System;
using System.Collections.Generic;
using System.Text;

namespace BELMS.Domain.Common.Constants
{
    public static class AssetMessages
    {
        public const string NotFound = "Asset was not found.";
        public const string SerialNumberExists = "Asset serial number already exists.";
        public const string AlreadyAssigned = "Asset is already assigned.";
        public const string NotAvailable = "Asset is not available.";
        public const string CreationFailed = "Failed to create asset.";
        public const string UpdateFailed = "Failed to update asset.";
        public const string DeletionFailed = "Failed to delete asset.";
    }
}
