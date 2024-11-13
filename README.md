# DTOMaker.CSPoco

[![Build-Deploy](https://github.com/Psiman62/DTOMaker-CSPoco/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Psiman62/DTOMaker-CSPoco/actions/workflows/dotnet.yml)

*Warning: This is pre-release software under active development. Not for production use. Breaking changes occur often.*

Source generator for C# POCOs (Plain Old C# Objects).

Features:
- implements data model interface;
- IFreezable support.

## Quick Start
To use this generator:
1. create a new C# library project to contain your POCOs;
2. add a package reference to DTOMaker.Core;
3. define your models in C# as interfaces with simple properties;
5. add a reference to this package;
6. build your project;
7. voila! your generated C# POCOs are ready to use.
