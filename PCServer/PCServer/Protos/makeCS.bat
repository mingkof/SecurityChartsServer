set protogen=.\protobuf-net\protogen.exe
set SRC_DIR=./protos/define
set DST_DIR=./protos/cs

%protogen% --proto_path=%SRC_DIR% --csharp_out=%DST_DIR% common.proto pb0x01.proto

call copyProtoToClient.bat

:: ./protobuf-net/protogen.exe --proto_path=./protos/define --csharp_out=./protos/cs pb0x01.proto common.proto
pause