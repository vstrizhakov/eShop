using AutoMapper;
using eShop.Common;
using eShop.Distribution.Models;
using eShop.Distribution.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Distribution.Controllers
{
    [Route("api/distribution/clients")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients(
            [FromServices] IAccountRepository accountRepository,
            [FromServices] IMapper mapper)
        {
            var accountId = User.GetAccountId().Value;
            var clientAccounts = await accountRepository.GetAccountsByProviderIdAsync(accountId);
            var clients = mapper.Map<IEnumerable<Client>>(clientAccounts);
            return Ok(clients);
        }
    }
}
