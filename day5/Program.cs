/*--- Day 5: Supply Stacks ---
The expedition can depart as soon as the final supplies have been unloaded from the ships. Supplies are stored in stacks of marked crates, but because the needed supplies are buried under many other crates, the crates need to be rearranged.

The ship has a giant cargo crane capable of moving crates between stacks. To ensure none of the crates get crushed or fall over, the crane operator will rearrange them in a series of carefully-planned steps. After the crates are rearranged, the desired crates will be at the top of each stack.

The Elves don't want to interrupt the crane operator during this delicate procedure, but they forgot to ask her which crate will end up where, and they want to be ready to unload them as soon as possible so they can embark.

They do, however, have a drawing of the starting stacks of crates and the rearrangement procedure (your puzzle input). For example:

    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2
In this example, there are three stacks of crates. Stack 1 contains two crates: crate Z is on the bottom, and crate N is on top. Stack 2 contains three crates; from bottom to top, they are crates M, C, and D. Finally, stack 3 contains a single crate, P.

Then, the rearrangement procedure is given. In each step of the procedure, a quantity of crates is moved from one stack to a different stack. In the first step of the above rearrangement procedure, one crate is moved from stack 2 to stack 1, resulting in this configuration:

[D]        
[N] [C]    
[Z] [M] [P]
 1   2   3 
In the second step, three crates are moved from stack 1 to stack 3. Crates are moved one at a time, so the first crate to be moved (D) ends up below the second and third crates:

        [Z]
        [N]
    [C] [D]
    [M] [P]
 1   2   3
Then, both crates are moved from stack 2 to stack 1. Again, because crates are moved one at a time, crate C ends up below crate M:

        [Z]
        [N]
[M]     [D]
[C]     [P]
 1   2   3
Finally, one crate is moved from stack 1 to stack 2:

        [Z]
        [N]
        [D]
[C] [M] [P]
 1   2   3
The Elves just need to know which crate will end up on top of each stack; in this example, the top crates are C in stack 1, M in stack 2, and Z in stack 3, so you should combine these together and give the Elves the message CMZ.

After the rearrangement procedure completes, what crate ends up on top of each stack?
*/

using System.Diagnostics;
using System;
using System.Text.RegularExpressions;

class Camp {

    private static int[] ParseStacks(string assignment) {
        string [] numbers = assignment.Split('-');
        int start = Int32.Parse(numbers[0]);
        int end = Int32.Parse(numbers[1]);
        return Enumerable.Range(start, end-start+1).ToArray();
    }
    public static void Main(string[] args) {
        using (StringReader reader = new StringReader(STRATEGY))
        {
            string? line = reader.ReadLine();
            int numStacks = line.Length / 4 + 1;
            List<char>[] stacks = new List<char>[numStacks];
            for(int i=0; i<numStacks; ++i) {
                stacks[i] = new List<char>();
            }
            while(line != null) {
                // did we run out of crates?
                if(line[1] == '1')
                    break;

                int offset = 0;
                while(offset < line.Length) {
                    // is this a crate?
                    if(line[offset] == '[') {
                        offset++;
                        char cargo = line[offset];
                        stacks[offset/4].Add(cargo);
                        offset+=3;
                    } else {
                        // skip the gap, next crate please
                        offset += 4;
                    }
                }
                
                line = reader.ReadLine();
            }

            // skip the blank line
            line = reader.ReadLine();
            Debug.Assert(line.Length == 0, "Unexpected line");

            // now read the instructions (move 1 from 2 to 1)
            Regex rx = new Regex(@"move (\d+) from (\d+) to (\d+)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            line = reader.ReadLine();
            while(line != null) {
                // Find matches.
                MatchCollection matches = rx.Matches(line);

                // Report the number of matches found.
                Console.WriteLine("{0} matches found in:\n   {1}",
                                matches.Count,
                                line);

                // Report on each match.
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    int countToMove = Int32.Parse(groups[1].Value);
                    int fromStack = Int32.Parse(groups[2].Value);
                    int toStack = Int32.Parse(groups[3].Value);

                    for(int i=0; i<countToMove; ++i) {
                        // add to the new stack
                        stacks[toStack-1].Insert(0, stacks[fromStack-1][0]);
                        // remove from the prev stack
                        stacks[fromStack-1].RemoveAt(0);
                    }
                }

                
                line = reader.ReadLine();
            }

            string tops="";
            for(int i=0; i<numStacks; ++i) {
                tops=tops+stacks[i][0];
            }
            Console.WriteLine(tops);
        }
    }

    private static string STRATEGY = """
    [V] [G]             [H]        
[Z] [H] [Z]         [T] [S]        
[P] [D] [F]         [B] [V] [Q]    
[B] [M] [V] [N]     [F] [D] [N]    
[Q] [Q] [D] [F]     [Z] [Z] [P] [M]
[M] [Z] [R] [D] [Q] [V] [T] [F] [R]
[D] [L] [H] [G] [F] [Q] [M] [G] [W]
[N] [C] [Q] [H] [N] [D] [Q] [M] [B]
 1   2   3   4   5   6   7   8   9 

move 3 from 2 to 5
move 2 from 9 to 6
move 4 from 7 to 1
move 7 from 3 to 4
move 2 from 9 to 8
move 8 from 8 to 6
move 1 from 7 to 4
move 8 from 6 to 4
move 4 from 5 to 7
move 3 from 4 to 9
move 2 from 6 to 3
move 11 from 4 to 1
move 1 from 3 to 4
move 2 from 3 to 1
move 1 from 7 to 6
move 14 from 1 to 6
move 7 from 4 to 3
move 2 from 5 to 9
move 5 from 6 to 4
move 9 from 6 to 1
move 3 from 4 to 8
move 1 from 7 to 6
move 3 from 4 to 1
move 7 from 3 to 8
move 5 from 9 to 5
move 4 from 1 to 4
move 3 from 7 to 2
move 5 from 6 to 2
move 3 from 4 to 1
move 7 from 8 to 5
move 3 from 6 to 8
move 11 from 2 to 1
move 1 from 4 to 3
move 1 from 3 to 9
move 2 from 2 to 9
move 8 from 5 to 4
move 1 from 1 to 7
move 1 from 9 to 5
move 8 from 4 to 1
move 1 from 6 to 8
move 2 from 9 to 1
move 4 from 5 to 3
move 2 from 7 to 3
move 40 from 1 to 2
move 24 from 2 to 9
move 1 from 5 to 6
move 11 from 2 to 3
move 9 from 3 to 5
move 12 from 9 to 4
move 6 from 5 to 7
move 4 from 7 to 4
move 2 from 5 to 1
move 2 from 1 to 9
move 1 from 6 to 8
move 9 from 4 to 8
move 6 from 4 to 9
move 17 from 9 to 6
move 1 from 4 to 6
move 17 from 6 to 5
move 1 from 1 to 4
move 2 from 7 to 9
move 1 from 6 to 7
move 2 from 2 to 9
move 2 from 7 to 2
move 6 from 3 to 8
move 3 from 5 to 9
move 1 from 4 to 9
move 2 from 3 to 7
move 4 from 5 to 6
move 1 from 7 to 4
move 1 from 4 to 2
move 1 from 7 to 5
move 9 from 8 to 1
move 1 from 1 to 2
move 2 from 9 to 3
move 7 from 2 to 7
move 1 from 9 to 5
move 12 from 8 to 7
move 3 from 1 to 9
move 2 from 6 to 4
move 9 from 9 to 3
move 1 from 6 to 7
move 1 from 9 to 5
move 1 from 6 to 1
move 9 from 7 to 1
move 7 from 1 to 8
move 4 from 3 to 9
move 5 from 7 to 1
move 3 from 9 to 1
move 4 from 7 to 2
move 12 from 1 to 5
move 2 from 9 to 4
move 7 from 8 to 2
move 7 from 5 to 7
move 4 from 3 to 4
move 1 from 8 to 1
move 2 from 2 to 1
move 2 from 3 to 1
move 3 from 2 to 7
move 13 from 5 to 4
move 1 from 8 to 3
move 1 from 3 to 8
move 1 from 3 to 5
move 1 from 8 to 7
move 17 from 4 to 8
move 5 from 2 to 6
move 2 from 1 to 6
move 5 from 6 to 3
move 9 from 7 to 1
move 4 from 4 to 3
move 1 from 6 to 2
move 4 from 7 to 4
move 1 from 6 to 5
move 2 from 3 to 2
move 15 from 1 to 4
move 6 from 5 to 4
move 4 from 3 to 5
move 4 from 5 to 2
move 2 from 2 to 4
move 11 from 8 to 1
move 2 from 8 to 3
move 5 from 3 to 7
move 4 from 2 to 8
move 2 from 2 to 9
move 4 from 7 to 8
move 11 from 4 to 6
move 2 from 5 to 4
move 3 from 6 to 9
move 4 from 1 to 4
move 15 from 4 to 9
move 1 from 7 to 3
move 2 from 1 to 2
move 6 from 4 to 5
move 11 from 8 to 2
move 16 from 9 to 4
move 2 from 9 to 1
move 4 from 2 to 3
move 8 from 4 to 9
move 1 from 8 to 7
move 5 from 4 to 7
move 6 from 7 to 3
move 10 from 9 to 5
move 5 from 3 to 1
move 1 from 1 to 4
move 5 from 1 to 9
move 5 from 1 to 7
move 5 from 4 to 1
move 4 from 1 to 6
move 3 from 1 to 9
move 10 from 5 to 9
move 2 from 7 to 1
move 5 from 3 to 6
move 4 from 5 to 7
move 4 from 2 to 6
move 2 from 5 to 6
move 5 from 2 to 7
move 18 from 6 to 1
move 5 from 9 to 2
move 7 from 9 to 6
move 16 from 1 to 7
move 4 from 6 to 1
move 1 from 2 to 6
move 2 from 2 to 6
move 1 from 2 to 4
move 4 from 9 to 3
move 1 from 2 to 8
move 5 from 7 to 5
move 2 from 9 to 3
move 1 from 5 to 9
move 7 from 3 to 4
move 1 from 9 to 7
move 8 from 1 to 9
move 1 from 8 to 9
move 3 from 6 to 9
move 17 from 7 to 5
move 3 from 4 to 8
move 3 from 4 to 2
move 3 from 8 to 3
move 3 from 3 to 7
move 7 from 9 to 3
move 6 from 5 to 9
move 4 from 9 to 3
move 10 from 7 to 2
move 15 from 5 to 2
move 4 from 6 to 3
move 1 from 3 to 2
move 23 from 2 to 5
move 2 from 4 to 6
move 2 from 6 to 7
move 1 from 7 to 2
move 1 from 6 to 9
move 5 from 9 to 8
move 3 from 8 to 7
move 5 from 2 to 6
move 2 from 2 to 3
move 2 from 6 to 3
move 3 from 6 to 2
move 3 from 6 to 8
move 10 from 5 to 9
move 2 from 7 to 5
move 1 from 5 to 8
move 13 from 9 to 5
move 6 from 5 to 6
move 1 from 6 to 1
move 1 from 7 to 3
move 1 from 7 to 3
move 13 from 5 to 6
move 3 from 3 to 5
move 1 from 2 to 1
move 4 from 8 to 9
move 2 from 2 to 6
move 2 from 5 to 3
move 2 from 3 to 6
move 5 from 6 to 4
move 9 from 5 to 9
move 10 from 6 to 9
move 1 from 1 to 7
move 3 from 3 to 9
move 1 from 8 to 1
move 3 from 6 to 3
move 1 from 7 to 6
move 1 from 8 to 7
move 2 from 6 to 1
move 2 from 6 to 4
move 3 from 4 to 6
move 2 from 1 to 4
move 10 from 9 to 6
move 6 from 4 to 9
move 17 from 9 to 1
move 4 from 9 to 5
move 19 from 1 to 7
move 4 from 5 to 6
move 1 from 9 to 3
move 5 from 3 to 4
move 5 from 4 to 8
move 17 from 6 to 9
move 17 from 9 to 2
move 1 from 6 to 1
move 1 from 1 to 2
move 1 from 8 to 3
move 2 from 3 to 2
move 5 from 7 to 1
move 1 from 7 to 3
move 5 from 2 to 9
move 4 from 8 to 2
move 2 from 7 to 8
move 3 from 9 to 3
move 7 from 3 to 9
move 2 from 8 to 7
move 8 from 2 to 9
move 5 from 9 to 6
move 4 from 3 to 9
move 11 from 2 to 3
move 2 from 6 to 5
move 1 from 9 to 4
move 10 from 7 to 3
move 3 from 1 to 8
move 2 from 6 to 7
move 15 from 3 to 8
move 2 from 3 to 2
move 2 from 1 to 3
move 14 from 9 to 6
move 1 from 4 to 9
move 14 from 6 to 3
move 5 from 7 to 2
move 2 from 9 to 2
move 1 from 5 to 3
move 1 from 5 to 8
move 12 from 3 to 7
move 13 from 7 to 8
move 1 from 6 to 7
move 5 from 2 to 6
move 1 from 6 to 2
move 1 from 7 to 6
move 4 from 6 to 8
move 31 from 8 to 7
move 15 from 7 to 8
move 7 from 7 to 5
move 4 from 2 to 3
move 1 from 6 to 2
move 3 from 5 to 8
move 9 from 7 to 4
move 2 from 2 to 9
move 4 from 5 to 6
move 13 from 3 to 9
move 3 from 3 to 5
move 13 from 9 to 1
move 1 from 3 to 2
move 2 from 6 to 5
move 1 from 3 to 4
move 2 from 6 to 5
move 1 from 9 to 1
move 6 from 8 to 9
move 5 from 5 to 2
move 2 from 9 to 8
move 2 from 1 to 6
move 1 from 9 to 4
move 12 from 8 to 4
move 2 from 6 to 9
move 11 from 4 to 3
move 9 from 4 to 2
move 4 from 9 to 7
move 2 from 5 to 6
move 8 from 3 to 4
move 2 from 3 to 9
move 2 from 8 to 9
move 4 from 4 to 9
move 2 from 6 to 7
move 1 from 3 to 7
move 2 from 9 to 1
move 5 from 4 to 2
move 9 from 1 to 8
move 1 from 4 to 9
move 4 from 9 to 3
move 1 from 3 to 6
move 4 from 8 to 7
move 1 from 3 to 6
move 4 from 1 to 7
move 1 from 3 to 8
move 1 from 1 to 8
move 2 from 6 to 7
move 2 from 9 to 1
move 1 from 4 to 5
move 1 from 1 to 5
move 11 from 8 to 4
move 12 from 2 to 8
move 1 from 9 to 8
move 2 from 4 to 5
move 1 from 1 to 8
move 5 from 2 to 1
move 1 from 3 to 2
move 9 from 7 to 3
move 6 from 7 to 5
move 1 from 3 to 4
move 1 from 5 to 1
move 4 from 2 to 5
move 4 from 4 to 1
move 2 from 7 to 3
move 3 from 4 to 1
move 6 from 3 to 7
move 9 from 8 to 7
move 3 from 8 to 7
move 11 from 5 to 9
move 2 from 4 to 8
move 5 from 8 to 7
move 1 from 9 to 8
move 12 from 9 to 5
move 1 from 4 to 5
move 5 from 1 to 8
move 6 from 8 to 3
move 1 from 3 to 8
move 3 from 7 to 9
move 4 from 7 to 6
move 3 from 1 to 3
move 3 from 1 to 6
move 1 from 8 to 1
move 7 from 6 to 2
move 3 from 1 to 8
move 7 from 3 to 4
move 3 from 4 to 1
move 1 from 4 to 2
move 3 from 1 to 2
move 1 from 7 to 6
move 1 from 8 to 5
move 9 from 5 to 3
move 1 from 6 to 9
move 11 from 3 to 6
move 1 from 4 to 1
move 1 from 3 to 4
move 8 from 6 to 9
move 1 from 3 to 1
move 1 from 9 to 1
move 2 from 6 to 2
move 5 from 5 to 7
move 5 from 9 to 3
move 2 from 8 to 5
move 1 from 1 to 2
move 1 from 9 to 1
move 15 from 7 to 4
move 1 from 1 to 6
move 1 from 6 to 9
move 3 from 9 to 3
move 1 from 3 to 5
move 5 from 5 to 3
move 9 from 2 to 9
move 5 from 4 to 1
move 1 from 6 to 7
move 7 from 9 to 3
move 1 from 4 to 7
move 1 from 9 to 6
move 1 from 6 to 5
move 2 from 1 to 4
move 3 from 9 to 3
move 1 from 5 to 6
move 7 from 4 to 3
move 1 from 9 to 3
move 16 from 3 to 1
move 9 from 1 to 3
move 5 from 4 to 2
move 1 from 6 to 9
move 12 from 1 to 9
move 3 from 2 to 9
move 5 from 7 to 3
move 2 from 4 to 8
move 2 from 7 to 2
move 12 from 3 to 5
move 6 from 2 to 9
move 12 from 3 to 1
move 2 from 8 to 6
move 1 from 6 to 1
move 6 from 5 to 8
move 5 from 3 to 2
move 2 from 5 to 8
move 8 from 1 to 8
move 13 from 9 to 7
move 4 from 7 to 5
move 4 from 1 to 4
move 8 from 5 to 6
move 1 from 1 to 6
move 4 from 7 to 3
move 1 from 3 to 1
move 1 from 1 to 9
move 4 from 9 to 5
move 3 from 3 to 7
move 12 from 8 to 7
move 2 from 4 to 3
move 2 from 6 to 9
move 4 from 8 to 2
move 2 from 3 to 9
move 2 from 4 to 7
move 3 from 5 to 7
move 2 from 9 to 7
move 3 from 6 to 1
move 4 from 6 to 7
move 1 from 5 to 4
move 1 from 9 to 3
move 12 from 2 to 5
move 4 from 9 to 7
move 11 from 5 to 1
move 1 from 6 to 5
move 1 from 1 to 4
move 10 from 1 to 2
move 2 from 5 to 1
move 1 from 3 to 5
move 7 from 2 to 5
move 8 from 7 to 8
move 2 from 2 to 8
move 3 from 9 to 4
move 5 from 4 to 3
move 1 from 5 to 7
move 3 from 7 to 1
move 3 from 5 to 8
move 1 from 2 to 5
move 12 from 7 to 6
move 4 from 1 to 3
move 2 from 5 to 6
move 7 from 3 to 7
move 14 from 6 to 4
move 1 from 5 to 6
move 3 from 1 to 3
move 4 from 3 to 2
move 2 from 5 to 8
move 11 from 7 to 4
move 7 from 4 to 5
move 1 from 3 to 4
move 1 from 5 to 6
move 14 from 8 to 7
move 11 from 7 to 3
move 2 from 2 to 6
move 1 from 2 to 3
move 5 from 5 to 4
move 4 from 6 to 4
move 8 from 7 to 8
move 3 from 7 to 3
move 1 from 2 to 1
move 5 from 8 to 2
move 4 from 4 to 3
move 1 from 2 to 9
move 1 from 1 to 9
move 3 from 2 to 1
move 1 from 5 to 4
move 3 from 8 to 1
move 1 from 7 to 4
move 4 from 3 to 9
move 1 from 8 to 7
move 2 from 9 to 1
move 6 from 3 to 4
move 28 from 4 to 7
move 15 from 7 to 8
move 3 from 3 to 8
move 1 from 2 to 9
move 2 from 3 to 2
move 7 from 1 to 4
move 10 from 4 to 5
move 10 from 5 to 6
move 3 from 8 to 2
move 1 from 1 to 7
move 1 from 4 to 7
move 1 from 9 to 6
move 9 from 6 to 7
move 1 from 2 to 4
move 1 from 9 to 5
""";
}