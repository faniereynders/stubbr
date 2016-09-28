using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Stubbr
{
    public class StubResponse
    {
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
        public int Status { get; set; }
    }
}
