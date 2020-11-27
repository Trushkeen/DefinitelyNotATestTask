using DefinetelyNotATestTask.Models;
using DefinetelyNotATestTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DefinetelyNotATestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController
    {
        //using static just for this case
        public static List<Order> Orders = new List<Order>();

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<Order> GetAllOrders()
        {
            return Orders;
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public StatusCodeResult CreateOrder(OrderVM orderVM)
        {
            try
            {
                if (orderVM.Content.Length > 10)
                {
                    throw new ArgumentException("Exceeded content's count");
                }
                Orders.Add(new Order()
                {
                    Id = orderVM.Id,
                    Status = orderVM.Status,
                    Cost = orderVM.Cost.Value,
                    Content = orderVM.Content,
                    ReceiverPhone = orderVM.ReceiverPhone,
                    ReceiverFullName = orderVM.ReceiverFullName,
                    PostMachineId = orderVM.PostMachineId
                });
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
            return new OkResult();
        }

        [HttpPost]
        [Route("Edit")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public StatusCodeResult EditOrder(OrderVM editOrderVM)
        {
            var order = Orders.Where((c) => c.Id == editOrderVM.Id).FirstOrDefault();
            if (order != null)
            {
                if (editOrderVM.Status != 0) order.Status = editOrderVM.Status;
                if (editOrderVM.Content != null) order.Content = editOrderVM.Content;
                if (editOrderVM.Cost != null) order.Cost = editOrderVM.Cost.Value;
                if (editOrderVM.ReceiverFullName != null) order.ReceiverFullName = editOrderVM.ReceiverFullName;
                if (editOrderVM.ReceiverPhone != null) order.ReceiverPhone = editOrderVM.ReceiverPhone;
                return new OkResult();
            }
            else return new NotFoundResult();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Order> GetOrder(int id)
        {
            var order = Orders.Where((c) => c.Id == id).FirstOrDefault();
            if (order == null)
            {
                return new NotFoundResult();
            }
            return order;
        }

        [HttpPost]
        [Route("Delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public StatusCodeResult CancelOrder(int id)
        {
            var order = Orders.Where((c) => c.Id == id).FirstOrDefault();
            if (order != null)
            {
                order.Status = OrderStatus.Canceled;
                return new OkResult();
            }
            return new NotFoundResult();
        }
    }
}
