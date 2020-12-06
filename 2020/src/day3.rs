pub fn solve(lines: &Vec<String>) {
    let num_trees = |dx, dy| {
        let (mut x, mut y) = (0, 0);
        let mut num_trees = 0;
        loop {
            let line = &lines[y];
            if line.chars().nth(x % line.len()).unwrap() == '#' {
                num_trees += 1
            }
            x += dx;
            y += dy;
            if y >= lines.len() {
                break
            }
        }

        num_trees
    };

    println!("{}", num_trees(3, 1));

    let part2: usize = [(1, 1), (3, 1), (5, 1), (7, 1), (1, 2)].iter().map(|&(dx, dy)| num_trees(dx, dy)).product();
    println!("{}", part2);
}