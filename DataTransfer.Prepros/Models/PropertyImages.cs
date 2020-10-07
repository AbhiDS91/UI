using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransfer.Prepros.Models
{
    public class PropertyImages
    {
        public string PropertyCode { get; set; }
        public List<Properties> Images { get; set; }
    }
}