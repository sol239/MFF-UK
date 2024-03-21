namespace hw2_Kings_path_on_a_chessboard;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;   // díky tomuhle pragram funguje v ReCodexu

public class Set
{
    public List<List<int>> mySet = new List<List<int>>();

    public Set()
    {
        this.mySet = new List<List<int>>();
    }

    public bool Contain(List<int> element)
    {
        foreach (List<int> subList in mySet)
        {
            if (subList[0] == element[0] && subList[1] == element[1])
            {
                return true;
            }
        }
        return false;
    }
    
    public void Add(List<int> element)
    {
        if (!mySet.Contains(element))
        {
            mySet.Add(element);
        }
    }

}
class Helper
{
    public static void PrintIntLOL(List<List<int>> list, string sep = ", ")
    {
        foreach (var VARIABLE in list)
        {
            Console.WriteLine(string.Join($"{sep}", VARIABLE));
        }
    }

    public static List<List<int>> AppendIntLOL(List<List<int>> stock, List<int> source)
    {
        List<int> copy = new List<int>();

        foreach (var VARIABLE in source)
        {
            copy.Add(VARIABLE);
        }
        
        stock.Add(copy);
        return stock;
    }
}

public class BoardManualInput
{
    public string startSymbol;
    public string endSymbol;
    public string yesSymbol;
    public string noSymbol;
    public List<int> boardDimension = new List<int>(2);
    private List<int> boardSpecs = new List<int>();
    public List<List<string>> board = new List<List<string>>();
    public List<string> boardSettings = new List<string>();
    
    public BoardManualInput( List<int> boardSpecs,List<int> boardDimension, string startSymbol = "S", string endSymbol = "E", string yesSymbol = ".", string noSymbol = "X")
    {
        this.startSymbol = startSymbol;
        this.endSymbol = endSymbol;
        this.yesSymbol = yesSymbol;
        this.noSymbol = noSymbol;
        this.boardDimension = boardDimension;
        this.boardSpecs = boardSpecs;
        this.boardSettings = new List<string> {yesSymbol, noSymbol, startSymbol, endSymbol};    }
    
    public static List<int> ReadBoardSpecification()
    {
        // method which reads input from console according to assignment
        // returns int list
        List<int> ResultList = new List<int>();
        while (true)
        {
            string input = Console.ReadLine();
            
            // break condition #1
            
            
            if (input == "" || input == " " || input == "\n")
            {                
                break;
            }
            
            
            
            string[] SeparatedInput = {};
            SeparatedInput = input.Split(" ");

            foreach (var VARIABLE in SeparatedInput)
            {
                ResultList.Add(int.Parse(VARIABLE));
            }

            // break condition #2
            if (ResultList.Count == ResultList[0] * 2 + 2 * 2 + 1)
            {
                break;
            }
            
        }

        return ResultList;
    }

    private List<List<int>> GetObstacleLocation()
    {
        List<List<int>> obstacleList = new List<List<int>>();
        List<int> obstacleLocation = new List<int>();
        int obstaclesCount = boardSpecs[0];
        List<int> obstacles = boardSpecs.GetRange(1, obstaclesCount * 2);

        int counter = 0;
        foreach (int location in obstacles)
        {
            counter += 1;
            obstacleLocation.Add(location);
            if (counter == 2)
            {
                obstacleList = Helper.AppendIntLOL(obstacleList, obstacleLocation);
                obstacleLocation.Clear();
                counter = 0;
            }
        }

        return obstacleList;
    }
    
    public void PrintBoard(string sep = " ")
    {
        foreach (var row in board)
        {
            Console.WriteLine(string.Join($"{sep}", row));
        }
        Console.WriteLine();
        return;
    }

    /// <summary>
    /// Prints dimension of the board in format int x int.
    /// </summary>
    public void PrintDimensions()
    {
        Console.WriteLine($"board dimensions: {(string.Join(" x ", boardDimension))}");
        Console.WriteLine();
        return;
    }

    public List<int> GetStart()
    {
        return new List<int> {boardSpecs[boardSpecs.Count -4] - 1, boardSpecs[boardSpecs.Count -3] - 1};    }
    public List<int> GetEnd()
    {
        return new List<int> {boardSpecs[boardSpecs.Count -2] - 1, boardSpecs[boardSpecs.Count -1] - 1};    }
    private void CreatePlainBoard()
    {
        
        for (int i = 0; i < boardDimension[1]; i++)
        {
            List<string> row = new List<string>();
            for (int j = 0; j < boardDimension[0]; j++)
            {
                row.Add(yesSymbol);
            }
            board.Add(row);
        }
    }
    public void ConstructBoard()
    {

        CreatePlainBoard();   

        List<List<int>> obstacles = GetObstacleLocation();
        
        
        // inserts obstacles into the board
        foreach (var obstacleLocation in obstacles)
        {
            int x = obstacleLocation[0] - 1;
            int y = obstacleLocation[1] - 1;

            board[y][x] = $"{noSymbol}";
        }
        
        // inserts start and end into the board
        int startX = GetStart()[0];
        int startY = GetStart()[1];
        int endX = GetEnd()[0];
        int endY = GetEnd()[1];

        board[startY][startX] = $"{startSymbol}";
        board[endY][endX] = $"{endSymbol}";
        
        //PrintBoard(board);
        
    }
    
}

public class BoardFromFile
{
    private string filePath;
    private List<string> boardInput = new List<string>();
    public string startSymbol;
    public string endSymbol;
    public string yesSymbol;
    public string noSymbol;
    public List<int> endLocation = new List<int>(2);
    public List<int> boardDimensions = new List<int>(2);
    public List<int> startLocation = new List<int>(2);
    public List<List<string>> board = new List<List<string>>();
    public List<string> boardSettings = new List<string>(4);


    
    public BoardFromFile(string filePath, string startSymbol = "S", string endSymbol = "C", string yesSymbol = ".", string noSymbol = "X")
    {
        this.filePath = filePath;
        //this.boardInput = ["4", "4", "X.XS", ".X.X", ".XXX","..C."];
        this.startSymbol = startSymbol;
        this.endSymbol = endSymbol;
        this.yesSymbol = yesSymbol;
        this.noSymbol = noSymbol;
        this.boardSettings = new List<string> {yesSymbol, noSymbol, startSymbol, endSymbol};

    }

    public void ReadFile()
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                boardInput.Add(line);
            }
            boardInput.TrimExcess();
        }
    }
    
    public void GetDimensions()
    {
        boardDimensions = new List<int> {int.Parse(boardInput[0]), int.Parse(boardInput[1])};
    }
    public void GetEnd()
    {
        for (int i = 0; i < boardInput.Count; i++)
        {
            if (boardInput[i].Contains(endSymbol))
            {
                endLocation = new List<int> { boardInput[i].IndexOf(endSymbol), i - 2 };
            }
        }
    }
    public void GetStart()
    {
        for (int i = 0; i < boardInput.Count; i++)
        {
            if (boardInput[i].Contains(startSymbol))
            {
                startLocation = new List<int> { boardInput[i].IndexOf(startSymbol), i - 2 };
            }
        }
    }

    public void GenerateBoard()
    {
        for (int i = 2; i < boardInput.Count; i++)
        {
            List<string> boardRow = new List<string>();

            for (int j = 0; j < boardInput[i].Length; j++)
            {
                boardRow.Add(boardInput[i][j].ToString());
            }
            board.Add(boardRow);
        }
    }

    public void FlushData()
    {
        boardInput.Clear();
        boardDimensions.Clear();
        startLocation.Clear();
        endLocation.Clear();
        board.Clear();
    }
    public void PrintBoard()
    {
        foreach (var row in board)
        {
            Console.WriteLine(string.Join("", row));
        }
    }
}

public class Solver
{
    
    private List<int> start;
    private List<int> end;
    private List<int> currentLocation;
    private List<List<string>> board;
    private List<List<int>> movesKing;
    private List<int> boardDimension = new List<int>();
    private List<string> boardSettings = new List<string>();
    
    public void VisualizeBoard(List<int> currentPosition)
    {
        for (int i = 0; i < board.Count; i++)
        {
            for (int j = 0; j < board[i].Count; j++)
            {
                if (i == currentPosition[1] && j == currentPosition[0])
                {
                    Console.Write("O ");
                }
                else
                {
                    Console.Write($"{board[i][j]} ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    public Solver(List<int> start, List<int> end, List<List<string>> board, List<string> boardSettings)
    {
        this.start = start;
        this.end = end;
        this.board = board;
        this.boardSettings = boardSettings;
        this.currentLocation = currentLocation;
        movesKing = new List<List<int>> {new List<int> {1, 0}, new List<int> {1, 1}, new List<int> {0, 1}, new List<int> {-1, 1}, new List<int> {-1, 0}, new List<int> {-1, -1}, new List<int> {0, -1}, new List<int> {1, -1}};
        this.boardDimension = new List<int> {board[0].Count, board.Count};    }

    public List<List<int>> CalculateReachablePositions(List<int> position)
    {
        int currentX = position[0];
        int currentY = position[1];

        int boardX = boardDimension[0] - 1;
        int boardY = boardDimension[1] - 1;
        
        
        List<List<int>> reachablePositions = new List<List<int>>();
        
        foreach (List<int> move in movesKing)
        {   
            int nextPositionX = currentX + move[0];
            int nextPositionY = currentY + move[1];
            
            //Console.Write($"{move[0]} {move[1]} => {nextPositionX}, {nextPositionY} | ");
            // Console.WriteLine($"{nextPositionX}, {nextPositionY}");
            // Console.WriteLine($"{nextPositionX} : {boardX}");
            // Console.WriteLine($"{nextPositionY} : {boardY}");
            // Console.WriteLine();
            
            if ((nextPositionX >= 0 && nextPositionX <= boardX) && (nextPositionY >= 0 && nextPositionY <= boardY) && board[nextPositionY][nextPositionX] != boardSettings[1])
            {
                reachablePositions.Add(new List<int> {nextPositionX, nextPositionY});            }

        }   
        
        return reachablePositions;
    }

    public int SolveBoardBFS()
    {
        Queue<(List<int>, int)> bsfQueue = new Queue<(List<int>, int)>();
        
        //HashSet<List<int>> visited = new HashSet<List<int>> { };
        Set visited = new Set();
        bsfQueue.Enqueue((start, 0));

        //Console.WriteLine("ENTER to start BFS...");
        //Console.ReadLine();

        List<int> Counter = new List<int>();
        //Console.WriteLine("Start BFS...");
        //Console.ReadLine();
        while (bsfQueue.Count > 0)
        {
            (List<int> currentPosition, int steps) = bsfQueue.Dequeue();
            
            if (currentPosition[0] == end[0] && currentPosition[1] == end[1])
            {
                return steps;
            }

            if (visited.Contain(currentPosition) == false)
            {
                visited.Add(currentPosition);
                currentLocation = currentPosition;
                foreach (List<int> nextPosition in CalculateReachablePositions(currentLocation))
                {
                    if (visited.Contain(nextPosition) == false)
                    {
                        bsfQueue.Enqueue((nextPosition, steps + 1));
                        
                        //Counter.Add(1);
                        //VisualizeBoard(currentPosition);
                        //Console.ReadLine();

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
     
        /*
        //Variant 1
        List<int> boardSpecs = BoardManualInput.ReadBoardSpecification();
        //List<int> boardSpecs = [2,3,3, 2, 1, 1, 1, 2, 2];   // expected input from ReCodEx
        
        // board initialization
        BoardManualInput board1 = new BoardManualInput(boardSpecs, new List<int> {8, 8});        board1.ConstructBoard();
        
        // board1.PrintBoard();
        // board1.PrintDimensions();
        
        Solver sol1 = new Solver(board1.GetStart(), board1.GetEnd(), board1.board, board1.boardSettings);
        
        // List<List<int>> possibleMoves = sol1.CalculateReachablePositions([1, 1]);
        
        Console.WriteLine(sol1.SolveBoardBFS());
        
        //Helper.PrintIntLOL(possibleMoves);
        */
        
        // Variant 2
        BoardFromFile board2 = new BoardFromFile("sachovnice.txt");
        board2.ReadFile();
        board2.GenerateBoard();
        board2.GetDimensions();
        board2.GetStart();
        board2.GetEnd();
        
        //board2.PrintBoard();
        
        List<int> startLocation = board2.startLocation;
        List<int> endLocation = board2.endLocation;
        List<List<string>> board = board2.board;
        List<string> boardSettings = board2.boardSettings;
        

        Solver sol2 = new Solver(startLocation, endLocation, board, boardSettings);
        
        Console.WriteLine(sol2.SolveBoardBFS());


    }
}