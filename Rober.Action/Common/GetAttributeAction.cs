using Rober.Action.Model.Common;
using Rober.Core.Action;
using Rober.IDAL.Repository;

namespace Rober.Action.Common
{
    public class GetAttributeAction : ActionSupport<GetAttributeRequest, GetAttributeResponse>
    {
        #region 构造函数
        private readonly IGenericAttributeRepository _genericAttributeRepository;
        public GetAttributeAction(IGenericAttributeRepository genericAttributeRepository)
        {
            this._genericAttributeRepository = genericAttributeRepository;
        }
        #endregion

        public override int DoExecute(GetAttributeRequest request, RequestHeader requestHeader, out GetAttributeResponse response)
        {
            response = new GetAttributeResponse()
            {
                GenericAttribute = _genericAttributeRepository.GetAttributeForEntity(request.EntityId, request.KeyGroup, request.Key)
            };

            SetSuccess();
            return Code;
        }
    }
}
