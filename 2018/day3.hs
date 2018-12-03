import Data.Char
import Data.Function
import Data.List
import qualified Data.Map as M
import Data.Maybe
import Control.Arrow

parseNums :: String -> [Int]
parseNums = groupBy ((==) `on` isDigit) >>> filter (head >>> isDigit) >>> map read

parseClaim :: String -> (Int, [(Int, Int)])
parseClaim claim =
    let [id, x, y, w, h] = parseNums claim
     in (id, [(x, y) | x <- [x..x+w-1], y <- [y..y+h-1]])

buildMap :: [(Int, [(Int, Int)])] -> M.Map (Int, Int) Int -> M.Map (Int, Int) Int
buildMap [] m = m
buildMap ((_, claim):xs) m =
    let m' = foldl (\m e -> M.alter (\n -> Just ((fromMaybe 0 n) + 1)) e m) m claim
     in buildMap xs m'

isNonOverlapping :: M.Map (Int, Int) Int -> (Int, [(Int, Int)]) -> Bool
isNonOverlapping m (_, pairs) =
    mapMaybe (\x -> M.lookup x m) >>> all (==1) $ pairs

findNonOverlapping :: M.Map (Int, Int) Int -> [(Int, [(Int, Int)])] -> Int
findNonOverlapping m [] = -1
findNonOverlapping m ((id, claim):xs) =
    let vals = mapMaybe (\x -> M.lookup x m) claim
     in if all (==1) vals
           then id
           else findNonOverlapping m xs

main = do
    contents <- readFile "day3.txt"
    let claims = map parseClaim $ lines contents
    let m = buildMap claims M.empty
    print $ filter (>1) >>> length $ M.elems m
    print $ filter (isNonOverlapping m) >>> map fst $ claims
