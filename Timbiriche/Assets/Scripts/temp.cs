using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{
    public LineRenderer[] renderers;
    public Transform[] gridVertices;
    public List<Color> lineColors = new List<Color>();

    void Start()
    {
        foreach (var render in renderers) {
            render.positionCount = 8;
        }


        InitializeLineColors();
        CreateLine();


        for (int i = 0; i < gridVertices.Length; i++)
        {
            var rendIndex = (i) / 8;
            var positionIndex = i - (8 * rendIndex);
            var vertexIndex = i - rendIndex;
            renderers[rendIndex].SetPosition(positionIndex, gridVertices[vertexIndex].position);
            //i = vertexIndex + 1;
        }
    }

    public void JoinVertices(int start, int end, Color color) { 
    
    }

    void InitializeLineColors()
    {
        // Define the colors for the segments of the line.
        lineColors.Add(Color.clear); // Empty
        lineColors.Add(Color.red);
        lineColors.Add(Color.clear); // Empty
        lineColors.Add(Color.blue);
        lineColors.Add(Color.red);
        lineColors.Add(Color.red);
        lineColors.Add(Color.clear); // Empty
        lineColors.Add(Color.red);
        lineColors.Add(Color.clear); // Empty
        lineColors.Add(Color.blue);
    }

    void CreateLine()
    {
        for (int i = 0; i < gridVertices.Length; i++)
        {
            var rendIndex = i / 8;
            var j = i - (8 * rendIndex);

            var gradient = renderers[rendIndex].colorGradient;
            var tempKeys = gradient.colorKeys;
            var alphaKeys = gradient.alphaKeys;

            tempKeys[j].color = lineColors[i];
            alphaKeys[j].alpha = lineColors[i].a;

                gradient.colorKeys = tempKeys;
            gradient.alphaKeys = alphaKeys;
            renderers[rendIndex].colorGradient = gradient;
        }
    }

    // You can update the LineRenderer properties and positions as needed.
    void Update()
    {
        // Example: You can change the color of individual segments by updating the lineColors list and calling CreateLine().
    }
}
