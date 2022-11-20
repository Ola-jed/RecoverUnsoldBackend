using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Opinions;

public interface IOpinionsService
{
    Task<bool> IsUserAuthor(Guid userId, Guid opinionId);
    Task<Page<OpinionReadDto>> Get(Guid orderId, PaginationParameter paginationParameter);
    Task<OpinionReadDto?> Get(Guid id);
    Task<OpinionReadDto> Publish(OpinionCreateDto opinionCreateDto, Guid orderId);
    Task Update(Guid id, OpinionUpdateDto opinionUpdateDto);
    Task Delete(Guid id);
}