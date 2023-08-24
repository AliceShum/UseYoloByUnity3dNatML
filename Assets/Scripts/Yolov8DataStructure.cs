using UnityEngine;

public class Yolov8DataStructure
{
    public Vector2 boxPos;
    public Vector2 boxSize;
    public float boxReliability;

    public Vector2[] pointPosArr;
    public float[] pointReliabilityArr;

    public override string ToString()
    {
        return string.Format(
            "boxPos: " + boxPos.x + "_" + boxPos.y + "\n"
            + "boxSize:" + boxSize.x + "_" + boxSize.y + "\n"
            + "boxReliability:" + boxReliability + "\n"
            + "pointPosArr 1: " + pointPosArr[0].x + "_" + pointPosArr[0].y + "   " + pointReliabilityArr[0] + "\n"
            + "pointPosArr 2: " + pointPosArr[1].x + "_" + pointPosArr[1].y + "   " + pointReliabilityArr[1] + "\n"
            + "pointPosArr 3: " + pointPosArr[2].x + "_" + pointPosArr[2].y + "   " + pointReliabilityArr[2] + "\n"
            + "pointPosArr 4: " + pointPosArr[3].x + "_" + pointPosArr[3].y + "   " + pointReliabilityArr[3] + "\n"
            + "pointPosArr 5: " + pointPosArr[4].x + "_" + pointPosArr[4].y + "   " + pointReliabilityArr[4] + "\n"
            + "pointPosArr 6: " + pointPosArr[5].x + "_" + pointPosArr[5].y + "   " + pointReliabilityArr[5] + "\n"
            + "pointPosArr 7: " + pointPosArr[6].x + "_" + pointPosArr[6].y + "   " + pointReliabilityArr[6] + "\n"
            + "pointPosArr 8: " + pointPosArr[7].x + "_" + pointPosArr[7].y + "   " + pointReliabilityArr[7] + "\n"
            + "pointPosArr 9: " + pointPosArr[8].x + "_" + pointPosArr[8].y + "   " + pointReliabilityArr[8] + "\n"
            + "pointPosArr 10: " + pointPosArr[9].x + "_" + pointPosArr[9].y + "   " + pointReliabilityArr[9] + "\n");
    }
}
