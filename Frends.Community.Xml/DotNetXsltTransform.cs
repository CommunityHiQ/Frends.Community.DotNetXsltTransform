using Frends.Tasks.Attributes;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

#pragma warning disable 1591

namespace Frends.Community.DotNetXsltTransform
{
    public class TransformInput
    {
        /// <summary>
        /// Input xml Document as XmlDocument or xml string.
        /// </summary>
        [DefaultDisplayType(DisplayType.Text)]
        public string Document { get; set; }


        /// <summary>
        /// Xsl style sheet for transform as string.
        /// </summary>
        [DefaultDisplayType(DisplayType.Text)]
        public string Stylesheet { get; set; }
    }

    [DisplayName("Xslt parameters")]
    public class TransformParameters
    {

        /// <summary>
        /// Xslt parameter Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Xslt parameter Value
        /// </summary>
        public string Value { get; set; }
    }

    public class TransformResult
    {
        /// <summary>
        /// String that contains output from transformation. It might be xml, csv or something else, depending on transformation
        /// </summary>
        public string Result { get; set; }

    }

    public class TransformData
    {
        /// <summary>
        /// Task for xslt transforms using .Net.
        /// This task supports only xslt 1.0 transforms, but it supports C# scripts inside xsl file. If you don't need support for it is propably better to use Xml.Transform task.
        /// </summary>
        /// <param name="input">xml Document or string</param>
        /// <param name="xsltParameters">Array of KeyValuePairs: Name and Value</param>
        /// <returns>Object{string}</returns>
        public static TransformResult DotNetXsltTransform(TransformInput input, TransformParameters[] xsltParameters)
        {

            var result = new TransformResult();
            var xmlString = "";

            if (input.Document.GetType() == typeof(string))
            {
                xmlString = input.Document;
            }
            else if (input.Document.GetType() == typeof(XmlDocument))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(input.Document);

                using (var stringWriter = new StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    xmlDoc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    xmlString = stringWriter.GetStringBuilder().ToString();
                }
            }
            else
            {
                throw new FormatException("Unsupported input type. The supported types are XmlDocument and String.");
            }

            result.Result = DotNetXsltTransformHelper(xmlString, input.Stylesheet, xsltParameters);
            return result;
        }
        private static string DotNetXsltTransformHelper(string xmlString, string xslt, TransformParameters[] xsltParameters)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(xslt)), inputDocument = XmlReader.Create(new StringReader(xmlString)))
            {
                var xsltSettings = new XsltSettings(true, true);

                var myXslTransform = new XslCompiledTransform();
                myXslTransform.Load(reader, xsltSettings, new XmlUrlResolver());

                var argsList = new XsltArgumentList();
                if (xsltParameters != null)

                    xsltParameters.ToList().ForEach(x => argsList.AddParam(x.Name, "", x.Value));

                using (var memoryStream = new MemoryStream())
                using (var xmlTextWriter = XmlWriter.Create(memoryStream, myXslTransform.OutputSettings))
                {
                    myXslTransform.Transform(inputDocument, argsList, xmlTextWriter);
                    var utf8WithoutBom = myXslTransform.OutputSettings.Encoding; // Encoding.UTF8;

                    var output = utf8WithoutBom.GetString(memoryStream.ToArray());
                    output = output.Replace("\n", Environment.NewLine);
                    output = output.Trim(new char[] { '\uFEFF' }); // removu utf-16 bom
                    return output;

                }
            }
        }
    }
}

