#![feature(str_split_once)]
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

fn main() {
    day11::solve(&read_lines("day11.txt"))
}

fn read_lines<P>(filename: P) -> Vec<String>
where P: AsRef<Path>, {
    let file = File::open(filename).unwrap();
    io::BufReader::new(file).lines().map(|l| l.unwrap()).collect()
}
