using Project.Data;
using Project.MessageEncoders;

namespace ConsoleApplication1
{
    class Program
    {
        static async Task Main()
        {
            //Get Encoded Message
            var encodedMessage = await GetData.DataFromFile();

            // Decode Encoded Message
            //await ParseEDI.Parse(encodedMessage);

            await SerializeEDI.Serialize();

            //Encode Decoded Message
        }
    }
}