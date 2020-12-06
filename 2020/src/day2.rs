pub fn solve(lines: &Vec<String>) {
    fn parse(s: &str) -> (i32, i32, char, &str) {
        let v: Vec<&str> = s.split(" ").collect();
        let (min, max) = v[0].split_once("-").unwrap();
        let c = v[1].chars().nth(0).unwrap();
        let pass = v[2];
        (min.parse().unwrap(), max.parse().unwrap(), c, pass)
    }
    let parsed: Vec<(i32, i32, char, &str)> = lines.iter().map(|s| parse(s)).collect();
    fn is_valid1<'r>((min, max, c, pass): &'r (i32, i32, char, &'r str)) -> bool {
        let occ = pass.chars().filter(|c2| *c2 == *c).count();
        occ >= *min as usize && occ <= *max as usize
    };

    println!("{}", parsed.iter().filter(|t| is_valid1(t)).count());

    fn is_valid2<'r>((min, max, c, pass): &'r (i32, i32, char, &'r str)) -> bool {
        (pass.chars().nth((*min-1) as usize).unwrap() == *c) ^ (pass.chars().nth((*max-1) as usize).unwrap() == *c)
    };

    println!("{}", parsed.iter().filter(|t| is_valid2(t)).count());
}