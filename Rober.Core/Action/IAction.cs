using System;

namespace Rober.Core.Action
{
    public interface IAction
    {
        int Execute(Object request,RequestHeader requestHeader, out Object response);
    }
}
