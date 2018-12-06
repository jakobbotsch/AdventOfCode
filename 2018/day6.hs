import Data.Char
import Data.Function
import Data.List
import qualified Data.Map as M

parseCoord :: String -> (Int, Int)
parseCoord s = (x, y)
    where [x, y] = map read . filter (isDigit . head) . groupBy ((==) `on` isDigit) $ s

part1 :: [(Int, Int)] -> Int
part1 coords =
    let minX = minimum $ map fst coords
        minY = minimum $ map snd coords
        maxX = maximum $ map fst coords
        maxY = maximum $ map snd coords
        grid = [(x, y) | x <- [minX..maxX], y <- [minY..maxY]]
        bestDistanceId x y =
            let distances = map (\(cx, cy) -> abs (cx - x) + abs (cy - y)) coords 
                (id1, d1) : (id2, d2) : tl = sortOn snd . zip [0..] $ distances
            in if d1 == d2
                then -1
                else id1
        bestDistMap = M.fromList $ map (\(x, y) -> ((x, y), bestDistanceId x y)) grid
        isBorder ((x, y), _) = x == minX || x == maxX || y == minY || y == maxY
        badIds = nub . map snd . filter isBorder $ M.assocs bestDistMap
        goodIds = delete (-1) . (\\badIds) . nub $ M.elems bestDistMap :: [Int]
        area id = length . filter (==id) $ M.elems bestDistMap
        areas = map area goodIds
    in maximum areas

part2 :: [(Int, Int)] -> Int
part2 coords =
    let minX = (minimum (map fst coords)) - 10000 `div` (length coords) - 1
        minY = (minimum (map snd coords)) - 10000 `div` (length coords) - 1
        maxX = (maximum (map fst coords)) + 10000 `div` (length coords) + 1
        maxY = (maximum (map snd coords)) + 10000 `div` (length coords) + 1
        grid = [(x, y) | x <- [minX..maxX], y <- [minY..maxY]]
        distanceSum (x, y) = sum . map (\(cx, cy) -> abs (cx - x) + abs (cy - y)) $ coords
        distances = map distanceSum grid
     in length . filter (<10000) $ distances

main = do
    contents <- readFile "day6.txt"
    let coords = map parseCoord $ lines contents
    print $ part1 coords
    print $ part2 coords
