using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StressBall", menuName = "Create New StressBall")]
public class StressBallSO : ScriptableObject
{
    public SizeType sizeType = SizeType.SMALL;

    public float minSize, maxSize;

    public float currentSize;
    
    public float sizeIfBlack = 5.0f;

    public bool isBlack;

    public Color[] possibleColors;
    public Color selectedColor;

    private void OnValidate() //Sets the correct values for each ball size type 
    {
        switch (sizeType)
        {
            case SizeType.SMALL:
                minSize = 1.0f;
                maxSize = 1.9f;
                break;
            case SizeType.MEDIUM:
                minSize = 2.0f;
                maxSize = 2.9f;
                break;
            case SizeType.LARGE:
                minSize = 3.0f;
                maxSize = 3.9f;
                break;
        }
        if(!isBlack)
            selectedColor = possibleColors[0];
    }
    public void ChangeColor(Color color) //Changes the color of the ball
    {
        selectedColor = color;
        if (color.Equals(Color.black) && sizeType.Equals(SizeType.LARGE))
            isBlack = true;
        else
            isBlack = false;
    }
    public float GetSize()
    {
        if (isBlack)
            return sizeIfBlack;
        else
            return currentSize;
    } //Get the correct size of the ball
}


public enum SizeType
{
    SMALL, MEDIUM, LARGE
} //SizeType Enum
