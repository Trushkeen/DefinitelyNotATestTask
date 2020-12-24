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
    public class PostMachinesController
    {
        private OrderDbContext db;

        public PostMachinesController(OrderDbContext options)
        {
            db = options;
        }

        /// <summary>
        /// Gets all post machines
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<PostMachine> GetAllPostMachines()
        {
            return db.PostMachines;
        }

        /// <summary>
        /// Get post machine by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<PostMachine> GetPostMachine(int id)
        {
            var machine = db.PostMachines.Where((c) => c.Id == id).FirstOrDefault();
            if (machine == null)
            {
                return new NotFoundResult();
            }
            return machine;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> CreatePostMachine(PostMachineVM model)
        {
            try
            {
                if (db.PostMachines.Where((c) => c.Id == model.Id).FirstOrDefault() != null)
                {
                    throw new Exception("Same ID already exists");
                }

                db.PostMachines.Add(new PostMachine()
                {
                    Id = model.Id.Value,
                    Address = model.Address,
                    IsWorking = model.IsWorking.Value
                });

                await db.SaveChangesAsync();

                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        [HttpDelete]
        [Route("Remove/{id}")]
        public async Task<ActionResult> DeletePostMachine(int id)
        {
            try
            {
                var machine = db.PostMachines.Where((c) => c.Id == id).FirstOrDefault();
                db.PostMachines.Remove(machine);

                await db.SaveChangesAsync();
            }
            catch
            {
                return new NotFoundResult();
            }
            return new OkResult();
        }
    }
}
