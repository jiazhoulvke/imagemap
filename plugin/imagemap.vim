function! ImageMap()
    let filename=getline(".")
    let result=system("ImageMap " . filename)
    if v:shell_error == 1
        echo "fail!"
    elseif v:shell_error == 0
        "echo result
        call setline(".",result)
    endif
endfunction

imap <M-m> <ESC>:call ImageMap()<CR>
