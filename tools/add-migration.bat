@echo off

set MIGRATION_NAME=%1

dotnet ef migrations add %MIGRATION_NAME% --project ..\UserManagement.Data\ --startup-project ..\UserManagement.Api\
