@echo off

dotnet ef migrations remove --project ..\UserManagement.Data\ --startup-project ..\UserManagement.Api\
