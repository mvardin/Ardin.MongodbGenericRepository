using Ardin.MongodbGenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMongoRepository<User> mongoRepository;

        public AccountController(IMongoRepository<User> mongoRepository)
        {
            this.mongoRepository = mongoRepository;
        }

        public record UserModel(string Username, string Password);
        [HttpPost]
        public IActionResult Login(UserModel userModel)
        {
            var logginedUser = mongoRepository.FindOne(a => a.Username == userModel.Username && a.Password == userModel.Password);
            var userLoggined = logginedUser != null;
            return Ok(new { Status = userLoggined, Message = (userLoggined ? "Login successfully" : "Wrong username or password") });
        }
    }
}
