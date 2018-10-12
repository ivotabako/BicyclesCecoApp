/// There is also a Visual Studio NuGet Package available, see
/// https://get.cmtelecom.com/microsoft-dotnet-nuget-pack/

using System;
using System.Net;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace BicyclesCecoApp
{
    
        

    /// <summary>
    /// The gateway accepts both the XML amd JSON formats
    /// </summary>
    public interface ISmsMessageBuilder
    {

        /// <summary>
        /// Creates a string according to the technical requirements
        /// of the CM MT gateway for sending a simple SMS text message
        /// </summary>
        /// <param name="productToken">Your product token</param>
        /// <param name="sender">A sendername/shortcode the SMS message</param>
        /// <param name="message">The text to be sent</param>
        /// <param name="recipient">The recipient's MSISDN</param>
        /// <returns>A string according to the technical requirements of the CM MT gateway,
        /// based on the provided parameters</returns>
        string CreateMessage(Guid productToken,
            string sender,
            string recipient,
            string message);

        /// <summary>
        /// The XML and JSON gateways use different URLs
        /// </summary>
        /// <returns>The target URL of either the XML or JSON gateway</returns>
        string GetTargetUrl();

        /// <summary>
        /// The JSON gateway requires you to set the content type to "application/json"
        /// </summary>
        /// <returns>The string of the content type to be used in the HTTP header </returns>
        string GetContentType();
    }

    public class XmlSmsMessageBuilder : ISmsMessageBuilder
    {
        public string CreateMessage(Guid productToken,
                                    string sender,
                                    string recipient,
                                    string message)
        {
            return
               new XElement("MESSAGES",
                   new XElement("AUTHENTICATION",
                       new XElement("PRODUCTTOKEN", productToken)
               ),
               new XElement("MSG",
                   new XElement("FROM", sender),
                   new XElement("TO", recipient),
                   new XElement("BODY", message)
               )
            ).ToString();
        }

        public string GetContentType()
        { return "application/xml"; }

        public string GetTargetUrl()
        { return "https://sgw01.cm.nl/gateway.ashx"; }
    }

    /// <summary>
    /// For JSON string building we recommend the Newtonsoft JSON NuGet package
    /// Feel free to substitute with your own preference in .net JSON library
    /// </summary>
    public class JsonSmsMessageBuilder : ISmsMessageBuilder
    {
        public string CreateMessage(Guid productToken,
                            string sender,
                            string recipient,
                            string message)
        {
            return new JObject
            {
                ["Messages"] = new JObject
                {
                    ["Authentication"] = new JObject
                    {
                        ["ProductToken"] = productToken
                    },
                    ["Msg"] = new JArray {
                        new JObject {
                            ["From"] = sender,
                            ["To"] = new JArray {
                                new JObject { ["Number"] = recipient }
                            },
                            ["Body"] = new JObject {
                                ["Content"] = message
                            }
                        }
                    }
                }
            }.ToString();
        }

        public string GetContentType()
        { return "application/json"; }

        public string GetTargetUrl()
        { return "https://gw.cmtelecom.com/v1.0/message"; }
    }
}
