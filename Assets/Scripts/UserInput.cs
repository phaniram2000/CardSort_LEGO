using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public LayerMask deckLayerMask;
    public LayerMask cubeLayerMask;
    public LayerMask DealButtonLayerMask;
    public float spacing = 0.1f;
    private int completedAnimations = 0;

    [Serializable]
    private enum ControllerState
    {
        Idle,
        SelectingDeck,
        SelectingCubes,
        MovingCubes,
        // TimeIntervel
    }

    [ShowInInspector] private ControllerState currentState = ControllerState.Idle;
    [Header("Deck Info")] public GameObject selectedDeck;
    public List<GameObject> selectedCubes = new List<GameObject>();
    public DeckManager selectedDeckManager, DestinationDeckmanager;
    public GameObject destinationDeck;
    [Header("Jump Info")] public float jumpHeight = 1f;
    public float jumpDuration = 0.5f;
    public float rotationAmount = 360f; // Rotation amount in degrees
    public Ease tweenease;
    private bool isAnimating = false;
    private int currentIndex = 0;
    public Tween jump;

    private void Update()
    {
        switch (currentState)
        {
            case ControllerState.Idle:
                HandleIdleState();
                HandleSelectingCubesState();
                OnPressedDeal();
                break;

            case ControllerState.SelectingDeck:
                HandleSelectingDeckState();
                break;

            case ControllerState.SelectingCubes:
                break;

            case ControllerState.MovingCubes:
                HandleMovingCubesState();
                break;
        }
    }

    private void OnPressedDeal()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, DealButtonLayerMask))
            {
                Deal dealScript = hit.transform.GetComponent<Deal>();
                dealScript.animateDealButton();
            }
        }
    }

    private void HandleIdleState()
    {
        // Check for deck selection using raycast from main camera
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, deckLayerMask))
            {
                SelectDeck(hit.transform.gameObject);
            }
        }

        // Check if selectedDeckManager's cubesInDeck count is 0, and if so, stay in idle state
        if (selectedDeckManager != null && selectedDeckManager.cubesInDeck.Count == 0)
        {
            selectedCubes.Clear();
            selectedDeck = null;
            selectedDeckManager = null;
            destinationDeck = null;
            DestinationDeckmanager = null;
            currentState = ControllerState.Idle;
        }
    }


    private void HandleSelectingDeckState()
    {
        // Do nothing here, since we're waiting for the user to select a deck
    }

    private void HandleSelectingCubesState()
    {
        // Check for cube selection using raycast from main camera
        if (!isAnimating && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, cubeLayerMask))
            {
                var cube = hit.transform.gameObject;
                SelectCube(cube);
            }
        }
    }

    private void HandleMovingCubesState()
    {
        // Check for cube movement using raycast from main camera
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            float distace = ray.direction.x;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, deckLayerMask))
            {
                DestinationDeckmanager = hit.transform.GetComponent<DeckManager>();
                MoveSelectedCubes(hit.transform.gameObject);
            }

            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, cubeLayerMask))
            {
                DestinationDeckmanager = hit.transform.GetComponent<Layer>().DeckManager;
                MoveSelectedCubes(hit.transform.gameObject);
            }
        }
    }

    private void SelectDeck(GameObject deck)
    {
        selectedDeck = deck;
        selectedDeckManager = deck.GetComponent<DeckManager>();
        selectedCubes.Clear(); // Clear the list before adding cubes

        // Add all cubes in the selected deck to the selectedCubes list
        foreach (var cube in selectedDeckManager.cubesInDeck)
        {
            selectedCubes.Add(cube);
        }

        currentState = ControllerState.MovingCubes;
    }

    private void SelectCube(GameObject cube)
    {
        if (cube != null)
        {
            Layer cubeScript = cube.GetComponent<Layer>();
            if (cubeScript != null)
            {
                int selectedCubeID = cubeScript.ID;

                // Set the selectedDeckManager based on the cube's DeckManager
                selectedDeckManager = cubeScript.DeckManager;
                selectedDeck = selectedDeckManager.gameObject;

                // Add all cubes with the same ID to the selectedCubes list
                selectedCubes = selectedDeckManager.CheckForSameId(selectedCubeID);

                // Move cubes up and change state if cubes are selected
                if (selectedCubes.Count > 0)
                {
                    foreach (GameObject selectedCube in selectedCubes)
                    {
                        if (selectedCube != null)
                        {
                            selectedCube.transform.Translate(Vector3.up, Space.World);
                        }
                    }

                    currentState = ControllerState.MovingCubes;
                }
                else
                {
                    // Perform DOPunchScale animation on the selected deck
                    clearEverything();
                }
            }
        }
    }

    private void MoveSelectedCubes(GameObject destination)
    {
        if (selectedCubes.Count > 0 && DestinationDeckmanager != selectedDeckManager)
        {
            //currentState = ControllerState.TimeIntervel;

            // Store the initial positions of the selected cubes
            List<Vector3> initialPositions = new List<Vector3>();
            foreach (GameObject cube in selectedCubes)
            {
                if (cube != null)
                {
                    initialPositions.Add(cube.transform.position);
                }
            }

            // Check if the destination's selectedCubes is empty or has the same ID as the selected cubes
            bool canMoveCubes = false;

            if (DestinationDeckmanager.cubesInDeck.Count == 0)
            {
                canMoveCubes = true;
            }
            else
            {
                Layer destinationTopCubeScript = DestinationDeckmanager.cubesInDeck[0].GetComponent<Layer>();
                Layer selectedCubeScript = selectedCubes[0].GetComponent<Layer>();

                if (destinationTopCubeScript != null && selectedCubeScript != null &&
                    destinationTopCubeScript.ID == selectedCubeScript.ID)
                {
                    canMoveCubes = true;
                }
            }

            if (canMoveCubes)
            {
                StartCoroutine(delayjump(destination));
            }
            else
            {
                // Move the selected cubes back to their initial positions
                foreach (GameObject selectedCube in selectedCubes)
                {
                    if (selectedCube != null)
                    {
                        selectedCube.transform.Translate(Vector3.down, Space.World);
                    }
                }

                clearEverything();
            }
        }
        else if (DestinationDeckmanager == selectedDeckManager)
        {
            foreach (GameObject selectedCube in selectedCubes)
            {
                if (selectedCube != null)
                {
                    selectedCube.transform.Translate(Vector3.down, Space.World);
                }
            }

            clearEverything();
        }
    }


    void clearEverything()
    {
        selectedCubes.Clear();
        selectedDeck = null;
        selectedDeckManager = null;
        destinationDeck = null;
        DestinationDeckmanager = null;
        currentState = ControllerState.Idle;
    }

    IEnumerator delayjump(GameObject destination)
    {
        Collider[] colliders = destination.GetComponentsInChildren<Collider>();

        // Get the position of the first cube in the destination deck manager's cubesInDeck
        Vector3 spawnPosition = GetFirstCubePosition(destination);

        foreach (GameObject cube in selectedCubes)
        {
            foreach (var collider in colliders)
            {
                if (collider.bounds.max.y > spawnPosition.y)
                {
                    // Set the highestY based on collider bounds
                    spawnPosition.y = collider.bounds.max.y;
                }
            }

            // Increment highestY for the next cube
            spawnPosition.y += spacing;

            if (cube != null)
            {
                cube.transform.parent = destination.transform.parent;

                // Determine the rotation axis based on the jump direction
                Vector3 rotationAxis;
                if (spawnPosition.x > cube.transform.position.x)
                {
                    rotationAxis = new Vector3(0, 0, -1);
                }
                else if (spawnPosition.x < cube.transform.position.x)
                {
                    rotationAxis = new Vector3(0, 0, 1);
                }
                else if (spawnPosition.z > cube.transform.position.z)
                {
                    rotationAxis = new Vector3(1, 0, 0);
                }
                else
                {
                    rotationAxis = new Vector3(-1, 0, 0);
                }

                // Jump the object using DOJump
                jump = cube.transform.DOJump(spawnPosition, jumpHeight, 1, jumpDuration)
                    .SetEase(tweenease)
                    .OnStart(() =>
                    {
                        // Rotate the object at the start of the jump
                        cube.transform.DORotate(rotationAmount * rotationAxis, jumpDuration, RotateMode.FastBeyond360);
                    });
                yield return new WaitForSeconds(0.05f);

                // Add the moved cube to the destination's DeckManager cubesInDeck list
                if (DestinationDeckmanager != null)
                {
                    DestinationDeckmanager.cubesInDeck.Insert(0, cube);
                    cube.GetComponent<Layer>().DeckManager = DestinationDeckmanager;
                }
            }
        }

        isAnimating = false;

        // Remove the moved cubes from selectedDeckManager's cubesInDeck list
        foreach (GameObject cube in selectedCubes)
        {
            selectedDeckManager.cubesInDeck.Remove(cube);
        }

        yield return new WaitForEndOfFrame();
        clearEverything();
    }

    Vector3 GetFirstCubePosition(GameObject destination)
    {
        if (DestinationDeckmanager != null && DestinationDeckmanager.cubesInDeck.Count > 0)
        {
            GameObject firstCube = DestinationDeckmanager.cubesInDeck[0];
            if (firstCube != null)
            {
                return firstCube.transform.position;
            }
        }

        // If no cubes in destination, use destination's position
        return destination.transform.position;
    }
}