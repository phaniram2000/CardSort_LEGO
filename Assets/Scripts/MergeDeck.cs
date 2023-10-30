using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeDeck : MonoBehaviour
{
    public List<GameObject> cubesInDeck = new List<GameObject>();
    public float spacing;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddCubesToDeck(List<GameObject> selectedCubes)
    {
        cubesInDeck.AddRange(cubesInDeck);
        ArrangeCubesInDeck();
        
    }

    private void ArrangeCubesInDeck()
    {
        float totalStackHeight = cubesInDeck.Count * spacing; // Total height of stacked cubes
        Vector3 basePosition = transform.position; // Base position to start stacking

        // Loop through each cube and adjust its position
        for (int i = 0; i < cubesInDeck.Count; i++)
        {
            GameObject cube = cubesInDeck[i];
            if (cube != null)
            {
                // Calculate the desired y-position for the cube
                float yOffset = i * spacing;

                // Set the new position for the cube
                cube.transform.position = new Vector3(basePosition.x, basePosition.y + yOffset, basePosition.z);
            }
        }
    }
}