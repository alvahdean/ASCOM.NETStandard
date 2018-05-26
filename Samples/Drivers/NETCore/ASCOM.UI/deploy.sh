#!/bin/bash
dotnet publish -c Debug -r linux-arm 
dotnet publish -c Release -r linux-arm 
rsync 
