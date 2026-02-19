using Riok.Mapperly.Abstractions;
using Contracts.Entities;
using Stocks;

namespace WebApi.Mappers;

[Mapper]
public partial class FiltersRequestMapper
{
    public partial FiltersRequest ToGrpc(Filters entity);
}
