using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LazorColour
{
    public bool isPrimary;
    public string colourName;
    public Color colour;
    public List<LazorColour> composition;

    public LazorColour(string colourName, Color colour)
    {
        isPrimary = true;
        this.colourName = colourName;
        this.colour = colour;

    }

    public LazorColour(string colourName, Color colour, LazorColour composition1, LazorColour composition2)
    {
        isPrimary = false;
        this.colourName = colourName;
        this.colour = colour;

        composition = new List<LazorColour>();
        composition.Add(composition1);
        composition.Add(composition2);
    }
}


  
