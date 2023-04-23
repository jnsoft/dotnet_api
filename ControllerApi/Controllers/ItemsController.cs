using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ControllerApi.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ItemsController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    private readonly IItemsDatabase _db;

    public ItemsController(ILogger<WeatherForecastController> logger, IItemsDatabase db)
    {
        _logger = logger;
        _db = db;
    }

    // GET: <ItemsController>
    [HttpGet]
    public IEnumerable<Item> Get() => _db.Items.Values.ToList();

    // GET <ItemsController>/5
    [HttpGet("{id:guid}")]
    public Item Get(Guid id) => _db.Items[id]; 

    [HttpPost]
    public void Post([FromBody] Item item) => _db.Items.Add(item.Id, item);

    // PUT <ItemsController>/5
    [Authorize(Policy = IdentityData.AdminUserPolicy)]
    [HttpPut("{id:guid}")]
    public void Put([FromBody] Item item) => _db.Items[item.Id] = item;

    // DELETE api/<ItemsController>/5
    [Authorize]
    [RequiresClaims(IdentityData.AdminUserClaim, "true")]
    [HttpDelete("{id:guid}")]
    public void Delete(Guid id) => _db.Items.Remove(id);
}
