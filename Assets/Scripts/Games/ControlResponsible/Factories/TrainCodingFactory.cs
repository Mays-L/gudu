using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCodingFactory : CodingFactory
{
    /// <summary>
    /// Variables for recording sequential data
    /// </summary>
    public int wagonIndex, difficulty;

    /// <summary>
    /// Variables for recording common data
    /// </summary>
    public int categoryIndex;
    public List<int> selectedImagesIndex, wagonsImagesIndex;

    /// <summary>
    /// CodingVariables are used to encode each desired variable
    /// </summary>
    CodingVariable _wagonIndexH, _wagonIndexL, _categoryIndex, _difficulty, _selectedImageIndex, _wagonImageIndex;

    /// <summary>
    /// Construtive function to initialize CodeVariables
    /// </summary>
    public TrainCodingFactory()
    {
        _wagonIndexH = new CodingVariable(64); //[0-63]: 64 states == 6 bits
        _wagonIndexL = new CodingVariable(64); //[0-63]: 64 states == 6 bits
        _categoryIndex = new CodingVariable(8); //[0-7]: 8 states == 3 bits
        _difficulty = new CodingVariable(4); //[0-3]: 4 states == 2 bits
        _selectedImageIndex = new CodingVariable(64); //[0-63]: 64 states == 6 bits
        _wagonImageIndex = new CodingVariable(64); //[0-63]: 64 states == 6 bits

        selectedImagesIndex = new List<int>();
        wagonsImagesIndex = new List<int>();
    }

    /// <summary>
    /// This function is used to encode the desired data for each level
    /// </summary>
    /// <returns></returns>
    public string EncodingCommonData()
    {
        List<char> charData = new List<char>();

        _categoryIndex.x = categoryIndex;
        charData.Add( patchingVariables(new CodingVariable[] { _categoryIndex }) );

        /* 
        // This part shows how one can decode each character of common data
        CodingVariable[] forDecoding = new CodingVariable[] { new CodingVariable(8) };
        breakingVariables(charData[i], ref forDecoding);
        Debug.Log(cloudArea[i].ToString() + "," + cloudType[i].ToString() + ":" + forDecoding[0].x.ToString() + "," + forDecoding[1].x.ToString());
        */

        for (int i = 0; i < selectedImagesIndex.Count; i++)
        {
            _selectedImageIndex.x = selectedImagesIndex[i];
            charData.Add( patchingVariables(new CodingVariable[] { _selectedImageIndex }) );

            /* 
            // This part shows how one can decode each character of common data
            CodingVariable[] forDecoding = new CodingVariable[] { new CodingVariable(64) };
            breakingVariables(charData[i], ref forDecoding);
            Debug.Log(cloudArea[i].ToString() + "," + cloudType[i].ToString() + ":" + forDecoding[0].x.ToString() + "," + forDecoding[1].x.ToString());
            */
        }

        for (int i = 0; i < wagonsImagesIndex.Count; i++)
        {
            _wagonImageIndex.x = wagonsImagesIndex[i];
            charData.Add(patchingVariables(new CodingVariable[] { _wagonImageIndex }));

            /* 
            // This part shows how one can decode each character of common data
            CodingVariable[] forDecoding = new CodingVariable[] { new CodingVariable(64) };
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
        _difficulty.x = difficulty;
        _wagonIndexH.x = wagonIndex / 64;
        _wagonIndexL.x = wagonIndex % 64;

        char Df = patchingVariables(new CodingVariable[] { _difficulty });
        char charDataH = patchingVariables(new CodingVariable[] { _wagonIndexH });
        char charDataL = patchingVariables(new CodingVariable[] { _wagonIndexL });

        /* 
        // This part shows how one can decode each character of sequential data
        CodingVariable[] forDecoding = new CodingVariable[] { new CodingVariable(31) };
        breakingVariables(charData, ref forDecoding);
        Debug.Log(cloudIndex.ToString() + ":" + forDecoding[0].x.ToString());
        */

        return Df.ToString() + charDataH.ToString() + charDataL.ToString();
    }

}
