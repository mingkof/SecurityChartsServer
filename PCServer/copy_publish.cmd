:: set SRC_DIR=..\部署\公安
set DST_DIR=.\PCServer\bin\Release\PublishOutput

xcopy ..\部署\公安\node_modules_bak %DST_DIR%\node_modules\  /s /e 
xcopy .\PCServer\Node %DST_DIR%\Node\ /s /e 
xcopy .\PCServer\static %DST_DIR%\static\ /s /e

pause