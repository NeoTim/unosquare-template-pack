using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwanAspNetCore.Models;
using System.Collections.Generic;
using System.Linq;
using Unosquare.Tubular;
using Unosquare.Tubular.ObjectModel;

namespace SwanAspNetCore.Controllers
{
    [Route("api/[controller]"), Authorize]
    public class EmployeeController : Controller
    {
        // TODO: Change to database
        readonly List<Employee> List = new List<Employee>
        {
            new Employee { Title = "Senior Developer", Name = "Mario Di Vece" },
            new Employee { Title = "Intern", Name = "Babu" }
        };

        [HttpPost, Route("paged")]
        public object GridData([FromBody] GridDataRequest request)
        {
            return request.CreateGridDataResponse(List.AsQueryable());
        }
    }
}
