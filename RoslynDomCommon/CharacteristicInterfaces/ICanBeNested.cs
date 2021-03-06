﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynDom.Common
{
    public interface ICanBeNested : IDom
    {
        bool IsNested { get; }
        IType ContainingType { get; }
        string OuterName { get; }
    }
}
