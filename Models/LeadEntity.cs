﻿using System.ComponentModel.DataAnnotations;

namespace AzureCustomerOPeration.Models
{
    public class LeadEntity
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }    
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

    }
}
