using BERTTokenizers;
using Microsoft.ML.OnnxRuntime;

namespace RecSysConverter.VideoEncoder
{ 
    public struct BertInput
    {
        public long[] InputIds { get; set; }
        public long[] AttentionMask { get; set; }
        public long[] TypeIds { get; set; }
    }

    public class Encoder
        : IDisposable
    {
        private const string ModelPath = @"Model/model_optimized.onnx";
        private readonly RunOptions _runOptions;
        private readonly InferenceSession _session;
        private readonly BertUncasedLargeTokenizer _tokenizer;
        public Encoder()
        {
            _runOptions = new RunOptions();
            // Get path to model to create inference session.
            _session = new InferenceSession(ModelPath);
            // Create Tokenizer and tokenize the sentence.
            _tokenizer = new BertUncasedLargeTokenizer();
        }

        public void Dispose()
        {
            _runOptions.Dispose();
            _session.Dispose();
        }

        public float[] Encode(string sentence)
        {
            // Get the sentence tokens.
            var tokens = _tokenizer.Tokenize(sentence);
            // Encode the sentence and pass in the count of the tokens in the sentence.
            var encoded = _tokenizer.Encode(tokens.Count(), sentence);
            // Break out encoding to InputIds, AttentionMask and TypeIds from list of (input_id, attention_mask, type_id).
            var bertInput = new BertInput()
            {
                InputIds = encoded.Select(t => t.InputIds).ToArray(),
                AttentionMask = encoded.Select(t => t.AttentionMask).ToArray(),
                TypeIds = encoded.Select(t => t.TokenTypeIds).ToArray(),
            };
            // Create input tensors over the input data.
            using var inputIdsOrtValue = OrtValue.CreateTensorValueFromMemory(bertInput.InputIds,
                  new long[] { 1, bertInput.InputIds.Length });
            using var attMaskOrtValue = OrtValue.CreateTensorValueFromMemory(bertInput.AttentionMask,
                  new long[] { 1, bertInput.AttentionMask.Length });
            using var typeIdsOrtValue = OrtValue.CreateTensorValueFromMemory(bertInput.TypeIds,
                  new long[] { 1, bertInput.TypeIds.Length });
            // Create input data for session. Request all outputs in this case.
            var inputs = new Dictionary<string, OrtValue>
                {
                    { "input_ids", inputIdsOrtValue },
                    { "attention_mask", attMaskOrtValue },
                    { "token_type_ids", typeIdsOrtValue }
                };
            long batchCount = 1;
            long sequienceLength = 4;
            long vectorSize = 312;

            float[] embedding = new float[312];
            using (var output = _session.Run(_runOptions, inputs, _session.OutputNames))
            {
                var shape = output[0].GetTensorTypeAndShape().Shape;
                batchCount = shape[0];
                sequienceLength = shape[1];
                vectorSize = shape[2];

                var vector = output[0].GetTensorDataAsSpan<float>();
                for (var b = 0; b < batchCount; b++)
                {
                    for (var i = 0; i < vectorSize; i++)
                    {
                        int index = (int)(b * sequienceLength * 312 + i);
                        embedding[i] += vector[index];
                    }
                }
            }

            return embedding;
        }
    }
}
