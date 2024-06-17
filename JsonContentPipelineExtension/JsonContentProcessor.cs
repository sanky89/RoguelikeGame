using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.ComponentModel;
using TInput = System.String;
using TOutput = JsonContentPipelineExtension.JsonContentResult;

namespace JsonContentPipelineExtension
{
    [ContentProcessor(DisplayName = "Json Processor")]
    internal class JsonContentProcessor : ContentProcessor<TInput, TOutput>
    {
        [DisplayName("Runtime Type")]
        public string RuntimeType { get; set; } = string.Empty;

        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            if(string.IsNullOrEmpty(RuntimeType))
            {
                throw new Exception("Runtime type not specified");
            }

            JsonContentResult result = new()
            {
                Json = input,
                RuntimeType = RuntimeType
            };

            return result;
        }
    }
}