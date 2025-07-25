﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Entities
{
    public class RequestResult : IRequestResult
    {

        public RequestResult(string Description)
        {
            this.Result = EnumRequestResult.Error;
            this.LongDescription = Description;
        }
        public string? LongDescription { get; set; }
        public string? ShortDescription { get; set; }
        public EnumRequestResult Result { get; set; }
        public EnumTo To { get; set; } = EnumTo.Client;
        public object Object { get; set; }
    }

}
