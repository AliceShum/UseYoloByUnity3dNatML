using UnityEngine;
using NatML;
using NatML.Features;

public class PredictSinglePicture : MonoBehaviour
{
    // the shape of the input data to the predictor
    //int[] INPUT_SHAPE = { 1, 3, 640, 640 };

    public MLModelData model_android;
    public MLModelData model_windows;

    private ModelPredictor predictor;

    //private MLArrayFeature<float> databuffer;

    public Texture2D inputTex;

    void Start()
    {
        //databuffer = new MLArrayFeature<float>(new float[INPUT_SHAPE[0] * INPUT_SHAPE[1] * INPUT_SHAPE[2] * INPUT_SHAPE[3]], INPUT_SHAPE);

        PredictPic();
    }

    async void PredictPic()
    {
        if (Application.isEditor)
        {
            predictor = await ModelPredictor.CreateFromFile(model_windows);
        }
        else
        {
            predictor = await ModelPredictor.CreateFromFile(model_android);
        }

        UsePredictorAndShowResult();
    }

    void UsePredictorAndShowResult()
    {
        if (predictor == null) return;

        MLImageFeature input = new MLImageFeature(inputTex);

        int[] outputShape = new int[] { };
        float[] outputs = predictor.Predict(out outputShape, input);

        Yolov8DataStructure data = SortData1(outputs, outputShape);
        GetComponent<ShowResult>().Show(data);
    }

    //全部数据整理一遍，对比可信度，得到最高可信度的数据
    void SortData(float[] outputs, int[] outputShape)
    {
        int batch = outputShape[0];  //1个batch
        int channels = outputShape[1];  // 35个channel
        int numOfData = outputShape[2];  //8400个数据
        int numOfAcupoints = 10; //多少个点

        float maxRealiability = 0;//最高的可信度
        int maxReliabilityIndex = -1; //在8400个数据中，最高可信度的数据index

        Yolov8DataStructure[] data = new Yolov8DataStructure[numOfData];
        for (int j = 0; j < numOfData; j++)
        {
            //UnityEngine.Debug.Log("================  " + j);
            data[j] = new Yolov8DataStructure();
            data[j].boxPos = new Vector2(outputs[numOfData * (1 - 1) + j], outputs[numOfData * (2 - 1) + j]);
            data[j].boxSize = new Vector2(outputs[numOfData * (3 - 1) + j], outputs[numOfData * (4 - 1) + j]);
            data[j].boxReliability = outputs[numOfData * (5 - 1) + j];

            data[j].pointPosArr = new Vector2[numOfAcupoints];
            data[j].pointReliabilityArr = new float[numOfAcupoints];
            for (int k = 0; k < numOfAcupoints; k++)
            {
                int channelIndex = 6 + 3 * k;
                int acupointXValueIndex = numOfData * (channelIndex - 1) + j;
                int acupointYValueIndex = numOfData * channelIndex + j;
                int acupointRealibilityIndex = numOfData * (channelIndex + 1) + j;

                data[j].pointPosArr[k] = new Vector2(outputs[acupointXValueIndex], outputs[acupointYValueIndex]);
                data[j].pointReliabilityArr[k] = outputs[acupointRealibilityIndex];
            }

            if (data[j].boxReliability > maxRealiability)
            {
                maxRealiability = data[j].boxReliability;
                maxReliabilityIndex = j;
            }
        }

        UnityEngine.Debug.Log("最高可信度的数据：" + data[maxReliabilityIndex].ToString());
    }

    //取出全部可信度数据，取可信度最高的那一个数据
    Yolov8DataStructure SortData1(float[] outputs, int[] outputShape)
    {
        int batch = outputShape[0];  //1个batch 
        int channels = outputShape[1];  // 35个channel
        int numOfData = outputShape[2];  //8400个数据
        int numOfAcupoints = 10; //多少个点

        float maxRealiability = 0; //最高的可信度
        int maxReliabilityIndex = -1; //在8400个数据中，最高可信度的数据index

        float[] realibilityArr = new float[numOfData];
        for (int j = 0; j < numOfData; j++)
        {
            //UnityEngine.Debug.Log("================  " + j);
            realibilityArr[j] = outputs[numOfData * (5 - 1) + j];

            if (realibilityArr[j] > maxRealiability)
            {
                maxRealiability = realibilityArr[j];
                maxReliabilityIndex = j;
            }
        }

        Yolov8DataStructure data = new Yolov8DataStructure();

        data.boxPos = new Vector2(outputs[numOfData * (1 - 1) + maxReliabilityIndex], outputs[numOfData * (2 - 1) + maxReliabilityIndex]);
        data.boxSize = new Vector2(outputs[numOfData * (3 - 1) + maxReliabilityIndex], outputs[numOfData * (4 - 1) + maxReliabilityIndex]);
        data.boxReliability = outputs[numOfData * (5 - 1) + maxReliabilityIndex];

        data.pointPosArr = new Vector2[numOfAcupoints];
        data.pointReliabilityArr = new float[numOfAcupoints];
        for (int k = 0; k < numOfAcupoints; k++)
        {
            int channelIndex = 6 + 3 * k;
            int acupointXValueIndex = numOfData * (channelIndex - 1) + maxReliabilityIndex;
            int acupointYValueIndex = numOfData * channelIndex + maxReliabilityIndex;
            int acupointRealibilityIndex = numOfData * (channelIndex + 1) + maxReliabilityIndex;

            data.pointPosArr[k] = new Vector2(outputs[acupointXValueIndex], outputs[acupointYValueIndex]);
            data.pointReliabilityArr[k] = outputs[acupointRealibilityIndex];
        }


        UnityEngine.Debug.Log(data.ToString());

        return data;
    }
}
