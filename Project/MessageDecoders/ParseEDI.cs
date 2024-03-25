using EdiEngine;
using EdiEngine.Runtime;
using EdiEngine.Standards.X12_004010.Maps;
using Newtonsoft.Json;
using Project.Constants;
using Project.Utility;

namespace Project.MessageDecoders;

record SegmentValue(string Name, string GroupName, Dictionary<string, string> Values);

/// <summary>
/// This class is dedicated to parse EDIs
/// </summary>
internal class ParseEDI
{
    public List<SegmentValue> SegmentList = new();
    public string GroupName = string.Empty;

    //Will be overidden
    public virtual Dictionary<string, Type> KeyTypePairs => new();

    /// <summary>
    /// Our generated json would be saved in this path
    /// </summary>
    public virtual string SavedJSONPath => string.Empty;

    /// <summary>
    /// This method takes in the edi string and parses it to our readable json format
    /// </summary>
    /// <param name="edi"></param>
    /// <returns></returns>
    public async Task<string> Parse(string edi)
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

        return JsonConvert.SerializeObject(SegmentList);
        

    }


    /// <summary>
    /// This method starts the process of getting all the segments
    /// </summary>
    /// <param name="content"></param>
    public void GetContentDetails(EdiBaseEntity content)
    {
        AddSegmentDictionaries(content, KeyTypePairs[content.Name]);
    }

    /// <summary>
    /// This method starts the process of getting all the loops
    /// </summary>
    /// <param name="content"></param>
    public void GetContentLoop(EdiBaseEntity content)
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
    public void GetSegmentLoop(EdiBaseEntity ediBaseEntity)
    {
        var ediLoop = ediBaseEntity as EdiLoop;
        foreach (var ediBase in ediLoop.Content)
        {
            if (ediBase.Type == "S")
                AddSegmentDictionaries(ediBase, KeyTypePairs[ediBase.Name]);

            if (ediBase.Type == "L")
            {
                GroupName = ediBase.Name;
                GetSegmentLoop(ediBase);

            }
        }
    }
    /// <summary>
    /// Creates a dictionary/key-value of named-field : value. 
    /// The named-field are gotten from the enum at that index and 
    /// the values are gotten from the content from the same index
    /// </summary>
    /// <param name="content"></param>
    /// <param name="enumClass"></param>
    public void AddSegmentDictionaries(EdiBaseEntity content, Type enumClass)
    {
        var ediContent = content as EdiSegment;
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        for (int i = 0; i < ediContent.Content.Count(); i++)
        {
            var enumString = Utils.GetEnumString(enumClass, i);

            keyValuePairs.Add(

                enumString,

                ediContent.Content.ElementAt(i).Val ?? ""

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
    public  string GetEDIVersion(EdiInterchange interchange)
    {
        var item = interchange.ISA.Content.ElementAt((int) new MainClassProperties.Properties.ISAProperties());
        if (item != null) return item.ToString();
        return string.Empty;
    }
}
