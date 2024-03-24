﻿using EdiEngine.Runtime;
using EdiEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Utility;
using System.Text.Json;
using Newtonsoft.Json;
using System.Reflection.Metadata;

namespace Project.MessageDecoders
{
    record SegmentValue(string Name, string GroupName, Dictionary<string, string> Values);
    internal static class ParseEDI
    {
        public static List<SegmentValue> SegmentList = new();

        public static string GroupName = string.Empty;
        public static async Task LoopThroughEDIList(string edl)
        {
            var interchanges = await ReturnInterChanges(edl);


            foreach (var interchange in interchanges)
            {
                string ediVersion = GetEDIVersion(interchange);

                foreach (var groups in interchange.Groups)
                {

                    foreach (var transaction in groups.Transactions)
                    {
                        //po = string.Empty;
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

        public static void GetContentDetails(EdiBaseEntity content)
        {
            //Console.WriteLine(content.Name);
            GetSegmentDictionaries(content, KeyTypePairs[content.Name]);
            /*
            switch (content.Name)
            {
                case "BPR":
                    GetSegmentDictionaries(content, typeof(SD.BPRProperties));
                    break;
                case "TRN":
                    GetSegmentDictionaries(content, typeof(SD.TRNProperties));
                    break;
                case "DTM":
                    GetSegmentDictionaries(content, typeof(SD.DTMProperties));
                    break;
            }
            */
            
        }

        public static void GetContentLoop(EdiBaseEntity content)
        {
            if (content.Type == "L")
            {
                GroupName = content.Name;
                GetSegmentLoop(content);
            }   
        }

        public static void GetSegmentLoop(EdiBaseEntity ediBaseEntity)
        {
            var ediLoop = ediBaseEntity as EdiLoop;
            foreach (var ediBase in ediLoop.Content)
            {
                if (ediBase.Type == "S")
                    GetSegmentDictionaries(ediBase, KeyTypePairs[ediBase.Name]);

                if (ediBase.Type == "L")
                {
                    GroupName = ediBase.Name;
                    GetSegmentLoop(ediBase);

                }
            }
        }
        public static void GetSegmentDictionaries(EdiBaseEntity content, Type enumClass)
        {
            var ediContent = content as EdiSegment;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            for (int i = 0; i < ediContent.Content.Count(); i++)
            {
                keyValuePairs.Add(

                    GetEnumString(enumClass, i),

                    ediContent.Content.ElementAt(i).Val

                    );
            }
            SegmentList.Add(new SegmentValue(content.Name,GroupName, keyValuePairs));
        }
        public static string GetEnumString(Type enumClass, int intValue) => Enum.GetName(enumClass, intValue);

        public static async Task<List<EdiInterchange>> ReturnInterChanges(string edi)
        {
            EdiDataReader r = new EdiDataReader();
            EdiBatch b = r.FromString(edi);
            string jsonTrans = JsonConvert.SerializeObject(b);
            await FileHelpers.SaveTextFile("Savedjsons/Experiment.json", jsonTrans);

            return b.Interchanges;
        }

        public static string GetEDIVersion(EdiInterchange interchange)
        {
            var item = interchange.ISA.Content.ElementAt((int)SD.ISAProperties.InterchangeControlVersionNumber);
            if (item != null) return item.ToString();
            return string.Empty;
        }
    }
}
