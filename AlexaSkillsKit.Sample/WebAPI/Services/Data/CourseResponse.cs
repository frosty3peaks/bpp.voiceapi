using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.WebAPI.Services.Data
{
    public class CourseResponse
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Paper { get; set; }
        public string Location { get; set; }
        public string Room { get; set; }
        public string Tutor { get; set; }
        public string Type { get; set; }
        public DateTime StartTime{ get; set; }
        public DateTime EndTime { get; set; }

    }
}