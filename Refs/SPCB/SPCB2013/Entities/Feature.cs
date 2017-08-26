using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SPBrowser.Entities
{
    /// <summary>
    /// Represents a feature.
    /// </summary>
    public class Feature
    {
        /// <summary>
        /// The identifier for this feature.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Friendly display name of the feature used publicly in the user interface.
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// Internal (folder) name for the feature.
        /// </summary>
        public string InternalName { get; set; }
        
        /// <summary>
        /// Gets a value that indicates whether the Feature is hidden within the Microsoft SharePoint Foundation user interface.
        /// </summary>
        public bool Hidden { get; set; }
        
        /// <summary>
        /// Gets the scope of this feature.
        /// </summary>
        public Scope Scope { get; set; }
        
        /// <summary>
        /// Is this an user defined (custom) feature definition, stored in the FeatureDefinition.xml
        /// </summary>
        [XmlIgnore]
        public bool IsCustomDefinition { get; set; }
    }

    /// <summary>
    /// Represents a collection of <see cref="Feature"/>.
    /// </summary>
    public class FeatureCollection : List<Feature>
    {
        /// <summary>
        /// Loads user defined features from custom FeatureDefinition.xml
        /// </summary>
        public void Load()
        {
            this.AddRange(OpenAndRead(Constants.CUSTOM_FEATURES_FILENAME));
        }

        /// <summary>
        /// Saves user defined features from custom FeatureDefinition.xml
        /// </summary>
        public void Save()
        {
            Write(Constants.CUSTOM_FEATURES_FILENAME, this);
        }

        private static FeatureCollection OpenAndRead(string fileName)
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

            foreach (Feature feature in features)
            {
                feature.IsCustomDefinition = true;
            }

            return features;
        }

        private static void Write(string fileName, FeatureCollection features)
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

    /// <summary>
    /// Feature scope
    /// </summary>
    public enum Scope
    {
        Farm,
        WebApplication,
        Site,
        Web
    }
}
