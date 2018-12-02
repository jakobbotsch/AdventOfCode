import qualified Data.Map.Strict as Map
import Data.List
import Control.Arrow

histocontains :: Int -> String -> Bool
histocontains n = sort >>> group >>> map length >>> any (== n)

checksum :: [String] -> Int
checksum ids = (length . filter (histocontains 2) $ ids) * (length . filter (histocontains 3) $ ids)

findPairs :: [String] -> [(String, String)]
findPairs ids = [(x, y) | x <- ids, y <- ids, filter (uncurry (/=)) >>> length >>> (==1) $ zip x y]

main = do
    contents <- readFile "day2.txt"
    let ids = lines contents
    print $ checksum ids
    print $ findPairs ids
