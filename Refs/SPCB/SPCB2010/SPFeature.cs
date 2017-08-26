using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SPBrowser
{
    public class SPFeature
    {
        public Guid Id { get; set; }
        
        public string DisplayName { get; set; }
        
        public string InternalName { get; set; }
        
        public bool Hidden { get; set; }
        
        public Scope Scope { get; set; }
        
        [XmlIgnore]
        public bool IsCustomDefinition { get; set; }
    }

    public class FeatureCollection : List<SPFeature>
    {
        public void Load()
        {
            this.AddRange(OpenAndRead(Constants.CUSTOM_FEATURES_FILENAME));
        }

        public void Save()
        {
            Write(Constants.CUSTOM_FEATURES_FILENAME, this);
        }

        public static FeatureCollection OpenAndRead(string fileName)
        {
            FeatureCollection features = new FeatureCollection();

            if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
            {
                // Create the serializer
                var serializer = new XmlSerializer(typeof(FeatureCollection));

                // Open config file
                using (var stream = new System.IO.StreamReader(fileName))
                {
                    // De-serialize the XML
                    features = serializer.Deserialize(stream) as FeatureCollection;
                }
            }

            foreach (SPFeature feature in features)
            {
                feature.IsCustomDefinition = true;
            }

            return features;
        }

        public static void Write(string fileName, FeatureCollection features)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                // Create the serializer
                var serializer = new XmlSerializer(typeof(FeatureCollection));

                // Open config file
                using (var stream = new System.IO.StreamWriter(fileName))
                {
                    // Serialize the XML
                    serializer.Serialize(stream, features);
                }
            }
        }
    }

    public enum Scope
    {
        Farm,
        WebApplication,
        Site,
        Web
    }
}
