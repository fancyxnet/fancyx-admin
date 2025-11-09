using Fancyx.Internal.Grpc.System;
using FluentValidation;

namespace Fancyx.Admin.Application.Grpc.Validators
{
    public class GetDictItemsReqValidator : AbstractValidator<GetDictItemsReq>
    {
        public GetDictItemsReqValidator()
        {
            RuleFor(x => x.DictType).NotEmpty();
        }
    }
}
