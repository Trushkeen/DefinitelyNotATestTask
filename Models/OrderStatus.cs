using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DefinetelyNotATestTask.Models
{
    public enum OrderStatus
    {
        Registered = 1,
        AtWarehouse = 2,
        TakenByCourier = 3,
        AtPostMachine = 4,
        Delivered = 5,
        Canceled = 6
    }
}
