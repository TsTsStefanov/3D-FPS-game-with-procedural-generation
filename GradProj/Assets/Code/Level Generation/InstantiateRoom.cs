using System;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;

public class InstantiateRoom : MonoBehaviour
{
    public GameObject Wall01;
    public GameObject Wall01_door;
    public GameObject Doors;
    public GameObject Wall01_corner01;
    public GameObject Wall01_corner02;
    public GameObject FillPiece;
    public GameObject StartingRoom;
    public GameObject Enemy01;
    public int maxRoomSize = 8;
    public float tileSize = 2f;
    public int roomAmount = 5;
    public SlidingDoors slidingDoors;
    LayoutGen layoutGen;
    RoomGen roomGen;
    GameObject roomBorders;
    GameObject roomFloor;
    GameObject enemyParent;
    GameObject Room;
    GameObject startRoom;
    GameObject Layout;
    int[] directions;
    bool isCleared = false;
    int[] rememberRoom;
    void Start()
    {
        ExecuteAll();
    }

    void Update()
    {
        EnemiesCleared();
    }

    void ExecuteAll()
    {
        rememberRoom = new int[roomAmount];
        layoutGen = new LayoutGen();
        roomGen = new RoomGen();
        Layout = new GameObject();
        Layout.transform.name = "Layout";
        directions = layoutGen.RandomWalk(layoutGen.Map(roomAmount), roomAmount);
        int startDirection;
        int endDirection;
        int[,] roomLayout;
        Vector3 startignRoomPosition = new Vector3(0, 0, 0);
        Vector3 nextExit = new Vector3(0, 0, 0);

        for (int i = 0; i < roomAmount; i++)
        {
            startDirection = InvertDirection(directions[i]);
            endDirection = directions[i + 1];
            Vector3[] doorCoords = new Vector3[2];
            if (endDirection == 0)
            {
                endDirection = startDirection;
            }

            roomLayout = roomGen.GenerateValid(startDirection, endDirection, maxRoomSize);
            PrintArray(roomLayout);
            if (i == 0)
            {
                startRoom = IstantiateStartingRoom(startDirection, startignRoomPosition);
            }
            roomBorders = IstantiateRoomBorders(roomLayout);
            roomFloor = IstantiateRoomFill(roomLayout);
            enemyParent = InstantiateEnemies(roomLayout);

            Room = new GameObject();
            Room.name = $"Room {i + 1}";
            doorCoords = FindDoorCoords(roomBorders, startDirection, endDirection);
            if (i + 1 != roomAmount)
            {
                InstantiateExitDoors(doorCoords[1], endDirection, roomBorders);
            }


            roomBorders.transform.parent = Room.transform;
            roomFloor.transform.parent = Room.transform;
            enemyParent.transform.parent = Room.transform;
            Vector3 offset = nextExit - doorCoords[0];
            Vector3 directionOffset = Vector3.zero;

            directionOffset = OffsetBy(startDirection, directionOffset, (int)tileSize);
            offset = offset + directionOffset;
            Room.transform.position += offset;
            Room.transform.parent = Layout.transform;
            nextExit = doorCoords[1] + offset;
        }
    }
    void InstantiateExitDoors(Vector3 doorExit, int endDirection, GameObject roomBorders)
    {
        GameObject newObject;
        Quaternion rotation = Quaternion.identity;
        rotation = RotationChange(endDirection, rotation);
        newObject = Instantiate(Doors, doorExit, rotation);
        newObject.transform.parent = roomBorders.transform;
    }
    void EnemiesCleared()
    {
        for (int room = 0; room < Layout.transform.childCount; room++)
        {
            if (Layout.transform.GetChild(room).name == $"Room {room + 1}")
            {
                if(room+1 == Layout.transform.childCount && rememberRoom[room]!=0)
                {
                    OtherMenus.winCondition = true;
                }
                if (rememberRoom[room] != 0)
                {
                    continue;
                }
                else
                {
                    for (int groupOfEnemies = 0; groupOfEnemies < 
                        Layout.transform.GetChild(room).childCount; groupOfEnemies++)
                    {
                        if (Layout.transform.GetChild(room).
                            transform.GetChild(groupOfEnemies).name == "Enemies in this room")
                        {
                            if (Layout.transform.GetChild(room).
                                transform.GetChild(groupOfEnemies).childCount == 0)
                            {
                                Debug.Log($"Room {room + 1} is empty!");
                                for(int roomBorder = 0; roomBorder < 
                                    Layout.transform.GetChild(room).childCount; roomBorder++)
                                {
                                    if (Layout.transform.GetChild(room).
                                        transform.GetChild(roomBorder).name=="RoomBorders")
                                    {
                                        for (int doors=0; doors< Layout.transform.GetChild(room).
                                            transform.GetChild(roomBorder).childCount; doors++)
                                        {
                                            if (Layout.transform.GetChild(room).transform.GetChild(roomBorder).
                                                transform.GetChild(doors).name.StartsWith("Doors"))
                                            {
                                                slidingDoors = Layout.transform.GetChild(room).
                                                    transform.GetChild(roomBorder).
                                                    transform.GetChild(doors).GetComponentInChildren<SlidingDoors>();
                                                slidingDoors.OpenDoors();
                                            }
                                        }
                                    }
                                }
                                isCleared = true;
                            }
                        }
                        if (isCleared)
                        {
                            rememberRoom[room] = room + 1;
                        }
                    }
                }
                isCleared = false;
            }
        }
    }
    GameObject IstantiateStartingRoom(int direction, Vector3 position)//
    {
        Quaternion rotation = Quaternion.identity;
        GameObject newObject;

        direction = InvertDirection(direction);
        rotation = RotationChange(direction, rotation);
        newObject = Instantiate(StartingRoom, position, rotation);
        newObject.transform.position = OffsetBy(direction, newObject.transform.position, -1);
        return newObject;
    }
    public GameObject IstantiateRoomBorders(int[,] roomLayout)//
    {
        Quaternion rotation = Quaternion.identity;
        Vector3 position = new Vector3(0, 0, 0);
        Vector3 startPosition = new Vector3(0, 0, 0);
        int direction = 1;
        GameObject newPiece;
        GameObject roomBorders = new GameObject();
        roomBorders.name = "RoomBorders";
        int[] startPoint = StartPoint(roomLayout);

        int x = startPoint[0]; int y = startPoint[1];
        int counter = 0;
        while (counter < WallAndCornerCounter(roomLayout))
        {
            
            position = PositionChange(direction, position);
            rotation = RotationChange(direction, rotation);

            if (direction == 1)
            {
                y++;
            }
            else if (direction == 2)
            {
                x++;
            }
            else if (direction == 3)
            {
                y--;
            }
            else if (direction == 4)
            {
                x--;
            }

            if (roomLayout[x, y] == 2)
            {
                newPiece = Instantiate(Wall01_corner01, position, rotation);
                newPiece.transform.parent = roomBorders.transform;
                direction = direction + 1;
                if (direction == 5)
                {
                    direction = 1;
                }
            }

            else if (roomLayout[x, y] == 3)
            { 
                newPiece=Instantiate(Wall01_corner02, position, rotation);
                newPiece.transform.parent = roomBorders.transform;
                direction = direction - 1;
                if (direction == 0)
                {
                    direction = 4;
                }
            }

            else if (roomLayout[x, y] == 4)
            {
                newPiece = Instantiate(Wall01_door, position, rotation);
                if (direction == 1)
                {
                    newPiece.name = "Wall01_door_Dir1";
                }
                else if(direction == 2)
                {
                    newPiece.name = "Wall01_door_Dir2";
                }
                else if(direction == 3)
                {
                    newPiece.name = "Wall01_door_Dir3";
                }
                else if (direction == 4)
                {
                    newPiece.name = "Wall01_door_Dir4";
                }
                newPiece.transform.parent = roomBorders.transform;
            }
            else if (roomLayout[x,y] == 1)
            {
                newPiece=Instantiate(Wall01, position, rotation);
                newPiece.transform.parent = roomBorders.transform;
            }
            counter++;
        }

        if (startPoint[2] != 0)
        {
            for(int i=0; i < startPoint[2]; i++)
            {
                position.x = position.x - tileSize;
            }
        }
        roomBorders.transform.position = position;
        return roomBorders;
    }
    GameObject IstantiateRoomFill(int[,] roomLayout)//
    {
        Vector3 position = new Vector3(0, 0, 0);
        Quaternion rotation = Quaternion.identity;
        GameObject newObject;
        GameObject roomFloor = new GameObject();
        roomFloor.name = "RoomFloor";
        int[] startPoint = StartPoint(roomLayout);

        for (int i = 0; i<roomLayout.GetLength(0); i++)
        {
            for (int j = 0; j<roomLayout.GetLength(1); j++)
            {
                if (roomLayout[i, j] == 5)
                {
                    position.x = i * tileSize; position.z=j*tileSize;
                    newObject = Instantiate(FillPiece, position, rotation);
                    newObject.transform.parent = roomFloor.transform;
                }
            }
        }

        rotation = Quaternion.Euler(0, 270, 0);
        roomFloor.transform.rotation = rotation;
        return roomFloor;
    }
    GameObject InstantiateEnemies(int[,] roomLayout)//
    {
        Vector3 position = new Vector3(0, 0, 0);
        Vector3 enemyPos = new Vector3(0, 0, 0);
        Quaternion rotation = Quaternion.identity;
        GameObject newObject;
        GameObject enemyParent = new GameObject();
        enemyParent.transform.name = "Enemies in this room";

        int[,] enemyPositions = PickEnemyPositions(roomLayout);
        for (int i = 0; i < roomLayout.GetLength(0); i++)
        {
            for (int j = 0; j < roomLayout.GetLength(1); j++)
            {
                for (int k = 0; k < enemyPositions.GetLength(0); k++)
                {
                    if (roomLayout[i, j] == 5)
                    {
                        position.x = i * tileSize; position.z = j * tileSize;
                    }
                    if (i == enemyPositions[k, 0] && j == enemyPositions[k, 1])
                    {
                        enemyPos = position;
                        enemyPos.y = position.y;
                        newObject = Instantiate(Enemy01, enemyPos, Quaternion.identity);
                        newObject.transform.parent = enemyParent.transform;
                        EnemyVision enemyVision = newObject.GetComponentInChildren<EnemyVision>();
                        enemyVision.Player = startRoom.transform.Find("Player").transform;
                        FollowCamera followCamera = newObject.GetComponentInChildren<FollowCamera>();
                        followCamera.Player = startRoom.transform.Find("Player").transform;
                    }
                }
            }
        }

        rotation = Quaternion.Euler(0, 270, 0);
        enemyParent.transform.rotation = rotation;
        return enemyParent;
    }

    int[,] PickEnemyPositions(int[,] roomLayout)//
    {
        int counter=0;
        System.Random rnd = new System.Random();
        

        for (int i = 0; i < roomLayout.GetLength(0); i++)
        {
            for (int j = 0; j < roomLayout.GetLength(1); j++)
            {
                if (roomLayout[i, j] == 5)
                {
                    counter++;
                }
            }
        }
        int[,] possiblePlaces = new int[counter, 2];

        int counterX = 0; int counterY = 0;
        for (int i = 0; i < roomLayout.GetLength(0); i++)
        {
            for (int j = 0; j < roomLayout.GetLength(1); j++)
            {
                if (roomLayout[i, j] == 5)
                {
                    possiblePlaces[counterX++, 0] = i; possiblePlaces[counterY++, 1] = j;
                }
            }
        }

        int amount = counter / 3;
        int[,] pickedCoords = new int[amount, 2];

        int[] rememberPick = new int[amount];
        int pick;
        for (int i=0;  i < amount; i++)
        {
            Debug.Log($"Turrets iteration: {i+1}");
            pick = rnd.Next(0,possiblePlaces.GetLength(0));
            bool alreadyPicked = true;
            if (i != 0)
            {
                while (alreadyPicked)
                {
                    alreadyPicked = false;
                    for (int j = 0; j < rememberPick.GetLength(0); j++)
                    {
                        if (rememberPick[j] == pick)
                        {
                            Debug.Log("Turrets in same position!");
                            alreadyPicked = true;
                            pick = rnd.Next(0, possiblePlaces.GetLength(0));
                            break;
                        }
                    }
                }
            }
            
            rememberPick[i] = pick;
            pickedCoords[i, 0] = possiblePlaces[pick, 0]; pickedCoords[i, 1] = possiblePlaces[pick, 1];
        }
        return pickedCoords;
    }

    int[] StartPoint(int[,] roomLayout)//
    {
        int[] startPoint = new int[3];
        int counter = 0;
        for (int i = 0; i < roomLayout.GetLength(0); i++)
        {
            if (roomLayout[0, i] == 0)
            {
                counter++;
            }
            if (roomLayout[0, i] == 2)
            {
                startPoint[0] = 0; startPoint[1] = i; startPoint[2] = counter;
                break;
            }
        }
        return startPoint;
    }

    Vector3[] FindDoorCoords(GameObject roomBorder, int startDirection, int endDirection) //
    {
        Vector3 startDoor = new Vector3(0, 0, 0);
        Vector3 endDoor = new Vector3(0, 0, 0);
        Vector3[] doorCoords = new Vector3[2];
        int children = roomBorder.transform.childCount;
        int direction = 0;

        for (int i = 0; i < children; i++)
        {
            if (roomBorder.transform.GetChild(i).name=="Wall01_door_Dir1")
            {
                direction = 1;
                Debug.Log("DIRECTION 1 TRIGGERED");
                if (direction == startDirection)
                {
                    startDoor = roomBorder.transform.GetChild(i).position;
                }
                if (direction == endDirection)
                {
                    endDoor = roomBorder.transform.GetChild(i).position;
                }
            }
            else if (roomBorder.transform.GetChild(i).name == "Wall01_door_Dir2")
            {
                direction = 2;
                Debug.Log("DIRECTION 2 TRIGGERED");
                if (direction == startDirection)
                {
                    startDoor = roomBorder.transform.GetChild(i).position;
                }
                if (direction == endDirection)
                {
                    endDoor = roomBorder.transform.GetChild(i).position;
                }
            }
            else if (roomBorder.transform.GetChild(i).name=="Wall01_door_Dir3")
            {
                direction = 3;
                Debug.Log("DIRECTION 3 TRIGGERED");
                if (direction == startDirection)
                {
                    startDoor = roomBorder.transform.GetChild(i).position;
                }
                if (direction == endDirection)
                {
                    endDoor = roomBorder.transform.GetChild(i).position;
                }
            }
            else if (roomBorder.transform.GetChild(i).name=="Wall01_door_Dir4")
            {
                direction = 4;
                Debug.Log("DIRECTION 4 TRIGGERED");
                if (direction == startDirection)
                {
                    startDoor = roomBorder.transform.GetChild(i).position;
                }
                if (direction == endDirection)
                {
                    endDoor = roomBorder.transform.GetChild(i).position;
                }
            }
        }

        doorCoords[0] = startDoor; doorCoords[1] = endDoor;
        return doorCoords;
    }
    Vector3 OffsetBy(int direction, Vector3 position, int amount)//
    {
        if (direction == 1)
        {
            position = new Vector3(0, 0, amount);
        }
        else if (direction == 2)
        {
            position = new Vector3(amount, 0, 0);
        }
        else if (direction == 3)
        {
            position = new Vector3(0, 0, -amount);
        }
        else if (direction == 4)
        {
            position = new Vector3(-amount, 0, 0);
        }
        return position;
    }


    
    int WallAndCornerCounter(int[,] roomLayout)//
    {
        int counter = 0;
        for (int i = 0; i < roomLayout.GetLength(0); i++)
        {
            for(int j = 0; j < roomLayout.GetLength(1); j++)
            {
                if (roomLayout[i,j]!=0 && roomLayout[i, j] != 5)
                {
                    counter++;
                }
            }
        }
        return counter;
    }
    Vector3 PositionChange(int direction, Vector3 position)//
    {
        if (direction == 1)
        {
            position.x = position.x - tileSize;
        }
        else if (direction == 2)
        {
            position.z = position.z + tileSize;
        }
        else if (direction == 3)
        {
            position.x = position.x + tileSize;
        }
        else if (direction == 4)
        {
            position.z = position.z - tileSize;
        }
        return position;
    }
    Quaternion RotationChange(int direction, Quaternion rotation)//
    {
        if (direction == 1)
        {
            rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction == 2)
        {
            rotation = Quaternion.Euler(0, 270, 0);
        }
        else if (direction == 3)
        {
            rotation = Quaternion.Euler(0, 360, 0);
        }
        else if (direction == 4)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }
        return rotation;
    }
    /*public int RotateDirection(int direction)
    {
        if (direction == 1)
        {
            direction = 2;
        }
        else if (direction == 2)
        {
            direction = 3;
        }
        else if (direction == 3)
        {
            direction = 4;
        }
        else if (direction == 4)
        {
            direction = 1;
        }
        return direction;
    }*/
    int InvertDirection(int direction)//
    {
        if (direction == 1)
        {
            direction = 3;
        }
        else if (direction == 2)
        {
            direction = 4;
        }
        else if (direction == 3)
        {
            direction = 1;
        }
        else if (direction == 4)
        {
            direction = 2;
        }
        return direction;
    }

    public void PrintArray(int[,] array)
    {
        for (int x = 0; x < array.GetLength(0); x++)
        {
            string row = "";

            for (int y = 0; y < array.GetLength(1); y++)
            {
                row += array[x, y] + " ";
            }

            Debug.Log(row);
        }
    }

}
