# Ranges
Some years ago I worked on a [2-7 triple draw](https://en.wikipedia.org/wiki/Lowball_(poker)#Deuce-to-seven) simulation in C#. One important part was having a syntax for ranges of hands (card combnations). The implementation used hands-on string parsing without the help of any parsing/tokenizing tools, was very limited and hard to extend.

This is my attempt in rewriting that part in F# using FParsec.

## Run it
To run the syntax evaluation, just use `dotnet run` followed by as many range expressions (in quotation marks) as you want.

## Range expressions
A range expression are built up of simple blocks, which are of the following form:
* They start with one or more concrete value characters '2, 3, ..., 9, T, J, Q, K, A',
* followed by zero or more 'x'es and 'y's, XOR one 'm',
* followed optionally by a '+'.

Examples are: "87x", "K9m", "Q85xy+".

Any range expressions can be combined by:
* enclosing them in (), and separating them by a ',' (logical OR)
* joining them by an '&' (logical AND)

They can also be negated by prefixing them with an '!'.

Here's a complete example: "(873, 62xy, 8TJ+) & !(72xy, 876) & 8JK+".
