using Microsoft.AspNetCore.Mvc;

namespace CacheRevokerMiddleware.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUsersService usersService) : ControllerBase
{
    [HttpPost("")]
    [CacheRevoker(domain: "usersCache", keyParam: "id")]
    public async Task<ActionResult> Create(UserModel userModel)
    {
        await usersService.Create(userModel);
        return Ok();
    }

    [HttpPut("{id}")]
    [CacheRevoker(domain: "usersCache", keyParam: "id")]
    public async Task<ActionResult> Update(int id, UpdateUserModel userModel)
    {
        await usersService.Update(userModel.ToUserModel(id));
        return Ok();
    }
}