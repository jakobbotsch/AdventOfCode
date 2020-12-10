pub fn solve(lines: &Vec<String>) {
    let mut jolts: Vec<i32> = lines.iter().map(|s| s.parse().unwrap()).collect();
    jolts.sort_unstable();
    jolts.insert(0, 0);
    jolts.push(jolts.last().unwrap() + 3);
    let mut diffs = [0; 4];
    for i in 0..jolts.len()-1 {
        let diff = jolts[i+1] - jolts[i];
        assert!(diff > 0 && diff <= 3);
        diffs[diff as usize] += 1
    }

    println!("{}", diffs[1] * diffs[3]);

    let mut num_arrangements: Vec<i64> = Vec::new(); // num_arrangements[i] = number of arrangements up to and including jolts[i]
    for (i, jolt) in jolts.iter().enumerate() {
        let mut this_num = 0;
        for j in (if i >= 3 { i - 3 } else { 0 }) .. i {
            if jolts[j] >= jolt - 3 {
                this_num += num_arrangements[j];
            }
        }

        num_arrangements.push(std::cmp::max(this_num, 1));
    }

    println!("{}", num_arrangements.last().unwrap());
}