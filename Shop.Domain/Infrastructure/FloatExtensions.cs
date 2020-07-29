using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain.Infrastructure
{
    public static class FloatExtensions
    {
        public static string GetValueString(this float value) =>
            $"$ {value.ToString()}";

    }
}
