using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Egypt.Models;
using Microsoft.ML.TensorFlow;


namespace aspnetcore.Controllers
{
    [ApiController]
    [Route("/predict")]
    public class PredictionController : ControllerBase
    {
        private InferenceSession _session;

        public PredictionController(InferenceSession session)
        {
            _session = session;
        }

        [HttpPost]
        public ActionResult Predict(InputData data)
        {

                //var tensor = new DenseTensor<float>(new[] { 1, data.Length });

                ////tensor.CopyFrom(data);

                //var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor("input", tensor) };
                //var results = _session.Run(inputs);

                //var output = results.First().AsTensor<string>();
                //string predictedHeadDirection = output.GetValue(0);

                //var prediction = new PredictionOutput { PredictedHeadDirection = predictedHeadDirection };

                //return Ok(prediction);



            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });

            Tensor<float> score = result.First().AsTensor<float>();

            var prediction = new PredictionOutput { PredictedHeadDirection = score.First().ToString() };

            // Assuming the output name is "predicted_head_direction"
            //var output = predictionResult.Outputs["predicted_head_direction"];
            //string predictedHeadDirection = output.AsEnumerable<string>().First();
            //var prediction = new PredictionOutput { PredictedHeadDirection = predictedHeadDirection };

            result.Dispose();

            return Ok(prediction);
        }
    }
}