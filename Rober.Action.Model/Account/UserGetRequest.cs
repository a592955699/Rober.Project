﻿using System;
using System.Collections.Generic;
using System.Text;
using Rober.Core.Action;

namespace Rober.Action.Model.Account
{
    public class UserGetRequest : SessionRequest
    {
        public int Id { get; set; }
    }
}
