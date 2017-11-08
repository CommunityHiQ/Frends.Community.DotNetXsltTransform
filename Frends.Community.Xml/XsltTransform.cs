using Frends.Tasks.Attributes;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Xsl;

namespace Frends.Community.Xml
{
    /// <summary>
    /// Input class
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Input XmlDocument or string.
        /// </summary>
        [DefaultDisplayType(DisplayType.Text)]
        public string Xml { get; set; }


        /// <summary>
        /// Xslt transform
        /// </summary>
        [DefaultDisplayType(DisplayType.Text)]
        public string Xslt { get; set; }
    }

    /// <summary>
    /// Xslt parameters class
    /// </summary>
    [DisplayName("Xslt parameters")]
    public class Parameters 
    {   

        /// <summary>
        /// Xslt parameter name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Xslt parameter value
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Result class
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Object{string}
        /// </summary>
        public string Xml { get; set; }

    }

    /// <summary>
    /// TransformData class
    /// </summary>
    public class TransformData
    {
        /// <summary>
        /// Xml task for xslt transforms.
        /// This task uses only .Net parser.
        /// </summary>
        /// <param name="input">xml document or string</param>
        /// <param name="XSLTParameters">Array of KeyValuePairs: Name and Value</param>
        /// <param name="cToken">Cancellation token</param>
        /// <returns>Object{string}</returns>
        public static Result XsltTransform(Input input, Parameters[] XSLTParameters, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            var result = new Result();
            var xmlString = "";

            if (input.Xml.GetType() == typeof(string))
            {
                xmlString = input.Xml;
            }
            else if (input.Xml.GetType() == typeof(XmlDocument))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(input.Xml);

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

            result.Xml = DotNetXsltTransform(xmlString, input.Xslt, XSLTParameters);
            return result;
        }
        private static String DotNetXsltTransform(string xmlString, string XSLT, Parameters[] XSLTParameters)
        {
            using (var stringReader = new StringReader(XSLT))
            {

                var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));

                using (XmlReader reader = XmlReader.Create(new StringReader(XSLT)), Inputdocument = XmlReader.Create(new StringReader(xmlString)))
                {
                    XsltSettings xslt_settings = new XsltSettings(true, true);

                    XslCompiledTransform myXslTransform = new XslCompiledTransform();
                    myXslTransform.Load(reader, xslt_settings, new XmlUrlResolver());

                    XsltArgumentList argsList = new XsltArgumentList();
                    if (XSLTParameters != null)

                        XSLTParameters.ToList().ForEach(x => argsList.AddParam(x.Name, "", x.Value));

                    MemoryStream memoryStream = new MemoryStream();
                    using (XmlWriter xmlTextWriter = XmlWriter.Create(memoryStream, myXslTransform.OutputSettings))
                    {
                        myXslTransform.Transform(Inputdocument, argsList, xmlTextWriter);
                        Encoding UTF8WithoutBom = myXslTransform.OutputSettings.Encoding; // Encoding.UTF8;

                        var output = UTF8WithoutBom.GetString(memoryStream.ToArray());
                        output = output.Replace("\n", Environment.NewLine);
                        output = output.Trim(new char[] { '\uFEFF' }); // removu utf-16 bom
                        return output;
                    }
                }
            }
        }
    }
}

