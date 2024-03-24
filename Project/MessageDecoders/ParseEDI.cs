using EdiEngine.Runtime;
using EdiEngine;
using Project.Utility;
using Newtonsoft.Json;
using Project.Constants;

namespace Project.MessageDecoders
{
    record SegmentValue(string Name, string GroupName, Dictionary<string, string> Values);
   
    /// <summary>
    /// This class is dedicated to parse EDIs
    /// </summary>
    internal static class ParseEDI
    {
        public static List<SegmentValue> SegmentList = new();
        public static string GroupName = string.Empty;

        /// <summary>
        /// This method takes in the edi string and parses it
        /// </summary>
        /// <param name="edi"></param>
        /// <returns></returns>
        public static async Task Parse(string edi)
        {
            var interchanges = await GetInterchangesAsync(edi);

            foreach (var interchange in interchanges)
            {
                string ediVersion = GetEDIVersion(interchange);

                foreach (var groups in interchange.Groups)
                {

                    foreach (var transaction in groups.Transactions)
                    {
                        foreach (var content in transaction.Content)
                        {
                            if (content.Type == "L")
                                GetContentLoop(content);
                            else
                                GetContentDetails(content);
                        }
                    }
                }
            }


            await FileHelpers.SaveTextFile("SavedJSONS/SegmentsNew.json", JsonConvert.SerializeObject(SegmentList));

        }


        /// <summary>
        /// This method starts the process of getting all the segments
        /// </summary>
        /// <param name="content"></param>
        public static void GetContentDetails(EdiBaseEntity content)
        {
            AddSegmentDictionaries(content, _835ClassProperties.KeyTypePairs[content.Name]);
        }

        /// <summary>
        /// This method starts the process of getting all the loops
        /// </summary>
        /// <param name="content"></param>
        public static void GetContentLoop(EdiBaseEntity content)
        {
            if (content.Type == "L")
            {
                GroupName = content.Name;
                GetSegmentLoop(content);
            }
        }

        /// <summary>
        /// This method runs a recursively and checks if there are loops "L" or segments "S".
        /// If there are segments then it gets the segment dictionaries and 
        /// if it's a loop it runs the method again
        /// </summary>
        /// <param name="ediBaseEntity"></param>
        public static void GetSegmentLoop(EdiBaseEntity ediBaseEntity)
        {
            var ediLoop = ediBaseEntity as EdiLoop;
            foreach (var ediBase in ediLoop.Content)
            {
                if (ediBase.Type == "S")
                    AddSegmentDictionaries(ediBase, _835ClassProperties.KeyTypePairs[ediBase.Name]);

                if (ediBase.Type == "L")
                {
                    GroupName = ediBase.Name;
                    GetSegmentLoop(ediBase);

                }
            }
        }
        /// <summary>
        /// Creates a dictionary/key-value of named-property : value. 
        /// The named-property are gotten from the enum at that index and 
        /// the values are gotten from the content from the same index
        /// </summary>
        /// <param name="content"></param>
        /// <param name="enumClass"></param>
        public static void AddSegmentDictionaries(EdiBaseEntity content, Type enumClass)
        {
            var ediContent = content as EdiSegment;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            for (int i = 0; i < ediContent.Content.Count(); i++)
            {
                keyValuePairs.Add(

                   Utils.GetEnumString(enumClass, i),

                    ediContent.Content.ElementAt(i).Val

                    );
            }
            SegmentList.Add(new SegmentValue(content.Name, GroupName, keyValuePairs));
        }
      
        /// <summary>
        /// Parses the edi into json format and gets the interchange property
        /// </summary>
        /// <param name="edi"></param>
        /// <returns></returns>
        public static async Task<List<EdiInterchange>> GetInterchangesAsync(string edi)
        {
            EdiDataReader r = new EdiDataReader();
            EdiBatch b = r.FromString(edi);

            /*
            string jsonTrans = JsonConvert.SerializeObject(b);
            await FileHelpers.SaveTextFile("Savedjsons/Experiment.json", jsonTrans);
            */

            return b.Interchanges;
        }

        /// <summary>
        /// This method gets the EDI message version from the interchange property
        /// </summary>
        /// <param name="interchange"></param>
        /// <returns></returns>
        public static string GetEDIVersion(EdiInterchange interchange)
        {
            var item = interchange.ISA.Content.ElementAt((int)_835ClassProperties.Properties.ISAProperties.InterchangeControlVersionNumber);
            if (item != null) return item.ToString();
            return string.Empty;
        }
    }
}
