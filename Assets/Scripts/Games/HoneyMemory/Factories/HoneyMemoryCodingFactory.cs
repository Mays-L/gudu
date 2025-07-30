using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyMemoryCodingFactory : CodingFactory
{
    /// <summary>
    /// Variables for recording sequential data
    /// </summary>
    public int nestIndex;

    /// <summary>
    /// Variables for recording common data
    /// </summary>
    public int difficulty, distractor;
    public int colNum, rowNum;
    public List<int> nestsCI, nestsRI, nestsType;

    /// <summary>
    /// CodingVariables are used to encode each desired variable
    /// </summary>
    CodingVariable _nestIndex, _difficulty, _distractor, _colNum, _rowNum, _nestCI, _nestRI, _nestType;

    /// <summary>
    /// Construtive function to initialize CodeVariables
    /// </summary>
    public HoneyMemoryCodingFactory()
    {
        _nestIndex = new CodingVariable(64); //[0-63]: 64 states == 6 bits
        _difficulty = new CodingVariable(4); //[0-3]:4 states == 2 bits
        _distractor = new CodingVariable(2); //[0-1]:2 states == 1 bit
        _colNum = new CodingVariable(64); //[0-63]: 64 states == 6 bits
        _rowNum = new CodingVariable(64); //[0-63]: 64 states == 6 bits
        _nestCI = new CodingVariable(32); //[0-31]: 32 states == 5 bits
        _nestRI = new CodingVariable(32); //[0-31]: 32 states == 5 bits
        _nestType = new CodingVariable(2); //[0-1]: 2 states == 1 bit

        nestsCI = new List<int>();
        nestsRI = new List<int>();
        nestsType = new List<int>();
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
        _colNum.x = colNum;
        _rowNum.x = rowNum;

        charData.Add( patchingVariables(new CodingVariable[] { _difficulty, _distractor }) );
        charData.Add( patchingVariables(new CodingVariable[] { _colNum }) );
        charData.Add( patchingVariables(new CodingVariable[] { _rowNum }) );

        /*
        // This part shows how one can decode the first dual characters of common data
        CodingVariable[] forDecoding1 = new CodingVariable[] { new CodingVariable(64) };
        CodingVariable[] forDecoding2 = new CodingVariable[] { new CodingVariable(64) };
        breakingVariables(_chC, ref forDecoding1);
        breakingVariables(_chR, ref forDecoding2);
        Debug.Log(colNum.ToString() + "," + rowNum.ToString() + ":" + forDecoding1[0].x.ToString() + "," + forDecoding2[0].x.ToString());
        */

        for (int i = 0; i < nestsCI.Count; i++)
        {
            _nestCI.x = nestsCI[i];
            _nestRI.x = nestsRI[i];
            _nestType.x = nestsType[i];

            charData.Add(patchingVariables( new CodingVariable[] { _nestCI }) );
            charData.Add(patchingVariables( new CodingVariable[] { _nestRI, _nestType }) );

            /*
            // This part shows how one can decode the next dual characters of common data
            CodingVariable[] forDecoding1n = new CodingVariable[] { new CodingVariable(32) };
            CodingVariable[] forDecoding2n = new CodingVariable[] { new CodingVariable(32), new CodingVariable(2) };
            breakingVariables(_chC, ref forDecoding1n);
            breakingVariables(_chR, ref forDecoding2n);
            Debug.Log(nestsCI[i].ToString() + "," + nestsRI[i].ToString() + "," + nestsType[i].ToString() + ":" + forDecoding1n[0].x.ToString() + "," + forDecoding2n[0].x.ToString() + "," + forDecoding2n[1].x.ToString());
            */
        }

        return new string(charData.ToArray());
    }

    /// <summary>
    /// This function is used to enconde the desired data for each response
    /// </summary>
    public string EncodingSequentialData()
    {
        _nestIndex.x = nestIndex;

        char charData = patchingVariables(new CodingVariable[] { _nestIndex });

        /*
        // This part shows how one can decode each character of sequential data
        CodingVariable[] forDecoding = new CodingVariable[] { new CodingVariable(64) };
        breakingVariables(charData, ref forDecoding);
        Debug.Log(nestIndex.ToString() + ":" + forDecoding[0].x.ToString());
        */

        return charData.ToString();
    }

}
