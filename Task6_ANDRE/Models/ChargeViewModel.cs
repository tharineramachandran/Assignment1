using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task6_ANDRE.Models
{
    public class ChargeViewModel
    {
        public string customerName { get; set; }
        public string PriceId { get; set; }
    }
}