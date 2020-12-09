use std::collections::VecDeque;

pub fn solve(lines: &Vec<String>) {
    let nums: Vec<i64> = lines.iter().map(|s| s.parse().unwrap()).collect();
    let mut prev = VecDeque::new();
    let mut part1 = 0;
    for num in nums.iter() {
        if prev.len() < 25 {
            prev.push_back(num);
            continue
        }

        let mut valid = false;
        'outer: for i in 0..prev.len() {
            for j in 0..prev.len() {
                if i == j {
                    continue
                }

                if prev[i] + prev[j] == *num {
                    valid = true;
                    break 'outer
                }
            }
        }

        if valid {
            prev.push_back(num);
            prev.pop_front();
        } else {
            println!("{}", num);
            part1 = *num;
            break
        }
    }

    let mut cum_sum = Vec::new(); // cum_sum[i] = sum nums[0] .. nums[i] (exclusive)
    let mut sum = 0;
    for n in nums.iter() {
        cum_sum.push(sum);
        sum += n;
    }
    cum_sum.push(sum);

    'part2: for i in 0..nums.len() {
        for j in i+1..=nums.len() {
            if cum_sum[j] - cum_sum[i] == part1 {
                let range = &nums[i..j];
                println!("{}", range.iter().min().unwrap() + range.iter().max().unwrap());
                break 'part2
            }
        }
    }
}