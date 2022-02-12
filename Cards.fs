namespace Own

module Cards = 
    
    type Value = Value of int
    
    type Suit = 
        | Clubs
        | Spades
        | Diamonds
        | Hearts

    type Card = Card of Value * Suit
    
    let getValueValue (Value value) = value
    
    let getSuit (Card (_, suit)) = suit 
    
    let getValue (Card (value,_)) = value 
    
    let getUnderlyingValue card = getValue card |> getValueValue
