using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Manufacturing.SetupForms.Dtos
{
   public class MFWorkingCenterDto
    {
        public CreateOrEditMFWCMDto HeaderWorkingCenter { get; set; }

        public ICollection<CreateOrEditMFWCRESDto> ResWorkingDetail { get; set; }
        public ICollection<CreateOrEditMFWCTOLDto> ToolWorkingDetail { get; set; }
    }
}
