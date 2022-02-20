namespace Ranges

module CardsParsers = 
    open FParsec
    open Cards

    let values = [| '2'; '3'; '4'; '5'; '6'; '7'; '8'; '9'; 'T'; 'J'; 'Q'; 'K'; 'A' |]
    let toValue char =
        let indexOption = Array.tryFindIndex (fun x -> x = char) values
        match indexOption with 
        | Some index -> Value (index + 2)
        | None -> raise (System.ArgumentException("Can only determine value of valid value character."))
    
    let suits = [ 's'; 'd'; 'h'; 'c']
    let toSuit char : Suit = 
        match char with
        | 's' -> Spades
        | 'd' -> Diamonds
        | 'h' -> Hearts
        | 'c' -> Clubs
        | _ -> raise (System.ArgumentException("Can only determine suit of valid suit character."))

    let valueParser: Parser<Value, unit> = anyOf values |>> toValue
    let suitParser = anyOf suits |>> toSuit
    let cardParser = valueParser .>>. suitParser |>> Card
    let cardsParser = many (cardParser .>> spaces) .>> eof

module RangeParsers = 
    open FParsec

    module WildCards = 
        let strictWildCardCharacter = 'x'
        let laxWildCardCharacter = 'y'
        let createWildCard (char: char) = 
            match char with
            | c when c = strictWildCardCharacter -> Range.StrictWildCard
            | c when c = laxWildCardCharacter -> Range.LaxWildCard
            | _ -> raise (System.ArgumentException("Can only create wild-cards from valid wild-card characters."))
        let strictWildCardParser = pchar strictWildCardCharacter
        let laxWildCardParser = pchar laxWildCardCharacter
        let anyWildCardParser = 
            strictWildCardParser <|> laxWildCardParser 
            |>> createWildCard
    
    module Exactness = 
        let exactnessCharacter = '+'
        let toExactness exactnessOption = 
            match exactnessOption with 
            | Some _ -> Range.OrBetter 
            | None -> Range.ExactlyLike
        let ExactnessParser = (opt (pchar exactnessCharacter)) |>> toExactness

    module Contribution =
        let negationCharacter = '!'
        let toContribution negOption = 
            match negOption with 
            | Some _ -> Range.Subtract
            | None -> Range.Add
        let contributionParser = opt (pchar negationCharacter) |>> toContribution

    let simpleRangeParser = 
        many1 CardsParsers.valueParser 
        .>>. many WildCards.anyWildCardParser 
        .>>. Exactness.ExactnessParser 
        .>> spaces 
        |>> (fun ((values, wildCards), exactness) -> Range.createSimpleRange values wildCards exactness) 

    module Composition = 
        let OrCompositionParser = pchar ',' .>> spaces
        let unionRangeParser = sepBy simpleRangeParser OrCompositionParser
        let blockParser = simpleRangeParser |>> (fun x -> [x]) <|> between (pchar '(') (pchar ')') unionRangeParser 
        let signedBlockParser = Contribution.contributionParser .>>. blockParser
        let andParser = spaces >>. pchar '&' .>> spaces
    
    let rangeParser = sepBy Composition.signedBlockParser Composition.andParser .>> eof
