pub fn solve(lines: &Vec<String>) {
    let earliest: i64 = lines[0].parse().unwrap();
    let ids: Vec<i64> =
        lines[1].split(",")
                .filter(|&s| s != "x")
                .map(|s| s.parse().unwrap())
                .collect();
    let (mut best, mut bestid) = (0i64, 0i64);
    for id in ids {
        let next = (earliest + id - 1) / id * id;
        if best < earliest || next < best {
            (best, bestid) = (next, id)
        }
    }

    println!("{}", bestid * (best - earliest));

    let mut remainders = Vec::new();
    let mut dividends: Vec<i64> = Vec::new();
    for (i, id) in lines[1].split(",").enumerate() {
        if id == "x" {
            continue
        }

        remainders.push(-(i as i64));
        dividends.push(id.parse().unwrap())
    }

    println!("ChineseRemainder[{{{}}}, {{{}}}]",
        remainders.iter().map(|i| i.to_string()).collect::<Vec<String>>().join(", "),
        dividends.iter().map(|i| i.to_string()).collect::<Vec<String>>().join(", "));
}