﻿/*--- Day 4: Camp Cleanup ---
Space needs to be cleared before the last supplies can be unloaded from the ships, and so several Elves have been assigned the job of cleaning up sections of the camp. Every section has a unique ID number, and each Elf is assigned a range of section IDs.

However, as some of the Elves compare their section assignments with each other, they've noticed that many of the assignments overlap. To try to quickly find overlaps and reduce duplicated effort, the Elves pair up and make a big list of the section assignments for each pair (your puzzle input).

For example, consider the following list of section assignment pairs:

2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8
For the first few pairs, this list means:

Within the first pair of Elves, the first Elf was assigned sections 2-4 (sections 2, 3, and 4), while the second Elf was assigned sections 6-8 (sections 6, 7, 8).
The Elves in the second pair were each assigned two sections.
The Elves in the third pair were each assigned three sections: one got sections 5, 6, and 7, while the other also got 7, plus 8 and 9.
This example list uses single-digit section IDs to make it easier to draw; your actual list might contain larger numbers. Visually, these pairs of section assignments look like this:

.234.....  2-4
.....678.  6-8

.23......  2-3
...45....  4-5

....567..  5-7
......789  7-9

.2345678.  2-8
..34567..  3-7

.....6...  6-6
...456...  4-6

.23456...  2-6
...45678.  4-8
Some of the pairs have noticed that one of their assignments fully contains the other. For example, 2-8 fully contains 3-7, and 6-6 is fully contained by 4-6. In pairs where one assignment fully contains the other, one Elf in the pair would be exclusively cleaning sections their partner will already be cleaning, so these seem like the most in need of reconsideration. In this example, there are 2 such pairs.

In how many assignment pairs does one range fully contain the other?
*/

/*
--- Part Two ---
It seems like there is still quite a bit of duplicate work planned. Instead, the Elves would like to know the number of pairs that overlap at all.

In the above example, the first two pairs (2-4,6-8 and 2-3,4-5) don't overlap, while the remaining four pairs (5-7,7-9, 2-8,3-7, 6-6,4-6, and 2-6,4-8) do overlap:

5-7,7-9 overlaps in a single section, 7.
2-8,3-7 overlaps all of the sections 3 through 7.
6-6,4-6 overlaps in a single section, 6.
2-6,4-8 overlaps in sections 4, 5, and 6.
So, in this example, the number of overlapping assignment pairs is 4.

In how many assignment pairs do the ranges overlap?
*/

using System.Linq; 
using System.Diagnostics;

class Camp {

    private static int[] AssignmentToInts(string assignment) {
        string [] numbers = assignment.Split('-');
        int start = Int32.Parse(numbers[0]);
        int end = Int32.Parse(numbers[1]);
        return Enumerable.Range(start, end-start+1).ToArray();
    }
    public static void Main(string[] args) {
        using (StringReader reader = new StringReader(STRATEGY))
        {
            int totalScore = 0;        
            string? line = reader.ReadLine();
            while(line != null) {
                string [] assignments = line.Split(',');
                int[][] elves = new int[2][];
                for(int i=0; i<2; ++i) {
                    elves[i] = AssignmentToInts(assignments[i]);
                }
                
                // get the size of the overlap, if it is equal to the size of either set then this is a total overlap
                int [] overlap = elves[0].Intersect(elves[1]).ToArray();
                if(overlap.Length>0) {
                    totalScore++;
                }
            
                line = reader.ReadLine();
            }

            Console.WriteLine(totalScore);
        }
    }

    private static string STRATEGY = """
38-41,38-38
18-65,18-65
1-3,4-39
41-42,40-40
1-90,89-90
30-84,31-85
2-98,64-97
75-75,15-76
81-81,22-81
29-92,30-30
95-95,14-95
12-71,70-70
12-93,13-99
31-74,5-32
5-45,2-44
17-85,17-20
75-96,81-99
67-72,73-85
77-84,76-81
6-67,68-68
41-87,87-89
55-56,19-56
1-98,24-97
13-82,14-82
2-99,1-99
4-58,5-57
20-67,66-66
22-85,22-86
41-97,40-40
84-88,84-85
12-34,23-69
35-60,39-61
16-28,12-27
89-93,93-94
52-92,53-91
63-73,7-64
3-54,2-54
76-76,1-76
10-17,16-83
84-88,35-85
23-25,24-63
3-70,2-38
21-64,65-65
1-97,2-97
11-37,36-93
43-60,14-44
15-16,15-52
53-79,11-78
2-96,97-98
19-97,20-99
33-92,32-92
19-26,26-64
66-87,58-66
58-79,43-78
88-98,89-98
3-91,3-38
47-47,47-93
27-85,28-85
1-46,2-47
1-3,4-78
26-52,36-51
41-54,42-54
35-99,1-34
55-95,94-96
18-46,45-54
93-98,2-86
5-90,57-99
2-85,5-86
13-86,13-86
91-93,55-92
41-72,40-86
68-70,38-69
3-4,3-97
57-88,19-87
44-84,3-45
3-87,2-87
2-88,89-89
9-75,8-10
33-46,17-32
14-81,81-82
19-89,20-89
38-82,39-81
3-85,3-97
16-43,43-43
10-10,11-90
2-79,2-96
19-60,61-86
2-97,1-98
11-22,11-12
2-25,24-25
21-89,22-90
10-35,11-35
33-35,32-35
41-41,8-42
4-74,2-3
1-2,3-90
60-91,59-92
5-90,90-97
95-96,2-96
49-81,49-81
9-37,21-57
41-76,40-77
93-93,3-93
9-96,1-6
9-53,54-95
66-93,80-92
32-32,32-34
73-95,27-72
47-88,46-46
57-91,51-92
78-78,78-81
8-57,58-81
5-49,14-18
87-87,22-88
3-66,67-67
94-96,15-94
3-70,3-70
9-94,10-65
11-59,58-59
32-82,12-83
44-89,43-89
55-95,7-54
5-18,19-49
88-89,34-89
1-84,84-84
62-92,31-63
93-95,30-88
10-36,10-36
32-91,90-92
72-86,73-92
54-70,5-70
20-77,19-78
14-27,28-86
19-52,19-70
5-98,6-94
74-97,74-75
23-42,23-23
13-19,13-19
21-31,30-31
22-67,68-69
18-69,19-76
6-7,6-97
3-19,4-20
2-66,44-67
39-77,40-84
15-72,2-9
81-90,90-93
45-86,44-72
21-57,45-58
23-72,20-22
10-63,36-62
15-78,78-79
59-90,59-90
55-61,60-82
75-97,61-97
71-76,71-76
62-62,13-61
5-5,4-54
74-75,15-74
83-84,56-84
25-41,24-42
4-4,7-87
19-84,26-83
1-98,9-97
7-30,29-29
11-93,2-12
71-76,77-77
44-88,44-89
1-17,16-37
62-62,27-61
11-77,11-77
80-80,80-82
90-90,31-90
6-77,77-85
30-31,31-93
36-62,35-36
69-91,11-90
4-17,16-16
5-71,33-96
1-99,1-99
83-83,57-82
12-75,55-76
41-41,8-40
1-99,4-92
30-31,31-57
25-54,26-26
7-14,67-78
17-97,97-97
10-49,21-50
12-95,1-96
90-99,1-98
92-93,31-92
15-16,15-48
33-97,55-98
29-92,29-83
58-58,5-59
23-80,79-86
7-70,45-71
5-21,5-20
34-40,33-39
41-92,40-99
13-96,12-77
28-63,27-64
10-94,10-93
25-82,82-82
32-91,11-32
16-97,15-96
27-49,37-50
23-85,10-84
2-55,35-56
98-99,20-93
2-86,2-87
12-41,5-11
59-84,58-84
20-62,75-89
8-45,7-45
25-32,33-44
27-28,29-77
2-67,66-67
41-88,42-96
40-40,4-40
1-96,97-98
42-95,41-93
8-79,9-78
7-89,9-25
99-99,9-99
88-91,82-87
21-90,90-90
53-94,93-99
26-59,54-78
30-99,34-89
51-69,51-69
7-98,1-97
30-81,82-82
16-93,18-89
10-91,90-91
2-7,8-50
35-76,35-35
49-96,95-97
45-57,86-91
7-56,61-65
12-87,74-83
92-92,17-92
4-18,3-19
36-78,23-37
5-92,4-93
4-93,8-92
59-82,81-81
18-88,19-88
24-64,24-65
28-91,5-90
74-86,73-87
20-35,19-31
99-99,10-97
9-10,10-70
94-94,21-94
27-37,37-94
3-82,81-84
55-80,49-54
77-78,40-77
16-48,15-48
3-96,97-99
32-97,96-96
14-84,9-60
17-91,16-16
94-94,16-95
12-61,6-61
14-95,98-99
63-64,59-64
64-82,82-84
4-99,3-62
4-98,2-97
85-99,27-84
19-73,19-73
35-64,10-93
5-13,9-20
8-40,41-76
90-95,29-89
24-97,21-22
53-56,52-56
21-59,58-59
41-93,40-94
2-86,2-85
42-95,95-95
37-89,6-89
52-63,45-51
9-89,21-88
32-84,33-83
11-17,18-90
13-16,13-16
97-97,2-97
68-72,68-69
18-23,18-48
28-68,29-70
11-94,93-96
11-94,94-94
97-98,2-98
19-73,74-74
40-60,59-60
38-53,37-54
10-15,16-74
46-55,45-67
19-54,20-53
32-32,33-97
20-86,12-19
23-79,78-90
12-53,23-52
20-93,21-95
3-49,14-50
67-76,51-66
1-35,36-62
36-68,35-35
12-81,21-82
33-41,34-42
13-65,32-64
37-85,36-86
6-50,31-50
13-16,12-12
36-50,36-50
16-76,17-66
71-71,54-71
6-57,15-58
3-5,6-52
3-91,1-92
69-96,45-68
36-44,36-46
21-61,20-50
71-73,11-72
7-97,8-90
7-73,1-6
39-45,39-46
16-90,3-91
25-29,26-28
49-73,28-89
95-97,14-96
26-75,50-74
70-84,70-92
27-43,14-77
34-42,43-43
38-62,37-63
73-78,15-78
83-83,3-82
16-17,17-91
4-68,3-69
8-80,9-79
4-87,1-86
2-30,2-30
66-82,55-81
78-78,79-86
45-86,49-86
2-6,5-98
12-29,11-30
48-52,53-53
45-72,46-54
82-82,48-81
93-94,42-94
1-84,2-98
14-75,13-28
19-19,20-91
63-63,39-64
2-59,58-60
63-63,16-63
2-98,3-99
52-69,51-52
16-61,16-62
29-73,70-76
41-86,2-94
84-85,1-85
13-89,12-89
7-21,23-93
42-46,43-70
27-82,27-82
93-95,6-94
52-55,54-54
60-61,17-61
66-66,17-67
25-80,81-85
35-92,11-36
68-92,68-92
98-99,44-72
31-36,32-37
5-94,6-95
16-76,75-99
23-76,23-76
14-66,66-94
7-37,5-58
59-62,1-81
31-60,31-93
54-54,53-59
15-17,16-16
7-96,8-97
66-71,67-72
49-50,51-92
3-32,13-31
74-76,4-75
4-87,86-87
4-89,4-89
37-69,37-70
67-89,1-23
89-89,37-89
3-12,3-12
4-84,5-83
10-92,10-92
18-92,92-92
6-82,3-99
6-7,6-90
3-91,2-92
68-69,9-69
22-23,24-67
60-93,61-94
47-56,55-60
8-75,1-2
20-29,23-30
39-43,43-51
7-66,6-38
43-64,44-63
70-91,29-90
55-56,7-56
66-85,67-84
56-58,54-57
33-76,76-76
6-8,9-70
25-33,47-89
24-38,39-92
13-46,13-45
45-46,45-54
8-37,47-92
8-66,12-67
9-58,48-59
94-97,41-95
67-95,5-67
8-80,17-78
69-74,15-71
27-54,55-58
8-90,7-91
9-96,6-97
2-99,3-33
1-2,5-85
54-68,54-67
46-91,81-92
6-45,46-46
7-78,3-58
20-83,82-84
26-82,10-27
45-72,9-72
2-80,3-77
96-97,50-91
46-62,46-62
1-18,1-2
1-99,1-97
9-97,8-97
43-45,43-47
10-87,10-11
30-31,31-54
6-10,9-98
46-95,35-96
13-85,84-86
10-85,10-85
85-97,27-63
1-95,98-99
3-35,4-36
21-84,98-98
85-85,44-85
42-84,38-82
2-52,1-99
14-86,38-95
24-30,25-30
76-93,25-75
6-98,3-99
10-66,9-66
83-85,25-84
8-9,8-11
16-81,16-93
5-66,3-66
26-64,33-63
32-42,33-65
78-85,77-94
99-99,96-99
12-90,8-91
34-35,35-84
79-96,97-97
18-93,17-94
26-93,25-94
12-67,12-12
26-97,26-95
3-41,2-42
37-44,43-44
17-64,11-78
3-94,3-94
5-53,1-1
34-78,77-90
29-66,65-66
23-40,12-41
8-19,19-79
45-84,37-44
2-87,86-88
38-79,37-68
19-30,20-75
3-78,39-78
9-84,84-84
16-59,15-58
37-83,42-84
49-88,50-89
32-97,33-99
31-76,30-68
10-66,5-67
25-97,26-97
40-84,39-85
37-52,37-53
11-13,12-91
25-64,24-54
4-69,69-70
9-97,8-97
7-43,7-43
9-68,14-69
17-90,17-90
21-88,20-89
10-57,20-56
12-88,97-99
32-98,31-98
45-64,65-74
59-60,58-62
9-41,10-41
3-9,7-10
32-51,32-51
48-97,48-73
15-79,58-80
4-9,12-76
24-77,39-78
6-82,5-6
28-50,27-49
40-72,39-72
24-37,36-38
14-96,5-95
88-95,36-87
22-85,22-27
10-98,4-97
46-89,46-89
22-94,6-23
3-43,3-97
29-42,24-43
2-92,92-92
27-98,26-98
2-87,2-94
87-89,59-87
29-29,29-83
33-33,34-42
7-85,8-86
20-20,14-21
22-22,22-34
7-68,27-68
5-54,6-53
12-91,11-90
54-81,54-81
2-95,1-95
6-55,6-54
49-92,91-95
5-90,4-91
30-84,32-83
13-61,12-60
11-98,26-98
7-76,77-77
4-37,37-38
71-81,81-81
6-58,58-70
31-83,30-31
48-56,49-55
4-98,8-97
3-69,16-68
40-41,13-40
60-60,61-76
38-38,13-38
37-97,36-54
65-65,1-65
18-18,18-90
87-87,8-87
6-98,64-71
79-90,8-80
11-44,12-69
12-90,64-91
6-75,5-6
18-90,58-91
67-92,33-93
95-96,1-96
73-91,90-93
84-94,17-94
25-62,28-84
26-27,27-71
94-98,17-95
33-96,95-95
1-2,1-91
25-28,26-26
28-85,84-92
91-96,48-87
46-93,95-97
4-98,97-99
51-63,52-61
56-89,6-55
5-93,2-94
9-93,6-8
6-53,5-52
70-92,91-91
28-98,27-95
3-45,45-55
1-55,1-54
75-82,74-81
52-91,11-51
29-82,25-81
32-71,53-72
31-72,32-71
2-99,4-97
54-79,54-80
89-96,80-90
4-4,5-93
63-65,9-64
9-58,10-25
14-72,4-76
23-58,24-59
96-99,4-95
17-17,18-80
6-29,13-30
97-99,9-98
97-99,21-96
44-46,43-53
87-89,36-88
42-89,19-90
12-12,10-14
22-77,23-77
81-83,3-82
47-47,3-47
16-79,15-80
25-30,12-24
3-94,94-94
34-64,33-63
24-85,24-85
52-54,40-53
63-88,62-62
12-75,12-12
11-63,39-64
95-95,12-95
89-91,88-98
59-83,9-60
97-99,1-98
22-98,28-82
96-97,44-94
5-88,3-91
53-85,52-84
50-88,48-71
5-95,94-95
71-72,29-71
18-96,97-97
39-41,49-59
46-72,11-47
34-49,34-49
88-90,75-89
2-94,1-92
66-96,65-66
29-82,3-81
19-86,87-91
65-68,5-30
66-85,66-85
6-87,87-90
26-88,27-88
6-19,18-19
20-83,21-82
32-56,25-32
37-77,38-77
1-36,1-36
11-59,10-60
81-92,30-91
21-77,2-20
80-82,14-81
1-43,44-99
1-2,3-79
21-92,93-93
1-77,69-97
70-90,90-91
10-44,6-9
63-90,14-90
64-75,63-76
72-95,36-95
1-2,4-90
23-50,22-51
12-81,18-82
12-13,12-90
29-83,29-30
2-97,1-96
7-96,95-97
15-96,16-97
1-1,1-83
43-53,42-53
14-26,15-64
12-94,13-85
40-40,40-46
13-25,14-22
90-94,90-94
9-91,12-26
4-96,97-97
1-8,7-58
27-92,74-91
27-90,90-91
19-99,16-18
42-87,41-88
51-85,36-52
7-94,4-6
2-43,2-3
26-92,27-96
7-87,8-88
2-97,10-37
2-90,1-2
31-78,77-77
1-97,99-99
67-90,89-98
45-51,44-85
70-97,69-97
71-94,95-96
1-82,6-81
3-77,46-77
4-90,3-3
57-71,56-70
68-98,68-69
50-70,51-70
94-97,62-95
13-94,94-96
13-92,14-14
67-99,68-98
9-9,22-94
21-91,91-94
37-78,37-79
21-84,22-22
27-39,40-99
26-86,25-74
1-4,2-24
5-50,51-94
9-10,9-76
70-73,73-73
69-87,25-68
34-85,34-86
44-64,63-90
58-59,58-58
45-98,47-99
4-6,10-44
31-82,54-81
20-58,19-58
12-68,12-13
64-64,54-64
19-23,18-22
60-67,59-67
7-7,6-96
14-37,37-38
28-88,66-96
72-90,45-57
50-97,19-97
6-29,6-29
15-43,15-43
14-82,82-82
10-64,8-8
70-82,80-83
11-97,10-98
23-77,24-77
6-84,6-6
98-99,5-98
65-65,65-72
48-73,13-88
41-65,40-46
50-81,12-82
42-44,43-43
55-89,63-90
4-77,76-76
37-92,38-75
60-62,15-61
12-95,96-98
19-92,20-91
4-56,1-57
27-94,48-94
85-88,77-84
58-59,12-59
1-65,51-66
70-72,4-70
86-86,15-85
78-99,99-99
7-8,7-94
23-88,23-87
69-77,77-83
39-75,40-75
1-94,2-95
32-79,31-80
31-43,32-43
34-37,33-38
68-92,67-93
37-85,33-84
26-59,25-60
53-76,54-54
25-94,94-95
39-72,72-72
16-17,17-17
4-98,98-98
1-48,1-2
90-92,28-94
77-77,65-78
19-60,60-61
15-98,16-99
25-92,25-93
13-85,12-86
6-57,57-57
88-94,56-87
66-83,84-84
88-90,57-89
11-11,10-63
20-46,21-47
25-31,29-32
7-94,6-95
8-69,8-50
32-86,33-86
19-98,99-99
38-58,37-59
19-38,39-88
7-88,2-6
16-93,17-99
41-95,42-94
52-54,51-98
10-89,89-90
50-95,26-94
87-88,13-88
7-90,7-90
78-96,86-98
26-98,42-99
7-75,6-76
4-5,4-96
10-62,10-63
21-95,95-95
39-98,26-40
15-16,10-16
59-60,17-60
56-80,48-55
6-22,7-9
27-82,33-81
77-87,38-86
96-97,34-97
6-60,6-6
73-77,71-77
9-88,9-89
51-76,53-75
12-89,13-83
5-99,4-4
7-96,5-96
27-43,22-25
6-24,4-4
91-91,3-92
97-98,22-98
44-77,76-77
27-49,18-28
10-93,12-89
7-72,8-8
26-70,21-25
96-97,15-96
8-92,6-7
7-62,32-63
34-74,21-33
9-94,10-94
10-74,49-75
20-20,21-41
28-78,29-81
75-85,76-84
66-66,41-67
66-66,10-65
18-95,17-96
18-94,18-94
72-72,6-72
18-77,17-76
2-98,3-98
23-87,24-29
16-62,63-73
13-88,9-12
17-34,11-35
29-82,28-78
44-87,86-87
40-60,39-39
5-5,8-28
4-86,47-87
60-89,60-89
48-87,49-87
76-91,77-83
30-57,29-56
58-96,18-59
15-63,14-91
35-82,16-46
14-14,15-75
80-81,3-80
21-22,20-93
49-86,87-87
2-2,4-76
52-78,77-79
13-57,65-68
51-67,48-50
3-85,5-84
4-97,1-98
21-31,32-32
26-94,25-25
6-37,6-37
16-98,15-98
5-86,85-85
38-98,5-98
16-18,31-68
50-63,51-99
43-58,42-58
5-93,5-94
15-76,64-76
29-68,29-68
6-30,30-30
51-52,52-60
31-55,56-83
62-88,88-88
36-75,74-74
53-72,54-54
67-86,68-87
50-83,2-90
14-94,14-15
49-66,32-67
3-81,16-60
47-86,48-92
3-99,4-99
60-94,59-59
24-49,6-50
8-8,9-94
90-93,1-88
21-72,71-73
95-95,12-86
64-64,10-64
1-99,1-99
57-67,64-68
17-18,17-57
2-71,70-93
3-86,4-86
30-80,30-80
3-77,76-77
45-55,45-55
46-77,65-81
1-97,1-98
53-68,52-67
16-33,32-59
93-95,70-94
8-85,86-92
19-90,89-91
83-93,94-99
45-45,46-90
58-68,57-69
27-34,28-30
94-95,1-94
33-79,30-78
61-61,24-61
82-83,59-82
81-83,80-83
22-68,96-97
82-82,17-81
43-53,43-43
13-62,14-37
68-85,67-88
32-88,32-38
63-70,52-62
52-52,51-52
2-57,57-86
21-88,21-22
28-29,28-67
37-90,37-91
4-94,3-3
13-76,13-76
9-23,22-86
84-84,24-84
50-85,49-49
15-52,14-14
24-53,30-49
25-65,25-65
1-87,3-88
14-41,14-41
34-97,35-98
10-80,10-75
2-67,19-68
16-94,15-82
17-54,25-55
17-66,66-77
10-92,91-93
10-90,10-11
64-66,18-65
16-37,15-38
4-83,6-84
4-82,3-60
62-74,63-85
10-62,9-63
54-93,53-92
48-59,60-77
9-73,72-74
17-24,25-82
32-73,31-40
13-43,13-44
37-38,71-80
96-99,84-97
8-58,32-58
11-57,5-58
63-98,62-92
""";
}