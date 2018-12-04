import Control.Arrow
import Data.Char
import Data.Function
import Data.List
import qualified Data.Map as M
import Data.Maybe

parseNums :: String -> [Int]
parseNums = groupBy ((==) `on` isDigit) >>> filter (head >>> isDigit) >>> map read

orderRecords :: [String] -> [String]
orderRecords = sortBy (compare `on` (parseNums >>> take 5))

-- Map from (guard, minute) into count of times guard has been asleep at that minute
buildSleepMap :: [String] -> (Int, Int) -> M.Map (Int, Int) Int
buildSleepMap [] _ = M.empty
buildSleepMap (rec:recs) (guard, asleepSince)
  | "Guard" `isInfixOf` rec = buildSleepMap recs ((parseNums rec) !! 5, 0)
  | "asleep" `isInfixOf` rec = buildSleepMap recs (guard, (parseNums rec) !! 4)
  | "wakes" `isInfixOf` rec =
  let wakesUpAt = (parseNums rec) !! 4
      m = buildSleepMap recs (guard, 0)
   in foldl' (\m e -> M.alter (\c -> Just ((fromMaybe 0 c) + 1)) (guard, e) m) m [asleepSince..wakesUpAt-1]

part1 :: M.Map (Int, Int) Int -> Int
part1 m =
    let guards = map fst >>> nub $ M.keys m
        totalAsleep g = map (\min -> M.lookup (g, min) m) >>> mapMaybe id >>> sum $ [0..59]
        bestGuard = maximumBy (compare `on` totalAsleep) guards
        bestGuardMinutes = filter (fst >>> fst >>> (==bestGuard)) $ M.toList m
        ((_, bestMinute), _) = maximumBy (compare `on` snd) bestGuardMinutes
     in bestGuard * bestMinute

part2 :: M.Map (Int, Int) Int -> Int
part2 m =
    let ((bestGuard, bestMinute), _) = maximumBy (compare `on` snd) $ M.toList m
     in bestGuard * bestMinute

main = do
    contents <- readFile "day4.txt"
    let records = orderRecords $ lines contents
    let sleepMap = buildSleepMap records (0, 0)
    print $ part1 sleepMap
    print $ part2 sleepMap
