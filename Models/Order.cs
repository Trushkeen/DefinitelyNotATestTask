using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DefinetelyNotATestTask.Models
{
    public class Order
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public string Content { get; set; }
        public decimal Cost { get; set; }
        public int PostMachineId { get; set; }
        public string ReceiverPhone
        {
            get
            {
                return _receiverPhone;
            }
            set
            {
                if (Regex.IsMatch(value, @"[+]7\d{3}-\d{3}-\d{2}-\d{2}"))
                {
                    _receiverPhone = value;
                }
                else
                {
                    throw new FormatException("Wrong phone format provided");
                }
            }
        }
        private string _receiverPhone;
        public string ReceiverFullName { get; set; }

        
    }
}
