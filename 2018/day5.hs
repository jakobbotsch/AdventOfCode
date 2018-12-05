import Data.Char
import Data.List

unitsReact :: Char -> Char -> Bool
unitsReact c1 c2 = c1 /= c2 && toUpper c1 == toUpper c2

part1 :: String -> String -> Int
part1 [] r = length r
part1 (unit:units) [] = part1 units [unit]
part1 (unit:units) (prev:prevs) =
    if unitsReact unit prev
    then part1 units prevs
    else part1 units (unit:prev:prevs)

part2 :: String -> Int
part2 polymer = minimum lengths
    where chars = nub . map toLower $ polymer
          remove c = filter ((/=c) . toLower) polymer
          lengths = map (\c -> part1 (remove c) []) chars

main = do
    contents <- readFile "day5.txt"
    print $ part1 contents []
    print $ part2 contents
