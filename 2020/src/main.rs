#![feature(str_split_once)]
#![feature(destructuring_assignment)]
extern crate regex;

use std::fs::File;
use std::io::{self, BufRead};
use std::path::Path;

mod day1;
mod day2;
mod day3;
mod day4;
mod day5;
mod day6;
mod day7;
mod day8;
mod day9;
mod day10;
mod day11;
mod day12;
mod day13;

fn main() {
    day13::solve(&read_lines("day13.txt"));
    std::io::stdin().lock().lines().next().unwrap().unwrap();
    day12::solve(&read_lines("day12.txt"));
    day11::solve(&read_lines("day11.txt"));
    day10::solve(&read_lines("day10.txt"));
    day9::solve(&read_lines("day9.txt"));
    day8::solve(&read_lines("day8.txt"));
    day7::solve(&read_lines("day7.txt"));
    day6::solve(&read_lines("day6.txt"));
    day5::solve(&read_lines("day5.txt"));
    day4::solve(&read_lines("day4.txt"));
    day3::solve(&read_lines("day3.txt"));
    day2::solve(&read_lines("day2.txt"));
    day1::solve(&read_lines("day1.txt"));
}

fn read_lines<P>(filename: P) -> Vec<String>
where P: AsRef<Path>, {
    let file = File::open(filename).unwrap();
    io::BufReader::new(file).lines().map(|l| l.unwrap()).collect()
}
