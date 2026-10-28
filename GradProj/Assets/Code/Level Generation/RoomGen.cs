using System;
using UnityEngine;

public class RoomGen
{
    System.Random rnd = new System.Random();

    int[,] Grid(int maxSize)
    {
        int x = rnd.Next(5, maxSize);
        int y = rnd.Next(5, maxSize);
        int[,] roomLayout = new int[x, y];
        return roomLayout;
    }
    int[,] StartingPoints(int[,] roomLayout)
    {
        int[,] cornerCoords = new int[4, 2];
        
        int upLeftX = rnd.Next(0, (roomLayout.GetLength(0) - 1) / 2 - 1);
        int upLeftY = rnd.Next(0, (roomLayout.GetLength(1) - 1) / 2 - 1);
        cornerCoords[0, 0] = upLeftX; cornerCoords[0, 1] = upLeftY;

        int upRightX = rnd.Next(0, (roomLayout.GetLength(0) - 1) / 2 - 1);
        int upRightY = rnd.Next(((roomLayout.GetLength(1) - 1) / 2) + 1, roomLayout.GetLength(1));
        cornerCoords[1, 0] = upRightX; cornerCoords[1, 1] = upRightY;

        int downRightX = rnd.Next(((roomLayout.GetLength(0) - 1) / 2) + 1, roomLayout.GetLength(0));
        int downRightY = rnd.Next(((roomLayout.GetLength(1) - 1) / 2) + 1, roomLayout.GetLength(1));
        cornerCoords[2, 0] = downRightX; cornerCoords[2, 1] = downRightY;

        int downLeftX = rnd.Next(((roomLayout.GetLength(0) - 1) / 2) + 1, roomLayout.GetLength(0));
        int downLeftY = rnd.Next(0, (roomLayout.GetLength(1) - 1) / 2 - 1);
        cornerCoords[3, 0] = downLeftX; cornerCoords[3, 1] = downLeftY;

        return cornerCoords;
    }

    int[,] GenerateRoom(int[,] roomLayout, int[,] cornerCoords)
    {
        int currX = 0;
        int currY = 0;
        int nextX = 0;
        int nextY = 0;
        int rndDist = 0;

        roomLayout[cornerCoords[0, 0], cornerCoords[0, 1]] = 1;
        roomLayout[cornerCoords[1, 0], cornerCoords[1, 1]] = 1;
        currX = cornerCoords[0, 0]; currY = cornerCoords[0, 1];
        nextX = cornerCoords[1, 0]; nextY = cornerCoords[1, 1];

        //Connecting first and second points:
        rndDist = rnd.Next(currY + 1, nextY - 1);
        while (currY <= rndDist)
        {
            currY++;
            roomLayout[currX, currY] = 1;
        }
        if (currX < nextX)
        {
            if (roomLayout[currX + 1, currY] != 0)
            {
                currY = currY + 1;
                roomLayout[currX, currY] = 1;
            }

            roomLayout[currX, currY] = 2;
            while (currX < nextX)
            {
                currX++;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 3;
        }
        else if (currX > nextX)
        {
            if (roomLayout[currX - 1, currY] != 0)
            {
                currY = currY + 1;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 3;
            while (currX > nextX)
            {
                currX--;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 2;
        }
        while (currY < nextY)
        {
            currY++;
            roomLayout[currX, currY] = 1;
        }

        roomLayout[cornerCoords[2, 0], cornerCoords[2, 1]] = 1;

        currX = cornerCoords[1, 0]; currY = cornerCoords[1, 1];
        nextX = cornerCoords[2, 0]; nextY = cornerCoords[2, 1];

        //Connecting second and third points:
        rndDist = rnd.Next(currX + 1, nextX - 1);
        while (currX < rndDist)
        {
            currX++;
            roomLayout[currX, currY] = 1;
        }
        if (currY < nextY)
        {
            if (roomLayout[currX, currY + 1] != 0)
            {
                currX = currX + 1;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 3;
            while (currY < nextY)
            {
                currY++;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 2;
        }
        else if (currY > nextY)
        {
            if (roomLayout[currX, currY - 1] != 0)
            {
                currX = currX + 1;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 2;
            while (currY > nextY)
            {
                currY--;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 3;
        }
        while (currX < nextX)
        {
            currX++;
            roomLayout[currX, currY] = 1;
        }

        roomLayout[cornerCoords[3, 0], cornerCoords[3, 1]] = 1;

        currX = cornerCoords[2, 0]; currY = cornerCoords[2, 1];
        nextX = cornerCoords[3, 0]; nextY = cornerCoords[3, 1];

        //Connecting third and fourth points:
        rndDist = rnd.Next(nextY + 1, currY - 1);
        while (currY > rndDist)
        {
            currY--;
            roomLayout[currX, currY] = 1;
        }
        if (currX > nextX)
        {
            if (roomLayout[currX - 1, currY] != 0)
            {
                currY = currY - 1;
                roomLayout[currX, currY] = 1;
            }

            roomLayout[currX, currY] = 2;
            while (currX > nextX)
            {
                currX--;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 3;
        }
        else if (currX < nextX)
        {
            if (roomLayout[currX + 1, currY] != 0)
            {
                currY = currY - 1;
                roomLayout[currX, currY] = 1;
            }

            roomLayout[currX, currY] = 3;
            while (currX < nextX)
            {
                currX++;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 2;
        }
        while (currY > nextY)
        {
            currY--;
            roomLayout[currX, currY] = 1;
        }

        currX = cornerCoords[3, 0]; currY = cornerCoords[3, 1];
        nextX = cornerCoords[0, 0]; nextY = cornerCoords[0, 1];

        //Connecting fourth and first points:
        rndDist = rnd.Next(nextX + 1, currX - 1);
        while (currX > rndDist)
        {
            currX--;
            roomLayout[currX, currY] = 1;
        }
        if (currY < nextY)
        {
            if (roomLayout[currX, currY + 1] != 0)
            {
                currX = currX - 1;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 2;
            while (currY < nextY)
            {
                currY++;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 3;
        }
        else if (currY > nextY)
        {
            if (roomLayout[currX, currY - 1] != 0)
            {
                currX = currX - 1;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 3;
            while (currY > nextY)
            {
                currY--;
                roomLayout[currX, currY] = 1;
            }
            roomLayout[currX, currY] = 2;
        }
        while (currX > nextX)
        {
            currX--;
            roomLayout[currX, currY] = 1;
        }

        roomLayout[cornerCoords[0, 0], cornerCoords[0, 1]] = 2;
        roomLayout[cornerCoords[1, 0], cornerCoords[1, 1]] = 2;
        roomLayout[cornerCoords[2, 0], cornerCoords[2, 1]] = 2;
        roomLayout[cornerCoords[3, 0], cornerCoords[3, 1]] = 2;
        roomLayout = AdjustArray(roomLayout);

        FillFloor(roomLayout);
        return roomLayout;
    }

    int[,] AdjustArray(int[,] roomLayout)
    {
        int count = 0;
        int emptyRowCount = 0;
        int emptyColumnCount = 0;
        int[] curr = new int[2];
        int[] next = new int[2];
        bool empty = true;

        while (empty)
        {
            count = 0;
            for (int i = 0; i < roomLayout.GetLength(1); i++)
            {
                if (roomLayout[0, i] == 0)
                {
                    count++;
                }
            }

            if (count == roomLayout.GetLength(1))
            {
                for (int i = 1; i < roomLayout.GetLength(0); i++)
                {
                    for (int j = 0; j < roomLayout.GetLength(1); j++)
                    {
                        roomLayout[i - 1, j] = roomLayout[i, j];
                        roomLayout[i, j] = 0;
                    }
                }
            }
            else
            {
                empty = false;
            }
        }

        empty = true;
        while (empty)
        {
            count = 0;
            for (int i = 0; i < roomLayout.GetLength(0); i++)
            {
                if (roomLayout[i, 0] == 0)
                {
                    count++;
                }
            }
            if (count == roomLayout.GetLength(0))
            {
                for (int i = 0; i < roomLayout.GetLength(0); i++)
                {
                    for (int j = 1; j < roomLayout.GetLength(1); j++)
                    {
                        roomLayout[i, j - 1] = roomLayout[i, j];
                        roomLayout[i, j] = 0;
                    }
                }
            }
            else
            {
                empty = false;
            }
        }

        empty = true;
        while (empty)
        {
            count = 0;
            for (int i = 0; i < roomLayout.GetLength(1); i++)
            {
                if (roomLayout[roomLayout.GetLength(0) - 1 - emptyRowCount, i] == 0)
                {
                    count++;
                }
            }

            if (count == roomLayout.GetLength(1))
            {
                emptyRowCount++;
            }
            else
            {
                empty = false;
            }
        }

        empty = true;
        while (empty)
        {
            count = 0;
            for (int i = 0; i < roomLayout.GetLength(0); i++)
            {
                if (roomLayout[i, roomLayout.GetLength(1) - 1 - emptyColumnCount] == 0)
                {
                    count++;
                }
            }
            if (count == roomLayout.GetLength(0))
            {
                emptyColumnCount++;
            }
            else
            {
                empty = false;
            }
        }

        int[,] tidyLayout = new int[roomLayout.GetLength(0) - emptyRowCount,
            roomLayout.GetLength(1) - emptyColumnCount];
        for (int i = 0; i < tidyLayout.GetLength(0); i++)
        {
            for (int j = 0; j < tidyLayout.GetLength(1); j++)
            {
                tidyLayout[i, j] = roomLayout[i, j];
            }
        }

        return tidyLayout;
    }
    public void FillFloor(int[,] roomLayout)
    {
        for (int i = 0; i < roomLayout.GetLength(0); i++)
        {
            for (int j = 0; j < roomLayout.GetLength(1); j++)
            {
                if (roomLayout[i, j] == 0)
                {
                    roomLayout[i, j] = 5;
                }
            }
        }

        if (roomLayout[0, 0] == 5)
        {
            Expand(roomLayout, 0, 0);
        }
        if (roomLayout[0, roomLayout.GetLength(1) - 1] == 5)
        {
            Expand(roomLayout, 0, roomLayout.GetLength(1) - 1);
        }
        if (roomLayout[roomLayout.GetLength(0) - 1, roomLayout.GetLength(1) - 1] == 5)
        {
            Expand(roomLayout, roomLayout.GetLength(0) - 1, roomLayout.GetLength(1) - 1);
        }
        if (roomLayout[roomLayout.GetLength(0) - 1, 0] == 5)
        {
            Expand(roomLayout, roomLayout.GetLength(0) - 1, 0);
        }
    }

    void Expand(int[,] roomLayout, int x, int y)
    {
        if (x < 0 || x >= roomLayout.GetLength(0) || y < 0 || y >= roomLayout.GetLength(1) || roomLayout[x, y] != 5)
        {
            return;
        }
        roomLayout[x, y] = 0;
        Expand(roomLayout, x - 1, y);
        Expand(roomLayout, x + 1, y);
        Expand(roomLayout, x, y - 1);
        Expand(roomLayout, x, y + 1);
    }
    int[,] FindPossible(int[,] roomLayout)
    {
        int longest = 0;
        if (roomLayout.GetLength(0) > roomLayout.GetLength(1))
        {
            longest = roomLayout.GetLength(0) + 1;
        }
        else
        {
            longest = roomLayout.GetLength(1) + 1;
        }
        int[,] possibleCoords = new int[4, longest];
        int counter;


        counter = 0;
        for (int i = 0; i < roomLayout.GetLength(1); i++) //Possible cells for TOP
        {
            if (roomLayout[0, i] == 1)
            {
                possibleCoords[0, counter++] = i;
            }
            else
            {
                continue;
            }
        }

        counter = 0;
        for (int i = 0; i < roomLayout.GetLength(0); i++) //Possible cells for RIGHT
        {
            if (roomLayout[i, roomLayout.GetLength(1) - 1] == 1)
            {
                possibleCoords[1, counter++] = i;
            }
            else
            {
                continue;
            }
        }

        counter = 0;
        for (int i = 0; i < roomLayout.GetLength(1); i++) //Possible cells for BOTTOM
        {
            if (roomLayout[roomLayout.GetLength(0) - 1, i] == 1)
            {
                possibleCoords[2, counter++] = i;
            }
            else
            {
                continue;
            }
        }

        counter = 0;
        for (int i = 0; i < roomLayout.GetLength(0); i++) //Possible cells for LEFT
        {
            if (roomLayout[i, 0] == 1)
            {
                possibleCoords[3, counter++] = i;
            }
            else
            {
                continue;
            }
        }

        counter = 0;
        for (int i = 0; i < possibleCoords.GetLength(0); i++)      
        {
            counter = 0;
            for (int j = 0; j < possibleCoords.GetLength(1); j++)
            {
                if (possibleCoords[i, j] == 0)
                {
                    counter++;
                }
            }
            if (counter == possibleCoords.GetLength(1))
            {
                possibleCoords[i, possibleCoords.GetLength(1) - 1] = 2;
            }
            else
            {
                possibleCoords[i, possibleCoords.GetLength(1) - 1] = 1;
            }
        }
        return possibleCoords;
    }
    int pickTopOrLeft(int number, int i, int[,] possibleCoords)
    {
        return number = possibleCoords[i, 0];
    }
    int pickBottomOrRight(int number, int i, int[,] possibleCoords)
    {
        int largest = possibleCoords[i, 0];
        for (int j = 0; j < possibleCoords.GetLength(1) - 1; j++)
        {
            if (possibleCoords[i, j] > largest)
            {
                largest = possibleCoords[i, j];
            }
        }
        return number = largest;
    }
    int[,] PickPossible(int[,] roomLayout, int[,] possibleCoords, int startDir, int endDir)
    {
        int number = 0;
        int[,] pickedCoords = new int[4, 2];
        for (int i = 0; i < possibleCoords.GetLength(0); i++)
        {
            if (possibleCoords[i, possibleCoords.GetLength(1) - 1] == 2)
            {
                continue;
            }
            else if (possibleCoords[i, possibleCoords.GetLength(1) - 1] == 1)
            {
                if (i == 0)
                {
                    if (i == startDir - 1)
                    {
                        number = pickTopOrLeft(number, i, possibleCoords);
                    }
                    else if (i == endDir - 1)
                    {
                        number = pickTopOrLeft(number, i, possibleCoords);
                    }
                }
                if (i == 1)
                {
                    if (i == startDir - 1)
                    {
                        number = pickTopOrLeft(number, i, possibleCoords);
                    }
                    else if (i == endDir - 1)
                    {
                        number = pickTopOrLeft(number, i, possibleCoords);
                    }
                }
                if (i == 2)
                {
                    if (i == startDir - 1)
                    {
                        number = pickBottomOrRight(number, i, possibleCoords);
                    }
                    else if (i == endDir - 1)
                    {
                        number = pickBottomOrRight(number, i, possibleCoords);
                    }
                }
                if (i == 3)
                {
                    if (i == startDir - 1)
                    {
                        number = pickBottomOrRight(number, i, possibleCoords);
                    }
                    else if (i == endDir - 1)
                    {
                        number = pickBottomOrRight(number, i, possibleCoords);
                    }
                }

            }

            if (i == 0)
            {
                pickedCoords[0, 0] = 0; pickedCoords[0, 1] = number;
            }
            else if (i == 1)
            {
                pickedCoords[1, 0] = number; pickedCoords[1, 1] = roomLayout.GetLength(1) - 1;
            }
            else if (i == 2)
            {
                pickedCoords[2, 0] = roomLayout.GetLength(0) - 1; pickedCoords[2, 1] = number;
            }
            else if (i == 3)
            {
                pickedCoords[3, 0] = number; pickedCoords[3, 1] = 0;
            }
        }
        return pickedCoords;
    }

    bool Validator(int[,] possibleCoords, int direction)
    {

        bool tempCheck = false;

        for (int i = 0; i < possibleCoords.GetLength(0); i++)
        {
            if (i == direction - 1)
            {
                if (possibleCoords[i, possibleCoords.GetLength(1) - 1] == 2)
                {
                    tempCheck = false;
                }
                else { tempCheck = true; }
            }
        }

        if (tempCheck)
        {
            return true;
        }
        else return false;
    }

    public int[,] GenerateValid(int startDirection, int endDirection, int maxSize)
    {
        int[,] roomSize;
        int[,] cornerCoords;
        int[,] roomLayout;
        int[,] possible;
        int[,] pickedPossible;
        do
        {
            roomSize = Grid(maxSize);
            cornerCoords = StartingPoints(roomSize);
            roomLayout = GenerateRoom(roomSize, cornerCoords);
            possible = FindPossible(roomLayout);
        }
        while (Validator(possible, startDirection) == false || 
        Validator(possible, endDirection) == false);
        pickedPossible = PickPossible(roomLayout, possible, startDirection, endDirection);
        PlaceDoors(roomLayout, pickedPossible, startDirection, endDirection);
        return roomLayout;
    }
    void PlaceDoors(int[,] roomLayout, int[,] pickedPossible, int startDirection, int endDirection)
    {
        int direction = 0;
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                direction = startDirection;
            }
            if (i == 1)
            {
                direction = endDirection;
            }

            if (direction == 1)
            {
                roomLayout[pickedPossible[0, 0], pickedPossible[0, 1]] = 4;
            }
            else if (direction == 2)
            {
                roomLayout[pickedPossible[1, 0], pickedPossible[1, 1]] = 4;
            }
            else if (direction == 3)
            {
                roomLayout[pickedPossible[2, 0], pickedPossible[2, 1]] = 4;
            }
            else if (direction == 4)
            {
                roomLayout[pickedPossible[3, 0], pickedPossible[3, 1]] = 4;
            }
        }
    }
    int[,] FindExistingDoors(int[,] roomLayout, int startDirection, int endDirection)
    {
        int[,] doors = new int[2, 2];
        int direction = 0;
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                direction = startDirection;
            }
            else if (i == 1)
            {
                direction = endDirection;
            }

            if (direction == 1)
            {
                for (int j = 0; j < roomLayout.GetLength(1); j++)
                {
                    if (roomLayout[0, j] == 4)
                    {
                        doors[i, 0] = 0; doors[i, 1] = j;
                    }
                }
            }
            else if (direction == 2)
            {
                for (int j = 0; j < roomLayout.GetLength(0); j++)
                {
                    if (roomLayout[j, roomLayout.GetLength(1) - 1] == 4)
                    {
                        doors[i, 0] = j; doors[i, 1] = roomLayout.GetLength(1) - 1;
                    }
                }
            }
            else if (direction == 3)
            {
                for (int j = roomLayout.GetLength(1) - 1; j > 0; j--)
                {
                    if (roomLayout[roomLayout.GetLength(0) - 1, j] == 4)
                    {
                        doors[i, 0] = roomLayout.GetLength(0) - 1; doors[i, 1] = j;
                    }
                }
            }
            else if (direction == 4)
            {
                for (int j = roomLayout.GetLength(0) - 1; j > 0; j--)
                {
                    if (roomLayout[j, 0] == 4)
                    {
                        doors[i, 0] = j; doors[i, 1] = 0;
                    }
                }
            }
        }
        return doors;
    }

}
