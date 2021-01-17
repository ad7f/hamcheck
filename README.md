# hamcheck

This tool takes a list of names and checks to see which ones are amateur radio operators.

On first run, please allow time for a copy of the database to be downloaded locally. 

The tool is designed so that you can copy and paste a list of names into the tool.

The currently accepted formats for name lines are:
```
LastName
LastName, First
LastName, Person1FirstName &amp; Person2FirstName
LastName, P1First P1Middle &amp; P2First P2Middle
```
Note: whitespace and single characters can be included in the list that is copy and pasted in, they will be ignored.

City lines are optional, but can help you scope to a particular area. When separated by a comma, two letter state abbreviations can also be used. This can be helpful in cases where many states have the same city name.

The acceptable formats for city lines are:
```
City
City, ST
```
