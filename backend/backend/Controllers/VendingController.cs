using Microsoft.AspNetCore.Mvc;
using backend.Models;

[ApiController]
[Route("api/[controller]")]
public class VendingController : ControllerBase
{
    private readonly IVendingRepository _vendingRepository;

    public VendingController(IVendingRepository vendingRepository)
    {
        _vendingRepository = vendingRepository;
    }

    [HttpGet("")]
    public IActionResult GetDrinks()
    {
        var drinks = _vendingRepository.GetDrinks();
        return Ok(drinks);
    }

    [HttpPost("")]
    public IActionResult Purchase([FromBody] PurchaseRequest request)
    {
        var result = _vendingRepository.Purchase(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}