pub fn solve(lines: &Vec<String>) {
    let (mut x, mut y, mut dx, mut dy) = (0, 0, 1, 0);
    for s in lines {
        let amt: i32 = s[1..].parse().unwrap();
        let rots = if amt/90*90 == amt { Some(amt/90) } else { None };
        match s.as_bytes()[0] {
            b'N' => y += amt,
            b'S' => y -= amt,
            b'E' => x += amt,
            b'W' => x -= amt,
            b'L' => {
                for _ in 0..rots.unwrap() {
                    (dx, dy) = (-dy, dx)
                }
            },
            b'R' => {
                for _ in 0..rots.unwrap() {
                    (dx, dy) = (dy, -dx)
                }
            },
            b'F' => (x, y) = (x + dx*amt, y + dy*amt),
            _ => panic!("unexpected")
        }
    }

    println!("{}", x.abs() + y.abs());

    (x, y, dx, dy) = (0, 0, 10, 1);

    for s in lines {
        let amt: i32 = s[1..].parse().unwrap();
        let rots = if amt/90*90 == amt { Some(amt/90) } else { None };
        match s.as_bytes()[0] {
            b'N' => dy += amt,
            b'S' => dy -= amt,
            b'E' => dx += amt,
            b'W' => dx -= amt,
            b'L' => {
                for _ in 0..rots.unwrap() {
                    (dx, dy) = (-dy, dx)
                }
            },
            b'R' => {
                for _ in 0..rots.unwrap() {
                    (dx, dy) = (dy, -dx)
                }
            },
            b'F' => (x, y) = (x + dx*amt, y + dy*amt),
            _ => panic!("unexpected")
        }
    }

    println!("{}", x.abs() + y.abs());
}