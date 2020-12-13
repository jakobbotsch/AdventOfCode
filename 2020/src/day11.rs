pub fn solve(lines: &Vec<String>) {
    let first_state: Vec<Vec<u8>> =
        lines.iter()
             .map(|s| s.clone().into_bytes())
             .collect();

    let neis = [(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)];
    
    let mut state = first_state.clone();
    loop {

        let is_occup = |x: i32, y: i32| -> bool {
            y >= 0 && (y as usize) < state.len() &&
            x >= 0 && (x as usize) < state[y as usize].len() &&
            state[y as usize][x as usize] == b'#'
        };

        let mut new_state = state.clone();
        for (y, l) in new_state.iter_mut().enumerate() {
            for (x, b) in l.iter_mut().enumerate() {
                let num_adj = neis.iter().filter(
                    |&&(dx, dy)| is_occup(x as i32 + dx, y as i32 + dy)).count();
                *b = match *b {
                    b'L' => if num_adj == 0 { b'#' } else { b'L' },
                    b'#' => if num_adj >= 4 { b'L' } else { b'#' },
                    b => b
                }
            }
        }

        if new_state == state {
            println!("{}", state.iter().map(|l| l.iter().filter(|&&b| b == b'#').count()).sum::<usize>());
            break
        }

        state = new_state
    }

    let mut state = first_state.clone();
    loop {
        let mut new_state = state.clone();

        for (y, l) in new_state.iter_mut().enumerate() {
            for (x, b) in l.iter_mut().enumerate() {

                if *b == b'.' {
                    continue
                }

                let is_occup = |dx: i32, dy: i32| -> bool {
                    let (mut cx, mut cy) = (x as i32, y as i32);
                    loop {
                        cx += dx;
                        cy += dy;

                        if cy < 0 || (cy as usize) >= state.len() {
                            return false
                        }

                        if cx < 0 || (cx as usize) >= state[cy as usize].len() {
                            return false
                        }

                        let b = state[cy as usize][cx as usize];
                        if b == b'#' {
                            return true
                        } else if b == b'L' {
                            return false
                        }
                    }
                };

                let num_adj = neis.iter().filter(
                    |&&(dx, dy)| is_occup(dx, dy)).count();
                *b = match *b {
                    b'L' => if num_adj == 0 { b'#' } else { b'L' },
                    b'#' => if num_adj >= 5 { b'L' } else { b'#' },
                    b => b
                }
            }
        }

        if new_state == state {
            println!("{}", state.iter().map(|l| l.iter().filter(|&&b| b == b'#').count()).sum::<usize>());
            break
        }

        state = new_state
    }
}