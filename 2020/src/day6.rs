pub fn solve(lines: &Vec<String>) {
    let mut i = 0;
    let mut part1 = 0;
    while i < lines.len() {
        let mut yeses = [false; 26];
        while i < lines.len() && lines[i] != "" {
            for c in lines[i].bytes() {
                yeses[(c-b'a') as usize] = true
            }

            i += 1
        }

        part1 += yeses.iter().filter(|&&b| b).count();
        i += 1
    }

    println!("{}", part1);

    let mut i = 0;
    let mut part2 = 0;
    while i < lines.len() {
        let mut yeses = [0; 26];
        let mut group_size = 0;
        while i < lines.len() && lines[i] != "" {
            for c in lines[i].bytes() {
                yeses[(c-b'a') as usize] += 1
            }

            group_size += 1;
            i += 1
        }

        part2 += yeses.iter().filter(|&&ny| ny == group_size).count();
        i += 1
    }

    println!("{}", part2)
}