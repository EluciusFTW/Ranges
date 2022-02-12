namespace Own

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

    type SimpleRange(values: Cards.Value list, wildCards: WildCard list, exactness: Exactness) = 
        member this.Values = values
        member this.WildCards = wildCards
        member this.Exactness = exactness

    let createSimpleRange (values: list<Cards.Value>) (wildCards: list<WildCard>) (exactness: Exactness) = 
        let valLength = List.length values
        let wcLength = List.length wildCards
        if valLength = 0 then None 
        elif valLength + wcLength > 5 then None 
        else Some (new SimpleRange(values, wildCards, exactness))
        
        