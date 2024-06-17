using Microsoft.Xna.Framework.Content;
using System;
using System.Text.Json;

namespace JsonContentPipeline
{
    public class JsonContentReader<T> : ContentTypeReader<T>
    {
        protected override T Read(ContentReader input, T existingInstance)
        {
            string json = input.ReadString();
            T result = JsonSerializer.Deserialize<T>(json);
            return result;
        }
    }
}
