using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace AutoInsurance.Models
{
    public class uploadDoc
    {
        public int id { get; set; }
        public IFormFile document { get; set; }
    }
    public class ClaimRegistration
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }   
        public string UpdatedBy { get; set; }       
        public string Documents { get; set; }   
        public string DocumentName { get; set; }
    }

    public class Document
    {
        public int Id { get; set; }
        public IFormFile Documents { get; set; }        
    }
}
