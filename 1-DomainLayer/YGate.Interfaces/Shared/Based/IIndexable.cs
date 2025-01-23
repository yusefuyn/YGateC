﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.Shared.Based
{
    public interface IIndexable
    {
        [Key]
        public int Id { get; set; }
    }
}
