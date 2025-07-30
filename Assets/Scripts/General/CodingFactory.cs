using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodingFactory
{
    /// <summary>
    /// This defines a class for coding variables
    /// </summary>
    protected class CodingVariable
    {
        private int xStates; //the number of states of the variable
        private int _xBitNumber; //the number of bits to present the variable
        public int xBitNumber { get { return _xBitNumber; } }

        private int _x; //the variable used for coding
        public int x { get { return _x; } set { _x = Mathf.Clamp(value, 0, xStates - 1); } }

        public CodingVariable(int _xStates)
        {
            xStates = Mathf.Clamp(_xStates, 1, 64);
            _xBitNumber = 0;
            while (Mathf.Pow(2, _xBitNumber) < xStates) _xBitNumber++;
        }

        public string xToBinary { get { return System.Convert.ToString(_x, 2).PadLeft(_xBitNumber, '0'); } }
    }

    #region Patching Data
    /// <summary>
    /// Patching several coding variables (up to 64 states == 6 bits) as a character
    /// </summary>
    protected char patchingVariables(CodingVariable[] _variables) 
    {
        string _data = "";
        for (int i=0;i<_variables.Length;i++)
        {
            _data += _variables[i].xToBinary;
        }
        return convertBinToChar(_data);
    }

    /// <summary>
    /// Converting the binary presentation of the data (up to 6 bits) to a character
    /// </summary>
    private char convertBinToChar(string binData) 
    {
        if (binData.Length > 6 || binData.Length < 1)
        {
            Debug.LogError("convertBinToChar: Error1!");
            return (char)0;
        }

        while (binData.Length < 8)
            binData = "0" + binData;

        byte data = 0;
        for (int i = 0; i < 8; i++)
            data |= (byte)((binData[i] == '1' ? 1 : 0) << (7 - i));

        byte b1 = (byte)'0', b2 = (byte)'A', b3 = (byte)'a', b4 = (byte)'#';

        if (data < 10)
            return (char)(b1 + data);
        else if (data < 36)
            return (char)(b2 + (data - 10));
        else if (data < 62)
            return (char)(b3 + (data - 36));
        else
            return (char)(b4 + (data - 62));
    }
    #endregion

    #region Breaking Data
    /// <summary>
    /// Breaking a character to an array of coding variables (up to 64 states == 6 bits)
    /// </summary>
    protected void breakingVariables(char charData, ref CodingVariable[] _variables)
    {
        string BinData = convertCharToBin(charData);

        int headI = 8 - _variables[_variables.Length - 1].xBitNumber;
        for (int i = _variables.Length - 1; i >= 0; i--)
        {
            string binData = BinData.Substring(headI, _variables[i].xBitNumber);
            //Debug.Log(BinData + "[" + headI.ToString() + "," + _variables[i].xBitNumber.ToString() + "]:" + binData);
            while (binData.Length < 8)
                binData = "0" + binData;

            byte data = 0;
            for (int j = 0; j < 8; j++)
                data |= (byte)((binData[j] == '1' ? 1 : 0) << (7 - j));
            _variables[i].x = data;

            if (i != 0)
                headI -= _variables[i - 1].xBitNumber;
        }
    }

    /// <summary>
    /// Converting the character to a binary presentation
    /// </summary>
    private string convertCharToBin (char charData)
    {
        byte b1 = (byte)'0', b2 = (byte)'A', b3 = (byte)'a', b4 = (byte)'#';

        byte _chData = (byte)charData;
        byte _data = 0;

        if (_chData >= b1 && _chData <= b1 + 9)
            _data = (byte)(_chData - b1);
        else if (_chData >= b2 && _chData <= b2 + 25)
            _data = (byte)(10 + _chData - b2);
        else if (_chData >= b3 && _chData <= b3 + 25)
            _data = (byte)(36 + _chData - b3);
        else if (_chData >= b4 && _chData <= b4 + 1)
            _data = (byte)(62 + _chData - b4);
        else
        {
            Debug.LogError("convertCharToBin: Error1!");
            return "11111111";
        }

        string binData = System.Convert.ToString(_data, 2);
        while (binData.Length < 8)
            binData = "0" + binData;

        return binData;
    }
    #endregion
}
