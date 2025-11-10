@echo off

dotnet ef database drop --project ..\UserManagement.Data\ --startup-project ..\UserManagement.Api\
