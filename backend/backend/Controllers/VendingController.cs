using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class VendingController : ControllerBase
{
    private readonly VendingService _service;

    public VendingController(VendingService service)
    {
        _service = service;
    }
}