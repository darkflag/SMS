using System;
using System.Text;
using System.Xml.Linq;
using System.Drawing;

namespace SPBrowser.Controls
{
    public class XMLViewer : System.Windows.Forms.RichTextBox
    {
        /// <summary>
        /// Gets and sets the XML font size.
        /// </summary>
        public int XmlFontSize
        {
            get { return _xmlFontSize; }
            set { _xmlFontSize = value; }
        }
        private int _xmlFontSize = 18;

        /// <summary>
        /// Gets and sets the indent.
        /// </summary>
        public int Indent
        {
            get { return _indent; }
            set { _indent = value; }
        }
        private int _indent = 2;

        /// <summary>
        /// The format settings.
        /// </summary>
        public XMLViewerSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = new XMLViewerSettings
                    {
                        AttributeKey = Color.Red,
                        AttributeValue = Color.Blue,
                        Tag = Color.Blue,
                        Element = Color.DarkRed,
                        Value = Color.Black,
                    };
                }
                return settings;
            }
            set
            {
                settings = value;
            }
        }
        private XMLViewerSettings settings;

        /// <summary>
        /// Convert the Xml to Rtf with some formats specified in the XMLViewerSettings,
        /// and then set the Rtf property to the value.
        /// </summary>
        /// <param name="includeDeclaration">
        /// Specify whether include the declaration.
        /// </param>
        public void Process(bool includeDeclaration)
        {
            try
            {
                // The Rtf contains 2 parts, header and content. The colortbl is a part of
                // the header, and the {1} will be replaced with the content.
                string rtfFormat = @"{{\rtf1\ansi\ansicpg1252\deff0\deflang1033\deflangfe2052
{{\fonttbl{{\f0\fnil Segoe UI;}}}}
{{\colortbl ;{0}}}
\viewkind4\uc1\pard\lang1033\f0 
{1}}}";

                // Get the XDocument from the Text property.
                var xmlDoc = XDocument.Parse(this.Text, LoadOptions.None);

                StringBuilder xmlRtfContent = new StringBuilder();

                // Set font size
                xmlRtfContent.AppendFormat(@"\fs{0}", _xmlFontSize);

                // If includeDeclaration is true and the XDocument has declaration,
                // then add the declaration to the content.
                if (includeDeclaration && xmlDoc.Declaration != null)
                {

                    // The constants in XMLViewerSettings are used to specify the order 
                    // in colortbl of the Rtf.
                    xmlRtfContent.AppendFormat(@"
\cf{0} <?\cf{1} xml \cf{2} version\cf{0} =\cf0 ""\cf{3} {4}\cf0 "" 
\cf{2} encoding\cf{0} =\cf0 ""\cf{3} {5}\cf0 ""\cf{0} ?>\par",
                        XMLViewerSettings.TagID,
                        XMLViewerSettings.ElementID,
                        XMLViewerSettings.AttributeKeyID,
                        XMLViewerSettings.AttributeValueID,
                        xmlDoc.Declaration.Version,
                        xmlDoc.Declaration.Encoding);
                }

                // Get the Rtf of the root element.
                string rootRtfContent = ProcessElement(xmlDoc.Root, 0);

                xmlRtfContent.Append(rootRtfContent);

                // Construct the completed Rtf, and set the Rtf property to this value.
                this.Rtf = string.Format(rtfFormat, Settings.ToRtfFormatString(),
                    xmlRtfContent.ToString());
            }
            catch (System.Xml.XmlException xmlException)
            {
                throw new ApplicationException(
                    "Please check the input Xml. Error:" + xmlException.Message,
                    xmlException);
            }
            catch
            {
                throw;
            }
        }

        // Get the Rtf of the xml element.
        private string ProcessElement(XElement element, int level)
        {
            // This viewer does not support the Xml file that has Namespace.
            if (!string.IsNullOrEmpty(element.Name.Namespace.NamespaceName))
            {
                // TODO: Support Content Type with syntax highlighting
                throw new XmlViewerNamespaceException();
            }

            string elementRtfFormat = string.Empty;
            StringBuilder childElementsRtfContent = new StringBuilder();
            StringBuilder attributesRtfContent = new StringBuilder();

            // Construct the indent.
            string indent = new string(' ', _indent * level);

            // If the element has child elements or value, then add the element to the 
            // Rtf. {{0}} will be replaced with the attributes and {{1}} will be replaced
            // with the child elements or value.
            if (element.HasElements || !string.IsNullOrWhiteSpace(element.Value))
            {
                elementRtfFormat = string.Format(@"
{0}\cf{1} <\cf{2} {3}{{0}}\cf{1} >\par
{{1}}
{0}\cf{1} </\cf{2} {3}\cf{1} >\par",
                    indent,
                    XMLViewerSettings.TagID,
                    XMLViewerSettings.ElementID,
                    element.Name);

                // Construct the Rtf of child elements.
                if (element.HasElements)
                {
                    foreach (var childElement in element.Elements())
                    {
                        string childElementRtfContent =
                            ProcessElement(childElement, level + 1);
                        childElementsRtfContent.Append(childElementRtfContent);
                    }
                }

                // If !string.IsNullOrWhiteSpace(element.Value), then construct the Rtf 
                // of the value.
                else
                {
                    childElementsRtfContent.AppendFormat(@"{0}\cf{1} {2}\par",
                        new string(' ', _indent * (level + 1)),
                        XMLViewerSettings.ValueID,
                        CharacterEncoder.Encode(element.Value.Trim()));
                }
            }

            // This element only has attributes. {{0}} will be replaced with the attributes.
            else
            {
                elementRtfFormat =
                    string.Format(@"
{0}\cf{1} <\cf{2} {3}{{0}}\cf{1} />\par",
                    indent,
                    XMLViewerSettings.TagID,
                    XMLViewerSettings.ElementID,
                    element.Name);
            }

            // Construct the Rtf of the attributes.
            if (element.HasAttributes)
            {
                foreach (XAttribute attribute in element.Attributes())
                {
                    string attributeRtfContent = string.Format(
                        @" \cf{0} {3}\cf{1} =\cf0 ""\cf{2} {4}\cf0 """,
                        XMLViewerSettings.AttributeKeyID,
                        XMLViewerSettings.TagID,
                        XMLViewerSettings.AttributeValueID,
                        attribute.Name,
                       CharacterEncoder.Encode(attribute.Value));
                    attributesRtfContent.Append(attributeRtfContent);
                }
                attributesRtfContent.Append(" ");
            }

            return string.Format(elementRtfFormat, attributesRtfContent,
                childElementsRtfContent);
        }
    }

    /// <summary>
    /// This exception is raised when the XML to parse contains a XML namespace.
    /// </summary>
    public class XmlViewerNamespaceException : ApplicationException
    {
        public XmlViewerNamespaceException() : base("This viewer does not support the Xml file that has Namespace.")
        {}
    }
}
