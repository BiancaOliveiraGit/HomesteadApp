using System;

namespace HomesteadAzureFunctionApp.Dto
{
    public class PushPayloadDto
    {
        public string Title { get; set; }
        public string MessageBody { get; set; }
        public string Icon { get; set; }
        public string Badge { get; set; }
        // can do more here List for actions,title,icon
    }
}