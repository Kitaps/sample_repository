;This script can does the same as move_max_gsd every 7 hours. Can replace module. 

Local $rounds = 30; Number of rounds until the next security desk window reset

while True; Cada 14 minutos aprieta ñ para que no entre en sleep. El sleep es cada 15 min.
    Sleep(1000 * 60 * 14)
    Send("ñ")

    $rounds += 1
    
    ;This should be a function but meh
    if $rounds > 30 Then; 30 * 14 = 420 == 7 hours
        WinSetState("Genetec Security Desk", "", @SW_RESTORE)
        WinMove("Genetec Security Desk", "", 3540, 570, 200, 200); If 4K is not main Desktop
        WinSetState("Genetec Security Desk", "", @SW_MAXIMIZE)
        $rounds = 0
    EndIf
WEnd