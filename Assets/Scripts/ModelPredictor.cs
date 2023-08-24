using NatML;
using NatML.Features;
using System.Threading.Tasks;
using UnityEngine;
using NatML.Types;

public class ModelPredictor : IMLPredictor<float[]>
{
    public static async Task<ModelPredictor> CreateFromFile(MLModelData data)
    {
        // Load edge model
        var model = await MLEdgeModel.Create(data);
        // Create predictor
        var predictor = new ModelPredictor(model);
        // Return predictor
        return predictor;
    }

    private MLEdgeModel model;

    private ModelPredictor(MLEdgeModel model)
    {
        this.model = model;
        // show the details of the current model
        Debug.Log(model);
    }

    public void Dispose()
    {

    }

    public float[] Predict(out int[] outputShape, params MLFeature[] inputs)
    {
        var input = inputs[0];
        // Predict
        var inputType = model.inputs[0] as MLImageType;
        var inputFeature = (input as IMLEdgeFeature).Create(inputType);
        var outputFeatures = model.Predict(inputFeature);

        var result = new MLArrayFeature<float>(outputFeatures[0]);
        outputShape = outputFeatures[0].shape;
        return result.ToArray();
    }

    public float[] Predict(params MLFeature[] inputs)
    {
        var input = inputs[0];
        // Predict
        var inputType = model.inputs[0] as MLImageType;
        var inputFeature = (input as IMLEdgeFeature).Create(inputType);
        var outputFeatures = model.Predict(inputFeature);

        var result = new MLArrayFeature<float>(outputFeatures[0]);

        return result.ToArray();
    }
}
