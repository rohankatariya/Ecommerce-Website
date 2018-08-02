using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ZensarProject.DAL
{
    public class CustomerData
    {
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Confirm_Password { get; set; }
        public string Gender { get; set; }
        public string Select_State { get; set; }
        public static class CustomHelper
        {
            public class CustomerDataCotext : DbContext
            {
                public DbSet<CustomerData> Customer { get; set; }
            }
        }

    }
}