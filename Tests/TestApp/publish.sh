#!/bin/bash

dotnet publish -c Debug -r linux-arm
dotnet publish -c Release -r linux-arm

rsync bin/Debug/netcoreapp2.0/linux-arm/publish/* pi@192.168.1.100:app/ASCOM.TestApp/Debug
rsync bin/Release/netcoreapp2.0/linux-arm/publish/* pi@192.168.1.100:app/ASCOM.TestApp/Release

scp ascom.db pi@192.168.1.100:app/ASCOM.TestApp/Debug
scp ascom.db pi@192.168.1.100:app/ASCOM.TestApp/Release
