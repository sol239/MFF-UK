namespace hw3_Kings_path_on_a_chessboard;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;   // thanks to this I can use .Contains() method on List

public class Set
{
    private List<List<int>> _mySet;
    
    /// <summary>
    /// Constructor of the Set class
    /// </summary>
    public Set()
    {
        _mySet = new List<List<int>>();
    }

    /// <summary>
    /// Checks if the set contains the given element
    /// </summary>
    /// <param name="element">The element to check for in the set</param>
    /// <returns>True if the element is in the set, false otherwise</returns>
    public bool Contains(List<int> element)
    {
        foreach (var list in _mySet)
        {
            if (list.SequenceEqual(element))
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Adds the given element to the set
    /// </summary>
    /// <param name="element">The element to add to the set</param>
    public void Add(List<int> element)
    {
        if (_mySet.Contains(element) == false)
        {
            _mySet.Add(element);
        }
    }
}

/// <summary>
/// This class represents a board read from a file.
/// </summary>
public class BoardFromFile
{
    private string _filePath;
    private List<string> _boardInput = new List<string>();
    
    private string _startSymbol;
    private string _endSymbol;
    private string _yesSymbol;
    private string _noSymbol;
    public List<int> EndLocation = new List<int>(2);
    private List<int> _boardDimensions = new List<int>(2);
    public List<int> StartLocation = new List<int>(2);
    public List<List<string>> Board = new List<List<string>>();
    public List<string> BoardSettings = new List<string>(4);


    
    public BoardFromFile(string filePath, string startSymbol = "S", string endSymbol = "C", string yesSymbol = ".", string noSymbol = "X")
    {
        _filePath = filePath;
        _startSymbol = startSymbol;
        _endSymbol = endSymbol;
        _yesSymbol = yesSymbol;
        _noSymbol = noSymbol;
        BoardSettings = new List<string> {yesSymbol, noSymbol, startSymbol, endSymbol};
    }

    /// <summary>
    /// Reads the file and stores the data.
    /// </summary>
    public void ReadFile()
    {
        using (StreamReader sr = new StreamReader(_filePath))
        {
            string firstLine = sr.ReadLine();
            _boardInput.Add(firstLine);
            int queriesCount = int.Parse(firstLine);
            
            queriesCount += 1;
            for (int i = 0; i < queriesCount; i++)
            {
                string line = sr.ReadLine();
                _boardInput.Add(line);
            }
        }
    }
    
    /// <summary>
    /// Gets the dimensions of the board.
    /// </summary>
    public void GetDimensions()
    {
        _boardDimensions = new List<int> {int.Parse(_boardInput[0]), int.Parse(_boardInput[1])};
    }
    
    /// <summary>
    /// Identifies the ending position on the board.
    /// </summary>
    public void GetEnd()
    {
        for (int i = 0; i < _boardInput.Count; i++)
        {
            if (_boardInput[i].Contains(_endSymbol))
            {
                EndLocation = new List<int> { _boardInput[i].IndexOf(_endSymbol), i - 2 };
            }
        }
    }
    
    /// <summary>
    /// Identifies the starting position on the board.
    /// </summary>
    public void GetStart()
    {
        for (int i = 0; i < _boardInput.Count; i++)
        {
            if (_boardInput[i].Contains(_startSymbol))
            {
                StartLocation = new List<int> { _boardInput[i].IndexOf(_startSymbol), i - 2 };
            }
        }
    }

    /// <summary>
    /// Generates the board based on the input data.
    /// </summary>
    public void GenerateBoard()
    {
        for (int i = 2; i < _boardInput.Count; i++)
        {
            List<string> boardRow = new List<string>(_boardDimensions[0]);

            for (int j = 0; j < _boardInput[i].Length; j++)
            {
                boardRow.Add(_boardInput[i][j].ToString());
            }
            Board.Add(boardRow);
        }
    }

    /// <summary>
    /// Clears all the data related to the current board.
    /// </summary>
    public void FlushData()
    {
        _boardInput.Clear();
        _boardDimensions.Clear();
        StartLocation.Clear();
        EndLocation.Clear();
        Board.Clear();
    }
    
    /// <summary>
    /// Prints the current state of the board to the console.
    /// </summary>
    public void PrintBoard()
    {
        foreach (var row in Board)
        {
            Console.WriteLine(string.Join("", row));
        }
    }
}


/// <summary>
/// This class is responsible for solving the board.
/// </summary>
public class Solver
{
    
    private List<int> _start;
    private List<int> _end;
    private List<int> _currentLocation;
    private List<List<string>> _board;
    private List<List<int>> _movesKing;
    private List<int> _boardDimension;
    private List<string> _boardSettings;
    
    /// <summary>
    /// Visualizes the current state of the board.
    /// </summary>
    /// <param name="currentPosition">The current position on the board.</param>
    public void VisualizeBoard(List<int> currentPosition)
    {
        for (int i = 0; i < _board.Count; i++)
        {
            for (int j = 0; j < _board[i].Count; j++)
            {
                if (i == currentPosition[1] && j == currentPosition[0])
                {
                    Console.Write("O ");
                }
                else
                {
                    Console.Write($"{_board[i][j]} ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    
    /// <summary>
    /// Constructor for the Solver class.
    /// </summary>
    /// <param name="start">The starting position on the board.</param>
    /// <param name="end">The ending position on the board.</param>
    /// <param name="board">The board to be solved.</param>
    /// <param name="boardSettings">The settings of the board.</param>
    public Solver(List<int> start, List<int> end, List<List<string>> board, List<string> boardSettings)
    {
        _start = start;
        _end = end;
        _board = board;
        _boardSettings = boardSettings;
        _movesKing = new List<List<int>> {new List<int> {1, 0}, new List<int> {1, 1}, new List<int> {0, 1}, new List<int> {-1, 1}, new List<int> {-1, 0}, new List<int> {-1, -1}, new List<int> {0, -1}, new List<int> {1, -1}};
        _boardDimension = new List<int> {board[0].Count, board.Count};    
    }

    
    /// <summary>
    /// Calculates the reachable positions from the current position.
    /// </summary>
    /// <param name="position">The current position on the board.</param>
    /// <returns>A list of reachable positions.</returns>
    public List<List<int>> CalculateReachablePositions(List<int> position)
    {
        int currentX = position[0];
        int currentY = position[1];

        int boardX = _boardDimension[0] - 1;
        int boardY = _boardDimension[1] - 1;
        
        
        List<List<int>> reachablePositions = new List<List<int>>();
        
        foreach (List<int> move in _movesKing)
        {   
            int nextPositionX = currentX + move[0];
            int nextPositionY = currentY + move[1];
            
            if ((nextPositionX >= 0 && nextPositionX <= boardX) && (nextPositionY >= 0 && nextPositionY <= boardY) && _board[nextPositionY][nextPositionX] != _boardSettings[1])
            {
                reachablePositions.Add(new List<int> {nextPositionX, nextPositionY});            
            }

        }   
        
        return reachablePositions;
    }
    
    /// <summary>
    /// Solves the board using Breadth-First Search (BFS) algorithm.
    /// </summary>
    /// <returns>The number of steps to reach the end position. Returns -1 if the end position is not reachable.</returns>
    public int SolveBoardBFS()
    {
        Queue<(List<int>, int)> bsfQueue = new Queue<(List<int>, int)>();
        
        //HashSet<List<int>> visited = new HashSet<List<int>> { };
        Set visited = new Set();
        bsfQueue.Enqueue((_start, 0));

        //Console.WriteLine("ENTER to start BFS...");
        //Console.ReadLine();

        //Console.WriteLine("Start BFS...");
        //Console.ReadLine();
        while (bsfQueue.Count > 0)
        {
            (List<int> currentPosition, int steps) = bsfQueue.Dequeue();
            
            if (currentPosition[0] == _end[0] && currentPosition[1] == _end[1])
            {
                return steps;
            }
            if (visited.Contains(currentPosition) == false)
            {
                visited.Add(currentPosition);
                _currentLocation = currentPosition;
                foreach (List<int> nextPosition in CalculateReachablePositions(_currentLocation))
                {
                    if (visited.Contains(nextPosition) == false)
                    {
                        bsfQueue.Enqueue((nextPosition, steps + 1));
                        //VisualizeBoard(currentPosition);
                    }
                }
            }
        }
        return -1;
    }
}

class Program
{
    static void Main(string[] args)
    {
        
        // Variant 2
        BoardFromFile board2 = new BoardFromFile("sachovnice.txt");

        
        board2.ReadFile();
        board2.GetDimensions();

        board2.GenerateBoard();
        board2.GetStart();
        board2.GetEnd();
        
        //board2.PrintBoard();
        
        List<int> startLocation = board2.StartLocation;
        List<int> endLocation = board2.EndLocation;
        List<List<string>> board = board2.Board;
        List<string> boardSettings = board2.BoardSettings;
        
        Solver sol2 = new Solver(startLocation, endLocation, board, boardSettings);
        
        Console.WriteLine(sol2.SolveBoardBFS());


    }
}