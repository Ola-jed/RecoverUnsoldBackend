using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldDomain.Dto;
using RecoverUnsoldDomain.Extensions;
using RecoverUnsoldDomain.Services.Auth;
using RecoverUnsoldDomain.Services.Offers;
using RecoverUnsoldDomain.Services.Products;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productsService;
    private readonly IOffersService _offersService;

    public ProductsController(IProductsService productsService, IOffersService offersService)
    {
        _productsService = productsService;
        _offersService = offersService;
    }

    [HttpGet]
    public async Task<UrlPage<ProductReadDto>> GetProducts([FromQuery] int page = 1, [FromQuery] int perPage = 10)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            perPage, page, this.GetCleanUrl(), nameof(page), nameof(perPage)
        );
        return await _productsService.GetProducts(urlPaginationParam);
    }

    [HttpGet("{id:guid}", Name = nameof(GetProduct))]
    [ProducesResponseType(typeof(ProductReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductReadDto>> GetProduct(Guid id)
    {
        var product = await _productsService.GetProduct(id);
        return product == null ? NotFound() : product;
    }

    [HttpGet("Distributors/{id:guid}")]
    public async Task<UrlPage<ProductReadDto>> GetDistributorProducts(Guid id, [FromQuery] int page = 1,
        [FromQuery] int perPage = 10)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            perPage, page, this.GetCleanUrl(), nameof(page), nameof(perPage)
        );
        return await _productsService.GetDistributorProducts(id, urlPaginationParam);
    }

    [HttpGet("/api/Offers/{id:guid}/Products")]
    public async Task<UrlPage<ProductReadDto>> GetOfferProducts(Guid id, [FromQuery] int page = 1,
        [FromQuery] int perPage = 10)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            perPage, page, this.GetCleanUrl(), nameof(page), nameof(perPage)
        );
        return await _productsService.GetOfferProducts(id, urlPaginationParam);
    }

    [Authorize(Roles = Roles.Distributor)]
    [HttpPost("/api/Offers/{id:guid}/Products")]
    [ProducesResponseType(typeof(ProductReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProductReadDto>> CreateProduct(Guid id, [FromForm] ProductCreateDto productCreateDto)
    {
        var distributorId = this.GetUserId();
        var offerOwnedByCurrentUser = await _offersService.IsOwner(distributorId, id);
        if (!offerOwnedByCurrentUser)
        {
            return Forbid();
        }

        var product = await _productsService.Create(id, productCreateDto);
        return CreatedAtRoute(nameof(GetProduct), new { id = product.Id }, product);
    }

    [Authorize(Roles = Roles.Distributor)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateProduct(Guid id, ProductUpdateDto productUpdateDto)
    {
        var distributorId = this.GetUserId();
        var isProductOwner = await _productsService.IsOwner(id, distributorId);
        if (!isProductOwner)
        {
            return NotFound();
        }

        await _productsService.Update(id, distributorId, productUpdateDto);
        return NoContent();
    }

    [Authorize(Roles = Roles.Distributor)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductReadDto>> DeleteProduct(Guid id)
    {
        var distributorId = this.GetUserId();
        var isProductOwner = await _productsService.IsOwner(id, distributorId);
        if (!isProductOwner)
        {
            return NotFound();
        }

        await _productsService.Delete(id, distributorId);
        return NoContent();
    }
}