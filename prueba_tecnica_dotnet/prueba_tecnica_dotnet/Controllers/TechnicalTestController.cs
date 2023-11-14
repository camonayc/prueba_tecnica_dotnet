using Microsoft.AspNetCore.Mvc;
using prueba_tecnica_dotnet.Models.Request;
using prueba_tecnica_dotnet.Models.Response;
using prueba_tecnica_dotnet.Services;

namespace prueba_tecnica_dotnet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TechnicalTestController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        public TechnicalTestController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost("login",Name = "Login")]
        public ActionResult<ValidateResponse> Login(UserRequest userRequest)
        {
            ValidateResponse respuesta = _databaseService.ValidateCredentials(userRequest);

            return respuesta;
        }
        [HttpPost("register", Name = "Register")]
        public ActionResult<ValidateResponse> Register(RegisterUserRequest registerRequest)
        {
            ValidateResponse respuesta = _databaseService.RegisterUser(registerRequest);

            return respuesta;
        }
    }
}