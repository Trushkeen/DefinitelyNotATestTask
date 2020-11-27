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
    public class PostMachinesController
    {
        //using static just for this case
        public static List<PostMachine> PostMachines { get; set; } = new List<PostMachine>();

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<PostMachine> GetAllPostMachines()
        {
            return PostMachines;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<PostMachine> GetPostMachine(int id)
        {
            var machine = PostMachines.Where((c) => c.Id == id).FirstOrDefault();
            if (machine == null)
            {
                return new NotFoundResult();
            }
            return machine;
        }

#if DEBUG
        [HttpPost]
        [Route("Create")]
        public StatusCodeResult CreatePostMachine(PostMachineVM model)
        {
            try
            {
                PostMachines.Add(new PostMachine()
                {
                    Id = model.Id.Value,
                    Address = model.Address,
                    IsWorking = model.IsWorking.Value
                });
                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        [HttpDelete]
        [Route("Remove/{id}")]
        public StatusCodeResult DeletePostMachine(int id)
        {
            try
            {
                var machine = PostMachines.Where((c) => c.Id == id).FirstOrDefault();
                PostMachines.Remove(machine);
            }
            catch
            {
                return new NotFoundResult();
            }
            return new OkResult();
        }
#endif

    }
}
