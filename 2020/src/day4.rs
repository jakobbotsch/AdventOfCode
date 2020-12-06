use regex::Regex;
use std::collections::HashMap;

pub fn check_passports(lines: &Vec<String>, is_valid: impl Fn(&HashMap<&str, &str>) -> bool) -> i32 {
    let mut num_valid = 0;
    let mut i = 0;
    while i < lines.len() {
        let mut fields = HashMap::new();
        while i < lines.len() && lines[i] != "" {
            for kvp in lines[i].split(" ") {
                let (k, v) = kvp.split_once(":").unwrap();
                fields.insert(k, v);
            }

            i += 1
        }

        if is_valid(&fields) {
            num_valid += 1
        }

        i += 1
    }

    num_valid
}

pub fn solve(lines: &Vec<String>) {
    let is_valid1 = |fields: &HashMap<&str, &str>| {
        ["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"].iter().all(|s| fields.contains_key(s))
    };
    println!("{}", check_passports(lines, is_valid1));

    let is_valid2 = |fields: &HashMap<&str, &str>| {
        let byr: i32 = fields.get("byr")?.parse().ok()?;
        let iyr: i32 = fields.get("iyr")?.parse().ok()?;
        let eyr: i32 = fields.get("eyr")?.parse().ok()?;
        if byr < 1920 || byr > 2002 || iyr < 2010 || iyr > 2020 || eyr < 2020 || eyr > 2030 {
            return None
        }
        let hgt = fields.get("hgt")?;
        let hgt_re = Regex::new(r"^((?P<cm>\d+)cm|(?P<in>\d+)in)$").unwrap();
        let hgt_cap = hgt_re.captures(hgt)?;
        if let Some(cm) = hgt_cap.name("cm") {
            let cm: i32 = cm.as_str().parse().ok()?;
            if cm < 150 || cm > 193 {
                return None
            }
        } else if let Some(inches) = hgt_cap.name("in") {
            let inches: i32 = inches.as_str().parse().ok()?;
            if inches < 59 || inches > 76 {
                return None
            }
        } else {
            return None
        }

        let hcl = fields.get("hcl")?;
        if !Regex::new(r"^#[0-9a-f]{6}$").unwrap().is_match(hcl) {
            return None
        }

        let ecl = fields.get("ecl")?;
        if !Regex::new(r"^(amb|blu|brn|gry|grn|hzl|oth)$").unwrap().is_match(ecl) {
            return None
        }

        let pid = fields.get("pid")?;
        if !Regex::new(r"^[0-9]{9}$").unwrap().is_match(pid) {
            return None
        }

        return Some(())
    };

    println!("{}", check_passports(lines, |f| is_valid2(f).is_some()));
}