# DeltaDebugger

Very basic implementation of a delta debugger for a WCNFParser. <br>
When executed, the program tries to shrink the input specified in `TestInput.txt` to the minimum while preserving the error.

# Example Input file
For example the `e` at the start of the line in the input file produces an error in the WCNFParser.
```
c Standarized MaxSat Instance
c{
c "sha1sum": "745088f95cc6a767845734f64a57421f05107714",
c "nvars": 47,
c "ncls": 186,
c "total_lits": 477,
c "nhards": 176,
c "nhard_nlits": 467,
c "nhard_len_stats":
c    { "min": 1,
c      "max": 4,
c      "ave": 2.6534,
c      "stddev": 0.6300 },
c "nsofts": 10,
c "nsoft_nlits": 10,
c "nsoft_len_stats":
c    { "min": 1,
c      "max": 1,
c      "ave": 1.0000,
c      "stddev": 0.0000 },
c "nsoft_wts": 1,
c "soft_wt_stats":
c    { "min": 1,
c      "max": 1,
c      "ave": 1.0000,
c      "stddev": 0.0000 }
c}
c------------------------------------------------------------
c 
1 -6 0
1 -7 0
1 -8 0
1 -9 0
1 -10 0
h -11 0
e 12 0
h 13 0
h 14 0
h -15 0
h -16 0
h -17 0
h -5 -6 0
h -12 -11 0
h 12 11 -13 0
h -12 15 14 0
h -15 -14 0
h 15 14 -13 0
h 15 12 -13 0
h -15 -12 -13 0
h 15 -12 13 0
h 5 -18 0
```

# Input file after execution
```
e 12 0 
```

# Example Output of the program
![image](https://user-images.githubusercontent.com/46893185/211212292-4822f675-e3d1-40d6-8ac6-428e0d65892f.png)

# Remarks

> Cant handle big inputs currently because the debugger tries to write while the input file is still in use

> Only shrinks the file to one error and discard any additional faulty input
