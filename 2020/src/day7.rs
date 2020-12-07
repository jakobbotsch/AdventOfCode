use regex::Regex;
use std::collections::{HashSet, HashMap};

#[derive(Debug, Clone)]
struct Bag {
    pub name: String,
    pub contained: Vec<(i32, String)>,
}

pub fn solve(lines: &Vec<String>) {
    let parse = |s: &String| {
        let s = s.replace(" bags", "");
        let s = s.replace(" bag", "");
        let (name, rest) = s.split_once(" contain ").unwrap();
        let v =
            if rest == "no other." {
                vec![]
            } else {
                rest.trim_end_matches(".").split(", ")
                    .map(|s| {
                        let (num, na) = s.split_once(" ").unwrap();
                        (num.parse().unwrap(), na.to_owned())
                    }).collect()
            };
        Bag { name: name.to_owned(), contained: v }
    };

    let parsed: Vec<_> = lines.iter().map(parse).collect();

    let mut left = parsed.clone();
    let mut sorted = vec![];
    while left.len() > 0 {
        for (i, left_bag) in left.iter().enumerate() {
            let contained = |name: &String| sorted.iter().any(|s: &Bag| s.name == *name);
            if left_bag.contained.iter().all(|dep| contained(&dep.1)) {
                sorted.push(left_bag.clone());
                left.swap_remove(i);
                break
            }
        }
    }

    let mut contains = HashSet::new();
    for b in sorted.iter() {
        if b.contained.iter().any(|b| b.1 == "shiny gold" || contains.contains(&b.1)) {
            contains.insert(b.name.clone());
        }
    }

    println!("{}", contains.len());

    let mut num_bags: HashMap<String, i64> = HashMap::new();
    for b in sorted.iter() {
        num_bags.insert(
            b.name.clone(),
            b.contained.iter().map(|b| b.0 as i64 + num_bags.get(&b.1).unwrap() * b.0 as i64).sum());
    }

    println!("{}", num_bags.get("shiny gold").unwrap());
}