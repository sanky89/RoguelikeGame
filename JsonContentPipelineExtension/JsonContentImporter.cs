using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.IO;
using System.Text.Json;
using TImport = System.String;

namespace JsonContentPipelineExtension
{
    [ContentImporter(".json", DisplayName = "Json Importer", DefaultProcessor = nameof(JsonContentProcessor))]
    public class JsonContentImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            var json = File.ReadAllText(filename);
            ThrowIfNotValidJson(json);
            return json;
        }

        private static void ThrowIfNotValidJson(string json)
        {
            if(string.IsNullOrEmpty(json))
            {
                throw new InvalidContentException("Json string is empty");
            }

            try
            {
                _ = JsonDocument.Parse(json);
            }
            catch(Exception e)
            {
                throw new InvalidContentException($"Json is invalid. See inner exception {e}");
            }
        }
    }
}