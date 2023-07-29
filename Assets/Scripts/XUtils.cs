using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
public static class XUtils
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }
    }

    public static void ImportImageData(ref List<float[,,]> trainingInputData, ref List<float[]> trainingOutputData, List<Texture2D[]> imageDataset, bool grayscale) {
        int num = Konstanta.REAL_OUTPUT_SIZE;
        trainingInputData = new List<float[,,]>();
        trainingOutputData = new List<float[]>();
        Debug.Log(imageDataset[0][0]);
        Debug.Log(imageDataset[0][0].width);
        // int num2 = imageDataset[0][0].width;
        // int num3 = imageDataset[0][0].height;
        Debug.Log("countdidalam:"+imageDataset.Count);
        int index = 0;
        while (index < imageDataset.Count)
        {
            int num5 = 0;
            while (true)
            {
                if (num5 >= imageDataset[index].Length)
                {
                    index++;
                    break;
                }
                float[,,] predictionInputData = new float[1, 1, 1];
                // ImportImageData(ref predictionInputData, imageDataset[index][num5], grayscale);
                Noedify_Utils.ImportImageData(ref predictionInputData, imageDataset[index][num5], grayscale);
                float[] item = new float[num];
                item[index] = 1f;
                trainingOutputData.Add(item);
                trainingInputData.Add(predictionInputData);
                num5++;
            }
        }
    }

    public static Tensor TransformInput3(Texture2D tex)  {
        float[,,,] inputImg = new float[1,32,32,1];
        for (int i = 0; i < 32; i++) {
			for (int j = 0; j < 32; j++) {
				inputImg [0, 32 - i - 1, j, 0] = tex.GetPixel(j, i).r;
			}
		}
        return new Tensor(1, 32, 32, 1, inputImg);
    }
}