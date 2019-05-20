using Core.Model;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Core.Domain.Logic.EmailGeneration
{
    public abstract class HtmlBase
    {
        protected const string AbstractTemplateName = "None";
        public abstract string HtmlTemplateName { get; }
        public string HtmlTemplate { get; protected set; }
        public Dictionary<string, dynamic> DataDictionary { get; protected set; } = new Dictionary<string, dynamic>();

        public HtmlBase()
        {
            HtmlTemplate = GetEmailTemplate();
        }

        public string GetEmailTemplate()
        {
            if (string.Equals(HtmlTemplateName, AbstractTemplateName))
                return String.Empty;

            var template = GetEmailTemplate(HtmlTemplateName);

            return template;
        }

        public static string GetEmailTemplate(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                return result;
            }
        }

        public string CombineHtmlWithData()
        {
            var sb = new StringBuilder(HtmlTemplate);

            foreach (var key in DataDictionary.Keys)
            {
                if (!(DataDictionary[key] is string)) continue;
                sb.Replace($"[[{key}]]", DataDictionary[key]);
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(sb.ToString());

            foreach (var key in DataDictionary.Keys)
            {
                var iEnumerableType = typeof(IEnumerable);
                var genericListType = DataDictionary[key].GetType();
                if (!iEnumerableType.IsAssignableFrom(genericListType)) continue;
                var nodes = doc.DocumentNode.SelectNodes($@"//*[@data-repeater='{key}']");

                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        var parent = nodes[0].ParentNode;
                        var oddCounter = 1;
                        foreach (var value in DataDictionary[key])
                        {
                            var cloned = node.Clone();
                            cloned.AddClass((oddCounter % 2) == 1 ? "row-odd" : "row-even");
                            oddCounter++;

                            if (value == null) continue;

                            var type = value.GetType() as Type;
                            var props = type.GetProperties();
                            foreach (var property in props)
                            {
                                var formater = property.GetCustomAttribute<Format>();
                                var objValue = property.GetValue(value);
                                var valueAsString = string.Empty;

                                if (formater != null && objValue != null)
                                {
                                    valueAsString = formater.GetFormatedString(objValue);
                                }
                                else
                                {
                                    valueAsString = objValue?.ToString();
                                }

                                cloned.InnerHtml = cloned.InnerHtml.Replace($"[[{property.Name}]]", valueAsString);
                            }
                            parent.InsertBefore(cloned, node);
                        }

                        parent.RemoveChild(node);
                    }
                }
            }

            return doc.DocumentNode.OuterHtml;
        }
    }
}
