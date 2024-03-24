using System;
using EdiEngine.Standards.X12_004010.Maps;
using System.Linq;
using EdiEngine;
using EdiEngine.Common.Definitions;
using EdiEngine.Runtime;
using SegmentDefinitions = EdiEngine.Standards.X12_004010.Segments;
using Project.Data;
using Project.MessageDecoders;
using Project.MessageEncoders;

namespace ConsoleApplication1
{
    class Program
    {
        static async Task Main()
        {
            //Get Encoded Message
            var encodedMessage =  await GetData.DataFromFile();

            // Decode Encoded Message
            //await ParseEDI.LoopThroughEDIList(encodedMessage);

            await SerializeEDI.Serialize();

            //Encode Decoded Message
        }
    }
}