pub fn solve(lines: &Vec<String>) {
    let vals: Vec<i32> = lines.iter().map(|s| s.parse().unwrap()).collect();
    'part1: for v1 in vals.iter() {
        for v2 in vals.iter() {
            if v1 + v2 == 2020 {
                println!("{}", v1*v2);
                break 'part1
            }
        }
    }
    'part2: for v1 in vals.iter() {
        for v2 in vals.iter() {
            for v3 in vals.iter() {
                if v1 + v2 + v3 == 2020 {
                    println!("{}", v1*v2*v3);
                    break 'part2
                }
            }
        }
    }
}

