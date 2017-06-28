using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnosquareAspNetCore.Models;
using System.Collections.Generic;
using System.Linq;
using Unosquare.Tubular;
using Unosquare.Tubular.ObjectModel;

namespace UnosquareAspNetCore.Controllers
{
    [Route("api/[controller]"), Authorize]
    public class EmployeeController : Controller
    {
        readonly static List<Employee> List = new List<Employee>();

        public EmployeeController()
        {
            if(List.Count == 0)
            {
                for (int i = 0; i < 500; i++)
                {
                    List.Add(new Employee { Id = i, Title = $"Senior Developer {i}", Name = $"Dev Name {i}" });
                }
            }
        }

        [HttpPost, Route("paged")]
        public object GridData([FromBody] GridDataRequest request)
        {
            return request.CreateGridDataResponse(List.AsQueryable());
        }

        [HttpGet, Route("{id}")]
        public object Get(int id)
        {
            return List.Where(e => e.Id == id).FirstOrDefault();
        }
    }
}
