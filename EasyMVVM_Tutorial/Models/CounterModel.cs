using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyMVVM_Tutorial.Models
{
    internal class CounterModel
    {
        private ulong _counterValue;

        public async Task<ulong> IncreaseCounter()
        {
            return _counterValue++;
        }

        public async Task ResetCounter()
        {
            _counterValue = 0;
        }
    }
}
