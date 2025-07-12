using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class VendingController : ControllerBase
{
    private readonly VendingRepository _vendingRepository;

    public VendingController(VendingRepository vendingRepository)
    {
        _vendingRepository = vendingRepository;
    }

    [HttpGet("")]
    public IActionResult GetDrinks() => Ok(_vendingRepository.Drinks);

    [HttpPost("")]
    public IActionResult Purchase([FromBody] PurchaseRequest request)
    {
        var result = _vendingRepository.Purchase(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}