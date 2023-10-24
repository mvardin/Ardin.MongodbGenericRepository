# Ardin.MongodbGenericRepository


1- add Ardin.MongodbGenericRepository from nuget 
Install-Package Ardin.MongodbGenericRepository

2- add your entities to ptoject

3- add 
        builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
to program

4- add this lines in app.settings

"MongoConnectionString": "mongodb://localhost:27017",
"MongoDatabase": "SampleApp"

5- use this code for testing and using

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
