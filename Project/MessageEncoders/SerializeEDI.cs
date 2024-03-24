using EdiEngine;
using EdiEngine.Common.Definitions;
using EdiEngine.Runtime;
using EdiEngine.Standards.X12_004010.Maps;
using Newtonsoft.Json;
using Project.Constants;
using Project.Data;
using Project.Utility;

using SegmentDefinitions = EdiEngine.Standards.X12_004010.Segments;


namespace Project.MessageEncoders
{
    internal class SerializeEDI
    {
       
        public record SegmentValue(string Name, string GroupName, Dictionary<string, string> Values);
        public static async Task Serialize()
        {
            var map = new M_835();

            EdiTrans t = new EdiTrans(map);

            var firstLevel = map.Content.FindAll(e => e.Name.StartsWith("L_"));
            GetAllLs(firstLevel);

            var obj = JsonConvert.DeserializeObject<List<SegmentValue>>(await GetData.DataFromSegmentsJSON());
            for (var i = 0; i < obj.Count; i++)
            {
                var item = obj[i];

                if (item.GroupName == string.Empty)
                {
                    var sDef = (MapSegment)map.Content.First(s => s.Name == item.Name);
                    t.Content.Add(MapSegmentMethod(sDef, item));
                }

                if (!string.IsNullOrEmpty(item.GroupName))
                {
                    var nDef = keyLoopPairs[item.GroupName];
                    t.Content.Add(MapLoopMethod(nDef, item));
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
        public static EdiSegment MapLoopMethod(MapLoop nDef, SegmentValue item)
        {
            var nnDef = (MapSegment)nDef.Content.First(s => s.Name == item.Name);
            var segn = new EdiLoop();
            
            return MapSegmentMethod(nnDef, item);

        }
        public static EdiSegment MapSegmentMethod(MapSegment sDef, SegmentValue item)
        {
          
            var segment = new EdiSegment(sDef);

            int shiftspace = 0; //Hack: incase of an error

            foreach (var val in item.Values)
            {
                
                var enumName = Enum.Parse(_835ClassProperties.KeyTypePairs[item.Name], val.Key);
                var index = Array.IndexOf(Enum.GetValues(_835ClassProperties.KeyTypePairs[item.Name]), enumName);

                try
                {
                    segment.Content.Add(new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[index + shiftspace], val.Value));
                }
                catch (Exception ex)
                {
                    /*
                     * Hack, if there is an error in an index 
                     * it simply shifts. [There is an internal problem with SVC index 0 and 4].*/

                    shiftspace++;
                    segment.Content.Add(new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[index + shiftspace], val.Value));

                    Console.WriteLine($"Error at position: {index} of {item.Name}");
                    Console.WriteLine(ex.Message);

                }
               
            }
            return segment;

        }

        public static Dictionary<string, MapLoop> keyLoopPairs = new();
        public static void GetAllLs(List<MapBaseEntity> firstLevel)
        {
            foreach (var entry in firstLevel)
            {

                var LoopEntry = (MapLoop)entry;
                keyLoopPairs[entry.Name] = LoopEntry;
               
                var foundEntries = LoopEntry.Content.FindAll(s => s.Name.StartsWith("L_"));
                GetAllLs(foundEntries);
            }
        }





        /*
         * seg.Content.AddRange(new[] {
            new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[0], "ST"),
            new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[1], "Retail"),
            new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[2], "9"),
            new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[3], "001001"),

        var sDef = (MapSegment)map.Content.First(s => s.Name == "TD5");

        var seg = new EdiSegment(sDef);
        seg.Content.AddRange(new[] {
            new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[0], "ST"),
            new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[1], "Retail"),
            new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[2], "9"),
            new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[3], "001001"),
        });

        t.Content.Add(seg);
        */

    }
}
