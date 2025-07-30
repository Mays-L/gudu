using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManager : Singleton<CloudsManager>
{
    // The clouds that is currently shown in the game
    List<GameObject> Clouds = new List<GameObject>();

    public CloudsCodingFactory hiddenDataEncoder = new CloudsCodingFactory();

    /// <summary>
    /// Showing the clouds on the screen
    /// </summary>
    /// <param name="NumberOfClouds">The number of clouds</param>
    /// <param name="NumberOfRainyClouds">The number of rainy clouds to show</param>
    internal void ShowClouds(int cloudsNumber, int rainyCloudsNumber, int rainyAreasNumber, int initialSpeed, int acceleration)
    {
        if (GameObject.Find("GamePanel") != null)
        {
            GameObject cloud = Resources.Load<GameObject>("Clouds/Prefabs/Cloud");
            Transform parent = GameObject.Find("GamePanel").transform;
            List<Vector3> positions = null;
            List<int> rainyClousIndex = GetRainyCloudsRandomIndex(rainyCloudsNumber, cloudsNumber);
            List<int> cloudArea = GetCloudsRandomAres(cloudsNumber, rainyClousIndex, rainyAreasNumber);

            hiddenDataEncoder.cloudArea.Clear();
            hiddenDataEncoder.cloudType.Clear();
            // create cloud and set parameters for each
            for (int i = 0; i < cloudsNumber; i++)
            {
                GameObject newCloudObject = Instantiate(cloud, new Vector3(0, 0, 0), Quaternion.identity, parent);

                //Set position
                if (positions == null)
                    positions = CalculatePositions(cloudsNumber, newCloudObject);
                newCloudObject.transform.localPosition = positions[i];

                // Set type and parameters
                Cloud newCloud = newCloudObject.GetComponent<Cloud>();
                newCloud.SetParameters(initialSpeed, acceleration, i, cloudArea[i]);
                hiddenDataEncoder.cloudArea.Add(cloudArea[i]);
                if (rainyClousIndex.IndexOf(i) != -1)
                {
                    newCloud.SetType(CloudType.Rainy);
                    hiddenDataEncoder.cloudType.Add(1);
                }
                else
                {
                    newCloud.SetType(CloudType.Normal);
                    hiddenDataEncoder.cloudType.Add(0);
                }
                //newCloud.SetScale(1.4f+(float)30/cloudsNumber);

                Clouds.Add(newCloudObject);
            }

            ShowMoving(8);
        }
    }

    private List<int> GetCloudsRandomAres(int _cloudsNumber, List<int> _rainyClousIndex, int _rainyAreasNumber)
    {
        List<int> outV = new List<int>();
        List<int> rainyAreas = new List<int>();
        while (rainyAreas.Count < _rainyAreasNumber)
        {
            int _rnd = UnityEngine.Random.Range(0, 4);
            if (!rainyAreas.Contains(_rnd))
                rainyAreas.Add(_rnd);
        }

        int j = 0;
        int k = 0;
        for (int i=0;i< _cloudsNumber; i++)
        {
            int targetArea = k;
            if (_rainyClousIndex.Contains(i))
            {
                targetArea = rainyAreas[j];
                j++;
                if (j >= _rainyAreasNumber)
                    j = 0;
            } else
            {
                k++;
                if (k > 3)
                    k = 0;
            }
            
            outV.Add(targetArea);
        }

        return outV;
    }

    public void ShowMoving(float time)
    {
        EventManager.Instance.InvokeEvent("StartingMovingEvent");
        Timers.Instance.StartTimer(time, "EndingMovingEvent");
    }

    /// <summary>
    /// Get random indexes for rainy clouds
    /// </summary>
    /// <param name="rainycloudsNumber">Number of rain clouds</param>
    /// <param name="cloudsNumber">Number of all clouds</param>
    /// <returns></returns>
    private List<int> GetRainyCloudsRandomIndex(int rainycloudsNumber, int cloudsNumber)
    {
        List<int> result = RandomGenerator.GetRandomList(rainycloudsNumber, cloudsNumber);
        return result;
    }

    /// <summary>
    /// Calculating the positions for spawning nests
    /// </summary>
    /// <param name="numberOfClouds"></param>
    /// <returns></returns>
    List<Vector3> CalculatePositions(int numberOfClouds, GameObject newCloudObject)
    {
        List<Vector3> positions = new List<Vector3>();
        CloudsPositionFactory HMPF = new CloudsPositionFactory();
        positions = HMPF.GetCloudsInitialPosition(numberOfClouds, (int)(((RectTransform)newCloudObject.transform).rect.height * newCloudObject.GetComponent<Transform>().localScale.x)); ;
        return positions;
    }

    internal void DisableTouching()
    {
        foreach(GameObject cloud in Clouds)
        {
            Cloud thiscl = cloud.GetComponent<Cloud>();
            thiscl.Stoped = false;
        }
    }

    internal void RemoveAllClouds()
    {
        while (Clouds.Count > 0)
        {
            Destroy(Clouds[Clouds.Count - 1]);
            Clouds.RemoveAt(Clouds.Count - 1);
        }
    }

    internal Vector3 GetTheAnswerPosition()
    {

        foreach (GameObject cloud in Clouds)
        {
            Cloud n = cloud.GetComponent<Cloud>();
            if (n.Type == CloudType.Rainy && !n.Clicked)
                if (n.Stoped)
                {
                    return new Vector3((int)n.transform.localPosition.x,(int)n.transform.localPosition.y,n.transform.localPosition.z);
                }
                else
                    return new Vector3(0, 0, 10);
        }
        return new Vector3(0, 0, 10);
    }
}
