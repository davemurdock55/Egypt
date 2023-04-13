using System;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace Egypt.Models
{
	public class InputData
	{

		public float SquareNorthSouthInput { get; set; }
        public float SquareEastWestInput { get; set; }
        public float DepthInput { get; set; }
        public float SouthToHeadInput { get; set; }
        public float WestToHeadInput { get; set; }
        public float LengthInput { get; set; }
        public float BurialNumberInput { get; set; }
        public float WestToFeetInput { get; set; }
        public float SouthToFeetInput { get; set; }
        public float WrappingInput { get; set; } //	we want to know if the wrapping is "w" (this is a boolean, but a float back here, I guess :P)

        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
                SquareNorthSouthInput, SquareEastWestInput, DepthInput, SouthToHeadInput,
                WestToHeadInput, LengthInput, BurialNumberInput, WestToFeetInput,
                SouthToFeetInput, WrappingInput
            };

            int[] dimensions = new int[] { 1, 10 };

            return new DenseTensor<float>(data, dimensions);
        }
    }
}

