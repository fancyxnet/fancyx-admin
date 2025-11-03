using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.EfCore;
using Fancyx.Internal.Grpc.System;
using Grpc.Core;

namespace Fancyx.Admin.Application.Grpc
{
    public class DictGrpcServiceHandler: Dict.DictBase
    {
        private readonly IRepository<DictData> _dictDataRepository;

        public DictGrpcServiceHandler(IRepository<DictData> dictDataRepository)
        {
            _dictDataRepository = dictDataRepository;
        }

        public override async Task<GetDictItemsRes> GetDictItems(GetDictItemsReq request, ServerCallContext context)
        {
            var items = await _dictDataRepository.Where(x => x.DictType == request.DictType).SelectToListAsync(x=>new DictItem { Label = x.Label,Value=x.Value});
            var res = new GetDictItemsRes() { };
            res.Items.AddRange(items);
            return res;
        }
    }
}
