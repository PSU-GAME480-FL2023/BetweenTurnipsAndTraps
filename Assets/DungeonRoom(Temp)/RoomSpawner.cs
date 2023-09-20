using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    //The minimum and maximum number of allowed rooms
    public int minRoomNumber;
    public int maxRoomNumber;
    //The current number of rooms spawned
    private int currentRoomNumber;

    //A grid representing the room layout. Finished rooms will display their entrance in the format "ULRD", where U = Up, L = Left, etc.
    private string[,] roomGrid;
    //Indicates which cycle this spot was marked to be made into a room
    private int roomGenerationTurn;

    //RoomTemplates object
    private RoomTemplates templates;

    //Length of grid
    private int gridLength;

    // Start is called before the first frame update
    void Start()
    {
        //Get the room list
        templates = GameObject.FindGameObjectWithTag("RoomList").GetComponent<RoomTemplates>();

        //Create the roomGrid in an odd number x odd number format
        if(maxRoomNumber % 2 == 0)
        {
            gridLength = (2 * maxRoomNumber) + 1;
        }
        else
        {
            gridLength = (2 * maxRoomNumber);
        }
        roomGrid = new string[gridLength, gridLength];

        //Fill in the grid with 0s, so that we don't get a certain error that occurs when comparing strings to null
        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridLength; j++)
            {
                roomGrid[i, j] = "0";
            }

        }

        //Start roomGenerationTurn at 0
        roomGenerationTurn = 0;

        //Get a random room
        int rand = Random.Range(0, templates.rooms.Length);

        //Instantiate that room
        Instantiate(templates.rooms[rand], new Vector3(0, 0, 0), templates.rooms[rand].transform.rotation);

        //Add that room to the center of roomGrid
        roomGrid[(gridLength / 2), (gridLength / 2)] = templates.rooms[rand].GetComponent<Room>().GetOpenings();

        //Set the current number of rooms to 1
        currentRoomNumber = 1;

        //Mark the new rooms around the starting room
        MarkNewRooms((gridLength / 2), (gridLength / 2));

        PrintRoomGrid();

        CreateNewFloor();
    }

    private void CreateNewFloor()
    {
        while (!(minRoomNumber <= currentRoomNumber))
        {
            //Bump the roomGenerationTurn up 1
            roomGenerationTurn++;

            //Find every "x" in the graph, which marks where a new room needs to be created
            for (int i = 0; i < gridLength; i++)
            {
                for (int j = 0; j < gridLength; j++)
                {
                    if (roomGrid[i, j].Equals(roomGenerationTurn.ToString()))
                    {
                        //Find the doors that this room must generate to match up with the surrounding rooms
                        string necessaryDoors = FindNecessaryDoors(i, j);

                        //Spawn a new room with necessaryDoors into that position and mark it on roomGrid
                        CreateNewRoom(i, j, necessaryDoors);

                        //Mark new surrounding rooms
                        MarkNewRooms(i, j);
                    }
                }
            }

            PrintRoomGrid();
        }

        if ((minRoomNumber <= currentRoomNumber) && (currentRoomNumber <= maxRoomNumber))
        {
            //Bump the roomGenerationTurn up 1
            roomGenerationTurn++;

            //Create dead ends to finish level
            CreateDeadEnds();
            PrintRoomGrid();
        }

        if (maxRoomNumber < currentRoomNumber)
        {
            Debug.Log("Limit stepped over");
        }
    }

    private void CreateNewRoom(int x, int y, string necessaryDoors)
    {
        bool fulfillsRequirements = false;
        int rand = 0;

        //Find a room that has the doors listed in necessaryDoors
        while (!fulfillsRequirements)
        {
            //Get random room
            rand = Random.Range(0, templates.rooms.Length);

            //Get doors from room
            string roomDoors = templates.rooms[rand].GetComponent<Room>().GetOpenings();

            for (int i = 0; i < necessaryDoors.Length; i++)
            {
                if (!(roomDoors.Contains(necessaryDoors[i])))
                {
                    break;
                }
                else if (i == necessaryDoors.Length - 1)
                {
                    fulfillsRequirements = true;
                }
            }
        }

        //Get width of room
        float width = templates.rooms[rand].GetComponent<SpriteRenderer>().bounds.size.x;

        //Get height of room
        float height = templates.rooms[rand].GetComponent<SpriteRenderer>().bounds.size.y;

        int distanceFromCenterX = y - (gridLength / 2);
        int distanceFromCenterY = -(x - (gridLength / 2));

        //Instantiate that room
        Instantiate(templates.rooms[rand], new Vector3(distanceFromCenterX * width, distanceFromCenterY * height, 0), templates.rooms[rand].transform.rotation);

        //Add that room to the x, y on roomGrid
        roomGrid[x, y] = templates.rooms[rand].GetComponent<Room>().GetOpenings();

    }

    private void CreateDeadEnds()
    {
        //Find every "x" in the graph, which marks where a new dead end needs to be created
        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridLength; j++)
            {
                if (roomGrid[i, j].Equals(roomGenerationTurn.ToString()))
                {
                    //Find the doors that this room must generate to match up with the surrounding rooms
                    string necessaryDoors = FindNecessaryDoors(i, j);
                    Debug.Log(necessaryDoors);

                    //Spawn a new room with necessaryDoors into that position and mark it on roomGrid
                    CreateNewDeadEnd(i, j, necessaryDoors);
                }
            }
        }
    }

    private void CreateNewDeadEnd(int x, int y, string necessaryDoors)
    {
        bool fulfillsRequirements = false;
        int rand = 0;

        //Find a room that has the doors listed in necessaryDoors
        while (!fulfillsRequirements)
        {
            //Get random room
            rand = Random.Range(0, templates.rooms.Length);
            Debug.Log(rand);

            //Get doors from room
            string roomDoors = templates.rooms[rand].GetComponent<Room>().GetOpenings();

            if (roomDoors == necessaryDoors)
            {
                fulfillsRequirements = true;
            }
        }

        //Get width of room
        float width = templates.rooms[rand].GetComponent<SpriteRenderer>().bounds.size.x;

        //Get height of room
        float height = templates.rooms[rand].GetComponent<SpriteRenderer>().bounds.size.y;

        int distanceFromCenterX = y - (gridLength / 2);
        int distanceFromCenterY = -(x - (gridLength / 2));

        //Instantiate that room
        Instantiate(templates.rooms[rand], new Vector3(distanceFromCenterX * width, distanceFromCenterY * height, 0), templates.rooms[rand].transform.rotation);

        //Add that room to the x, y on roomGrid
        roomGrid[x, y] = templates.rooms[rand].GetComponent<Room>().GetOpenings();
    }

    //Mark the new rooms to be made around location [x][y] with an x on roomGrid unless there is a room already in that position
    private void MarkNewRooms(int x, int y)
    {
        //Mark room above current room
        if (roomGrid[x, y].Contains("U") && roomGrid[x - 1, y].Equals("0"))
        {
            roomGrid[x - 1, y] = (roomGenerationTurn + 1).ToString();
            currentRoomNumber++;
        }
        //Mark room to the left of current room
        if (roomGrid[x, y].Contains("L") && roomGrid[x, y - 1].Equals("0"))
        {
            roomGrid[x, y - 1] = (roomGenerationTurn + 1).ToString();
            currentRoomNumber++;
        }
        //Mark room to the right of current room
        if (roomGrid[x, y].Contains("R") && roomGrid[x, y + 1].Equals("0"))
        {
            roomGrid[x, y + 1] = (roomGenerationTurn + 1).ToString();
            currentRoomNumber++;
        }
        //Mark room below current room
        if (roomGrid[x, y].Contains("D") && roomGrid[x + 1, y].Equals("0"))
        {
            roomGrid[x + 1, y] = (roomGenerationTurn + 1).ToString();
            currentRoomNumber++;
        }
    }

    //Find each surrounding entrance to a room that needs to be created
    private string FindNecessaryDoors(int x, int y)
    {
        //String that will contain all doors the room at x, y must have
        string necessaryDoors = "";

        //See if there is a door above current room
        if (roomGrid[x - 1, y].Contains("D"))
        {
            necessaryDoors = necessaryDoors + "U";
        }
        //See if there is a door to the left of current room
        if (roomGrid[x, y - 1].Contains("R"))
        {
            necessaryDoors = necessaryDoors + "L";
        }
        //See if there is a door to the right of current room
        if (roomGrid[x, y + 1].Contains("L"))
        {
            necessaryDoors = necessaryDoors + "R";
        }
        //See if there is a door below current room
        if (roomGrid[x + 1, y].Contains("U"))
        {
            necessaryDoors = necessaryDoors + "D";
        }

        return necessaryDoors;
    }

    //Print the room grid to the console
    //NOTE: The grid will only show two lines in the console. Click on it and the full grid will appear in the window below
    private void PrintRoomGrid()
    {
        string mapString = "";

        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridLength; j++)
            {
                mapString = mapString + roomGrid[i, j] + ", ";
            }
            mapString = mapString + "\n";
        }

        Debug.Log(mapString);
    }
}