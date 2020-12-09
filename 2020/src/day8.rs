use std::collections::HashSet;

#[derive(Clone)]
enum Inst {
    Acc(i32),
    Jmp(i32),
    Nop(i32),
}

fn parse_insts(lines: &Vec<String>) -> Vec<Inst> {
    lines.iter().map(|s| {
        let (inst, arg) = s.split_once(" ").unwrap();
        let arg = arg.parse().unwrap();
        match inst {
            "acc" => Inst::Acc(arg),
            "jmp" => Inst::Jmp(arg),
            "nop" => Inst::Nop(arg),
            _ => panic!(format!("Invalid instruction '{}'", inst))
        }
    }).collect()
}

#[derive(Default)]
struct Vm {
    ip: i32,
    acc: i32,
}

impl Vm {
    pub fn new() -> Self {
        Default::default()
    }

    pub fn run(&mut self, inst: &Inst) {
        match inst {
            &Inst::Acc(imm) => {
                self.acc += imm;
                self.ip += 1
            },
            &Inst::Jmp(imm) => self.ip += imm,
            &Inst::Nop(_) => self.ip += 1
        }
    }
}

pub fn solve(lines: &Vec<String>) {
    let insts = parse_insts(lines);
    let mut executed = HashSet::new();
    let mut vm = Vm::new();
    while executed.insert(vm.ip) {
        vm.run(&insts[vm.ip as usize])
    }

    println!("{}", vm.acc);

    for i in 0..insts.len() {
        let mut insts = insts.clone();
        insts[i] = match insts[i] {
            Inst::Nop(imm) => Inst::Jmp(imm),
            Inst::Jmp(imm) => Inst::Nop(imm),
            _ => continue
        };
        executed.clear();
        vm = Vm::new();
        while vm.ip >= 0 && (vm.ip as usize) < insts.len() && executed.insert(vm.ip) {
            vm.run(&insts[vm.ip as usize])
        }

        if vm.ip == (insts.len() as i32) {
            println!("{}", vm.acc);
            break
        }
    }
}