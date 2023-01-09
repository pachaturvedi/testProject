using System;
using System.Collections.Generic;
using System.Text;

namespace TEsting
{
    public class ShoeboxEventMetaData
    {
        public bool LiftrMetadata { get; set; } = false;

        public string ResourceId { get; set; }

        public string Category { get; set; }

        public string Type { get; set; }

        public DateTime QueueMessageIngestedByOBOTimeStamp { get; set; }

        public DateTime QueueMessageProcessingStartTimeStamp { get; set; }
    }
}
