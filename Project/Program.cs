using Newtonsoft.Json;
using Project.Data;
using Project.MessageDecoders;
using Project.MessageEncoders;
using Project.Utility;

namespace ConsoleApplication1
{
    class Program
    {
        static async Task Main()
        {
            //Get Encoded Message
            var encodedMessage = await GetData.DataFromFile();

            // Decode Encoded Message
            var generatedJSON =  await new _835Decoder().Parse(encodedMessage);

            //Check SavedJSONS folder
            await FileHelpers.SaveTextFile(new _835Decoder().SavedJSONPath,
                generatedJSON);


            //Get Output message
            var retrievedJSON = await FileHelpers.ReadTextFile(
                new _835Decoder().SavedJSONPath);

            //Output Edi generated message
            await new _835Encoder().Serialize(retrievedJSON);

         
        }
    }
}