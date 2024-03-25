using EdiEngine.Common.Definitions;
using EdiEngine.Standards.X12_004010.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MessageEncoders
{
    internal class _835Encoder:SerializeEDI
    {
        public override MapLoop GetMap() => new M_835();
        
    }
}
