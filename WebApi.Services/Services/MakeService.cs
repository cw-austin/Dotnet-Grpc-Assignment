using WebApi.Domain.DTOs;
using WebApi.Mappers;
using WebApi.WebApi.Repository.Repositories;

namespace WebApi.Services;

public class MakeService : IMakeService
{
    private readonly IMakeRepository _makeRepository;
    private readonly MetadataMapper _mapper;

    public MakeService(IMakeRepository makeRepository, MetadataMapper mapper)
    {
        _makeRepository = makeRepository;
        _mapper = mapper;
    }

    public async Task<List<MakeDto>> GetMakesAsync()
    {
        var items = await _makeRepository.GetMakesAsync();
        
        // Use the metadata mapper to convert gRPC items to DTOs
        return _mapper.MapToMakeDtos(items);
    }
}