@echo off

set "TARGET_MIGRATION=%~1"

dotnet ef database update %TARGET_MIGRATION% --project ..\UserManagement.Data\ --startup-project ..\UserManagement.Api\
