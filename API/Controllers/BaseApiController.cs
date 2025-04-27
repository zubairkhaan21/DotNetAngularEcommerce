using System;
using API.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected async Task<ActionResult> CreatePagesResult<T>(IGenericRepository<T> repository, 
        ISpecification<T> spec, int pageIndex, int pageSize) where T : BaseEntity
    {
        var totalItems = await repository.CountAsync(spec);
        var items = await repository.ListAsync(spec);
        var pagination = new Pagination<T>(pageIndex, pageSize, totalItems, items);
        return Ok(pagination);
    }
   
}
