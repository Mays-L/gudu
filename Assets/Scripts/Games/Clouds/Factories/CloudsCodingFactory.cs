using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsCodingFactory : CodingFactory
{
    /// <summary>
    /// Variables for recording sequential data
    /// </summary>
    public int cloudIndex;

    /// <summary>
    /// Variables for recording common data
    /// </summary>
    public int difficulty, distractor;
    public List<int> cloudArea, cloudType;

    /// <summary>
    /// CodingVariables are used to encode each desired variable
    /// </summary>
    CodingVariable _cloudIndex, _difficulty, _distractor, _cloudArea, _cloudType;

    /// <summary>
    /// Construtive function to initialize CodeVariables
    /// </summary>
    public CloudsCodingFactory()
    {
        _cloudIndex = new CodingVariable(31); //[0-30]: 31 states (up to 32) == 5 bits
        _difficulty = new CodingVariable(4); //[0-3]:4 states == 2 bits
        _distractor = new CodingVariable(4); //[0-3]:4 states == 2 bits
        _cloudArea = new CodingVariable(4); //[0-3]: 4 states == 2 bits
        _cloudType = new CodingVariable(2); //[0-1]: 2 states == 1 bit

        cloudArea = new List<int>();
        cloudType = new List<int>();
    }

    /// <summary>
    /// This function is used to encode the desired data for each level
    /// </summary>
    /// <returns></returns>
    public string EncodingCommonData()
    {
        List<char> charData = new List<char>();

        _difficulty.x = difficulty;
        _distractor.x = distractor;

        charData.Add( patchingVariables(new CodingVariable[] { _difficulty, _distractor }) );

        for (int i = 0; i < cloudArea.Count; i++)
        {
            _cloudArea.x = cloudArea[i];
            _cloudType.x = cloudType[i];

            charData.Add( patchingVariables(new CodingVariable[] { _cloudArea, _cloudType }) );

            /* 
            // This part shows how one can decode each character of common data
            CodingVariable[] forDecoding = new CodingVariable[] { new CodingVariable(4), new CodingVariable(2) };
            breakingVariables(charData[i], ref forDecoding);
            Debug.Log(cloudArea[i].ToString() + "," + cloudType[i].ToString() + ":" + forDecoding[0].x.ToString() + "," + forDecoding[1].x.ToString());
            */
        }

        return new string(charData.ToArray());
    }

    /// <summary>
    /// This function is used to enconde the desired data for each response
    /// </summary>
    public string EncodingSequentialData()
    {
        _cloudIndex.x = cloudIndex;

        char charData = patchingVariables(new CodingVariable[] { _cloudIndex });

        /* 
        // This part shows how one can decode each character of sequential data
        CodingVariable[] forDecoding = new CodingVariable[] { new CodingVariable(31) };
        breakingVariables(charData, ref forDecoding);
        Debug.Log(cloudIndex.ToString() + ":" + forDecoding[0].x.ToString());
        */

        return charData.ToString();
    }

}
