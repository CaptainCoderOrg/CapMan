#!/bin/bash
dotnet format style --verify-no-changes --verbosity diagnostic
dotnet build
dotnet test
