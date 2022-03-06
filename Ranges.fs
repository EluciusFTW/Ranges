namespace Ranges

module Range = 
    type WildCard = 
        | StrictWildCard
        | LaxWildCard
    
    type Exactness =
        | ExactlyLike
        | OrBetter

    type Contribution = 
        | Add 
        | Subtract

    // type PRange = PRange of Cards.Value list
    // type ARange = ARange of Cards.Value list * WildCard list 
    // type APRange = APRange of Cards.Value list * WildCard list 
    
    type AnyRange = 
        // | PointRange of PRange
        // | AreaRange of ARange 
        // | AreaPlusRange of APRange
        | PointRange of Cards.Value list
        | AreaRange of Cards.Value list * int * int
        | AreaPlusRange of Cards.Value list * int * int 
        
    let strictWCs cards = 
        cards 
        |> List.filter (fun c -> 
            match c with 
            | WildCard.StrictWildCard -> true 
            | _ -> false)
        |> List.length

    let laxWCs cards = 
        cards 
        |> List.filter (fun c -> 
            match c with 
            | WildCard.StrictWildCard -> false 
            | _ -> true)
        |> List.length

    let wildCardsTuple cards = (strictWCs cards, laxWCs cards)
    
    let factory (values: Cards.Value list) (wildCards: WildCard list) (exactness: Exactness) = 
        match (values, wildCards, exactness) with
        | (_, _, OrBetter) -> AreaPlusRange (values, strictWCs wildCards, laxWCs wildCards)
        | (_, [], _) -> PointRange values
        | _ -> AreaRange (values, strictWCs wildCards, laxWCs wildCards)

    let rec resolve range =
        match range with
        | PointRange _ -> [ range ]
        | AreaRange(r, s, l) -> 
            match (r, s, l) with
            | (r, 0, 0) -> [ PointRange r ]
            | (r, 0, l) -> resolve (AreaRange(r, 0, l-1)) // [ PointRange [Cards.Value 3] ] @ (resolve (AreaRange(r, 0, l-1)))
            | (r, s, l) when s > 0 -> resolve (AreaRange(r, s-1, l))
            | _ -> raise (System.ArgumentException()) 
        | AreaPlusRange(r, s, l) -> resolve (AreaRange(r, s, l))

    let getValues range =
        match range with
        | PointRange values -> values
        | _ -> []

    let getAllValues (values: Cards.Value list) (wildCards: WildCard list) (exactness: Exactness) = 
        factory values wildCards exactness 
        |> resolve
        |> List.map getValues

    type SimpleRange(values: Cards.Value list, wildCards: WildCard list, exactness: Exactness) = 
        member this.Values = values
        member this.WildCards = wildCards
        member this.Exactness = exactness

    let createSimpleRange (values: Cards.Value list) (wildCards: WildCard list) (exactness: Exactness) = 
        let valLength = List.length values
        let wcLength = List.length wildCards
        if valLength = 0 then None 
        elif valLength + wcLength > 5 then None 
        else Some (new SimpleRange(values, wildCards, exactness))
        
        