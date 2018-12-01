import qualified Data.Set as Set

findDup :: [Int] -> Set.Set Int -> Int -> Int
findDup (x:xs) sums cur =
    let new = cur + x in
    if Set.member new sums
       then new
       else findDup xs (Set.insert new sums) new

main = do
    contents <- readFile "day1.txt"
    let contentsWithoutPlus = filter (\c -> c /= '+') contents
    let freqs = map read (lines contentsWithoutPlus) :: [Int]
    let freqsInf = freqs ++ freqsInf
    putStrLn (show (sum freqs))
    putStrLn (show (findDup freqsInf Set.empty 0))
