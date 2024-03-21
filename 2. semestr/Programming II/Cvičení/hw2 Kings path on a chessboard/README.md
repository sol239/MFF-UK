**Variant 1**
Write a program finding a shortest path with a chess king on a chessboard 8x8 where several squares cannot be accessed (by the king).

Input is given in this ordering:

Number of obstacles
Coordinates of the obstacles (pairs of numbers 1.. 8)
Coordinates of the starting square
Coordinates of the end square.
Number of the obstacles is on a separate line, obstacles are described each on a separate line (i.e., one pair of numbers on a line). On a line the numbers are separated by the space-character.

Output is either -1 (if the king cannot reach the end-square) or number of steps that the king has to perform.

**Sample input:**
1
2 1
1 1
2 2
**Appropriate output:**
1

**Variant 2**

From the file sachovnice.txt read the description of a chessboard and write out the length of a king's shortest path to a destination square. Write the output to standard output (i.e., the terminal). The chessboard is described in the following way. The description starts with two lines containing positive integers x and y indicating the number of rows and columns of the given chessboard (respectively). These numbers are followed by x lines, each consisting of y letters describing individual positions with the following meaning:

dot: we can access this position,
X (uppercase letter), this position is forbidden,
S (also uppercase), the king starts here,
C (also uppercase), the destination position.
If there exists no path between the given pair of positions, write -1.

Example:
Input:
4
4
X.XS
.X.X
.XXX
..C.
Output:
6