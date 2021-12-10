using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParcheggioAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using NLog;

namespace ParcheggioAPI.Controllers
{
    [ApiController]
    public class LoginLogoutController : ControllerBase
    {
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();
        [HttpPost("/api/Login")]
        public ActionResult Login([FromBody] User usercredentials)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if(usercredentials.Password != null && usercredentials.Username != null)
                {
                    var candidate = model.Users.FirstOrDefault(fod => fod.Username == usercredentials.Username && fod.Password == usercredentials.Password);
                    if (candidate == null) return NotFound();

                    var TokenHandler = new JwtSecurityTokenHandler();
                    var TokenDescriptor = new SecurityTokenDescriptor
                    {
                        SigningCredentials = new SigningCredentials(SecurityKeyGenerator.GetSecurityKey(), SecurityAlgorithms.HmacSha256Signature),
                        Expires = DateTime.UtcNow.AddDays(1),
                        Subject = new ClaimsIdentity(
                            new Claim[]
                            {
                            new Claim("Id", candidate.Id.ToString()),
                            new Claim("Username", candidate.Username),
                            }
                        )
                    };

                    SecurityToken token = TokenHandler.CreateToken(TokenDescriptor);
                    
                    logger.Log(LogLevel.Info, "Login utente con username {User}.", usercredentials.Username);
                    LoginClass LC = new LoginClass
                    {
                        Token = TokenHandler.WriteToken(token).ToString(),
                        Username = candidate.Username
                    };
                    return Ok(LC);
                }
                else
                {
                    return Problem();
                }
            }
        }

        [Authorize]
        [HttpPost("/api/Logout")]
        public ActionResult Logout()
        {
            
            var username = HttpContext.User.Claims.FirstOrDefault(fod => fod.Type == "Username").Value;
            var id = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(fod => fod.Type == "Id").Value);
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var candidate = model.Users.FirstOrDefault(fod => fod.Id == id && fod.Username == username);
                if (candidate == null) return NotFound("Impossibile effettuare il logout, token non valido");
                else
                    logger.Log(LogLevel.Info, "Logout utente con username {User}.", candidate.Username);
                    return Ok("Logout effettuato con successo");
            }    
        }
        
        [Authorize]
        [HttpGet("/api/Admin")]
        public ActionResult IsAdmin()
        {
            var username = HttpContext.User.Claims.FirstOrDefault(fod => fod.Type == "Username").Value;
            var id = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(fod => fod.Type == "Id").Value);
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var candidate = model.Users.FirstOrDefault(fod => fod.Id == id && fod.Username == username);
                if (candidate == null) return NotFound("Errore");
                else
                    if(candidate.Username == "Admin" && candidate.Password == "Admin") return Ok(true);
                    return Ok(false);
            }
        }
    }
}
