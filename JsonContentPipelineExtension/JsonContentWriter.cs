using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TInput = JsonContentPipelineExtension.JsonContentResult;

namespace JsonContentPipelineExtension
{


    [ContentTypeWriter]
    internal class JsonContentWriter : ContentTypeWriter<TInput>
    {
        private string _runtimeIdentifier;
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return _runtimeIdentifier;
        }

        protected override void Write(ContentWriter output, TInput value)
        {
            _runtimeIdentifier = value.RuntimeType;
            output.Write(value.Json);
        }
    }
}
