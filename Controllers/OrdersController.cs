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
        public static List<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// Get all orders 
        /// </summary>
        /// <returns>All orders</returns>
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<Order> GetAllOrders()
        {
            return Orders;
        }

        /// <summary>
        /// Create order
        /// </summary>
        /// <param name="model">Order model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult CreateOrder(OrderVM model)
        {
            try
            {
                if (model.Content.Length > 10)
                {
                    throw new ArgumentException("Exceeded content's count");
                }

                if (Orders.Where((c) => c.Id == model.Id).FirstOrDefault() != null)
                {
                    throw new Exception("Same ID already exists");
                }

                var order = new Order()
                {
                    Id = model.Id,
                    Status = model.Status,
                    Cost = model.Cost.Value,
                    Content = model.Content,
                    ReceiverPhone = model.ReceiverPhone,
                    ReceiverFullName = model.ReceiverFullName,
                    PostMachineId = model.PostMachineId
                };

                var machine = PostMachinesController.PostMachines.Where((c) => c.Id == order.PostMachineId).FirstOrDefault();
                if (machine == null)
                {
                    return new NotFoundResult();
                }

                if (!machine.IsWorking)
                {
                    throw new Exception("Post machine isn't working now");
                }

                Orders.Add(order);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
            return new OkResult();
        }

        /// <summary>
        /// Edit order
        /// </summary>
        /// <param name="model">Order model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Edit")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult EditOrder(OrderVM model)
        {
            var order = Orders.Where((c) => c.Id == model.Id).FirstOrDefault();
            if (order != null)
            {
                if (model.Status != 0) order.Status = model.Status;
                if (model.Content != null) order.Content = model.Content;
                if (model.Cost != null) order.Cost = model.Cost.Value;
                if (model.ReceiverFullName != null) order.ReceiverFullName = model.ReceiverFullName;
                if (model.ReceiverPhone != null) order.ReceiverPhone = model.ReceiverPhone;
                return new OkResult();
            }
            else return new NotFoundResult();
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        /// <param name="id">Order's ID</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets order's status to "Canceled"
        /// </summary>
        /// <param name="id">Order's ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Cancel")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult CancelOrder(int id)
        {
            var order = Orders.Where((c) => c.Id == id).FirstOrDefault();
            if (order != null)
            {
                order.Status = OrderStatus.Canceled;
                return new OkResult();
            }
            return new NotFoundResult();
        }

#if DEBUG
        [HttpDelete]
        [Route("Delete")]
        public ActionResult DeleteOrder(int id)
        {
            var order = Orders.Where((c) => c.Id == id).FirstOrDefault();
            if (order != null)
            {
                Orders.Remove(order);
                return new OkResult();
            }
            return new NotFoundResult();
        }
#endif
    }
}
