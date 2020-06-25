using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ASPCore.Models
{
    public class Refund
    {
        public string SubscriptionName { get; set; }
        public string RefundAmount { get; set; }
    }
}
