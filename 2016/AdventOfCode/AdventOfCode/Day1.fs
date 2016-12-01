module Day1
open System

let solve (input : string) = 
    let turns = input.Split ([| ", " |], StringSplitOptions.None)
    let applyTurn (curX, curY, dirX, dirY, visited : Set<int * int>) (turn : string) =
        if Set.contains (curX, curY) visited then
            (curX, curY, dirX, dirY, visited)
        else
            let (dirX, dirY) =
                if turn.[0] = 'R' then
                    (dirY, -dirX)
                else
                    (-dirY, dirX)

            let distance = int turn.[1..]
            let (newX, newY) = (curX + dirX * distance, curY + dirY * distance)
            (newX, newY, dirX, dirY, Set.add (curX, curY) visited)

    let visited : Set<int * int> = Set.empty
    let (endX, endY, _, _, _) = Seq.fold applyTurn (0, 0, 0, 1, visited) turns
    printfn "Distance: %O" (abs(endX) + abs(endY))