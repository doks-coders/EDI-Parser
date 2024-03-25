using Project.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MessageDecoders
{
    internal class _835Decoder:ParseEDI
    {
        public override Dictionary<string, Type> KeyTypePairs => _835ClassProperties.KeyTypePairs;

        public override string SavedJSONPath => "Output/Edi-Json.json";
    }
}
