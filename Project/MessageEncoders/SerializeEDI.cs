using EdiEngine;
using EdiEngine.Common.Definitions;
using EdiEngine.Runtime;
using EdiEngine.Standards.X12_004010.Maps;
using Newtonsoft.Json;
using Project.Constants;
using Project.Data;
using SegmentDefinitions = EdiEngine.Standards.X12_004010.Segments;


namespace Project.MessageEncoders;

internal class SerializeEDI
{
    /// <summary>
    /// This is the format of the segment value that is saved in our json file.
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="GroupName"></param>
    /// <param name="Values"></param>
    public record SegmentValue(string Name, string GroupName, Dictionary<string, string> Values);

    public virtual MapLoop GetMap() => new M_100();

    /// <summary>
    /// This method gets the content of our formatted JSON file and turns it into an Edi file
    /// </summary>
    /// <returns></returns>
    public async Task Serialize(string customJSON)
    {
        var map = GetMap();

        EdiTrans t = new EdiTrans(map);

        var firstLevel = map.Content.FindAll(e => e.Name.StartsWith("L_"));

        SetAllLoops(firstLevel);

        var obj = JsonConvert.DeserializeObject<List<SegmentValue>>(customJSON);

        for (var i = 0; i < obj.Count; i++)
        {
            var item = obj[i];

            if (item.GroupName == string.Empty)
            {
                var segment = (MapSegment)map.Content.First(s => s.Name == item.Name);
                t.Content.Add(MapSegmentMethod(segment, item));
            }

            if (!string.IsNullOrEmpty(item.GroupName))
            {
                var loopedSegment = keyLoopPairs[item.GroupName];
                t.Content.Add(MapLoopMethod(loopedSegment, item));
            }
        }

        var g = new EdiGroup("OW");

        g.Transactions.Add(t);

        var interchange = new EdiInterchange();
        interchange.Groups.Add(g);

        EdiBatch b = new EdiBatch();
        b.Interchanges.Add(interchange);

        //Add all service segments
        EdiDataWriterSettings settings = new EdiDataWriterSettings(
            new SegmentDefinitions.ISA(), new SegmentDefinitions.IEA(),
            new SegmentDefinitions.GS(), new SegmentDefinitions.GE(),
            new SegmentDefinitions.ST(), new SegmentDefinitions.SE(),
            "ZZ", "SENDER", "ZZ", "RECEIVER", "GSSENDER", "GSRECEIVER",
            "00401", "004010", "T", 100, 200, "\r\n", "*");

        EdiDataWriter w = new EdiDataWriter(settings);
        Console.WriteLine(w.WriteToString(b));
    }

    /// <summary>
    /// This gets the MapSegment from the loop
    /// </summary>
    /// <param name="loopedSegment"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public EdiSegment MapLoopMethod(MapLoop loopedSegment, SegmentValue item)
    {
        var segment = (MapSegment)loopedSegment.Content.First(s => s.Name == item.Name);

        return MapSegmentMethod(segment, item);

    }

    /// <summary>
    /// This method gets the index position of the property names in the selected Class properties 
    /// as well as the value.
    /// and creates an EDISegment with it
    /// </summary>
    /// <param name="mappedSegment"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public EdiSegment MapSegmentMethod(MapSegment mappedSegment, SegmentValue item)
    {
        var segment = new EdiSegment(mappedSegment);

        int shiftspace = 0; //Hack: incase of an error

        foreach (var val in item.Values)
        {

            var enumName = Enum.Parse(_835ClassProperties.KeyTypePairs[item.Name], val.Key);
            var index = Array.IndexOf(Enum.GetValues(_835ClassProperties.KeyTypePairs[item.Name]), enumName);

            try
            {
                segment.Content.Add(new EdiSimpleDataElement((MapSimpleDataElement)mappedSegment.Content[index + shiftspace], val.Value));
            }
            catch (Exception ex)
            {
                /*
                 * Hack, if there is an error in an index 
                 * it simply shifts to the next position. [There is an internal problem with SVC index 0 and 4].*/

                shiftspace++;
                segment.Content.Add(new EdiSimpleDataElement((MapSimpleDataElement)mappedSegment.Content[index + shiftspace], val.Value));

                /********
                Console.WriteLine($"Error at position: {index} of {item.Name}");
                Console.WriteLine(ex.Message);
                ********/

            }

        }
        return segment;

    }
    /// <summary>
    /// This dictionary contains all loopNames and the loops in selected version ex: M_835 
    /// </summary>
    public Dictionary<string, MapLoop> keyLoopPairs = new();

    /// <summary>
    /// This method gets all the loops and their names and appends them to the keyLoopPairs' dictionary.
    /// They will be used for accessing the loops quicker.
    /// </summary>
    /// <param name="baseEntities"></param>
    public void SetAllLoops(List<MapBaseEntity> baseEntities)
    {
        foreach (var entry in baseEntities)
        {
            var LoopEntry = (MapLoop)entry;
            keyLoopPairs[entry.Name] = LoopEntry;

            var foundEntries = LoopEntry.Content.FindAll(s => s.Name.StartsWith("L_"));
            SetAllLoops(foundEntries);
        }
    }


}
