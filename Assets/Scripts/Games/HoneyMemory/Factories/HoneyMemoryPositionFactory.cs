using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyMemoryPositionFactory : PositionFactory
{
    public void GetNestsInitialPosition(int numberOfNests, int numberOfHoneys, float nestWidth, float nestHeight, float margin, int numberOfTargetedAreas, ref List<Vector3> targetedPositions, ref List<Vector3> allPositions)
    {
        if (allPositions == null)
            allPositions = new List<Vector3>();
        else
            allPositions.Clear();

        if (targetedPositions == null)
            targetedPositions = new List<Vector3>();
        else
            targetedPositions.Clear();

        int maxRow = 0;
        int maxCol = 0;
        Vector3 newPos;

        setGamePlayParameters();

        float width = Math.Abs(leftButtom.x - rightButtom.x);
        float height = Math.Abs(leftUp.y - leftButtom.y);
        bool landscape = width > height;

        int MaxNumberOfNests = 64;
        if (landscape)
        {
            while (maxRow * maxCol < MaxNumberOfNests)
            {
                maxCol = Convert.ToInt32(width * ++maxRow/height);
            }
        }
        else
        {
            while (maxRow * maxCol < MaxNumberOfNests)
            {
                maxRow = Convert.ToInt32(height * ++maxCol / width);
            }
        }

        NestsManager.Instance.hiddenDataEncoder.colNum = maxCol;
        NestsManager.Instance.hiddenDataEncoder.rowNum = maxRow;
        NestsManager.Instance.hiddenDataEncoder.nestsCI.Clear();
        NestsManager.Instance.hiddenDataEncoder.nestsRI.Clear();
        NestsManager.Instance.hiddenDataEncoder.nestsType.Clear();

        List<List<int[]>> row_col_MatrixOptions = new List<List<int[]>>();
        //maxRow = landscape ? maxRow : maxRow + 1;
        //maxCol = landscape ? maxCol + 1 : maxCol;
        for (int i = 0; i < maxRow; i++)
        {
            row_col_MatrixOptions.Add(new List<int[]>());
            for (int j = 0; j < maxCol; j++)
            {
                row_col_MatrixOptions[i].Add(new int[2]);
                row_col_MatrixOptions[i][j][0] = i;
                row_col_MatrixOptions[i][j][1] = j;
            }
        }

        float t = (float)Math.Sqrt(3) / 2;

        float x_loc = -((maxCol - 1) * (nestWidth + margin)) / 2f + center.x;
        float y_loc = ((maxRow - 1)  * (((nestHeight + (t / 2f) * nestHeight) / 2f) + margin)) / 2f + center.y;

        List<int> TargetedAreas = new List<int>();

        /*if ((numberOfTargetedAreas == 1 && numberOfNests > 15) || (numberOfTargetedAreas == 2 && numberOfNests > 30) || (numberOfTargetedAreas == 3 && numberOfNests > 45) || (numberOfTargetedAreas == 4 && numberOfNests > 60))
        {
            numberOfTargetedAreas = numberOfNests / 15;
        }*/

        numberOfTargetedAreas = (numberOfTargetedAreas > 4) ? 4 : numberOfTargetedAreas;
        numberOfTargetedAreas = (numberOfTargetedAreas < 1) ? 1 : numberOfTargetedAreas;

        //Debug.Log("TargetedAreas");
        for (int i = 0;i < numberOfTargetedAreas;i++)
        {
            int newAreaIndex = UnityEngine.Random.Range(0, 4);
            while (TargetedAreas.Contains(newAreaIndex))
                newAreaIndex = UnityEngine.Random.Range(0, 4);
            TargetedAreas.Add(newAreaIndex);
            //Debug.Log(newAreaIndex);
        }

        int randomRowIndex = 0, randomColIndex = 0;
        for (int i = 0; i < numberOfHoneys; i++)
        {
            int j = i % TargetedAreas.Count;
            switch (TargetedAreas[j])
            {
                case 0:
                    randomRowIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions.Count / 2);
                    randomColIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions[randomRowIndex].Count / 2);
                    break;

                case 1:
                    randomRowIndex = UnityEngine.Random.Range(row_col_MatrixOptions.Count / 2, row_col_MatrixOptions.Count);
                    randomColIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions[randomRowIndex].Count / 2);
                    break;

                case 2:
                    randomRowIndex = UnityEngine.Random.Range(row_col_MatrixOptions.Count / 2, row_col_MatrixOptions.Count);
                    randomColIndex = UnityEngine.Random.Range(row_col_MatrixOptions[randomRowIndex].Count / 2, row_col_MatrixOptions[randomRowIndex].Count);
                    break;

                case 3:
                    randomRowIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions.Count / 2);
                    randomColIndex = UnityEngine.Random.Range(row_col_MatrixOptions[randomRowIndex].Count / 2, row_col_MatrixOptions[randomRowIndex].Count);
                    break;

                default:
                    randomRowIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions.Count);
                    randomColIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions[randomRowIndex].Count);
                    break;
            }

            int chosenRow = row_col_MatrixOptions[randomRowIndex][randomColIndex][0];
            int chosenCol = row_col_MatrixOptions[randomRowIndex][randomColIndex][1];

            float newX = x_loc + chosenCol * (nestWidth + margin);
            float newY = y_loc - chosenRow * (((nestHeight + t / 2 * nestHeight) / 2) + margin);

            if (chosenRow % 2 == 1) newX -= (nestWidth + margin) / 2;

            row_col_MatrixOptions[randomRowIndex].RemoveAt(randomColIndex);
            if (row_col_MatrixOptions[randomRowIndex].Count <= 0) row_col_MatrixOptions.RemoveAt(randomRowIndex);

            NestsManager.Instance.hiddenDataEncoder.nestsCI.Add(chosenCol);
            NestsManager.Instance.hiddenDataEncoder.nestsRI.Add(chosenRow);
            NestsManager.Instance.hiddenDataEncoder.nestsType.Add(1);

            newPos = new Vector3(newX, newY, -10);
            allPositions.Add(newPos);
            targetedPositions.Add(newPos);

            bool condition1 = newPos.x > rightUp.x - nestWidth || newPos.x < leftUp.x + nestWidth;
            bool condition2 = newPos.y > leftUp.y - nestHeight || newPos.y < leftButtom.y + nestHeight;
            while ((condition1 || condition2))
            {
                CameraController.Instance.camera.orthographicSize += 0.1f;

                SetScreenParametersAfterChangingSizeOfCamera();

                condition1 = newPos.x > rightUp.x - nestWidth || newPos.x < leftUp.x + nestWidth;
                condition2 = newPos.y > leftUp.y - nestHeight || newPos.y < leftButtom.y + nestHeight;
            }
        }

        for (int i = 0; i < (numberOfNests - numberOfHoneys); i++)
        {
            switch (i % 4)
            {
                case 0:
                    randomRowIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions.Count / 2);
                    randomColIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions[randomRowIndex].Count / 2);
                    break;

                case 1:
                    randomRowIndex = UnityEngine.Random.Range(row_col_MatrixOptions.Count / 2, row_col_MatrixOptions.Count);
                    randomColIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions[randomRowIndex].Count / 2);
                    break;

                case 2:
                    randomRowIndex = UnityEngine.Random.Range(row_col_MatrixOptions.Count / 2, row_col_MatrixOptions.Count);
                    randomColIndex = UnityEngine.Random.Range(row_col_MatrixOptions[randomRowIndex].Count / 2, row_col_MatrixOptions[randomRowIndex].Count);
                    break;

                case 3:
                    randomRowIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions.Count / 2);
                    randomColIndex = UnityEngine.Random.Range(row_col_MatrixOptions[randomRowIndex].Count / 2, row_col_MatrixOptions[randomRowIndex].Count);
                    break;

                default:
                    randomRowIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions.Count);
                    randomColIndex = UnityEngine.Random.Range(0, row_col_MatrixOptions[randomRowIndex].Count);
                    break;
            }

            int chosenRow = row_col_MatrixOptions[randomRowIndex][randomColIndex][0];
            int chosenCol = row_col_MatrixOptions[randomRowIndex][randomColIndex][1];

            float newX = x_loc + chosenCol * (nestWidth + margin);
            float newY = y_loc - chosenRow * (((nestHeight + t / 2 * nestHeight) / 2) + margin);

            if (chosenRow % 2 == 1) newX -= (nestWidth + margin) / 2;

            row_col_MatrixOptions[randomRowIndex].RemoveAt(randomColIndex);
            if (row_col_MatrixOptions[randomRowIndex].Count <= 0) row_col_MatrixOptions.RemoveAt(randomRowIndex);

            NestsManager.Instance.hiddenDataEncoder.nestsCI.Add(chosenCol);
            NestsManager.Instance.hiddenDataEncoder.nestsRI.Add(chosenRow);
            NestsManager.Instance.hiddenDataEncoder.nestsType.Add(0);

            newPos = new Vector3(newX, newY, -10);
            allPositions.Add(newPos);

            bool condition1 = newPos.x > rightUp.x - nestWidth || newPos.x < leftUp.x + nestWidth;
            bool condition2 = newPos.y > leftUp.y - nestHeight || newPos.y < leftButtom.y + nestHeight;
            while ((condition1 || condition2))
            {
                CameraController.Instance.camera.orthographicSize += 0.1f;

                SetScreenParametersAfterChangingSizeOfCamera();

                condition1 = newPos.x > rightUp.x - nestWidth || newPos.x < leftUp.x + nestWidth;
                condition2 = newPos.y > leftUp.y - nestHeight || newPos.y < leftButtom.y + nestHeight;
            }
        }
    }

    /*
    public static List<Vector3> GetNestsInitialPosition2(int number, float nestWidth, float margin)
    {
        bool landscape = ScreenWidth > ScreenHeight;
        List<Vector3> positions = new List<Vector3>();

        int columnNumber = (int)Math.Ceiling(Math.Sqrt(number));
        float t = (float)Math.Sqrt(3) / 2;
        float nestHeight = (int)(t * nestWidth);
        int rowNumber = (int)Math.Ceiling((double)number / columnNumber);

        float rectWidth = (columnNumber - 1) * (nestWidth * 3 / 4 + margin);
        float rectHeight = (rowNumber - 1) * (margin + nestHeight);

        for (int i = 0; i < number; i++)
        {
            int x = i % columnNumber;
            int y = i / columnNumber;
            bool oddColumn = x % 2 == 1;
            float heightOffset = oddColumn ? nestHeight / 2 : 0;
            Vector3 position = new Vector3(x * (margin + nestWidth * 3 / 4  ) - rectWidth / 2, y * (nestHeight + margin) + heightOffset - rectHeight / 2, 0);
            //if(!landscape)
            //{
            //    float temp = position.x;
            //    position.x = position.y;
            //    position.y = temp;
            //}
            positions.Add(position);
        }

        return positions;

    }
    */
}
