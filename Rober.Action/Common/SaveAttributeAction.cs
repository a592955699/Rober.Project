using Rober.Action.Model.Common;
using Rober.Core.Action;
using Rober.IDAL.Repository;

namespace Rober.Action.Common
{
    public class SaveAttributeAction : ActionSupport<SaveAttributeRequest, SaveAttributeResponse>
    {
        #region 构造函数
        private readonly IGenericAttributeRepository _genericAttributeRepository;
        public SaveAttributeAction(IGenericAttributeRepository genericAttributeRepository)
        {
            this._genericAttributeRepository = genericAttributeRepository;
        }
        #endregion

        public override int DoExecute(SaveAttributeRequest request, RequestHeader requestHeader, out SaveAttributeResponse response)
        {
            response = new SaveAttributeResponse();
            _genericAttributeRepository.SaveAttribute(request.EntityId, request.KeyGroup, request.Key, request.Value.ToString());
            SetSuccess();
            return Code;
        }
    }
}
