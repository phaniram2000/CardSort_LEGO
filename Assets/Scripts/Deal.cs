using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deal : MonoBehaviour
{
    public Animator Anim;
    public List<DeckManager> activeDeckManagers = new List<DeckManager>();
    public List<GameObject> cubePrefab;
    public float spacing;
    public int Index;
    Tween Jump;
    

    private void Start()
    {
        DOVirtual.DelayedCall(1f, FindActiveDeckManagers);
    }

    private void FindActiveDeckManagers()
    {
        DeckManager[] allDeckManagers = FindObjectsOfType<DeckManager>();

        foreach (DeckManager deckManager in allDeckManagers)
        {
            if (deckManager.isActive && deckManager.deckState == DeckState.Deck)
            {
                activeDeckManagers.Add(deckManager);
            }
        }
    }

    public void animateDealButton()
    {
        Anim.SetTrigger("Press");
        DealCubes();
    }

    private void DealCubes()
    {
        StartCoroutine(SpawnCards());
        if (Index >= activeDeckManagers.Count - 1)
        {
            Index = 0;
        }
    }

    private void Update()
    {
    }

    IEnumerator SpawnCards()
    {
        if (activeDeckManagers.Count == 0 || cubePrefab == null)
        {
            yield break;
        }

        // chooseMaterial(cubePrefab);

        float yOffset = 0f; // Initialize the Y-axis spacing offset
        float gap = 0.3f; // Define the gap between cubes
        List<GameObject> SpwanCubes = new List<GameObject>();
        int CardID = Random.Range(0, 3);
        for (int i = 0; i < 3; i++)
        {
             GameObject cube = Instantiate(cubePrefab[CardID], transform.position, Quaternion.identity);
            cube.SetActive(true);
            Vector3 spawnPosition = activeDeckManagers[Index].transform.position;
            SpwanCubes.Add(cube.gameObject);
            // Check if there are cubes at the spawn position
            bool cubesAtSpawnPosition = CheckCubesAtPosition(spawnPosition);

            if (cubesAtSpawnPosition)
            {
                // Find the highest cube at the spawn position
                float highestCubeY = GetHighestCubeYAtPosition(spawnPosition);

                // Move the cube on top of the highest cube at the spawn position with a gap
                spawnPosition.y = highestCubeY + gap + yOffset;
            }
            else
            {
                // No cubes at the spawn position, move the cube to the deck
                spawnPosition.y = activeDeckManagers[Index].transform.position.y + .3f + yOffset;
                ;
            }

            cube.transform.parent = activeDeckManagers[Index].transform.parent;
            cube.GetComponent<Layer>().DeckManager = activeDeckManagers[Index];
            activeDeckManagers[Index].cubesInDeck.Insert(0, cube);
            float rotationDirection = Mathf.Sign(spawnPosition.x - cube.transform.position.x);

            Jump = cube.transform.DOJump(spawnPosition, 1f, 1, 0.5f)
                .OnStart(() =>
                {
                    cube.transform.DORotate(new Vector3(0, 360 * rotationDirection, 0), 0.5f,
                        RotateMode.FastBeyond360);
                });

            yield return new WaitForSeconds(0.05f);

            // Increment the yOffset to add the specified gap between cubes
            yOffset += gap;
        }

        yield return new WaitForEndOfFrame();
        Index++;
        for (int i = 0; i < SpwanCubes.Count; i++)
        {
            SpwanCubes[i].layer = 3;
        }
    }

// Check if there are cubes at a given position (within a certain threshold)
    bool CheckCubesAtPosition(Vector3 position, float threshold = 0.1f)
    {
        foreach (var cube in activeDeckManagers[Index].cubesInDeck)
        {
            if (Mathf.Abs(cube.transform.position.x - position.x) < threshold &&
                Mathf.Abs(cube.transform.position.z - position.z) < threshold)
            {
                return true;
            }
        }

        return false;
    }

// Find the highest cube's Y position at a given position
    float GetHighestCubeYAtPosition(Vector3 position, float threshold = 0.1f)
    {
        float highestY = float.MinValue;

        foreach (var cube in activeDeckManagers[Index].cubesInDeck)
        {
            if (Mathf.Abs(cube.transform.position.x - position.x) < threshold &&
                Mathf.Abs(cube.transform.position.z - position.z) < threshold)
            {
                highestY = Mathf.Max(highestY, cube.transform.position.y);
            }
        }

        return highestY;
    }

    private ColorType colorType;

    public void chooseMaterial(GameObject Card)
    {
        GetcolorIDtoSpwan();
        int randomElement = GameManager.instance.ColorID[colorindex];
        Debug.Log(randomElement);
        switch (randomElement)
        {
            case 1:
                Card.transform.GetComponent<MeshRenderer>().material =
                    GameManager.instance.CardProperties_.CardMatYellow;
                Card.transform.GetComponent<Layer>().ID = 0;
                break;
            case 2:
                Card.transform.GetComponent<MeshRenderer>().material =
                    GameManager.instance.CardProperties_.CardMatGreen;
                Card.transform.GetComponent<Layer>().ID = 1;

                break;
            case 3:
                Card.transform.GetComponent<MeshRenderer>().material =
                    GameManager.instance.CardProperties_.CardMatBlue;
                Card.transform.GetComponent<Layer>().ID = 2;

                break;
            case 4:
                Card.transform.GetComponent<MeshRenderer>().material =
                    GameManager.instance.CardProperties_.CardMatRed;
                Card.transform.GetComponent<Layer>().ID = 3;

                break;
            case 5:
                Card.transform.GetComponent<MeshRenderer>().material =
                    GameManager.instance.CardProperties_.CardMatViolet;
                Card.transform.GetComponent<Layer>().ID = 4;

                break;
            case 6:
                Card.transform.GetComponent<MeshRenderer>().material =
                    GameManager.instance.CardProperties_.CardMatWhite;
                Card.transform.GetComponent<Layer>().ID = 5;

                break;
            case 7:
                Card.transform.GetComponent<MeshRenderer>().material =
                    GameManager.instance.CardProperties_.CardMatBlack;
                Card.transform.GetComponent<Layer>().ID = 6;

                break;
            case 8:
                Card.transform.GetComponent<MeshRenderer>().material =
                    GameManager.instance.CardProperties_.CardMatGrey;
                Card.transform.GetComponent<Layer>().ID = 7;

                break;
            default:
                break;
        }
    }

    public int colorindex = 0;

    void GetcolorIDtoSpwan()
    {
        if (colorindex < GameManager.instance.ColorID.Count)
        {
            colorindex++;
        }
        else
        {
            colorindex = 0;
        }
    }

    // Calculate the spacing based on colliders in the active deck manager
    float CalculateSpacingFromColliders()
    {
        if (activeDeckManagers.Count == 0 || cubePrefab == null)
        {
            return spacing; // Default spacing
        }

        // Assume that all colliders have the same bounds (you can adjust this if necessary)
        Collider[] colliders = activeDeckManagers[Index].GetComponentsInChildren<Collider>();
        if (colliders.Length > 0)
        {
            Bounds bounds = colliders[0].bounds;
            foreach (var collider in colliders)
            {
                bounds.Encapsulate(collider.bounds);
            }

            // Calculate the spacing based on the bounds
            return bounds.size.z + spacing;
        }

        return spacing; // Default spacing
    }
    
}