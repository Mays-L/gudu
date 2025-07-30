using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwayCodingFactory : CodingFactory
{
    /// <summary>
    /// Variables for recording sequential data
    /// </summary>
    public int gateLine, selectedCar, unselectedNumber, difficulty;

    /// <summary>
    /// Variables for recording common data
    /// </summary>
    public List<int> carsName, carsColor, carsLine, linesName, linesColor;

    /// <summary>
    /// CodingVariables are used to encode each desired variable
    /// </summary>
    CodingVariable _gateLine, _selectedCar, _unselectedNumber, _carName, _carColor, _carLine, _lineName, _lineColor, _difficulty;

    /// <summary>
    /// Construtive function to initialize CodeVariables
    /// </summary>
    public HighwayCodingFactory()
    {
        _gateLine = new CodingVariable(4); //[0-3]: 4 states == 2bits
        _selectedCar = new CodingVariable(2); //[0-1]: 2 states == 1 bit
        _unselectedNumber = new CodingVariable(8); //[0-7]: 8 states == 3 bits

        carsName = new List<int>();
        carsColor = new List<int>();
        carsLine = new List<int>();
        linesName = new List<int>();
        linesColor = new List<int>();

        _carName = new CodingVariable(4); //[0-3]: 4 states == 2 bits
        _carColor = new CodingVariable(4); //[0-3]: 4 states == 2 bits
        _carLine = new CodingVariable(4); //[0-3]: 4 states == 2 bits
        _lineName = new CodingVariable(4); //[0-3]: 4 states == 2 bits
        _lineColor = new CodingVariable(4); //[0-3]: 4 states == 2 bits
        _difficulty = new CodingVariable(4); //[0-3]: 4 states == 2 bits
    }

    /// <summary>
    /// This function is used to encode the desired data for each level
    /// </summary>
    /// <returns></returns>
    public string EncodingCommonData()
    {
        List<char> charData = new List<char>();

        for (int i = 0; i < linesName.Count; i++)
        {
            _lineName.x = linesName[i];
            _lineColor.x = linesColor[i];

            charData.Add ( patchingVariables(new CodingVariable[] { _lineName, _lineColor }) );
            
            /*
            // This part shows how one can decode each character of common data
            CodingVariable[] forDecoding = new CodingVariable[] { new CodingVariable(4), new CodingVariable(4) };
            breakingVariables(charData[charData.Count-1], ref forDecoding);
            Debug.Log(linesName[i].ToString() + "," + linesColor[i].ToString() + ":" + forDecoding[0].x.ToString() + "," + forDecoding[1].x.ToString());
            */
        }

        for (int i=0;i<carsName.Count;i++)
        {
            _carName.x = carsName[i];
            _carColor.x = carsColor[i];
            _carLine.x = carsLine[i];

            charData.Add( patchingVariables(new CodingVariable[] { _carName, _carColor, _carLine }) );
            
            /*
            // This part shows how one can decode each character of common data
            CodingVariable[] forDecoding1 = new CodingVariable[] { new CodingVariable(4), new CodingVariable(4), new CodingVariable(4) };
            breakingVariables(charData[charData.Count - 1], ref forDecoding1);
            Debug.Log(carsName[i].ToString() + "," + carsColor[i].ToString() + "," + carsLine[i].ToString() + ":" + forDecoding1[0].x.ToString() + "," + forDecoding1[1].x.ToString() + "," + forDecoding1[2].x.ToString() );
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

        char ch1Data = patchingVariables(new CodingVariable[] { _difficulty });

        _gateLine.x = gateLine;
        _selectedCar.x = selectedCar;
        _unselectedNumber.x = unselectedNumber;

        char ch2Data = patchingVariables(new CodingVariable[] { _gateLine, _selectedCar, _unselectedNumber });

        /*
        // This part shows how one can decode each character of sequential data
        CodingVariable[] forDecoding1 = new CodingVariable[] { new CodingVariable(4), new CodingVariable(2), new CodingVariable(8) };
        breakingVariables(chData, ref forDecoding1);
        Debug.Log(gateLine.ToString() + "," + selectedCar.ToString() + "," + unselectedNumber.ToString() + ":" + forDecoding1[0].x.ToString() + "," + forDecoding1[1].x.ToString() + "," + forDecoding1[2].x.ToString() );
        */

        return ch1Data.ToString() + ch2Data.ToString();
    }

}
