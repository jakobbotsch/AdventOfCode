use std::collections::HashSet;

pub fn solve(lines: &Vec<String>) {
    fn parse_seat(s: &str) -> (i32, i32) {
        let row = i32::from_str_radix(&s[0..7].replace("F", "0").replace("B", "1"), 2).unwrap();
        let col = i32::from_str_radix(&s[7..10].replace("L", "0").replace("R", "1"), 2).unwrap();
        (row, col)
    }

    let seats: Vec<_> = lines.iter().map(|s| parse_seat(s)).collect();
    println!("{}", seats.iter().map(|&(row, col)| row*8 + col).max().unwrap());

    let seat_ids: HashSet<_> = seats.iter().map(|&(row, col)| row*8 + col).collect();

    for id in 1..128*8 {
        if !seat_ids.contains(&id) && seat_ids.contains(&(id-1)) && seat_ids.contains(&(id+1)) {
            println!("{}", id);
            break
        }
    }
}