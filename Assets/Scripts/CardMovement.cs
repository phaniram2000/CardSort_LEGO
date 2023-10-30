using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour
{
    // public List<GameObject> objectList;
    // public Transform targetPosition;
    //
    // void Start()
    // {
    //     
    // }
    //
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         MoveConsecutiveSameIdObjectsToPosition();
    //     }
    // }
    //
    // void MoveConsecutiveSameIdObjectsToPosition()
    // {
    //     if (objectList.Count == 0)
    //     {
    //         return;
    //     }
    //
    //     int targetId = objectList[0].GetComponent<Layer>().id;
    //     List<Transform> matchedObjects = new List<Transform>();
    //
    //     for (int i = 0; i < objectList.Count; i++)
    //     {
    //         int objectId = objectList[i].GetComponent<Layer>().id;
    //
    //         if (objectId == targetId)
    //         {
    //             matchedObjects.Add(objectList[i].transform);
    //         }
    //         else
    //         {
    //             break;
    //         }
    //     }
    //
    //     MoveObjectsToPosition(matchedObjects, targetPosition);
    //     RemoveObjectsFromList(matchedObjects);
    // }
    //
    // void MoveObjectsToPosition(List<Transform> objects, Transform position)
    // {
    //     foreach (Transform objTransform in objects)
    //     {
    //         objTransform.position = position.position;
    //     }
    // }
    //
    // void RemoveObjectsFromList(List<Transform> objects)
    // {
    //     foreach (Transform objTransform in objects)
    //     {
    //         objectList.Remove(objTransform.gameObject);
    //     }
    // }
    public List<GameObject> cubesInDeck = new List<GameObject>();

    public int GetFirstCubeID()
    {
        if (cubesInDeck.Count > 0)
        {
            Layer cubeScript = cubesInDeck[0].GetComponent<Layer>();
            if (cubeScript != null)
            {
                return cubeScript.ID;
            }
        }
        return -1; // Return a value that represents no valid ID
    }

    public List<GameObject> CheckForSameId(int targetID)
    {
        List<GameObject> selectedCubes = new List<GameObject>();

        foreach (var cube in cubesInDeck)
        {
            Layer cubeScript = cube.GetComponent<Layer>();
            if (cubeScript != null && cubeScript.ID == targetID)
            {
                selectedCubes.Add(cube);
            }
            else
            {
                break; // Stop adding cubes when a different ID is encountered
            }
        }

        return selectedCubes;
    }
}