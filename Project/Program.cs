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

            //Check Output folder
            await FileHelpers.SaveTextFile(new _835Decoder().SavedJSONPath,
                generatedJSON);


            //Get Output message
            var retrievedJSON = await FileHelpers.ReadTextFile(
                new _835Decoder().SavedJSONPath);

            //Output Edi generated message
            var ediOutput =  await new _835Encoder().Serialize(retrievedJSON);

            //Save Edi message
            await FileHelpers.SaveTextFile(new _835Encoder().SavedEDIPath,
                ediOutput);

        }
    }
}