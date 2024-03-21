namespace hw04_Water_transfer;
using System;
using System.Collections.Generic;
using System.Linq;  

/// <summary>
/// Represents a set of elements
/// </summary>
public class Set
{
    private List<List<int>> _mySet;
    
    /// <summary>
    /// Constructor of the Set class
    /// </summary>
    public Set()
    {
        this._mySet = new List<List<int>>();
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
/// Class for solving the water transfer problem
///
/// </summary>
class Solver
{
    private int _capacityA;
    private int _currentAmmountA;
    private int _capacityB;
    private int _currentAmmountB;
    private int _capacityC;
    private int _currentAmountC;
    private List<int> _initialState;
    
    /// <summary>
    /// Constructor of the Solver class. Initializes the fields.
    /// </summary>
    /// <param name="capacityA">the capacity of the cup A</param>
    /// <param name="currentAmmountA">the initial amount in the cup A</param>
    /// <param name="capacityB">the capacity of the cup B</param>
    /// <param name="currentAmmountB">the initial amount in the cup B</param>
    /// <param name="capacityC">the capacity of the cup C</param>
    /// <param name="currentAmountC">the initial amount in the cup C</param>
    public Solver(int capacityA, int currentAmmountA, int capacityB, int currentAmmountB, int capacityC, int currentAmountC)
    {
        _capacityA = capacityA;
        _currentAmmountA = currentAmmountA;
        _capacityB = capacityB;
        _currentAmmountB = currentAmmountB;
        _capacityC = capacityC;
        _currentAmountC = currentAmountC;
        _initialState = new List<int> {_capacityA, _currentAmmountA, _capacityB, _currentAmmountB, _capacityC, _currentAmountC};    
    }
    
    
    /// <summary>
    /// Recalculates the amount of water in the cups
    /// </summary>
    /// <param name="volumesAndCapacities">A list of the current volumes and capacities of the cups</param>
    /// <returns>A tuple of the new amounts in the cups</returns>
    private (int, int) Flow(List<int> volumesAndCapacities)
    {
        int toCurrentAmount = volumesAndCapacities[1];
        int fromCurrentAmount = volumesAndCapacities[3];

        while (fromCurrentAmount > 0 & toCurrentAmount < volumesAndCapacities[0])
        {
            toCurrentAmount += 1;
            fromCurrentAmount -= 1;
        }

        return (toCurrentAmount, fromCurrentAmount);
    }

    /// <summary>
    /// Calculates the next possible states of the cups
    /// </summary>
    /// <param name="currentState">The current state of the cups</param>
    /// <returns>A list of the next possible states</returns>
    private List<List<int>> CalculateNextStates (List<int> currentState)
    {
        List<List<int>> nextStates = new List<List<int>>();
        
        // preparing the variables
        int capacityA = currentState[0];
        int currentAmountA = currentState[1];
        int capacityB = currentState[2];
        int currentAmountB = currentState[3];
        int capacityC = currentState[4];
        int currentAmountC = currentState[5];

        int aCurrentAmountNext;
        int bCurrentAmountNext;
        int cCurrentAmountNext;
        
        // A -> B ... moves water from the cup A to the cup B
        List<int> aToB = new List<int> {capacityA, currentAmountA,capacityB, currentAmountB };   // List of varuables to be used in the Flow function
        (aCurrentAmountNext, bCurrentAmountNext) = Flow(aToB);   // The result of the Flow function
        nextStates.Add(new List<int>(){capacityA, aCurrentAmountNext, capacityB, bCurrentAmountNext, capacityC, currentAmountC});   // Adding the result to the nextStates list
        
        // A -> C ... moves water from A to C
        List<int> aToC = new List<int> {capacityA, currentAmountA,capacityC, currentAmountC };
        (aCurrentAmountNext, cCurrentAmountNext) = Flow(aToC);
        nextStates.Add(new List<int>(){capacityA, aCurrentAmountNext, capacityB, currentAmountB, capacityC, cCurrentAmountNext});
        
        // B -> A
        List<int> bToA = new List<int> {capacityB, currentAmountB,capacityA, currentAmountA };
        (bCurrentAmountNext, aCurrentAmountNext) = Flow(bToA);
        nextStates.Add(new List<int>(){capacityA, aCurrentAmountNext, capacityB, bCurrentAmountNext, capacityC, currentAmountC});
        
        // B -> C
        List<int> bToC = new List<int> {capacityB, currentAmountB,capacityC, currentAmountC };
        (bCurrentAmountNext, cCurrentAmountNext) = Flow(bToC);
        nextStates.Add(new List<int>(){capacityA, currentAmountA, capacityB, bCurrentAmountNext, capacityC, cCurrentAmountNext});
        
        // C -> A
        List<int> cToA = new List<int> {capacityC, currentAmountC,capacityA, currentAmountA };
        (cCurrentAmountNext, aCurrentAmountNext) = Flow(cToA);
        nextStates.Add(new List<int>(){capacityA, aCurrentAmountNext, capacityB, currentAmountB, capacityC, cCurrentAmountNext});
        
        // C -> B
        List<int> cToB = new List<int> {capacityC, currentAmountC,capacityB, currentAmountB };
        (cCurrentAmountNext, bCurrentAmountNext) = Flow(cToB);
        nextStates.Add(new List<int>(){capacityA, currentAmountA, capacityB, bCurrentAmountNext, capacityC, cCurrentAmountNext});

        return nextStates;

    }
    
    /// <summary>
    /// Gets the possible volumes that can be achieved
    /// </summary>
    /// <param name="cupsVolumes">The volumes of the cups</param>
    /// <returns>A list of possible volumes</returns>
    private List<int> GetPossibleVolumes(List<int> cupsVolumes)
    {
        List<int> possibleVolumes = new List<int>();
        int maxVolume = cupsVolumes.Max();

        for (int i = 0; i <= maxVolume; i++)
        {
            possibleVolumes.Add(i);
        }

        return possibleVolumes;
    }
    
    /// <summary>
    /// Checks if the desired volume can be achieved
    /// </summary>
    /// <param name="currentVolumes">The current volumes of the cups</param>
    /// <param name="expectedVolume">The desired volume</param>
    /// <returns>True if the volume can be achieved, false otherwise</returns>
    private bool VolumeCheck(List<int> currentVolumes, int expectedVolume)
    {
        foreach (int volume in currentVolumes)
        {
            if (volume == expectedVolume)
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Solves the problem using Breadth-First Search (BFS).
    /// </summary>
    /// <param name="volume">The desired volume.</param>
    /// <returns>A tuple of the desired volume and the number of steps required to achieve it.</returns>
    /// <remarks>
    /// This method uses the Breadth-First Search (BFS) algorithm to find the shortest path to the desired volume.
    /// It starts by enqueuing the initial state of the cups and the number of steps taken so far (0 at the start) into a queue.
    /// Then, it enters a loop that continues until the queue is empty.
    /// In each iteration of the loop, it dequeues a state from the queue and checks if the desired volume can be achieved from the current state.
    /// If the volume can be achieved, it returns the volume and the number of steps taken to reach this state.
    /// If the volume cannot be achieved from the current state, it adds the current state to a set of visited states and calculates the next possible states.
    /// It then enqueues each next state that has not been visited yet into the queue, along with the number of steps taken to reach that state (one more than the current state).
    /// If the queue becomes empty and the method has not returned, it means that the desired volume cannot be achieved. In this case, it returns the volume and -1.
    /// </remarks>
    private (int, int) SolveBFS(int volume)
    {
        // Create a queue for BFS and enqueue the initial state and the number of steps taken so far (0)
        Queue<(List<int>, int)> bfsQueue = new Queue<(List<int>, int)>();
        bfsQueue.Enqueue((_initialState, 0));

        // Create a set to keep track of visited states
        Set visited = new Set();
        
        // Continue until the queue is empty
        while (bfsQueue.Count > 0)
        {
            // Dequeue a state from the queue
            (List<int> currentState, int steps) = bfsQueue.Dequeue();
            
            if (VolumeCheck(new List<int> {currentState[1], currentState[3], currentState[5]}, volume))
            {
                // If the volume can be achieved, return the volume and the number of steps taken to reach this state
                return (volume, steps);
            }
            if (visited.Contains(currentState) == false)
            {
                
                visited.Add(currentState);
                
                // Calculate the next possible states
                foreach (List<int> nextState in CalculateNextStates(currentState))
                {
                    // Enqueue each next state that has not been visited yet, along with the number of steps taken to reach that state (one more than the current state)
                    if (visited.Contains(nextState) == false)
                    {
                        bfsQueue.Enqueue((nextState, steps + 1));
                    }
                }
            }
        }
        return (volume, -1);
    }

    /// <summary>
    /// Gets the solutions to the problem
    /// </summary>
    /// <returns>A list of tuples, each containing a volume and the number of steps required to achieve it</returns>
    private List<(int, int)> GetSolutions()
    {
        List<(int, int)> solutions = new List<(int, int)>();
        
        // GetPossibleVolumes(new List<int>() {aCapacity, bCapacity, cCapacity}
        foreach (int volume in GetPossibleVolumes(new List<int>() {_capacityA, _capacityB, _capacityC}))
        {
            (int vol, int steps) = SolveBFS(volume);
            solutions.Add((vol, steps));
            
        }
        return solutions;
    }

    /// <summary>
    /// Checks if the water transfer is possible based on the number of steps.
    /// </summary>
    /// <param name="steps">The number of steps required for the water transfer.</param>
    /// <returns>True if the water transfer is possible, false otherwise.</returns>
    private bool IsPossibleWaterTransfer(int steps)
    {
        return steps != -1;
    }
    
    /// <summary>
    /// Prints the solutions to the console
    /// </summary>
    public void PrintSolutions()
    {
        List<string> resultList = new List<string>();
            
        foreach ((int volume, int steps) solution in GetSolutions())
        {
            if (IsPossibleWaterTransfer(solution.steps))
            {
                resultList.Add($"{solution.volume}:{solution.steps}");
            }
        }
        Console.WriteLine(string.Join(" ", resultList));
    }
}


/// <summary>
/// Class for reading and processing input from the console.
/// </summary>
class Reader
{
    /// <summary>
    /// Checks if the given string is not in the list of characters to avoid.
    /// </summary>
    /// <param name="givenstring">The string to check.</param>
    /// <param name="characters">The list of characters to avoid.</param>
    /// <returns>True if the string is not in the list of characters to avoid, false otherwise.</returns>
    public bool IsGivenString(string givenstring,List<string> characters)
    {
        foreach (string character in characters)
        {
            if (givenstring == character)
            {
                return false;
            }
        }
        return true;
    }
    
    /// <summary>
    /// Reads the input from the console and returns it as a list of integers.
    /// </summary>
    /// <returns>A list of integers representing the input.</returns>
    public List<int> ReadInput()
    {
        List<int> resultList = new List<int>();
        List<string> charactersToAvoid = new List<string>() {"\n", " ", "", "\t"};
        
        string[] inputArray = Console.ReadLine().Split();

        foreach (string _ in inputArray)
        {
            if (IsGivenString(_, charactersToAvoid))
            {
                resultList.Add(int.Parse(_));
            }
        }
        return resultList;
    }

}
class Program
{
    /// <summary>
    /// Reads the input from the console
    /// </summary>
    /// <returns>A list of integers representing the input</returns>
    
    static void Main(string[] args)
    {
        Reader rd = new Reader();
        List<int> inputList = rd.ReadInput();
        Solver solution1 = new Solver(inputList[0], inputList[3], inputList[1], inputList[4], inputList[2], inputList[5]);
        solution1.PrintSolutions();
    }
}
