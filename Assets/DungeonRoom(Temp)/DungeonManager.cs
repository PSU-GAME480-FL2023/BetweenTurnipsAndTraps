using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    //Number of total floors
    public int numberOfFloors;

    //Current floor
    private int currentFloorNumber;

    //Minimum size of each floor
    public int[] floorMinSizes;

    //Maximum size of each floor
    public int[] floorMaxSizes;

    //We can change the scene
    private bool sceneReloaded;

    //On Awake
    void Start()
    {
        //Destroy this if there is already an instant of this gameObject
        if (Instance == null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            DontDestroyOnLoad(gameObject);
            Instance = this;

            //Set the current floor number
            currentFloorNumber = 0;

            //Make sure number of floors equals the arrays for floor size
            if (numberOfFloors != floorMinSizes.Length || numberOfFloors != floorMaxSizes.Length)
            {
                Debug.Log("Please make sure your floorMinSizes, floorMaxSizes, and numberOfFloors are all the same length");
            }

            //Go to the first floor if this is the first run
            ChangeFloor();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    //Move to the next floor by regenerating map
    public void ChangeFloor()
    {
        //If we are at the final floor, leave
        if (currentFloorNumber == numberOfFloors)
        {
            Debug.Log("Dungeon Beaten!");
        }
        //Progress to the next floor
        else
        {
            currentFloorNumber++;
            Debug.Log("The current floor is " + currentFloorNumber);

            //Get the room spawner
             RoomSpawner roomSpawner = GameObject.FindGameObjectWithTag("RoomSpawner").GetComponent<RoomSpawner>();

            //Create new floor
            roomSpawner.BeginFloorCreation(floorMinSizes[currentFloorNumber - 1], floorMaxSizes[currentFloorNumber - 1]);
        }
    }

    //Reloads the scene
    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    //
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeFloor();
    }

    //Flee the dungeon (called on death or player deciding to leave)
    public void FleeDungeon()
    {
        Debug.Log("Dungeon Left");
    }
}
