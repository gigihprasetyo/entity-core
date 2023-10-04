using Microsoft.AspNetCore.Mvc;

namespace qcs_product.API.Controllers
{
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]

    //Controller for New Users

    public class NewUsersControllers : ControllerBase
    {
        //create list for new users data to be accessed throughout this class
        private static List<NewUsers> users = new List<NewUsers>
            {
                new NewUsers{
                    Id = 001,
                    FirstName = "Aini",
                    LastName = "Sadilah",
                    Height = "161",
                    Weight = "56"
                },
                
                new NewUsers{
                    Id = 002,
                    FirstName = "Ainu",
                    LastName = "Fadilah",
                    Height = "162",
                    Weight = "57"
                }
            };

        [HttpGet]
        //recalling the users data list to write users data in the database
        public async Task<ActionResult<List<NewUsers>>> Get()
        {  
            return Ok(users);
        }

        [HttpGet("{id}")]
        //get data from list by id
        public async Task<ActionResult<NewUsers>> Get(int id)
        {  
            //check if the id is on the list
            var user = user.Find(u => u.Id == id);
            if (user == null)
                return BadRequest("User not found!");
                
            return Ok(user);
        }

        [HttpPost]
        //post method
        public async Task<ActionResult<List<NewUsers>>> AddUser(NewUsers user)
        {  
            users.Add(user);
            return Ok(users);
        }

        [HttpPut]
        //Updating data from the list by id
        public async Task<ActionResult<List<NewUsers>>> Update(NewUsers req)
        {  
            //check if the id is on the list
            var user = user.Find(u => u.Id == req.Id);
            if (user == null)
                return BadRequest("User not found!");
            
            //updating data on the list
            user.FirstName = req.FirstName;
            user.LastName = req.LastName;
            user.Height = req.Height;
            user.Weight = req.Weight;

            return Ok(users);
        }

        [HttpDelete("{id}")]
        //Delete data from the list by id
        public async Task<ActionResult<List<NewUsers>>> Delete(NewUsers id)
        {  
            //check if the id is on the list
            var user = user.Find(u => u.Id == id);
            if (user == null)
                return BadRequest("User not found!");
            
            //updating data on the list
            Users.Remove(user);
            return Ok(users);
        }
    }
}