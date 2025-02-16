using System;
using System.Collections.Generic;
using UnityEngine;

public enum Square {
    Top, Bottom, Left, Right, Front, Back
};

public class CubeOrient {
    public static int SquareSize = 7;

    public Square square;
    public Vector2Int pos;
    public Vector2Int dir;

    public Square upSquare;
    public bool goingInWorldUp;

    public CubeOrient() {
        square = Square.Left;
        pos = new Vector2Int(SquareSize/2, SquareSize/2);
        dir = new Vector2Int(1, 0);
    }
    public static CubeOrient Copy(CubeOrient cubeOrient) {
        CubeOrient co = new CubeOrient();
        co.square = cubeOrient.square;
        co.pos = cubeOrient.pos;
        co.dir = cubeOrient.dir;
        co.upSquare = cubeOrient.upSquare;
        co.goingInWorldUp = cubeOrient.goingInWorldUp;
        return co;
    }
    public void RandomizePosition() {
        square = (Square)UnityEngine.Random.Range(0, 6);
        pos = new Vector2Int(UnityEngine.Random.Range(0, SquareSize), UnityEngine.Random.Range(0, SquareSize));
    }

    // Returns the direction of movement for the cube orient to move towards the world up.
    public Vector2Int WorldUp() {
        // This is fully genuinely the worst code I have ever written in my life. May god forgive me
        CubeOrient t1 = Copy(this);
        CubeOrient t2 = Copy(this);
        CubeOrient t3 = Copy(this);
        CubeOrient t4 = Copy(this);
        t1.dir = Vector2Int.up;
        t2.dir = Vector2Int.down;
        t3.dir = Vector2Int.left;
        t4.dir = Vector2Int.right;
        for (int i = 0; i < 1000; i++) {
            t1.Go(false);
            t2.Go(false);
            t3.Go(false);
            t4.Go(false);
            if (t1.square == upSquare) { return Vector2Int.up; }
            if (t2.square == upSquare) { return Vector2Int.down; }
            if (t3.square == upSquare) { return Vector2Int.left; }
            if (t4.square == upSquare) { return Vector2Int.right; }
        }
        Debug.Log("EVIL " + ToString());
        return Vector2Int.zero;
    }

    public Vector2Int WorldDown() {
        return Opposite(WorldUp());
    }

    public Vector2Int WorldLeft() {
        return Left(WorldUp());
    }

    public Vector2Int WorldRight() {
        return Right(WorldUp());
    }

    // Transform a vector by 90 degrees to the left.
    public static Vector2Int Left(Vector2Int vec) {
        return new Vector2Int(-vec.y, vec.x);
    }
    // Transform a vector by 90 degrees to the right.
    public static Vector2Int Right(Vector2Int vec) {
        return new Vector2Int(vec.y, -vec.x);
    }
    // Transform a vector by 180 degrees.
    public static Vector2Int Opposite(Vector2Int vec) {
        return new Vector2Int(-vec.x, -vec.y);
    }

    public void UpInput() {
        Vector2Int newDir = WorldUp();
        if (newDir != Opposite(dir)) {
            dir = newDir;
            goingInWorldUp = true;
        }
    }
    public void DownInput() {
        Vector2Int newDir = Opposite(WorldUp());
        if (newDir != Opposite(dir)) {
            dir = newDir;
            goingInWorldUp = false;
        }
    }
    public void LeftInput() {
        Vector2Int newDir = Left(WorldUp());
        if (newDir != Opposite(dir)) {
            dir = newDir;
            goingInWorldUp = false;
        }
    }
    public void RightInput() {
        Vector2Int newDir = Right(WorldUp());
        if (newDir != Opposite(dir)) {
            dir = newDir;
            goingInWorldUp = false;
        }
    }

    // Returns the square that is to a direction of the current square.
    Square ToLeft() {
        if (square == Square.Right) { return Square.Front; }
        if (square == Square.Left) { return Square.Back; }
        return Square.Left;
    }
    Square ToRight() {
        if (square == Square.Right) { return Square.Back; }
        if (square == Square.Left) { return Square.Front; }
        return Square.Right;
    }
    Square Above() {
        if (square == Square.Top) { return Square.Back; }
        if (square == Square.Back) { return Square.Bottom; }
        if (square == Square.Bottom) { return Square.Front; }
        return Square.Top;
    } 
    Square Below() {
        if (square == Square.Top) { return Square.Front; }
        if (square == Square.Bottom) { return Square.Back; }
        if (square == Square.Back) { return Square.Top; }
        return Square.Bottom;
    }
    Square Next() {
        if (dir == Vector2.up) { return Above(); }
        if (dir == Vector2.down) { return Below(); }
        if (dir == Vector2.left) { return ToLeft(); }
        if (dir == Vector2.right) { return ToRight(); }
        return square;
    }

    public void Go(bool check = true) {
        Square prev = square;
        Vector2Int prevDir = dir, prevWorldUp = Vector2Int.zero;
        if (check) { prevWorldUp = WorldUp(); }
        if (dir == Vector2Int.up) { GoUp(); }
        else if (dir == Vector2Int.down) { GoDown(); }
        else if (dir == Vector2Int.left) { GoLeft(); }
        else if (dir == Vector2Int.right) { GoRight(); }
        if (check) {
            if (square != prev && prevDir == prevWorldUp) { upSquare = Next(); }
            else if (square != prev && prevDir == -prevWorldUp) { upSquare = prev; }
        }
    }

    void GoUp() {
        pos.y++;
        if (pos.y < SquareSize) { return; }
        if (square == Square.Left) {
            pos = new Vector2Int(0, SquareSize - 1 - pos.x);
            dir = Vector2Int.right;
        }
        else if (square == Square.Right) {
            pos = new Vector2Int(SquareSize - 1, pos.x);
            dir = Vector2Int.left;
        }
        else { pos.y = 0; }
        square = Above();
    }
    void GoDown() {
        pos.y--;
        if (pos.y >= 0) { return; }
        if (square == Square.Left) {
            pos = new Vector2Int(0, pos.x);
            dir = Vector2Int.right;
        }
        else if (square == Square.Right) {
            pos = new Vector2Int(SquareSize - 1, SquareSize - pos.x - 1);
            dir = Vector2Int.left;
        }
        else { pos.y = SquareSize - 1; }
        square = Below();
    }
    void GoLeft() {
        pos.x--;
        if (pos.x >= 0) { return; }
        else if (square == Square.Top) {
            pos = new Vector2Int(SquareSize - 1 - pos.y, SquareSize - 1);
            dir = Vector2Int.down;
        }
        else if (square == Square.Bottom) {
            pos = new Vector2Int(pos.y, 0);
            dir = Vector2Int.up;
        }
        else if (square == Square.Left) {
            pos = new Vector2Int(0, SquareSize - 1 - pos.y);
            dir = Vector2Int.right;
        }
        else if (square == Square.Back) {
            pos = new Vector2Int(0, SquareSize - 1 - pos.y);
            dir = Vector2Int.right;
        }
        else { pos.x = SquareSize - 1; }
        square = ToLeft();
    }
    void GoRight() {
        pos.x++;
        if (pos.x < SquareSize) { return; }
        else if (square == Square.Top) {
            pos = new Vector2Int(pos.y, SquareSize - 1);
            dir = Vector2Int.down;
        }
        else if (square == Square.Bottom) {
            pos = new Vector2Int(SquareSize - 1 - pos.y, 0);
            dir = Vector2Int.up;
        }
        else if (square == Square.Right) {
            pos = new Vector2Int(SquareSize - 1, SquareSize - 1 - pos.y);
            dir = Vector2Int.left;
        }
        else if (square == Square.Back) {
            pos = new Vector2Int(SquareSize - 1, SquareSize - 1 - pos.y);
            dir = Vector2Int.left;
        }
        else { pos.x = 0; }
        square = ToRight();
    }

    public Vector3 WorldPosition(bool onlySide = false) {
        Vector3 vec = Vector3.zero;
        float half = (SquareSize - 1) / 2.0f;
        Vector2 newPos = new Vector2(pos.x, pos.y);
        if (onlySide) {
            newPos.x = half;
            newPos.y = half;
        }
        if (square == Square.Top) {
            vec.y = half + 1;
            vec.x = newPos.x - half;
            vec.z = newPos.y - half;
        }
        if (square == Square.Bottom) {
            vec.y = -half - 1;
            vec.x = newPos.x - half;
            vec.z = half - newPos.y;
        }
        if (square == Square.Back) {
            vec.z = half + 1;
            vec.x = newPos.x - half;
            vec.y = half - newPos.y;
        }
        if (square == Square.Front) {
            vec.z = -half - 1;
            vec.x = newPos.x - half;
            vec.y = newPos.y - half;
        }
        if (square == Square.Left) {
            vec.x = -half - 1;
            vec.z = half - newPos.x;
            vec.y = newPos.y - half;
        }
        if (square == Square.Right) {
            vec.x = half + 1;
            vec.z = newPos.x - half;
            vec.y = newPos.y - half;
        }
        return vec;
    }

    public Vector3 FacePosition(Square face) {
        CubeOrient faceOrient = new CubeOrient();
        faceOrient.square = face;
        return faceOrient.WorldPosition(true);
    }

    public Vector3 SnakeUp() {
        Vector3 facePos = FacePosition(upSquare);
        return facePos - WorldPosition();
    }

    public override string ToString()
    {
        Vector3 wp = WorldPosition();
        return $"Curr: {square.ToString()} Square Pos: ({pos.x},{pos.y}) Dir: ({dir.x},{dir.y}) World pos: ({wp.x},{wp.y},{wp.z}) Up Square: {upSquare.ToString()} {goingInWorldUp}";
    }
}
