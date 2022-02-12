namespace Own

module Output =
    open FParsec
    let Dump parser input =
        printfn "Input: %s" input 

        match run parser input with
        | Success(result, _, _)   -> printfn "Success: %A" result
        | Failure(message, _, _) -> printfn "Failure: %s" message
        printfn ""

module Main =  
    open FParsec
    
    [<EntryPoint>]
    let main args =
        printfn ""
        printfn "Succeding ranges:"
        List.iter (Output.Dump RangeParsers.rangeParser) [
            //"873xy+";
            //"!8Kxx";
            // "(873 , 2xy  ,8TJ+)"   
            "(873 , 2xy  ,8TJ+) & !(72xy, 876) & 8JK+"   
            // "8TJx";  
            // "2xy"; 
            // "2xy+";   
            // "873 , 2xy  ,8TJ+,3x"
            // "2xyJ";   
            // "2Fxy";   
            // "FL2x";   
            // "25+x";   
            // "2Jx+y";   
        ]

        // CardsParsers.values
        // |> Array.map (fun v -> CardsParsers.toValue v)
        // |> Array.iter (fun q -> printfn "value: %i" (Cards.getValueValue q))


        // printfn "value: %i" (CardsParsers.toValue 'r')
       // printfn "WCparsed: %A" (run RangeParsers.typedAnyWildCardParser "z")
        0