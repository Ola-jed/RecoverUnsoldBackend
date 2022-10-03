using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldDomain.Dto;

namespace RecoverUnsoldDomain.Services.Opinions;

public interface IOpinionsService
{
    Task<bool> IsUserAuthor(Guid userId, Guid opinionId);
    Task<UrlPage<OpinionReadDto>> Get(Guid orderId, UrlPaginationParameter urlPaginationParameter);
    Task<OpinionReadDto?> Get(Guid id);
    Task<OpinionReadDto> Publish(OpinionCreateDto opinionCreateDto, Guid orderId);
    Task Update(Guid id, OpinionUpdateDto opinionUpdateDto);
    Task Delete(Guid id);
}