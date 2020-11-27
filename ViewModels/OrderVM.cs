using DefinetelyNotATestTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DefinetelyNotATestTask.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public string[] Content { get; set; }
        public decimal? Cost { get; set; }
        public int PostMachineId { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverFullName { get; set; }

        public OrderVM Assign(Order order)
        {
            Id = order.Id;
            Status = order.Status;
            Cost = order.Cost;
            Content = order.Content;
            ReceiverPhone = order.ReceiverPhone;
            ReceiverFullName = order.ReceiverFullName;
            PostMachineId = order.PostMachineId;
            return this;
        }
    }
}
