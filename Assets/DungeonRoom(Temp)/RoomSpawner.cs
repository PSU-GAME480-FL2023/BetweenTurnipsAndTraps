using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomSpawner : MonoBehaviour
{
    //The minimum and maximum number of allowed rooms
    private int minRoomNumber;
    private int maxRoomNumber;
    //The current number of rooms that have established grids
    private int currentRoomNumber;
    //The total number of rooms marked in the grid
    private int totalRoomNumber;

    //A grid representing the room layout. Finished rooms will display their entrance in the format "ULRD", where U = Up, L = Left, etc.
    private string[,] roomGrid;
    //Indicates which cycle this spot was marked to be made into a room
    private int roomGenerationTurn;

    //RoomTemplates object
    private RoomTemplates templates;

    //Length of grid
    private int gridLength;

    //Coordinates of dead end rooms
    private int[,] deadEndRoomCoordinates;
    private int stairsRoomNumber;

    // Called by Dungeon Mangager. Begins creation of dungeon
    public void BeginFloorCreation(int minRoomNum, int maxRoomNum)
    {
        //Set min and max room number
        minRoomNumber = minRoomNum;
        maxRoomNumber = maxRoomNum;

        //Get the room list
        templates = GameObject.FindGameObjectWithTag("RoomList").GetComponent<RoomTemplates>();

        //Create the roomGrid in an odd number by odd number format
        gridLength = (2 * maxRoomNumber) + 1;

        roomGrid = new string[gridLength, gridLength];

        bool permittedFloor = false;

        //Keep creating new maps on the roomGrid until we create one that fulfills the requirements
        while (!permittedFloor)
        {
            //Fill in the grid with 0s, which represent an empty space
            for (int i = 0; i < gridLength; i++)
            {
                for (int j = 0; j < gridLength; j++)
                {
                    roomGrid[i, j] = "0";
                }

            }

            permittedFloor = CreateNewFloor();
        }

        //Instantiate rooms in the roomGrid in the actual scene
        InstantiateFloor();

        PrintRoomGrid();
    }

    //Creates a new floor returns true if the floor is successfully created within the given min and max or false if it does not
    private bool CreateNewFloor()
    {
        //Start roomGenerationTurn at 0
        roomGenerationTurn = 0;

        //Get a random room
        int rand = Random.Range(0, templates.startingRooms.Length);

        //Add that room to the center of roomGrid
        roomGrid[(gridLength / 2), (gridLength / 2)] = templates.startingRooms[rand].GetComponent<Room>().GetOpenings();

        //Set the current number of rooms to 1
        currentRoomNumber = 1;
        totalRoomNumber = 1;

        //Mark the new rooms around the starting room
        MarkNewRooms((gridLength / 2), (gridLength / 2));

        //Set to false when a room is created on a whole grid check. If it is true by the end of the grid check, no new rooms were created,
        //which means we must leave the loop as the map is closed and new rooms cannot be created
        bool noRoomsCreated;

        while (!(minRoomNumber <= totalRoomNumber))
        {
            noRoomsCreated = true;

            //Bump the roomGenerationTurn up 1
            roomGenerationTurn++;

            //Find every tile in the roomGrid that matches the roomGenerationTurn, which marks where a new room needs to be created
            for (int i = 0; i < gridLength; i++)
            {
                for (int j = 0; j < gridLength; j++)
                {
                    if (roomGrid[i, j].Equals(roomGenerationTurn.ToString()))
                    {
                        //Add to current number of rooms with doors
                        currentRoomNumber++;

                        //Find the doors that this room must generate to match up with the surrounding rooms
                        string necessaryDoors = FindNecessaryDoors(i, j);

                        //Find the directions that this room cannot generate a new door because there is a wall in that direction
                        string unpermittedWalls = FindUnpermittedWalls(i, j);

                        //Spawn a new room with necessaryDoors into that position
                        CreateNewRoom(i, j, necessaryDoors, unpermittedWalls);

                        //Mark new surrounding rooms
                        MarkNewRooms(i, j);

                        noRoomsCreated = false;
                    }
                }
            }

            PrintRoomGrid();

            if (noRoomsCreated)
            {
                break;
            }
        }

        //If the number of rooms generated is between the min and max number allowed, make every room that needs to be created a dead end
        if ((minRoomNumber <= totalRoomNumber) && (totalRoomNumber <= maxRoomNumber))
        {
            //Bump the roomGenerationTurn up 1
            roomGenerationTurn++;

            Debug.Log(totalRoomNumber);
            Debug.Log(currentRoomNumber);
            //Create the list we will use to keep track of the coordinates of the dead ends
            deadEndRoomCoordinates = new int[2, totalRoomNumber - currentRoomNumber];

            //Create dead ends to finish level
            CreateDeadEnds();
            PrintRoomGrid();

            return true;
        }

        //If the number of rooms generated went under the minimum or over the maximum, regenerate the level
        else
        {
            Debug.Log("Not withing min and max. Reset necessary");
        }

        return false;
    }

    //Creates a new room at x, y while creating doors in the direction of necessaryDoors and not creating doors in the direction of unpermittedWalls
    private void CreateNewRoom(int x, int y, string necessaryDoors, string unpermittedWalls)
    {
        //If the room generated has the necessary doors listed
        bool hasNecessaryDoors = false;
        //If the room generated does not have doors leading into the walls of the surrounding rooms
        bool blockUnpermittedWalls = false;

        //If there are no solid walls on any side of x, y, set blockUnpermittedWalls to true because we do not need to check for it
        if (unpermittedWalls.Length == 0)
        {
            blockUnpermittedWalls = true;
        }

        int rand = 0;

        //Keep choosing random rooms from the list until we find a room that has the doors listed in necessaryDoors and does not have any doors included in unpermittedWalls
        while (!(hasNecessaryDoors && blockUnpermittedWalls))
        {
            //Reset the bool checks to their initial state
            hasNecessaryDoors = false;
            blockUnpermittedWalls = false;

            if (unpermittedWalls.Length == 0)
            {
                blockUnpermittedWalls = true;
            }

            //Get random room
            rand = Random.Range(0, templates.startingRooms.Length);

            //Get doors from room
            string roomDoors = templates.startingRooms[rand].GetComponent<Room>().GetOpenings();

            //Check to see if the doors in this room match up with necessaryDoors
            for (int i = 0; i < necessaryDoors.Length; i++)
            {
                if (!(roomDoors.Contains(necessaryDoors[i])))
                {
                    break;
                }
                else if (i == necessaryDoors.Length - 1)
                {
                    hasNecessaryDoors = true;
                }
            }

            //Check to see if the doors in this room do not include doors in unpermittedWalls
            for (int j = 0; j < unpermittedWalls.Length; j++)
            {
                if (roomDoors.Contains(unpermittedWalls[j]))
                {
                    break;
                }
                else if (j == unpermittedWalls.Length - 1)
                {
                    blockUnpermittedWalls = true;
                }
            }
        }

        //Add that room to the x, y on roomGrid
        roomGrid[x, y] = templates.startingRooms[rand].GetComponent<Room>().GetOpenings();
    }

    //Turn every remaining room that needs to be created and turn it into a dead end
    //Puts these rooms into a list for generation further down the line
    private void CreateDeadEnds()
    {
        //Location in dead end room coordinates
        int deadEndNumber = 0;

        //Find every "x" in the graph, which marks where a new dead end needs to be created
        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridLength; j++)
            {
                if (roomGrid[i, j].Equals(roomGenerationTurn.ToString()))
                {
                    //Add this to the list of dead end room coordinates
                    deadEndRoomCoordinates[0, deadEndNumber] = i;
                    deadEndRoomCoordinates[1, deadEndNumber] = j;

                    deadEndNumber++;

                    //Find the doors that this room must generate to match up with the surrounding rooms
                    string necessaryDoors = FindNecessaryDoors(i, j);

                    //Spawn a new room with necessaryDoors into that position and mark it on roomGrid
                    CreateNewDeadEnd(i, j, necessaryDoors);
                }
            }
        }
    }

    //Create a new dead end at x, y. Add it to deadEnd
    private void CreateNewDeadEnd(int x, int y, string necessaryDoors)
    {
        bool fulfillsRequirements = false;
        int rand = 0;

        //Find a room that has the doors listed in necessaryDoors
        while (!fulfillsRequirements)
        {
            //Get random room
            rand = Random.Range(0, templates.startingRooms.Length);

            //Get doors from room
            string roomDoors = templates.startingRooms[rand].GetComponent<Room>().GetOpenings();

            if (roomDoors == necessaryDoors)
            {
                fulfillsRequirements = true;
            }
        }

        //Add that room to the x, y on roomGrid
        roomGrid[x, y] = templates.startingRooms[rand].GetComponent<Room>().GetOpenings();
    }

    //Mark the new rooms to be made around location [x][y] with an x on roomGrid unless there is a room already in that position
    private void MarkNewRooms(int x, int y)
    {
        //Mark room above current room
        if (roomGrid[x, y].Contains("U") && roomGrid[x - 1, y].Equals("0"))
        {
            roomGrid[x - 1, y] = (roomGenerationTurn + 1).ToString();
            totalRoomNumber++;
        }
        //Mark room to the left of current room
        if (roomGrid[x, y].Contains("L") && roomGrid[x, y - 1].Equals("0"))
        {
            roomGrid[x, y - 1] = (roomGenerationTurn + 1).ToString();
            totalRoomNumber++;
        }
        //Mark room to the right of current room
        if (roomGrid[x, y].Contains("R") && roomGrid[x, y + 1].Equals("0"))
        {
            roomGrid[x, y + 1] = (roomGenerationTurn + 1).ToString();
            totalRoomNumber++;
        }
        //Mark room below current room
        if (roomGrid[x, y].Contains("D") && roomGrid[x + 1, y].Equals("0"))
        {
            roomGrid[x + 1, y] = (roomGenerationTurn + 1).ToString();
            totalRoomNumber++;
        }
    }

    //Find each adjacent entrance to a room that is going to be created
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

    //Find the rooms adjacent to a room that is going to be created that does not have a door leading to that room
    private string FindUnpermittedWalls(int x, int y)
    {
        //String that will contain all directions the room at x, y cannot create a door
        string unpermittedWalls = "";

        //If there is a room above and it does not contain a down entrance, add it to surroudingWalls
        if (!roomGrid[x - 1, y].Equals("0") && !roomGrid[x - 1, y].Contains("D"))
        {
            unpermittedWalls = unpermittedWalls + "U";
        }
        //If there is a room to the left and it does not contain a right entrance, add it to surroudingWalls
        if (!roomGrid[x, y - 1].Equals("0") && !roomGrid[x, y - 1].Contains("R"))
        {
            unpermittedWalls = unpermittedWalls + "L";
        }
        //If there is a room to the right and it does not contain a left entrance, add it to surroudingWalls
        if (!roomGrid[x, y + 1].Equals("0") && !roomGrid[x, y + 1].Contains("L"))
        {
            unpermittedWalls = unpermittedWalls + "R";
        }
        //If there is a room below and it does not contain a up enterance, add it to surroudingWalls
        if (!roomGrid[x + 1, y].Equals("0") && !roomGrid[x + 1, y].Contains("U"))
        {
            unpermittedWalls = unpermittedWalls + "D";
        }

        return unpermittedWalls;
    }

    //Instantiate rooms in the roomGrid
    private void InstantiateFloor()
    {
        //Find every non-0 space in the roomGrid, then instantiate it into the scene
        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridLength; j++)
            {
                if (!roomGrid[i, j].Equals("0"))
                {
                    string necessaryDoors = FindNecessaryDoors(i, j);

                    InstantiateRoom(i, j, necessaryDoors);
                }
            }
        }
    }

    //Put the start room down if (0, 0)
    //Choose random dead end as stairs
    //If room is dead end
        //If stairs
        //Else

    //Find a room that matches the doors of room x, y and instantiate it
    private void InstantiateRoom(int x, int y, string necessaryDoors)
    {
        bool fulfillsRequirements = false;
        int rand = 0;

        //Find a room that has the doors listed in necessaryDoors
        while (!fulfillsRequirements)
        {
            //If this is starting room
            if(x == (gridLength / 2) && y == (gridLength / 2))
            {
                rand = Random.Range(0, templates.startingRooms.Length);

                //Get doors from room
                string roomDoors = templates.startingRooms[rand].GetComponent<Room>().GetOpenings();

                if (roomDoors == necessaryDoors)
                {
                    fulfillsRequirements = true;
                }
            }
            //If room is not dead end, spawn danger room
            else if (IsDeadEnd(x, y) == false)
            {
                rand = Random.Range(0, templates.dangerRooms.Length);

                //Get doors from room
                string roomDoors = templates.dangerRooms[rand].GetComponent<Room>().GetOpenings();

                if (roomDoors == necessaryDoors)
                {
                    fulfillsRequirements = true;
                }
            }
            //If room is dead end and stairs room, spawn stairs room
            else if (IsDeadEnd(x, y) == true && deadEndRoomCoordinates[0, stairsRoomNumber] == x && deadEndRoomCoordinates[1, stairsRoomNumber] == y)
            {
                rand = Random.Range(0, templates.stairsRooms.Length);

                //Get doors from room
                string roomDoors = templates.stairsRooms[rand].GetComponent<Room>().GetOpenings();

                if (roomDoors == necessaryDoors)
                {
                    fulfillsRequirements = true;
                }
            }
            //If room is normal dead end, spawn treasure room
            else
            {
                rand = Random.Range(0, templates.treasureRooms.Length);

                //Get doors from room
                string roomDoors = templates.treasureRooms[rand].GetComponent<Room>().GetOpenings();

                if (roomDoors == necessaryDoors)
                {
                    fulfillsRequirements = true;
                }
            }
            /*rand = Random.Range(0, templates.rooms.Length);

            //Get doors from room
            string roomDoors = templates.rooms[rand].GetComponent<Room>().GetOpenings();

            if (roomDoors == necessaryDoors)
            {
                fulfillsRequirements = true;
            }*/
        }

        //If this is starting room
        if (x == (gridLength / 2) && y == (gridLength / 2))
        {
            BoundsInt bounds = new BoundsInt(Vector3Int.zero, Vector3Int.zero);

            Tilemap[] tilemaps = templates.startingRooms[rand].GetComponentsInChildren<Tilemap>();

            bounds = tilemaps[0].cellBounds;

            foreach (Tilemap tilemap in tilemaps)
            {
                Vector3Int minPosition = Vector3Int.Min(bounds.min, tilemap.cellBounds.min);
                Vector3Int maxPosition = Vector3Int.Max(bounds.max, tilemap.cellBounds.max);

                bounds.SetMinMax(minPosition, maxPosition);
            }

            Vector3 cellSize = templates.startingRooms[rand].cellSize;

            //Get width of room
            float width = bounds.size.x * cellSize.x;
            //Get height of room
            float height = bounds.size.y * cellSize.y;

            int distanceFromCenterX = y - (gridLength / 2);
            int distanceFromCenterY = -(x - (gridLength / 2));

            //Instantiate that room
            Instantiate(templates.startingRooms[rand], new Vector3(distanceFromCenterX * width, distanceFromCenterY * height, 0), templates.startingRooms[rand].transform.rotation);
        }
        //If room is not dead end, spawn danger room
        else if (IsDeadEnd(x, y) == false)
        {
            BoundsInt bounds = new BoundsInt(Vector3Int.zero, Vector3Int.zero);

            Tilemap[] tilemaps = templates.dangerRooms[rand].GetComponentsInChildren<Tilemap>();

            bounds = tilemaps[0].cellBounds;

            foreach (Tilemap tilemap in tilemaps)
            {
                Vector3Int minPosition = Vector3Int.Min(bounds.min, tilemap.cellBounds.min);
                Vector3Int maxPosition = Vector3Int.Max(bounds.max, tilemap.cellBounds.max);

                bounds.SetMinMax(minPosition, maxPosition);
            }

            Vector3 cellSize = templates.dangerRooms[rand].cellSize;

            //Get width of room
            float width = bounds.size.x * cellSize.x;
            //Get height of room
            float height = bounds.size.y * cellSize.y;

            int distanceFromCenterX = y - (gridLength / 2);
            int distanceFromCenterY = -(x - (gridLength / 2));

            //Instantiate that room
            Instantiate(templates.dangerRooms[rand], new Vector3(distanceFromCenterX * width, distanceFromCenterY * height, 0), templates.dangerRooms[rand].transform.rotation);
        }
        //If room is dead end and stairs room, spawn stairs room
        else if (IsDeadEnd(x, y) == true && deadEndRoomCoordinates[0, stairsRoomNumber] == x && deadEndRoomCoordinates[1, stairsRoomNumber] == y)
        {
            BoundsInt bounds = new BoundsInt(Vector3Int.zero, Vector3Int.zero);

            Tilemap[] tilemaps = templates.stairsRooms[rand].GetComponentsInChildren<Tilemap>();

            bounds = tilemaps[0].cellBounds;

            foreach (Tilemap tilemap in tilemaps)
            {
                Vector3Int minPosition = Vector3Int.Min(bounds.min, tilemap.cellBounds.min);
                Vector3Int maxPosition = Vector3Int.Max(bounds.max, tilemap.cellBounds.max);

                bounds.SetMinMax(minPosition, maxPosition);
            }

            Vector3 cellSize = templates.stairsRooms[rand].cellSize;

            //Get width of room
            float width = bounds.size.x * cellSize.x;
            //Get height of room
            float height = bounds.size.y * cellSize.y;

            int distanceFromCenterX = y - (gridLength / 2);
            int distanceFromCenterY = -(x - (gridLength / 2));

            //Instantiate that room
            Instantiate(templates.stairsRooms[rand], new Vector3(distanceFromCenterX * width, distanceFromCenterY * height, 0), templates.stairsRooms[rand].transform.rotation);
        }
        //If room is normal dead end, spawn treasure room
        else
        {
            BoundsInt bounds = new BoundsInt(Vector3Int.zero, Vector3Int.zero);

            Tilemap[] tilemaps = templates.treasureRooms[rand].GetComponentsInChildren<Tilemap>();

            bounds = tilemaps[0].cellBounds;

            foreach (Tilemap tilemap in tilemaps)
            {
                Vector3Int minPosition = Vector3Int.Min(bounds.min, tilemap.cellBounds.min);
                Vector3Int maxPosition = Vector3Int.Max(bounds.max, tilemap.cellBounds.max);

                bounds.SetMinMax(minPosition, maxPosition);
            }

            Vector3 cellSize = templates.treasureRooms[rand].cellSize;

            //Get width of room
            float width = bounds.size.x * cellSize.x;
            //Get height of room
            float height = bounds.size.y * cellSize.y;

            int distanceFromCenterX = y - (gridLength / 2);
            int distanceFromCenterY = -(x - (gridLength / 2));

            //Instantiate that room
            Instantiate(templates.treasureRooms[rand], new Vector3(distanceFromCenterX * width, distanceFromCenterY * height, 0), templates.treasureRooms[rand].transform.rotation);
        }

        /*BoundsInt bounds = new BoundsInt(Vector3Int.zero, Vector3Int.zero);

        Tilemap[] tilemaps = templates.rooms[rand].GetComponentsInChildren<Tilemap>();

        bounds = tilemaps[0].cellBounds;

        foreach (Tilemap tilemap in tilemaps)
        {
            Vector3Int minPosition = Vector3Int.Min(bounds.min, tilemap.cellBounds.min);
            Vector3Int maxPosition = Vector3Int.Max(bounds.max, tilemap.cellBounds.max);

            bounds.SetMinMax(minPosition, maxPosition);
        }

        Vector3 cellSize = templates.rooms[rand].cellSize;

        //Get width of room
        float width = bounds.size.x * cellSize.x;
        //Get height of room
        float height = bounds.size.y * cellSize.y;

        int distanceFromCenterX = y - (gridLength / 2);
        int distanceFromCenterY = -(x - (gridLength / 2));

        //Instantiate that room
        Instantiate(templates.rooms[rand], new Vector3(distanceFromCenterX * width, distanceFromCenterY * height, 0), templates.rooms[rand].transform.rotation);*/
    }

    private bool IsDeadEnd(int x, int y)
    {
        bool isDeadEnd = false;

        for(int i = 0; i < deadEndRoomCoordinates.GetLength(1); i++)
        {
            if (deadEndRoomCoordinates[0, i] == x && deadEndRoomCoordinates[1, i] == y)
            {
                isDeadEnd = true;
            }
        }

        return isDeadEnd;
    }

    //Create the stairs Room
    private void ChooseStairsRoom()
    {
        int rand = Random.Range(0, deadEndRoomCoordinates.GetLength(1));

        stairsRoomNumber = rand;
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