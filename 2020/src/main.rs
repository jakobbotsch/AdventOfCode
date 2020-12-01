use std::fs::File;
use std::io::{self, BufRead};
use std::path::Path;

mod day1;

fn main() {
    day1::solve(&read_lines("day1.txt"))
}

fn read_lines<P>(filename: P) -> Vec<String>
where P: AsRef<Path>, {
    let file = File::open(filename).unwrap();
    io::BufReader::new(file).lines().map(|l| l.unwrap()).collect()
}
