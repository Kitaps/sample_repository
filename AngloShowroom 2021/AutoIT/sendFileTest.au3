#include <file.au3>

Local $source_path 
Local $destiny_path

$source_path = "C:\Users\salatech\Desktop\Showroom\3. Build\Anglo Build\Anglo Showroom_Data\StreamingAssets\data\test.txt"
;$destiny_path = "C:\Users\salatech\Desktop\Showroom\3. Build\Anglo Build\Anglo Showroom_Data\StreamingAssets\data\test_folder\"
$destiny_path = "\\192.168.0.135\TransferFSMtoShowroom\"
;FileCopy ( $source_path, $destiny_path, $FC_OVERWRITE + $FC_CREATEPATH )

while True; Cada 14 minutos aprieta ñ para que no entre en sleep. El sleep es cada 15 min.
    
    FileCopy ( $source_path, $destiny_path, $FC_OVERWRITE + $FC_CREATEPATH )
    Sleep(1000 * 10)

    
WEnd