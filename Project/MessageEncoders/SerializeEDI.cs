using EdiEngine;
using EdiEngine.Common.Definitions;
using EdiEngine.Runtime;
using EdiEngine.Standards.X12_004010.Maps;
using Newtonsoft.Json;
using Project.Data;
using Project.Utility;

using SegmentDefinitions = EdiEngine.Standards.X12_004010.Segments;


namespace Project.MessageEncoders
{
    internal class SerializeEDI
    {
        public static Dictionary<string, Type> KeyTypePairs = new()
        {
            {"BPR",typeof(SD.BPRProperties)},
            {"TRN",typeof(SD.TRNProperties)},
            {"DTM",typeof(SD.DTMProperties)},

            {"N1",typeof(SD.N1Properties)},
            {"N3", typeof(SD.N3Properties)},
            {"N4",typeof(SD.N4Properties)},
            {"REF", typeof(SD.REFProperties) },

            {"LX", typeof(SD.LXProperties) },
            {"CLP", typeof(SD.CLPProperties) },
            {"NM1", typeof(SD.NM1Properties) },

            {"SVC", typeof(SD.SVCProperties) },
            {"CAS", typeof(SD.CASProperties) },
            {"LQ", typeof(SD.LQProperties) }

        };

        public record SegmentValue(string Name, string GroupName, Dictionary<string, string> Values);
        public static async Task Serialize()
        {
            var map = new M_835();

            EdiTrans t = new EdiTrans(map);

            // W05

            var obj = JsonConvert.DeserializeObject<List<SegmentValue>>(await GetData.DataFromSegmentsJSON());


            for (var i = 0; i < obj.Count; i++)
            {
                var item = obj[i];

                if (item.GroupName == string.Empty)
                {
                    var sDef = (MapSegment)map.Content.First(s => s.Name == item.Name);
                    t.Content.Add(MapSegmentMethod(sDef, item));
                }

                if (item.GroupName == "L_N1")
                {
                    var nDef = (MapLoop)map.Content.First(s => s.Name == item.GroupName);
                    t.Content.Add(MapLoopMethod(nDef, item));
                }

                if (item.GroupName == "L_LX")
                {
                    var nDef = (MapLoop)map.Content.First(s => s.Name == item.GroupName);
                    t.Content.Add(MapLoopMethod(nDef, item));
                }
                if (item.GroupName == "L_CLP")
                {
                    var llX = (MapLoop)map.Content.First(s => s.Name == "L_LX");
                    var nDef = (MapLoop)llX.Content.First(s => s.Name == item.GroupName);
                    t.Content.Add(MapLoopMethod(nDef, item));
                }

                if (item.GroupName == "L_SVC")
                {
                    var llX = (MapLoop)map.Content.First(s => s.Name == "L_LX");
                    var llc = (MapLoop)llX.Content.First(s => s.Name == "L_CLP");
                    var llsvc = (MapLoop)llc.Content.First(s => s.Name == item.GroupName);
                    t.Content.Add(MapLoopMethod(llsvc, item));
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
            var segment = new EdiSegment(nnDef);

            int shiftspace = 0; //Hack: incase of an error

            foreach (var val in item.Values)
            {

                var enumName = Enum.Parse(KeyTypePairs[item.Name], val.Key);
                var index = Array.IndexOf(Enum.GetValues(KeyTypePairs[item.Name]), enumName);

                try
                {
                    segment.Content.Add(new EdiSimpleDataElement((MapSimpleDataElement)nnDef.Content[index + shiftspace], val.Value));
                }
                catch (Exception ex)
                {
                    //Hack, incase there is an error in one index, it simply shifts. [There is an internal problem with SVC index 0].
                    shiftspace++;
                    segment.Content.Add(new EdiSimpleDataElement((MapSimpleDataElement)nnDef.Content[index + shiftspace], val.Value));

                    Console.WriteLine($"Error at position: {index} of {item.Name}");
                    Console.WriteLine(ex.Message);

                }
            }
            return segment;

        }
        public static EdiSegment MapSegmentMethod(MapSegment sDef, SegmentValue item)
        {
            var seg = new EdiSegment(sDef);

            foreach (var val in item.Values)
            {
                var enumName = Enum.Parse(KeyTypePairs[item.Name], val.Key);
                var index = Array.IndexOf(Enum.GetValues(KeyTypePairs[item.Name]), enumName);

                seg.Content.Add(new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[index], val.Value));
            }
            return seg;
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
