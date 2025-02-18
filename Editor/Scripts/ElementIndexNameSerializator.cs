using DA_Assets.UEL;
using System.Collections.Generic;

namespace DA_Assets.UEL
{
    internal class ElementIndexNameSerializator
    {
        internal static string ConvertGuidsToJson(string targetFieldName, string[] array)
        {
            Dictionary<string, object> mainObj = new Dictionary<string, object>
            {
                ["name"] = targetFieldName,
                ["type"] = -1,
                ["arraySize"] = array.Length,
                ["arrayType"] = "string",
                ["children"] = new List<object>
                {
                    new Dictionary<string, object>
                    {
                        ["name"] = "Array",
                        ["type"] = -1,
                        ["arraySize"] = array.Length,
                        ["arrayType"] = "string",
                        ["children"] = new List<object>
                        {
                            new Dictionary<string, object>
                            {
                                ["name"] = "size",
                                ["type"] = 12,
                                ["val"] = array.Length
                            }
                        }
                    }
                }
            };

            List<object> arrayChildren = new List<object>();

            foreach (string child in array) 
            {
                arrayChildren.Add(new Dictionary<string, object>
                {
                    ["name"] = "data",
                    ["type"] = 3,
                    ["val"] = child
                });
            }

            List<object> rootChildren = (List<object>)mainObj["children"];
            Dictionary<string, object> firstChild = (Dictionary<string, object>)rootChildren[0];
            List<object> innerChildren = (List<object>)firstChild["children"];
            innerChildren.AddRange(arrayChildren);

            string json = "GenericPropertyJSON:" + JsonSerializerInternal.Serialize(mainObj);
            return json;
        }

        internal static string ConvertElementsToJson(string targetFieldName, ElementIndexName[] array)
        {
            Dictionary<string, object> mainObj = new Dictionary<string, object>
            {
                ["name"] = targetFieldName,
                ["type"] = -1,
                ["arraySize"] = array.Length,
                ["arrayType"] = nameof(ElementIndexName),
                ["children"] = new List<object>
                {
                    new Dictionary<string, object>
                    {
                        ["name"] = "Array",
                        ["type"] = -1,
                        ["arraySize"] = array.Length,
                        ["arrayType"] = nameof(ElementIndexName),
                        ["children"] = new List<object>
                        {
                            new Dictionary<string, object>
                            {
                                ["name"] = "size",
                                ["type"] = 12,
                                ["val"] = array.Length
                            }
                        }
                    }
                }
            };

            List<object> arrayChildren = new List<object>();

            foreach (ElementIndexName element in array)
            {
                arrayChildren.Add(new Dictionary<string, object>
                {
                    ["name"] = "data",
                    ["type"] = -1,
                    ["children"] = new List<object>
                    {
                        new Dictionary<string, object>
                        {
                            ["name"] = "Index",
                            ["type"] = 0,
                            ["val"] = element.Index
                        },
                        new Dictionary<string, object>
                        {
                            ["name"] = "Name",
                            ["type"] = 3,
                            ["val"] = element.Name
                        }
                    }
                });
            }

            List<object> rootChildren = (List<object>)mainObj["children"];
            Dictionary<string, object> firstChild = (Dictionary<string, object>)rootChildren[0];
            List<object> innerChildren = (List<object>)firstChild["children"];
            innerChildren.AddRange(arrayChildren);

            string json = "GenericPropertyJSON:" + JsonSerializerInternal.Serialize(mainObj);
            return json;
        }
    }
}
