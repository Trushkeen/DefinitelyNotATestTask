using DefinetelyNotATestTask.Models;
using DefinetelyNotATestTask.Utils;
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
        private OrderDbContext db;

        public OrdersController(OrderDbContext context)
        {
            db = context;
        }

        /// <summary>
        /// Get all orders 
        /// </summary>
        /// <returns>All orders</returns>
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<Order> GetAllOrders()
        {
            return db.Orders;
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
        public async Task<ActionResult> CreateOrder(OrderVM model)
        {
            try
            {
                if (model.Content.Length > 255)
                {
                    throw new ArgumentException("Exceeded content's count");
                }

                if (db.Orders.Where((c) => c.Id == model.Id).FirstOrDefault() != null)
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

                var machine = db.PostMachines.Where((c) => c.Id == order.PostMachineId).FirstOrDefault();
                if (machine == null)
                {
                    return new NotFoundResult();
                }

                if (!machine.IsWorking)
                {
                    throw new Exception("Post machine isn't working now");
                }

                db.Orders.Add(order);

                await db.SaveChangesAsync();
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
        public async Task<ActionResult> EditOrder(OrderVM model)
        {
            var order = db.Orders.Where((c) => c.Id == model.Id).FirstOrDefault();
            if (order != null)
            {
                if (model.Status != 0) order.Status = model.Status;
                if (model.Content != null) order.Content = model.Content;
                if (model.Cost != null) order.Cost = model.Cost.Value;
                if (model.ReceiverFullName != null) order.ReceiverFullName = model.ReceiverFullName;
                if (model.ReceiverPhone != null) order.ReceiverPhone = model.ReceiverPhone;
                await db.SaveChangesAsync();
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
            var order = db.Orders.Where((c) => c.Id == id).FirstOrDefault();
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
        public async Task<ActionResult> CancelOrder(int id)
        {
            var order = db.Orders.Where((c) => c.Id == id).FirstOrDefault();
            if (order != null)
            {
                order.Status = OrderStatus.Canceled;
                await db.SaveChangesAsync();
                return new OkResult();
            }
            return new NotFoundResult();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var order = db.Orders.Where((c) => c.Id == id).FirstOrDefault();
            if (order != null)
            {
                db.Orders.Remove(order);
                await db.SaveChangesAsync();
                return new OkResult();
            }
            return new NotFoundResult();
        }
    }
}
