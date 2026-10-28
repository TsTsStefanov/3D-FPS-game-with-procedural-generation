using System.Collections.Generic;
using UnityEngine;

public class LayoutGen
{
    public System.Random rnd = new System.Random();
    public int[,] Map(int roomAmount)
    {
        int size;
        if ((roomAmount + 2) % 2 != 0)
        {
            size = roomAmount + 2;
        }
        else
        {
            size = roomAmount + 3;
        }
        int[,] arr = new int[size, size];
        return arr;
    }
    int InvertDirection(int direction)
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
    void CheckNeighbors(int[,] arr, int x, int y, int direction)
    {
        direction = InvertDirection(direction);
        if (x != 0)
        {
            if (arr[x - 1, y] != 1)
            {
                arr[x - 1, y] = 2;
            }
            if (direction == 1)
            {
                if (y != arr.GetLength(1) - 1)
                {
                    if (arr[x - 1, y + 1] != 1) { arr[x - 1, y + 1] = 2; }
                }
                if (y != 0)
                {
                    if (arr[x - 1, y - 1] != 1) { arr[x - 1, y - 1] = 2; }
                }
            }
        }
        if (y != arr.GetLength(1) - 1)
        {
            if (arr[x, y + 1] != 1)
            {
                arr[x, y + 1] = 2;
            }
            if (direction == 2)
            {
                if (x != arr.GetLength(0) - 1)
                {
                    if (arr[x + 1, y + 1] != 1) { arr[x + 1, y + 1] = 2; }
                }
                if (x != 0)
                {
                    if (arr[x - 1, y + 1] != 1) { arr[x - 1, y + 1] = 2; }
                }
            }
        }
        if (x != arr.GetLength(0) - 1)
        {
            if (arr[x + 1, y] != 1)
            {
                arr[x + 1, y] = 2;
            }
            if (direction == 3)
            {
                if (y != arr.GetLength(1) - 1)
                {
                    if (arr[x + 1, y + 1] != 1) { arr[x + 1, y + 1] = 2; }
                }
                if (y != 0)
                {
                    if (arr[x + 1, y - 1] != 1) { arr[x + 1, y - 1] = 2; }
                }
            }
        }
        if (y != 0)
        {
            if (arr[x, y - 1] != 1)
            {
                arr[x, y - 1] = 2;
            }
            if (direction == 4)
            {
                if (x != arr.GetLength(0) - 1)
                {
                    if (arr[x + 1, y - 1] != 1) { arr[x + 1, y - 1] = 2; }
                }
                if (x != 0)
                {
                    if (arr[x - 1, y - 1] != 1) { arr[x - 1, y - 1] = 2; }
                }
            }
        }
    }
    public int[] RandomWalk(int[,] arr, int iterationAmount)
    {
        int[] directions = new int[iterationAmount + 1];
        int currDir;

        int x = arr.GetLength(0) / 2;
        int y = arr.GetLength(1) / 2;
        int prevX = x;
        int prevY = y;
        arr[x, y] = 1;

        Stack<int[]> visited = new Stack<int[]>();
        visited.Push(new int[] { x, y });

        int iteration = 0;
        while (iteration < iterationAmount)
        {
            bool upBlocked = (x == 0 || arr[x - 1, y] != 0);
            bool rightBlocked = (y == arr.GetLength(1) - 1 || arr[x, y + 1] != 0);
            bool downBlocked = (x == arr.GetLength(0) - 1 || arr[x + 1, y] != 0);
            bool leftBlocked = (y == 0 || arr[x, y - 1] != 0);

            if (upBlocked && rightBlocked && downBlocked && leftBlocked)
            {
                if (visited.Count <= 1)
                {
                    break;
                }

                int[] currentCoords = visited.Pop();
                arr[currentCoords[0], currentCoords[1]] = 0;

                int[] prevCoords = visited.Peek();
                x = prevCoords[0]; y = prevCoords[1];
                prevX = x; prevY = y;

                continue;
            }

            currDir = rnd.Next(1, 5);
            if (currDir == 1)
            {
                if (x == 0 || arr[x - 1, y] != 0) { continue; }
                if (iteration == 0)
                {
                    arr[x, y + 1] = 2; arr[x, y - 1] = 2;
                    arr[x + 1, y] = 2; arr[x + 1, y + 1] = 2; arr[x + 1, y - 1] = 2;
                }
                x--;
            }
            else if (currDir == 2)
            {
                if (y == arr.GetLength(1) - 1 || arr[x, y + 1] != 0) { continue; }
                if (iteration == 0)
                {
                    arr[x - 1, y] = 2; arr[x + 1, y] = 2; arr[x, y - 1] = 2;
                    arr[x - 1, y - 1] = 2; arr[x + 1, y - 1] = 2;
                }
                y++;
            }
            else if (currDir == 3)
            {
                if (x == arr.GetLength(0) - 1 || arr[x + 1, y] != 0) { continue; }
                if (iteration == 0)
                {
                    arr[x, y + 1] = 2; arr[x, y - 1] = 2;
                    arr[x - 1, y] = 2; arr[x - 1, y + 1] = 2; arr[x - 1, y - 1] = 2;
                }
                x++;
            }
            else if (currDir == 4)
            {
                if (y == 0 || arr[x, y - 1] != 0) { continue; }
                if (iteration == 0)
                {
                    arr[x - 1, y] = 2; arr[x + 1, y] = 2; arr[x, y + 1] = 2;
                    arr[x - 1, y + 1] = 2; arr[x + 1, y + 1] = 2;
                }
                y--;
            }

            if (iteration > 0)
            {
                CheckNeighbors(arr, prevX, prevY, currDir);
            }

            arr[x, y] = 1;
            prevX = x;
            prevY = y;

            visited.Push(new int[] { x, y });

            directions[iteration] = currDir;
            iteration++;
        }

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                arr[i, j] = 0;
            }
        }
        
        return directions;
    }
}
