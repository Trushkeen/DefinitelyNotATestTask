using DefinetelyNotATestTask.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DefinetelyNotATestTask.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class PostMachinesController
    {
        //using static just for this case
        public static List<PostMachine> PostMachines { get; set; } = new List<PostMachine>();


    }
}
