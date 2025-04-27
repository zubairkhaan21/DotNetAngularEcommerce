using System;
using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorizeRequest()
    {
        return Unauthorized();
    }

    [HttpGet("badrequest/")]
    public ActionResult GetServerError()
    {
        return BadRequest("This is a bad request");
    }

    [HttpGet("bad-request")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ProblemDetails { Title = "This is a bad request" });
    }

    [HttpGet("notfound")]
    public IActionResult GetNotFoundRequest()
    {
        return NotFound();
    }

    [HttpGet("internalerror")]
    public IActionResult GetnternalError()
    {
        throw new Exception("This is test exception for server error");
    }

    [HttpPost("Validationerror")]
    public IActionResult GetValidationError(CreateProductDto product)
    {
        return BadRequest(new ProblemDetails { Title = "This is a validation error" });
    }
}

