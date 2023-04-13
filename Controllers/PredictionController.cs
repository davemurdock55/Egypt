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

            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });

            Tensor<string> score = result.First().AsTensor<string>();

            var prediction = new PredictionOutput { PredictedHeadDirection = score.First().ToString() };

            result.Dispose();

            return Ok(prediction);
        }
    }
}