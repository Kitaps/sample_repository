; #include "test.au3"; this is the way to import modules 

Func Handle(ByRef $command)

    Switch $command

        Case "Exit"
            Return 1

        Case "VolumeDown"
            Send("{VOLUME_DOWN}")

        Case "VolumeUp"
            Send("{VOLUME_UP}")
        
        Case "VolumeMute"
            Send("{VOLUME_MUTE}")

        ; The next commands are example commands to switch to another window
        Case "Banana"
            WinActivate("VoiceMeeter")

        Case "CSP"
            WinActivate("CLIP STUDIO")
        
        Return 0
    
    EndSwitch

EndFunc

