﻿using System;
using Abp;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Dto
{
    public class ImportICSegment2FromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
